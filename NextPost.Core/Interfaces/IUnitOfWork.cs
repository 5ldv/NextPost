using NextPost.Core.Interfaces.Repositories;
using NextPost.Core.Interfaces.Repository;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<AppUser> Users { get; }
        IGenericRepository<AppRole> Roles { get; }
        IAuthorRepository Authors { get; }
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }

        Task<int> SaveChangesAsync();
    }
}
