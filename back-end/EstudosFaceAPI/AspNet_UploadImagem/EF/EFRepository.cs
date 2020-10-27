using System;
using System.Linq;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNet_UploadImagem.EF
{
    /// <summary>
    /// Classe genérica onde é feita a comunicação com o banco de dados
    /// </summary>
    /// <typeparam name="TEntity">Entidade de domínio, recebe modelo de uma tabela no banco</typeparam>
    public abstract class EFRepository<TEntity> : BaseRepository, IRepository<TEntity> where TEntity : EntidadeBase
    {
        protected readonly ContextoDBAplicacao Contexto;

        /// <summary>
        /// Construtor sobrecarregado nas repositorys
        /// </summary>
        /// <param name="contexto"></param>
        protected EFRepository(ContextoDBAplicacao contexto)
        {
            Contexto = contexto;
        }

        /// <summary>
        /// Encerra a conexão
        /// </summary>
        public void Dispose()
        {
            Contexto?.Dispose();
        }

        /// <summary>
        /// Busca registro por ID
        /// </summary>
        /// <param name="ID">ID do registro a ser buscado</param>
        /// <returns>Objeto encontrado</returns>
        public TEntity PegarPorID(int ID)
        {
            var context = Contexto.Set<TEntity>().AsQueryable();
            return context.AsNoTracking().FirstOrDefault(x => x.ID == ID);
        }

        /// <summary>
        /// Salva no banco
        /// </summary>
        /// <param name="objeto">Registro a ser salvo</param>
        /// <returns>Objeto salvo</returns>
        public TEntity Salvar(TEntity objeto)
        {
            Contexto.Set<TEntity>().Add(objeto);
            try
            {
                Contexto.SaveChanges();
                return objeto;
            }catch(Exception e)
            {
                throw new Exception("Ocorreu um erro ao salvar: ",e);
            }
        }

        /// <summary>
        /// Atualiza o registro no banco
        /// </summary>
        /// <param name="objeto">Registro a ser atualizado</param>
        /// <returns>Objeto após atualizar</returns>
        public TEntity Atualizar(TEntity objeto)
        {
            try
            {
                Contexto.Set<TEntity>().Attach(objeto);
                Contexto.Entry(objeto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                Contexto.SaveChanges();

                return objeto;
            }catch(Exception e)
            {
                throw new Exception("Ocorreu um erro ao atualizar: ", e);
            }
        }

        public bool Apagar(TEntity objeto)
        {
            try
            {
                Contexto.Set<TEntity>().Attach(objeto);
                Contexto.Entry(objeto).State = EntityState.Deleted;
                Contexto.SaveChanges();
                return true;
            }catch(Exception e)
            {
                throw new Exception("Erro ao excluír.", e);
            }
        }

        /// <summary>
        /// Apaga registros do banco
        /// </summary>
        /// <param name="ID">ID do registro a ser excluído</param>
        public bool Apagar(int ID)
        {
            try
            {
                var entity = PegarPorID(ID);
                if(entity != null)
                {
                    Contexto.Set<TEntity>().Remove(entity);
                    Contexto.SaveChanges();
                }
                return true;
            }catch(Exception e)
            {
                throw new Exception("Erro ao excluír.", e);
            }
        }

        /// <summary>
        /// Apaga uma lista de registros
        /// </summary>
        /// <param name="objetos">Lista dos objetos a serem excluídos</param>
        public void ApagarLista(List<TEntity> objetos)
        {
            Contexto.Set<TEntity>().RemoveRange(objetos);
            Contexto.SaveChanges();
        }

        /// <summary>
        /// Pega todos os registros disponíveis
        /// </summary>
        /// <returns>Lista de objetos</returns>
        public IList<TEntity> PegarTudo()
        {
            var context = Contexto.Set<TEntity>().AsQueryable();
            return context.AsNoTracking().ToList();
        }
    }
}
