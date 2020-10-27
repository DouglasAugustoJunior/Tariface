using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DeteccaoFacialComURLImagemConsole
{
    class Program
    {
        #region Constantes
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable("FACE_SUBSCRIPTION_KEY");             // Chave Azure
        private static readonly string faceEndpoint    = Environment.GetEnvironmentVariable("FACE_ENDPOINT")+ "face/v1.0/detect"; // Endereço Azure
        #endregion Constantes

        static void Main()
        {
            Console.WriteLine("Detector de rostos:");
            Console.WriteLine("Insira o caminho para uma imagem com rostos que você deseja analisar: ");
            string caminhoArquivoImagem = Console.ReadLine();

            if (File.Exists(caminhoArquivoImagem))
            {
                try
                {
                    SolicitaAnaliseImagem(caminhoArquivoImagem);
                    Console.WriteLine("\nAguarde um momento enquanto a imagem é processada... .\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.Message + "\nPressione Enter para sair...\n");
                }
            }
            else
            {
                Console.WriteLine("\nCaminho inválido.\nPressione Enter para sair...\n");
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Envia a imagem para Face API e exibe o retorno
        /// </summary>
        /// <param name="caminhoArquivoImagem">Caminho da imagem a ser analisada</param>
        static async void SolicitaAnaliseImagem(string caminhoArquivoImagem)
        {
            HttpClient cliente = new HttpClient();
            HttpResponseMessage resposta;

            cliente.DefaultRequestHeaders.Add( name: "Ocp-Apim-Subscription-Key", value: subscriptionKey); // Cabeçalho da solicitação

            string parametrosSolicitacao =
                "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise"; // 3° Parâmetro opcional

            string uri = faceEndpoint + "?" + parametrosSolicitacao; // Monta o URI para a solicitacao da Face API

            // Corpo da solicitação, recebe a imagem JPEG armazenada localmente
            byte[] corpoDadosBytes = PegaArrayBytesImagem(caminhoArquivoImagem);

            using (ByteArrayContent content = new ByteArrayContent(corpoDadosBytes))
            {
                // Este exemplo usa o tipo de conteúdo "application/octet-stream"
                // Os outros tipos de conteúdo que você pode usar são "application/json"
                // e "multipart/form-data"
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                resposta = await cliente.PostAsync(uri, content); // Executa a chamada na Face API

                string jsonResposta = await resposta.Content.ReadAsStringAsync(); // Pega o JSON de resposta da API

                Console.WriteLine("\nResposta:\n");
                Console.WriteLine(FormataJSON(jsonResposta));
                Console.WriteLine("\nPressione Enter para sair...");
            }
        }

        /// <summary>
        /// Retorna a matriz de bytes da imagem
        /// </summary>
        /// <param name="caminhoArquivoImagem">Caminho da imagem</param>
        /// <returns></returns>
        static byte[] PegaArrayBytesImagem(string caminhoArquivoImagem)
        {
            using (FileStream arquivoStream = new FileStream(caminhoArquivoImagem, FileMode.Open, FileAccess.Read))
            {
                BinaryReader leitorDeBinario = new BinaryReader(arquivoStream);
                return leitorDeBinario.ReadBytes((int)arquivoStream.Length);
            }
        }

        /// <summary>
        /// Formata a string JSON fornecida adicionando quebras de linha e recuos.
        /// </summary>
        /// <param name="json">JSON para formatar</param>
        /// <returns></returns>
        static string FormataJSON(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }
    }
}