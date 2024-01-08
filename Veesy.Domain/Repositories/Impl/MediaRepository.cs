using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories.Impl;

public class MediaRepository : RepositoryBase<Media>, IMediaRepository 
{
    public MediaRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}