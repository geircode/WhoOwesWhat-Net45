using System;
using System.Collections.Generic;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface ITransactionQuery
    {
        bool IsPersonUsedInAnyPosts(Guid personGuid);
        List<Post> GetPostsInUseByPersonGuid(Guid personGuid);
        List<Post> GetPostsInUseByPersonGuidAndUpdatedAfter(Guid personGuid, DateTime lastSynchronizedToServer);
    }
}