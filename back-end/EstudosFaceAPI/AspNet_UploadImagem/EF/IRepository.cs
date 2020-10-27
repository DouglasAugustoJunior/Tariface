using System;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IList<TEntity> PegarTudo();
        TEntity PegarPorID(int ID);
        TEntity Salvar(TEntity objeto);
        TEntity Atualizar(TEntity objeto);
        bool Apagar(int ID);
        bool Apagar(TEntity objeto);
        void ApagarLista(List<TEntity> objetos);
    }
}
