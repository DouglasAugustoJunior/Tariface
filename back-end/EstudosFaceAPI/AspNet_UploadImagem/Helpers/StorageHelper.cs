using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using AspNet_UploadImagem.Models.AzureStorage;

namespace AspNet_UploadImagem.Helpers
{
    public class StorageHelper
    {
        private readonly StorageConfig _storageConfiguracao = null;

        /// <summary>
        /// Construtor injetando configuração do storage
        /// </summary>
        /// <param name="storageConfiguracao"></param>
        public StorageHelper(StorageConfig storageConfiguracao)
        {
            _storageConfiguracao = storageConfiguracao;
        }

        /// <summary>
        /// Realiza o upload para o storage
        /// </summary>
        /// <param name="streamArquivo">Arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <returns>Endereço da imagem</returns>
        public async Task<JsonImage> Upload(Stream streamArquivo, string nomeArquivo)
        {
            JsonImage json = new JsonImage(url: await UploadArquivoStorage(streamArquivo: streamArquivo, nomeArquivo: nomeArquivo, storageConfiguracao: _storageConfiguracao));

            return json;
        }

        /// <summary>
        /// Cria credenciais para upload e chama método do storage para upload
        /// </summary>
        /// <param name="streamArquivo">Arquivo que vai subir para o Azure</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="storageConfiguracao">Configuração do Azure</param>
        /// <returns></returns>
        private static async Task<string> UploadArquivoStorage(Stream streamArquivo, string nomeArquivo, StorageConfig storageConfiguracao)
        {
            var storageCredenciais = new StorageCredentials(accountName: storageConfiguracao.NomeConta, keyValue: storageConfiguracao.ChaveConta);
            var storageConta       = new CloudStorageAccount(storageCredentials: storageCredenciais, useHttps: true);
            var blobCliente        = storageConta.CreateCloudBlobClient();
            var container          = blobCliente.GetContainerReference(containerName: storageConfiguracao.ImagemContainer);
            var blocoBlob          = container.GetBlockBlobReference(blobName: nomeArquivo);

            await blocoBlob.UploadFromStreamAsync(source: streamArquivo);

            return blocoBlob.SnapshotQualifiedStorageUri.PrimaryUri.ToString();
        }

        /// <summary>
        /// Apaga uma imagem do Storage
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo a ser excluído</param>
        /// <returns>True para sucesso e false para erro</returns>
        public async Task<bool> Delete(string nomeArquivo)
        {
            var storageCredenciais = new StorageCredentials(accountName: _storageConfiguracao.NomeConta, keyValue: _storageConfiguracao.ChaveConta);
            var storageConta = new CloudStorageAccount(storageCredentials: storageCredenciais, useHttps: true);
            var blobCliente = storageConta.CreateCloudBlobClient();
            var container = blobCliente.GetContainerReference(containerName: _storageConfiguracao.ImagemContainer);
            var blob = container.GetBlockBlobReference(nomeArquivo);
            try
            {
                bool retorno = await blob.DeleteIfExistsAsync();
                if (retorno) return true;
                else return false;
            }catch(Exception e)
            {
                throw new Exception("Erro ao excluír imagem. " + e);
            }
        }

        /// <summary>
        /// Valida se a imagem está num formato aceito
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo para validar</param>
        /// <returns></returns>
        public bool EhImagemValida(string nomeArquivo)
        {
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // Formatos aceitos
            return formats.Any(item => nomeArquivo.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}