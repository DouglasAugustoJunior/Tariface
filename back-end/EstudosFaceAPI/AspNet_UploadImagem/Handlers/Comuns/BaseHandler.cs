using AspNet_UploadImagem.Commands;
using FluentValidator;

namespace AspNet_UploadImagem.Handlers
{
    public class BaseHandler<THandler> : Notifiable, ICommandHandler<THandler> where THandler : ICommand
    {
    }
}
