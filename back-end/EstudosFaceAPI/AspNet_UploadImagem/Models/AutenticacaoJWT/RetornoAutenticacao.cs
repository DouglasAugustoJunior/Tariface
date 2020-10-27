namespace AspNet_UploadImagem.Models.AutenticacaoJWT
{
    public class RetornoAutenticacao
    {
        public string Token { get; set; }
        public Usuario usuario { get; set; }
    }
}
