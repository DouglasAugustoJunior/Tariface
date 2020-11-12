using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using AspNet_UploadImagem.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using AspNet_UploadImagem.Models.FaceAPI;
using AspNet_UploadImagem.Models.AzureStorage;
using AspNet_UploadImagem.EF.Repository.Grupos;
using AspNet_UploadImagem.EF.Repository.Imagens;
using AspNet_UploadImagem.EF.Repository.Usuarios;
using AspNet_UploadImagem.Handlers.HistoricoFolder;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using AspNet_UploadImagem.Models.HistoricoFolder.Enums;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AspNet_UploadImagem.Handlers
{
    public class ImagemHandler
    {

        #region Constantes
        private static   IWebHostEnvironment _environment;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IGrupoPessoaRepository _grupoPessoaRepository;
        private readonly IImagemRepository _imagemRepository;
        private readonly HistoricoHandler _historicoHandler;
        private readonly StorageConfig _storageConfig = null;
        private readonly FaceAPI _faceAPI = null;
        // O modelo de reconhecimento 3 é usado para extração de recursos(barba,oculos,humor...), o 1 para simplesmente reconhecer/detectar um rosto.
        // As chamadas de API para detecção que são usadas com Verify, Find Similar ou Identify devem compartilhar o mesmo modelo de reconhecimento.
        const string RECOGNITION_MODEL1 = RecognitionModel.Recognition01;
        //const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;
        private readonly IFaceClient _client;
        #endregion Constantes

        /// <summary>
        /// Construtor injetando Enviroment
        /// </summary>
        /// <param name="environment"></param>
        public ImagemHandler(
            IWebHostEnvironment environment,
            IGrupoPessoaRepository grupoPessoaRepository,
            IUsuarioRepository usuarioRepository,
            IImagemRepository imagemRepository,
            IOptions<StorageConfig> config,
            IOptions<FaceAPI> configFaceAPi,
            HistoricoHandler historicoHandler)
        {
            _environment           = environment;
            _usuarioRepository     = usuarioRepository;
            _grupoPessoaRepository = grupoPessoaRepository;
            _imagemRepository      = imagemRepository;
            _storageConfig         = config.Value;
            _faceAPI               = configFaceAPi.Value;
            _client                = Autenticacao(endpoint: _faceAPI.ENDPOINT, key: _faceAPI.SUBSCRIPTION_KEY);
            _historicoHandler = historicoHandler;

        }

        /// <summary>
        /// Monta string com status da aplicação
        /// </summary>
        /// <returns>Retorna o status formatado</returns>
        public string InformacoesExecucao()
        {
            string texto = " Web API TariFace - Em execução: " + DateTime.Now.ToLongTimeString()+ $"\n Ambiente: {_environment.EnvironmentName}";
            return texto;
        }

        /// <summary>
        /// Autenticação no servidor Azure
        /// </summary>
        /// <param name="endpoint">Endereço da aplicação</param>
        /// <param name="key">Chave da conta</param>
        /// <returns>Instância do cliente autenticado no Azure</returns>
        public static IFaceClient Autenticacao(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        /// <summary>
        /// Cadastra imagem no sistema
        /// </summary>
        /// <param name="arquivo">Arquivo</param>
        /// <param name="idUsuario">ID do usuário</param>
        /// <returns>String de sucesso ou falha</returns>
        public async Task<string> CadastraImagemAsync(IFormFile arquivo, int idUsuario)
        {
            if (arquivo != null && arquivo.Length > 0) // Se houver arquivo
            {
                try
                {
                    Stream streamArquivoImagem;      // Imagem com rostos
                    IList<DetectedFace> listaRostos; // Rostos detectados
                    var upload = new StorageHelper(_storageConfig);
                    var format = arquivo.FileName.Trim('\"');
                    JsonImage caminho = new JsonImage();

                    if (upload.EhImagemValida(format)) // Se imagem estiver num formato válido
                    {

                        // Verifica se existem rostos
                        using (streamArquivoImagem = arquivo.OpenReadStream())
                        {
                            listaRostos = await _client.Face.DetectWithStreamAsync(
                                    image: streamArquivoImagem, // Stream do arquivo de imagem local
                                    returnFaceId: true,         // Especifica o retorno do faceId
                                    returnFaceLandmarks: false  // Especifica não retornar os pontos de referência do rosto
                            );
                            if (listaRostos.Count <= 0) return "Nenhum rosto detectado na imagem, por favor, tente uma imagem melhor.";
                        }
                        streamArquivoImagem.Close();

                        // Faz o Upload
                        using (var stream = arquivo.OpenReadStream())
                        {
                            caminho = await upload.Upload(stream, arquivo.FileName);
                        }

                        Imagem imagem = new Imagem(idUsuario: idUsuario, url: caminho.Url, nome: arquivo.FileName);
                        _imagemRepository.Salvar(imagem);

                        string cadastroTreino = await CadastraRostosEndPointAsync(idUsuario: idUsuario); // Cadastra o rosto e treina no FaceAPI

                        return "Upload realizado com sucesso! "+cadastroTreino;
                    }
                    else { // Caso formato seja inválido
                        return "Formato de imagem não suportado!";
                    }

                }catch(APIErrorException f)
                {
                    return "Erro na FaceAPI ao reconhecer rostos: "+ f.Response.Content;
                }
                catch (Exception ex)
                {
                    return "Erro ao realizar upload: " + ex.ToString();
                }
            }
            else
            {
                return "Ocorreu uma falha no envio do arquivo...";
            }
        }

        /// <summary>
        /// Cadastra rosto somente com idUsuario
        /// </summary>
        /// <param name="idUsuario">ID do usuário para consultar na base</param>
        /// <returns></returns>
        public async Task<string> CadastraRostosEndPointAsync(int idUsuario)
        {
            Usuario usuario = _usuarioRepository.PegaComIncludes(ID: idUsuario);
            if (usuario.PersonId != Guid.Empty) return await AdicionaRostoAoPersonGroupPersonAsync(usuario: usuario);
            else
            {
                var criado = await CriaPersonGroupPersonAsync(idUsuario: idUsuario);
                if (criado) return "Imagem cadastrada e treinada com sucesso.";
                else return "Não foi possível treinar. Erro ao cadastrar PersonGroupPerson.";
            }
        }

        /// <summary>
        /// Adiciona os rostos das imagens ao PersonGroupPerson do usuário
        /// </summary>
        /// <param name="usuario">Objeto do usuário</param>
        /// <returns></returns>
        public async Task<string> AdicionaRostoAoPersonGroupPersonAsync(Usuario usuario)
        {
            try
            {
                IList<Imagem> listaImagens = usuario.Imagens;

                // Adiciona os rostos ao grupo de pessoa da pessoa conforme as imagem na pasta
                foreach (Imagem img in listaImagens)
                {
                    await Task.Delay(250);
                    PersistedFace face = await _client.PersonGroupPerson.AddFaceFromUrlAsync(
                        personGroupId: usuario.GrupoPessoa.IDAzure,
                        personId: usuario.PersonId,
                        url: img.URL,
                        img.Nome);
                }

                bool treinamento = await TreinaComImagensUsuarioAsync(grupoPessoasIDAzure: usuario.GrupoPessoa.IDAzure); // Treina o grupo de pessoas com o novo usuário
                if (treinamento) return "Rostos adicionados com sucesso! Treinamento realizado com sucesso!";
                else return "Rostos adicionados com sucesso! Ocorreu um erro no treinamento.";

            }
            catch (Exception e)
            {
                return "Falha ao adicionar rostos: " + e;
            }
        }

        /// <summary>
        /// Realiza treinamento com as imagens salvas do usuário
        /// </summary>
        /// <param name="grupoPessoasIDAzure">ID do usuário que vai realizar o treinamento das imagens</param>
        /// <returns>String de Sucesso ou Falha</returns>
        public async Task<bool> TreinaComImagensUsuarioAsync(string grupoPessoasIDAzure)
        {
            try
            {
                // Inicia o treino do grupo de pessoas
                await _client.PersonGroup.TrainAsync(grupoPessoasIDAzure);

                // Espera até o treinamento terminar
                while (true)
                {
                    await Task.Delay(1000);
                    var statusTreinamento = await _client.PersonGroup.GetTrainingStatusAsync(grupoPessoasIDAzure);
                    if (statusTreinamento.Status == TrainingStatusType.Succeeded) return true;
                    else if (statusTreinamento.Status == TrainingStatusType.Failed) throw new Exception("Treinamento falhou!" + statusTreinamento.Message);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao realizar o treinamento: " + e);
            }
        }

        /// <summary>
        /// Cria a coleção de reconhecimento do usuário
        /// </summary>
        /// <param name="idUsuario">ID do usuário que vai ter o treinamento das imagens</param>
        /// <returns>true ou Throw</returns>
        public async Task<bool> CriaPersonGroupPersonAsync(int idUsuario)
        {
            try
            {
                Usuario usuario = _usuarioRepository.PegaComIncludes(ID: idUsuario);
                Person pessoa = await _client.PersonGroupPerson.CreateAsync(personGroupId: usuario.GrupoPessoa.IDAzure, name: usuario.Nome); // Cria uma grupo a pessoa
                usuario.PersonId = pessoa.PersonId;
                _usuarioRepository.Atualizar(usuario);
                AdicionaRostoAoPersonGroupPersonAsync(usuario: usuario).Wait(); // Adiciona os rostos
                return true;
            }
            catch (APIErrorException ef)
            {
                throw new Exception("Ocorreu um erro na Face API ao criar PersonGroupPerson: " + ef.Response.Content);
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao criar: " + e);
            }
        }

        /// <summary>
        /// Recebe a imagem para realizar o reconhecimento facial
        /// </summary>
        /// <param name="arquivo">Arquivo com imagem do usuário</param>
        /// <returns>Pessoa encontrada</returns>
        public async Task<string> ReconhecimentoFacialAsync(IFormFile arquivo)
        {
            Stream streamArquivoImagem;                     // Imagem com rostos
            IList<DetectedFace> listaRostos;                // Rostos detectados
            IList<Guid?> rostoAlvoIds = new List<Guid?>();  // FaceIds dos rostos
            try
            {
                GrupoPessoa grupo = _grupoPessoaRepository.PegarPrimeiro(); // Pega o grupoPessoasID
                
                // Pega os rostos
                using (streamArquivoImagem = arquivo.OpenReadStream())
                {
                    listaRostos = await _client.Face.DetectWithStreamAsync(
                            image: streamArquivoImagem, // Stream do arquivo de imagem local
                            returnFaceId: true,         // Especifica o retorno do faceId
                            returnFaceLandmarks: false  // Especifica não retornar os pontos de referência do rosto
                    );
                }

                if(listaRostos.Count > 0)
                {
                    // Pega os FaceIds
                    foreach (DetectedFace rostoDetectadoImagem in listaRostos)
                    {
                        rostoAlvoIds.Add(rostoDetectadoImagem.FaceId);
                    }

                    // Encontra um rosto semelhante na lista de IDs. Comparando apenas o primeiro na lista para fins de teste.
                    IList<IdentifyResult> resultadosSimilares = await _client.Face.IdentifyAsync(
                        faceIds: rostoAlvoIds,         // rostos para validar
                        personGroupId: grupo.IDAzure,  // grupo de pessoas
                        largePersonGroupId: null,      // se usa o large group
                        maxNumOfCandidatesReturned: 1, // máximo de pessoas retornadas
                        confidenceThreshold: 0.5);     // mínimo de confiança

                    if (resultadosSimilares[0].Candidates.Count < 1) return "Nenhuma correspondência para esse rosto foi encontrada.";
                    else
                    {
                        Usuario usuario = _usuarioRepository.PegaPorPersonID(personID: resultadosSimilares[0].Candidates[0].PersonId);
                        if (RealizaDebito(usuario: usuario)) return "Liberado";
                        else return "Saldo insuficiente!";
                        //return $"Rosto é semelhante a pessoa {usuario.Nome} com confiança de: {resultadosSimilares[0].Candidates[0].Confidence * 100}%. "+ RealizaDebito(usuario: usuario);
                    }
                }
                else
                {
                    return "Nenhum rosto identificado na imagem.";
                }
                
            }
            catch(APIErrorException ef)
            {
                //throw new Exception("Ocorreu um erro na Face API ao reconhecer rosto: "+ ef.Response.Content);
                return "Ocorreu um erro na Face API ao reconhecer rosto";
            }catch(Exception e)
            {
                //throw new Exception("Ocorreu um erro ao ao realizar reconhecimento: ", e);
                return "Ocorreu um erro ao ao realizar reconhecimento";
            }
        }

        /// <summary>
        /// Desconta o valor da passagem do usuário
        /// </summary>
        /// <param name="usuario">Usuário identificado na catraca</param>
        /// <returns>Mensagem se foi liberado ou não</returns>
        private bool RealizaDebito(Usuario usuario)
        {
            if (usuario.Saldo >= Tarifas.Comum)
            {
                usuario.Saldo -= Tarifas.Comum;
                var result = _usuarioRepository.Atualizar(objeto: usuario);
                if (result != null)
                {
                    _historicoHandler.CriaHistorico(usuario: usuario, ETipoTransacao.Debito, EStatusTransacao.Concluida, Tarifas.Comum);
                    //return $"Catraca liberada, saldo atual R${usuario.Saldo}";
                    return true;
                }
                else
                {
                    _historicoHandler.CriaHistorico(usuario: usuario, ETipoTransacao.Debito, EStatusTransacao.Cancelada, Tarifas.Comum);
                    //return "Não foi possível realizar o débito, tente novamente.";
                    return false;
                }

            }
            else return false;
        }

        /// <summary>
        /// Cria o grupo de pessoas para realizar o reconhecimento facial
        /// </summary>
        /// <param name="grupoPessoas">Grupo de pessoas</param>
        /// <returns>String de Sucesso ou Falha</returns>
        public async Task<string> CriaGrupoPessoasAsync(string grupoPessoas)
        {
            try
            {
                string grupoPessoasID = Guid.NewGuid().ToString();
                await _client.PersonGroup.CreateAsync(personGroupId: grupoPessoasID, name: grupoPessoas, recognitionModel: RECOGNITION_MODEL1);
                GrupoPessoa grupo = new GrupoPessoa(idAzure: grupoPessoasID, nome: grupoPessoas);
                _grupoPessoaRepository.Salvar(grupo);
                return $"Grupo de pessoas {grupoPessoas} - {grupoPessoasID} criado com sucesso!";
            }
            catch (APIErrorException ef)
            {
                return "Ocorreu um erro na Face API ao criar GrupoPessoas: " + ef.Response.Content;
            }
            catch (Exception e)
            {
                return "Ocorreu um erro ao criar: " + e;
            }
        }

        /// <summary>
        /// Cadastra a imagem de perfil do usuário
        /// </summary>
        /// <param name="arquivo">Imagem</param>
        /// <param name="idUsuario">ID do usuário</param>
        /// <returns>Mensagem de sucesso ou falha dos processos</returns>
        internal async Task<string> CadastraImagemPerfilAsync(IFormFile arquivo, int idUsuario)
        {
            if (arquivo != null && arquivo.Length > 0) // Se houver arquivo
            {
                try
                {
                    Stream streamArquivoImagem;      // Imagem com rostos
                    IList<DetectedFace> listaRostos; // Rostos detectados
                    var upload = new StorageHelper(_storageConfig);
                    var format = arquivo.FileName.Trim('\"');
                    JsonImage caminho = new JsonImage();

                    if (upload.EhImagemValida(format)) // Se imagem estiver num formato válido
                    {

                        // Verifica se existem rostos
                        using (streamArquivoImagem = arquivo.OpenReadStream())
                        {
                            listaRostos = await _client.Face.DetectWithStreamAsync(
                                    image: streamArquivoImagem, // Stream do arquivo de imagem local
                                    returnFaceId: true,         // Especifica o retorno do faceId
                                    returnFaceLandmarks: false  // Especifica não retornar os pontos de referência do rosto
                            );
                            if (listaRostos.Count <= 0) return "Nenhum rosto detectado na imagem, por favor, tente uma imagem melhor.";
                        }
                        streamArquivoImagem.Close();

                        // Faz o Upload
                        using (var stream = arquivo.OpenReadStream())
                        {
                            caminho = await upload.Upload(stream, arquivo.FileName);
                        }

                        Imagem imagem = new Imagem(idUsuario: idUsuario, url: caminho.Url, nome: arquivo.FileName)
                        {
                            Perfil = true
                        };
                        Imagem imagemPerfilAtual = _imagemRepository.PegarImagemPerfil(idUsuario: idUsuario);
                        var result = _imagemRepository.Salvar(imagem);
                        if(result != null && imagemPerfilAtual != null) // Se salvou e existia uma imagem de perfil anterior
                        {
                            upload.Delete(imagemPerfilAtual.Nome).Wait();                  // Apaga a imagem no Storage
                            _imagemRepository.Apagar(objeto:imagemPerfilAtual); // Apaga imagem no banco
                        }

                        return "Upload da imagem de perfil realizado com sucesso!";
                    }
                    else
                    { // Caso formato seja inválido
                        return "Formato de imagem não suportado!";
                    }

                }
                catch (APIErrorException f)
                {
                    return "Erro na FaceAPI ao reconhecer rostos: " + f.Response.Content;
                }
                catch (Exception ex)
                {
                    return "Erro ao realizar upload: " + ex.ToString();
                }
            }
            else
            {
                return "Ocorreu uma falha no envio do arquivo...";
            }
        }

        #region Exclusões

        /// <summary>
        /// Exclui o grupo de pessoas que contêm os perfis dos usuários cadastrados
        /// </summary>
        /// <param name="grupoPessoas">Grupo de pessoas</param>
        /// <returns>String de Sucesso ou Falha</returns>
        public async Task<string> ExcluiGrupoPessoas(string grupoPessoas)
        {
            try
            {
                await _client.PersonGroup.DeleteAsync(personGroupId: grupoPessoas);
                return $"Grupo de pessoas {grupoPessoas} excluído com sucesso!";
            }
            catch (APIErrorException ef)
            {
                return "Ocorreu um erro na Face API ao excluir o GrupoPessoas: " + ef.Response.Content;
            }
            catch (Exception e)
            {
                return "Ocorreu um erro ao excluir: " + e;
            }
        }

        /// <summary>
        /// Exclui o PersonGroupPerson que contêm os padrões do usuário
        /// </summary>
        /// <param name="grupoPessoas">Grupo de pessoas do usuário</param>
        /// <param name="idUsuario">ID do usuário que vai ter o reconhecimento excluído</param>
        /// <returns>String de Sucesso ou Falha</returns>
        public async Task<string> ExcluiPersonGroupPerson(string grupoPessoas, int idUsuario)
        {
            try
            {
                Usuario usuario = _usuarioRepository.PegarPorID(idUsuario);
                await _client.PersonGroupPerson.DeleteAsync(personGroupId: grupoPessoas,personId: usuario.PersonId);
                return $"PersonGroupPerson do usuário {idUsuario} excluído com sucesso!";
            }
            catch (APIErrorException ef)
            {
                return "Ocorreu um erro na Face API ao excluir PersonGroupPerson: " + ef.Response.Content;
            }
            catch (Exception e)
            {
                return "Ocorreu um erro ao excluir: " + e;
            }
        }

        #endregion Exclusões
    }
}
