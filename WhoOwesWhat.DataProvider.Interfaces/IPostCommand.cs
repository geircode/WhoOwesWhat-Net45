using System;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IPostCommand
    {
        void SavePost(Domain.DTO.Post post);
        void DeletePost(Guid postGuid);
        void UnDeletePost(Guid postGuid);
    }
}