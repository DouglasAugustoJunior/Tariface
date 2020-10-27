namespace AspNet_UploadImagem.Models.AzureStorage
{
    public class JsonImage
    {
        public string Url { get; set; }

        public JsonImage() { }

        public JsonImage(string url) {
            Url = url;
        }
    }
}
