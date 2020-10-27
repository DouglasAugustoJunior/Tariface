namespace AspNet_UploadImagem.Models.AzureStorage
{
    public class StorageConfig
    {
        public string NomeConta { get; set; }
        public string ChaveConta { get; set; }
        public string NomeFila { get; set; }
        public string ImagemContainer { get; set; }
        public string MiniaturaContainer { get; set; }
    }
}
