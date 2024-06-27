using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IPostQuery
    {
        Domain.DTO.Post GetPostByPostGuid(Guid postGuid);
        List<Domain.DTO.Post> GetPostsCreatedByAndUpdatedAfter(Guid personGuid, DateTime? lastSynchronizedToServer);
    }
}
