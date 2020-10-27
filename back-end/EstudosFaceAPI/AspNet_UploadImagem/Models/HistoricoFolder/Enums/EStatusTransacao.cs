using System.ComponentModel;

namespace AspNet_UploadImagem.Models.HistoricoFolder.Enums
{
    public enum EStatusTransacao
    {
        [Description("Concluída")]
        Concluida = 1,
        [Description("Cancelada")]
        Cancelada = 2
    }
}
