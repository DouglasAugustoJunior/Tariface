using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace DetectaRostoImagemDetalhesFaceAPI
{
    /// <summary>
    /// Lógica do MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constantes
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable("FACE_SUBSCRIPTION_KEY"); // Chave Azure
        private static readonly string faceEndpoint = Environment.GetEnvironmentVariable("FACE_ENDPOINT");            // Endereço Azure

        private readonly IFaceClient faceAPIclient = new FaceClient(
            new ApiKeyServiceClientCredentials(subscriptionKey),
            new System.Net.Http.DelegatingHandler[] { });

        private IList<DetectedFace> rostosDetectados;
        private string[]            descricoesRostosDetectados;
        private double              fatorRedimensionamento; // O fator de redimensionamento da imagem exibida.

        private const string defaultStatusBarText = "Posicione o ponteiro do mouse sobre um rosto para ver sua descrição.";
        #endregion Constantes

        public MainWindow()
        {
            InitializeComponent();

            if (Uri.IsWellFormedUriString(faceEndpoint, UriKind.Absolute)) { faceAPIclient.Endpoint = faceEndpoint; } // Se a URL do Azure estiver correta
            else
            {
                MessageBox.Show(faceEndpoint,
                    "URL do Azure inválida!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Exibe a imagem e chama UploadEDetectaRosto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Evento do click do botão</param>
        private async void BuscaClick(object sender, RoutedEventArgs e)
        {
            var modalSelecionarArquivo = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JPEG Image(*.jpg)|*.jpg" // Tipo do arquivo que o usuário pode selecionar
            }; // Cria o modal para selecionar a imagem
            bool? retornoModal = modalSelecionarArquivo.ShowDialog(this);       // Abre o modal

            if (!(bool)retornoModal) return; // Retorna se o usuário fechar sem selecionar um arquivo

            string diretorioArquivo = modalSelecionarArquivo.FileName; // Diretório da imagem

            Uri arquivoUri = new Uri(diretorioArquivo);   // Pega o arquivo de imagem selecionado pelo usuário
            
            BitmapImage bitmapOrigem = new BitmapImage(); // Recebe o arquivo Uri
            bitmapOrigem.BeginInit();
            bitmapOrigem.CacheOption = BitmapCacheOption.None;
            bitmapOrigem.UriSource = arquivoUri;
            bitmapOrigem.EndInit();

            ImagemRosto.Source = bitmapOrigem; // Exibe a imagem selecionada no componente de imagem no xaml

            Title = "Detectando...";
            rostosDetectados = await UploadEDetectaRosto(diretorioArquivo); // Detecta qualquer rosto na imagem
            Title = String.Format("Detecção finalizada. {0} rosto(s) detectados", rostosDetectados.Count);

            if (rostosDetectados.Count > 0)
            {
                DrawingVisual visual = new DrawingVisual();          // Renderiza desenhos na imagem
                DrawingContext drawingContext = visual.RenderOpen(); // Desenha os retângulos

                // Preparando para desenhar os retângulos ao redor dos rostos
                drawingContext.DrawImage(bitmapOrigem, new Rect(0, 0, bitmapOrigem.Width, bitmapOrigem.Height));
                double dpi = bitmapOrigem.DpiX;
                fatorRedimensionamento = (dpi == 0) ? 1 : 96 / dpi; // Algumas imagens não contêm informações de dpi.
                
                descricoesRostosDetectados = new String[rostosDetectados.Count];

                for (int i = 0; i < rostosDetectados.Count; ++i)
                {
                    DetectedFace rosto = rostosDetectados[i];

                    // Desenha o retângulo no rosto
                    drawingContext.DrawRectangle(
                        brush:     Brushes.Transparent,                            // Fundo transparente
                        pen:       new Pen(brush: Brushes.DarkBlue, thickness: 2), // Cor e espessura
                        rectangle: new Rect(
                            rosto.FaceRectangle.Left   * fatorRedimensionamento,
                            rosto.FaceRectangle.Top    * fatorRedimensionamento,
                            rosto.FaceRectangle.Width  * fatorRedimensionamento,
                            rosto.FaceRectangle.Height * fatorRedimensionamento
                        )                                                          // Dimensões
                    );

                    descricoesRostosDetectados[i] = DescricaoRosto(rosto); // Salva a descrição do rosto
                }

                drawingContext.Close();

                // Exibe a imagem com o retângulo ao redor do rosto.
                RenderTargetBitmap rostoComRetanguloBitmap = new RenderTargetBitmap(
                    (int)(bitmapOrigem.PixelWidth  * fatorRedimensionamento), // Largura
                    (int)(bitmapOrigem.PixelHeight * fatorRedimensionamento), // Altura
                    96,                                                       // DPI X
                    96,                                                       // DPI Y
                    PixelFormats.Pbgra32);                                    // Formato do pixel

                rostoComRetanguloBitmap.Render(visual);       // Redenriza imagem com retângulos
                ImagemRosto.Source = rostoComRetanguloBitmap; // Exibe a imagem no componente de imagem no xaml

                faceDescriptionStatusBar.Text = defaultStatusBarText; // Adiciona o texto no status no xaml
            }
        }

        /// <summary>
        /// Exibe a descrição da face quando o mouse está sobre um retângulo de face
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Evento do mouse</param>
        private void MouseSobreRetangulo(object sender, MouseEventArgs e)
        {
            if (rostosDetectados == null) return; // Se a chamada a Face API não foi finalizada

            // Pega a posição do ponteiro do mouse com relação a imagem
            Point mouseXY = e.GetPosition(ImagemRosto);

            ImageSource imagemOrigem  = ImagemRosto.Source;
            BitmapSource bitmapOrigem = (BitmapSource)imagemOrigem;

            // Ajuste de escala entre o tamanho real e o tamanho exibido.
            var escala = ImagemRosto.ActualWidth / (bitmapOrigem.PixelWidth / fatorRedimensionamento);

            // Verifica se a posição do mouse está sobre um retângulo de um rosto.
            bool mouseSobreRetangulo = false;

            for (int i = 0; i < rostosDetectados.Count; ++i)
            {
                FaceRectangle RetanguloRosto = rostosDetectados[i].FaceRectangle;
                double esquerda              = RetanguloRosto.Left   * escala;
                double cima                  = RetanguloRosto.Top    * escala;
                double largura               = RetanguloRosto.Width  * escala;
                double altura                = RetanguloRosto.Height * escala;

                // Exibe a descrição do rosto se o mouse estiver sobre o retângulo do rosto.
                if (mouseXY.X >= esquerda && mouseXY.X <= (esquerda + largura) &&
                    mouseXY.Y >= cima     && mouseXY.Y <= (cima     + altura ))
                {
                    faceDescriptionStatusBar.Text = descricoesRostosDetectados[i];
                    mouseSobreRetangulo = true;
                    break;
                }
            }

            // Caso o mouse não esteja sobre nenhum retângulo exibe essa mensagem
            if (!mouseSobreRetangulo) faceDescriptionStatusBar.Text = defaultStatusBarText;
        }

        /// <summary>
        /// Carrega o arquivo de imagem e chama DetectWithStreamAsync
        /// </summary>
        /// <param name="caminhoArquivoImagem">Caminho do arquivo de imagem da imagem local</param>
        /// <returns></returns>
        private async Task<IList<DetectedFace>> UploadEDetectaRosto(string caminhoArquivoImagem)
        {
            // Lista dos atributos do rosto
            IList<FaceAttributeType?> atributosRosto = new FaceAttributeType?[] {
                FaceAttributeType.Gender,   // Gênero
                FaceAttributeType.Age,      // Idade
                FaceAttributeType.Smile,    // Sorriso
                FaceAttributeType.Emotion,  // Emoção
                FaceAttributeType.Glasses,  // Óculos
                FaceAttributeType.Hair      // Cabelo
            };

            // Chama a Face API
            try
            {
                #pragma warning disable IDE0063 // Usar a instrução 'using' simples
                using (Stream streamArquivoImagem = File.OpenRead(caminhoArquivoImagem))
#pragma warning restore IDE0063 // Usar a instrução 'using' simples
                {
                    IList<DetectedFace> listaRostos = await faceAPIclient.Face.DetectWithStreamAsync(
                            image:                streamArquivoImagem, // Stream do arquivo de imagem local
                            returnFaceId:         true,                // Especifica o retorno do faceId
                            returnFaceLandmarks:  false,               // Especifica não retornar os pontos de referência do rosto
                            returnFaceAttributes: atributosRosto       // Lista de atributos que ele deve retornar
                    );

                    return listaRostos;
                }
            }
            // Caso ocorra um erro da Face API
            catch (APIErrorException f)
            {
                MessageBox.Show(f.Message, "Erro na Face API: ");
                return new List<DetectedFace>();
            }
            // Caso ocorra um erro genérico
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Erro");
                return new List<DetectedFace>();
            }
        }

        /// <summary>
        /// Cria uma string com todas as descrições do rosto
        /// </summary>
        /// <param name="rosto">Rosto com descrições para exibir</param>
        /// <returns></returns>
        private string DescricaoRosto(DetectedFace rosto)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Rosto: ");
            sb.Append(rosto.FaceAttributes.Gender); // Gênero
            sb.Append(", ");
            sb.Append(rosto.FaceAttributes.Age); // Idade
            sb.Append(", ");
            sb.Append(String.Format("Sorrindo {0:F1}%, ", rosto.FaceAttributes.Smile * 100));

            // Adiciona as emoções acima de 10%
            sb.Append("Emoção: ");
            Emotion pontosDeEmocao = rosto.FaceAttributes.Emotion;
            if (pontosDeEmocao.Anger >= 0.1f)     sb.Append( String.Format(" Raiva {0:F1}%, ",      pontosDeEmocao.Anger     * 100));
            if (pontosDeEmocao.Contempt >= 0.1f)  sb.Append( String.Format(" Desprezo {0:F1}%, ",   pontosDeEmocao.Contempt  * 100));
            if (pontosDeEmocao.Disgust >= 0.1f)   sb.Append( String.Format(" Desgosto {0:F1}%, ",   pontosDeEmocao.Disgust   * 100));
            if (pontosDeEmocao.Fear >= 0.1f)      sb.Append( String.Format(" Medo {0:F1}%, ",       pontosDeEmocao.Fear      * 100));
            if (pontosDeEmocao.Happiness >= 0.1f) sb.Append( String.Format(" Felicidade {0:F1}%, ", pontosDeEmocao.Happiness * 100));
            if (pontosDeEmocao.Neutral >= 0.1f)   sb.Append( String.Format(" Neutro {0:F1}%, ",     pontosDeEmocao.Neutral   * 100));
            if (pontosDeEmocao.Sadness >= 0.1f)   sb.Append( String.Format(" Tristeza {0:F1}%, ",   pontosDeEmocao.Sadness   * 100));
            if (pontosDeEmocao.Surprise >= 0.1f)  sb.Append( String.Format(" Surpresa {0:F1}%, ",   pontosDeEmocao.Surprise  * 100));

            sb.Append(rosto.FaceAttributes.Glasses); // Óculos
            sb.Append(", ");

            sb.Append("Cabelo: ");

            // Exibe calvice acima de 1%
            if (rosto.FaceAttributes.Hair.Bald >= 0.01f)
                sb.Append(String.Format("Calvo {0:F1}% ", rosto.FaceAttributes.Hair.Bald * 100));

            // Exibe todos os atributos de cor de cabelo acima de 10%
            IList<HairColor> coresCabelo = rosto.FaceAttributes.Hair.HairColor;
            foreach (HairColor corCabelo in coresCabelo)
            {
                if (corCabelo.Confidence >= 0.1f)
                {
                    sb.Append(corCabelo.Color.ToString());
                    sb.Append(String.Format(" {0:F1}% ", corCabelo.Confidence * 100));
                }
            }

            // Retorna a string completa
            return sb.ToString();
        }
    }
}
