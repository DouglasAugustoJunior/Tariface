using System.ComponentModel;

namespace AspNet_UploadImagem.Models.HistoricoFolder.Enums
{
    public enum ETipoTransacao
    {
        [Description("Crédito")]
        Credito = 1,
        [Description("Débito")]
        Debito = 2
    }
}
