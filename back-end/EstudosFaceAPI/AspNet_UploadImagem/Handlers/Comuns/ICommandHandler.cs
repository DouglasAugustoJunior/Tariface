using AspNet_UploadImagem.Commands;

namespace AspNet_UploadImagem.Handlers
{
    internal interface ICommandHandler<T> where T : ICommand
    {
    }
}