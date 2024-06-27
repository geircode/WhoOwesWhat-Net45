using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.Domain.Interfaces
{
    public interface ISyncPostsCommand
    {
        void SyncPosts(string username, List<SyncPostModel> syncPostModels);
    }     
    
    public interface ISyncPostCommand
    {
        void SyncPost(string username, SyncPostModel syncPostModel);
    }

    public interface ISyncPostQuery
    {
        List<Post> GetPostConflicts(string username, List<SyncPostModel> syncPostModels);
        List<Post> GetUnsynchronizedPosts(string username, DateTime? lastSynchronizedToServer);
        List<Post> GetUnsynchronizedPostsWithoutPostConflicts(string username, DateTime? lastSynchronizedToServer, List<Post> postConflicts);
        List<Post> GetFilterPostConflictsFromUnsynchronizedPosts(List<Post> postConflicts, List<Post> unsynchronizedPosts);
        bool IsEqualServerVersion(SyncPostModel syncPostModel, DTO.Post post);
    }


    public interface IDeletePostLogic
    {
        void DeletePost(Guid postGuid, Guid ownerGuid);
        void UnDeletePost(Guid postGuid, Guid ownerGuid);
    }

}
