using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace face_quickstart
{
    class Program
    {
        #region Constantes
        // URL para blobs das imagens
        //const string IMAGE_BASE_URL = "https://csdx.blob.core.windows.net/resources/Face/Images/";

        // O modelo de reconhecimento 3 é usado para extração de recursos(barba,oculos,humor...), o 1 para simplesmente reconhecer/detectar um rosto.
        // As chamadas de API para detecção que são usadas com Verify, Find Similar ou Identify devem compartilhar o mesmo modelo de reconhecimento.
        //const string RECOGNITION_MODEL1 = RecognitionModel.Recognition01;
        //const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;

        static string grupoPessoasID = null; // Recebe o ID do grupo de pessoas

        // Uma foto de um grupo de pessoas que inclui algumas das pessoas que você deseja identificar em seu dicionário.
        static readonly string nomeArquivoImagemOrigem = "identification1.jpg";
        #endregion Constantes

        static void Main()
        {
            string SUBSCRIPTION_KEY = Environment.GetEnvironmentVariable("FACE_SUBSCRIPTION_KEY"); // Chave Azure
            string ENDPOINT         = Environment.GetEnvironmentVariable("FACE_ENDPOINT");         // Endereço Azure
            IFaceClient client      = Autenticacao(endpoint: ENDPOINT, key: SUBSCRIPTION_KEY);
            ExcluiGrupoPessoas(client: client, grupoPessoasID: grupoPessoasID).Wait();
            //DetectaRostoDetalhado(client: client, url: IMAGE_BASE_URL, RECOGNITION_MODEL3: RECOGNITION_MODEL3).Wait(); // Detectar - obter detalhes de rostos.
            //BuscaSimilar(client: client, url: IMAGE_BASE_URL, RECOGNITION_MODEL1: RECOGNITION_MODEL1).Wait();
            ///CriaGruposPessoas(client: client, url: IMAGE_BASE_URL, RECOGNITION_MODEL1: RECOGNITION_MODEL1).Wait();
        }
        
        /// <summary>
        /// Autenticação no servidor Azure
        /// </summary>
        /// <param name="endpoint">Endereço da aplicação</param>
        /// <param name="key">Chave da conta</param>
        /// <returns></returns>
        public static IFaceClient Autenticacao(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        /// <summary>
        /// Detecta recursos de rostos e os identifica.
        /// </summary>
        /// <param name="client">Autenticação</param>
        /// <param name="url">URL das imagens</param>
        /// <param name="RECOGNITION_MODEL3">Modelo de reconhecimento</param>
        /// <returns></returns>
        public static async Task DetectaRostoDetalhado(IFaceClient client, string url, string RECOGNITION_MODEL3)
        {
            Console.WriteLine("======== Detector Rostos ========");
            Console.WriteLine();

            // Lista de nomes das imagens
            List<string> listaNomesImagens = new List<string>
                    {
                        "detection1.jpg",    // mulher solteira com óculos
                        "detection2.jpg", // (opcional: homem solteiro)
                        "detection3.jpg", // (opcional: único trabalhador da construção civil)
                        "detection4.jpg", // (opcional: 3 pessoas no café, 1 desfocada)
                        "detection5.jpg", // família: mulher, criança e homem
                        "detection6.jpg"  // casal de idosos, masculino feminino   
                    };

            foreach (var nomeArquivoImagem in listaNomesImagens)
            {
                IList<DetectedFace> rostosDetectados;

                // Detecta rostos com todos os atributos do url da imagem
                rostosDetectados = await client.Face.DetectWithUrlAsync(
                    $"{url}{nomeArquivoImagem}",
                    returnFaceAttributes: new List<FaceAttributeType?> {
                        FaceAttributeType.Accessories,
                        FaceAttributeType.Age,
                        FaceAttributeType.Blur,
                        FaceAttributeType.Emotion,
                        FaceAttributeType.Exposure,
                        FaceAttributeType.FacialHair,
                        FaceAttributeType.Gender,
                        FaceAttributeType.Glasses,
                        FaceAttributeType.Hair,
                        FaceAttributeType.HeadPose,
                        FaceAttributeType.Makeup,
                        FaceAttributeType.Noise,
                        FaceAttributeType.Occlusion,
                        FaceAttributeType.Smile },
                    recognitionModel: RECOGNITION_MODEL3
                    );

                Console.WriteLine($"{rostosDetectados.Count} rosto(s) detectado da imagem `{nomeArquivoImagem}`.");
                Console.WriteLine();

                // Analisa e imprime todos os atributos de cada rosto detectado de cada imagem
                for(int indiceRosto = 1; indiceRosto <= rostosDetectados.Count; indiceRosto++)
                {
                    var rosto = rostosDetectados[indiceRosto-1];
                    Console.WriteLine($"Atributos do {indiceRosto}° rosto de {nomeArquivoImagem}:");
                    Console.WriteLine();

                    #region caixa delimitadora do rosto
                    Console.WriteLine($"Retângulo: | Esquerda: {rosto.FaceRectangle.Left} | Cima: {rosto.FaceRectangle.Top} | Largura: {rosto.FaceRectangle.Width} | Altura: {rosto.FaceRectangle.Height}");
                    #endregion caixa delimitadora do rosto

                    #region Emoções do rosto
                    string tipoEmocao = string.Empty;
                    double valorEmocao = 0.0;
                    Emotion emocao = rosto.FaceAttributes.Emotion;

                    if (emocao.Anger > valorEmocao) { valorEmocao = emocao.Anger; tipoEmocao = "Raiva"; }
                    if (emocao.Contempt > valorEmocao) { valorEmocao = emocao.Contempt; tipoEmocao = "Desprezo"; }
                    if (emocao.Disgust > valorEmocao) { valorEmocao = emocao.Disgust; tipoEmocao = "Nojo"; }
                    if (emocao.Fear > valorEmocao) { valorEmocao = emocao.Fear; tipoEmocao = "Medo"; }
                    if (emocao.Happiness > valorEmocao) { valorEmocao = emocao.Happiness; tipoEmocao = "Felicidade"; }
                    if (emocao.Neutral > valorEmocao) { valorEmocao = emocao.Neutral; tipoEmocao = "Neutro"; }
                    if (emocao.Sadness > valorEmocao) { valorEmocao = emocao.Sadness; tipoEmocao = "Tristeza"; }
                    if (emocao.Surprise > valorEmocao) { tipoEmocao = "Surpresa"; }
                    Console.WriteLine($"Emoção: {tipoEmocao}");
                    #endregion Emoções do rosto

                    #region Cor do cabelo
                    Hair cabelo = rosto.FaceAttributes.Hair;
                    string cor = null;
                    double confiancaMinimaCorCabelo = 0.0f;

                    if (cabelo.HairColor.Count == 0)
                    { // Se não houver cor no cabelo
                        if (cabelo.Invisible) cor = "Invisível";
                        else cor = "Careca";
                    }

                    foreach (HairColor corCabelo in cabelo.HairColor)
                    {
                        if (corCabelo.Confidence <= confiancaMinimaCorCabelo) continue;

                        confiancaMinimaCorCabelo = corCabelo.Confidence;
                        cor = corCabelo.Color.ToString();
                    }
                    Console.WriteLine($"Cor Cabelo: {cor}");
                    #endregion Cor do cabelo

                    #region Acessórios do rosto
                    List<Accessory> listaAcessorios = (List<Accessory>)rosto.FaceAttributes.Accessories;
                    int             contador        = rosto.FaceAttributes.Accessories.Count;
                    string          acessorio;
                    string[]        acessorioArray  = new string[contador];

                    if (contador == 0) acessorio = "Nenhum acessório"; // Se o contador for zero não existem acessórios
                    else
                    {
                        for (int i = 0; i < contador; ++i) { acessorioArray[i] = listaAcessorios[i].Type.ToString(); } // Adiciona uma posição no Array para cada acessório
                        acessorio = string.Join(", ", acessorioArray); // Junta todos os acessórios num único string
                    }
                    Console.WriteLine($"Acessórios: {acessorio}");
                    #endregion Acessórios do rosto

                    #region Outros atributos do rosto
                    Console.WriteLine($"Sorriso: { (rosto.FaceAttributes.Smile > 0 ? "Sim" : "Não") }");
                    Console.WriteLine($"Idade: {rosto.FaceAttributes.Age}");
                    Console.WriteLine($"Gênero: {rosto.FaceAttributes.Gender}");
                    Console.WriteLine($"Barba: {string.Format("{0}", rosto.FaceAttributes.FacialHair.Moustache + rosto.FaceAttributes.FacialHair.Beard + rosto.FaceAttributes.FacialHair.Sideburns > 0 ? "Sim" : "Não")}");
                    Console.WriteLine($"Maquiagem: {string.Format("{0}", (rosto.FaceAttributes.Makeup.EyeMakeup || rosto.FaceAttributes.Makeup.LipMakeup) ? "Sim" : "Não")}");
                    Console.WriteLine($"Óculos: {rosto.FaceAttributes.Glasses}");
                    Console.WriteLine($"Postura da cabeça: {string.Format("| Pitch: {0} | Roll: {1} | Yaw: {2}", Math.Round(rosto.FaceAttributes.HeadPose.Pitch, 2), Math.Round(rosto.FaceAttributes.HeadPose.Roll, 2), Math.Round(rosto.FaceAttributes.HeadPose.Yaw, 2))}");
                    Console.WriteLine($"Nível de desfoque: {rosto.FaceAttributes.Blur.BlurLevel}");
                    Console.WriteLine($"Exposição: {rosto.FaceAttributes.Exposure.ExposureLevel}");
                    Console.WriteLine($"Ruído: {rosto.FaceAttributes.Noise.NoiseLevel}");
                    Console.WriteLine($"Obstrução:" +
                                      $" {string.Format(" | Olhos obstruídos: {0}", rosto.FaceAttributes.Occlusion.EyeOccluded ? "Sim" : "Não")}" +
                                      $" {string.Format(" | Testa obstruída: {0}", rosto.FaceAttributes.Occlusion.ForeheadOccluded ? "Yes" : "Não")}" +
                                      $" {string.Format(" | Boca obstruída: {0}", rosto.FaceAttributes.Occlusion.MouthOccluded ? "Yes" : "Não")}");
                    #endregion Outros atributos do rosto

                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Compara rostos sem trazer detalhes de cada objeto
        /// </summary>
        /// <param name="client">Autenticação</param>
        /// <param name="url">URL das imagens</param>
        /// <param name="RECOGNITION_MODEL1">Modelo de reconhecimento</param>
        /// <returns>Lista de rostos detectados</returns>
        private static async Task<List<DetectedFace>> DetectaRosto(IFaceClient client, string url, string RECOGNITION_MODEL1)
        {
            // Detecta rostos no URL da imagem. Como está apenas reconhecendo, usar o modelo de reconhecimento 1.
            IList<DetectedFace> rostosDetectados = await client.Face.DetectWithUrlAsync(url, recognitionModel: RECOGNITION_MODEL1);
            Console.WriteLine($"{rostosDetectados.Count} rostos(s) detectados da imagem `{Path.GetFileName(url)}`");
            return rostosDetectados.ToList();
        }

        /// <summary>
        /// Pega uma imagem e procura um rosto semelhante em outra
        /// </summary>
        /// <param name="client">Autenticação</param>
        /// <param name="url">URL das imagens</param>
        /// <param name="RECOGNITION_MODEL1">Modelo de reconhecimento</param>
        /// <returns></returns>
        public static async Task BuscaSimilar(IFaceClient client, string url, string RECOGNITION_MODEL1)
        {
            Console.WriteLine("======== Busca Similar ========");
            Console.WriteLine();

            List<string> nomesArquivosImagensDestino = new List<string>
                        {
                            "Family1-Dad1.jpg",
                            "Family1-Daughter1.jpg",
                            "Family1-Mom1.jpg",
                            "Family1-Son1.jpg",
                            "Family2-Lady1.jpg",
                            "Family2-Man1.jpg",
                            "Family3-Lady1.jpg",
                            "Family3-Man1.jpg"
                        };

            string nomeArquivoImagemOrigem = "findsimilar.jpg";
            IList<Guid?> rostoAlvoIds      = new List<Guid?>();

            foreach (var nomeArquivoImagemDestino in nomesArquivosImagensDestino)
            {
                // Detecta rostos no URL da imagem de destino.
                var rostos = await DetectaRosto(client, $"{url}{nomeArquivoImagemDestino}", RECOGNITION_MODEL1);
                // Adiciona o ID do rosto detectado a lista
                rostoAlvoIds.Add(rostos[0].FaceId.Value);
            }

            // Detecta rostos no URL da imagem de origem.
            IList<DetectedFace> rostosDetectados = await DetectaRosto(client, $"{url}{nomeArquivoImagemOrigem}", RECOGNITION_MODEL1);
            Console.WriteLine();

            // Encontra rosto(s) semelhante(s) na lista de IDs. Comparando apenas o primeiro na lista para fins de teste.
            IList<SimilarFace> resultadosSimilares = await client.Face.FindSimilarAsync(rostosDetectados[0].FaceId.Value, null, null, rostoAlvoIds);

            foreach (var resultadoSimilar in resultadosSimilares)
            {
                Console.WriteLine($"Rosto de {nomeArquivoImagemOrigem} & ID:{resultadoSimilar.FaceId} são semelhantes com confiança de: {resultadoSimilar.Confidence}.");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Cria um grupo de pessoas, depois cria um PersonGroupPerson para cada pessoa e adiciona rostos a esse PersonGroupPerson
        /// </summary>
        /// <param name="client">Autenticação</param>
        /// <param name="url">URL das imagens</param>
        /// <param name="RECOGNITION_MODEL1">Modelo de reconhecimento</param>
        /// <returns></returns>
        public static async Task CriaGruposPessoas(IFaceClient client, string url, string RECOGNITION_MODEL1)
        {
            // Cria um dicionário para as imagens, agrupando outras semelhantes na mesma chave.
            Dictionary<string, string[]> dicionarioPessoas = new Dictionary<string, string[]>
                { 
                    { "Family1-Dad",      new[] { "Family1-Dad1.jpg",      "Family1-Dad2.jpg"      } },
                    { "Family1-Mom",      new[] { "Family1-Mom1.jpg",      "Family1-Mom2.jpg"      } },
                    { "Family1-Son",      new[] { "Family1-Son1.jpg",      "Family1-Son2.jpg"      } },
                    { "Family1-Daughter", new[] { "Family1-Daughter1.jpg", "Family1-Daughter2.jpg" } },
                    { "Family2-Lady",     new[] { "Family2-Lady1.jpg",     "Family2-Lady2.jpg"     } },
                    { "Family2-Man",      new[] { "Family2-Man1.jpg",      "Family2-Man2.jpg"      } }
                };

            grupoPessoasID = Guid.NewGuid().ToString(); // Cria uma grupo de pessoas
            await client.PersonGroup.CreateAsync(personGroupId: grupoPessoasID, grupoPessoasID, recognitionModel: RECOGNITION_MODEL1);
            Console.WriteLine($"Grupo de pessoas criado com o ID: ({grupoPessoasID}).");

            // Os rostos semelhantes serão agrupados em um grupo de uma única pessoa.
            foreach (var rostoAgrupado in dicionarioPessoas.Keys)
            {
                // Limit TPS
                await Task.Delay(250);
                Person pessoa = await client.PersonGroupPerson.CreateAsync(personGroupId: grupoPessoasID, name: rostoAgrupado); // Cria uma grupo para cada pessoa da lista
                Console.WriteLine($"Criado grupo de pessoas para pessoa: '{rostoAgrupado}'.");

                // Adiciona os rostos ao grupo de pessoa de cada pessoa conforme a imagem no dicionario
                foreach (var imagemSimilar in dicionarioPessoas[rostoAgrupado])
                {
                    PersistedFace face = await client.PersonGroupPerson.AddFaceFromUrlAsync(
                        personGroupId: grupoPessoasID,
                        personId:      pessoa.PersonId,
                        url:           $"{url}{imagemSimilar}",
                        imagemSimilar);
                    Console.WriteLine($"Adicionado rosto '{face.PersistedFaceId}' ao grupo de pessoas '{rostoAgrupado}' da imagem `{imagemSimilar}`");
                }
            }

            Treinamento(client: client,url: url, RECOGNITION_MODEL1: RECOGNITION_MODEL1).Wait();
        }

        /// <summary>
        /// Realiza o treinamento com o grupo de pessoas criado
        /// </summary>
        /// <param name="client">Autenticação</param>
        /// <param name="url">URL das imagens</param>
        /// <param name="RECOGNITION_MODEL1">Modelo de reconhecimento</param>
        public static async Task Treinamento(IFaceClient client, string url, string RECOGNITION_MODEL1)
        {
            // Inicia o treino do grupo de pessoas
            await client.PersonGroup.TrainAsync(grupoPessoasID);
            Console.WriteLine();
            Console.WriteLine($"Treino do grupo de pessoas {grupoPessoasID}.");

            // Espera até o treinamento terminar
            while (true)
            {
                await Task.Delay(1000);
                var statusTreinamento = await client.PersonGroup.GetTrainingStatusAsync(grupoPessoasID);
                Console.WriteLine($"Status treinamento: {statusTreinamento.Status}.");
                if (statusTreinamento.Status == TrainingStatusType.Succeeded) { break; }
            }

            Identifica(client: client, url: url, RECOGNITION_MODEL1: RECOGNITION_MODEL1).Wait();
        }

        /// <summary>
        /// Identifica rostos na imagem comparando com o grupo de pessoas criado e treinado
        /// </summary>
        /// <param name="client">Autenticação</param>
        /// <param name="url">URL das imagens</param>
        /// <param name="RECOGNITION_MODEL1">Modelo de reconhecimento</param>
        public static async Task Identifica(IFaceClient client, string url, string RECOGNITION_MODEL1)
        {
            List<Guid?> listaIdsRostosDetectadosImagemOrigem = new List<Guid?>();
            // Detecta rostos na imagem de origem
            List<DetectedFace> rostosDetectados = await DetectaRosto(client: client, url: $"{url}{nomeArquivoImagemOrigem}", RECOGNITION_MODEL1: RECOGNITION_MODEL1);

            // Adiciona rostos detectados a lista de ids de rostos detectados
            foreach (var rostoDetectado in rostosDetectados) { listaIdsRostosDetectadosImagemOrigem.Add(rostoDetectado.FaceId.Value); }

            // Identifica rostos no grupo de pessoas e gera uma lista
            IList<IdentifyResult> resultadosIdentificados = await client.Face.IdentifyAsync(faceIds: listaIdsRostosDetectadosImagemOrigem, personGroupId: grupoPessoasID);

            foreach (var resultadoIdentificado in resultadosIdentificados)
            {
                Person pessoa = await client.PersonGroupPerson.GetAsync(personGroupId: grupoPessoasID, personId: resultadoIdentificado.Candidates[0].PersonId);
                Console.WriteLine(
                    $"A pessoa '{pessoa.Name}' teve o rosto identificado na imagem: {nomeArquivoImagemOrigem},"+
                    $" ID Rosto: {resultadoIdentificado.FaceId}," +
                    $" Confiança: {resultadoIdentificado.Candidates[0].Confidence * 100}%.");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Excluí o grupo de pessoas
        /// </summary>
        /// <param name="client">Autenticação</param>
        /// <param name="grupoPessoasID">Id do grupo de pessoas que será excluído</param>
        /// <returns></returns>
        public static async Task ExcluiGrupoPessoas(IFaceClient client, string grupoPessoasID)
        {
            //string[] g = { "4b325fbb-2941-448b-bc14-31cd575faf07" };
            string[] g = { grupoPessoasID };

            foreach(string grupo in g)
            {
                await client.PersonGroup.DeleteAsync(grupo);
                Console.WriteLine($"Grupo de pessoas {grupo} excluído com sucesso.");
            }
        }
    }
}


