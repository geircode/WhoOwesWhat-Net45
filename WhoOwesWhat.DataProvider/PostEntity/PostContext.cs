using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider.PostEntity
{
    public interface IPostContext
    {
        Entity.Post GetPost(int postId);
        Entity.Post GetPostByPostGuid(Guid postGuid);
        void Add(Post postDb);
        void SaveChanges();
        IQueryable<Post> GetPosts();

    }

    public class PostContext : IPostContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public PostContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public Post GetPost(int postId)
        {
            return _whoOwesWhatContext.GetPostSqlRepository().GetAll().SingleOrDefault(a => a.PostId == postId);
        }


        public Post GetPostByPostGuid(Guid postGuid)
        {
            return _whoOwesWhatContext.GetPostSqlRepository().GetAll().SingleOrDefault(a => a.PostGuid == postGuid);
        }

        public void Add(Post postDb)
        {
            _whoOwesWhatContext.GetPostSqlRepository().Add(postDb);
        }

        public void SaveChanges()
        {
            _whoOwesWhatContext.SaveChanges();
        }

        public IQueryable<Post> GetPosts()
        {
            var result = _whoOwesWhatContext.GetPostSqlRepository().GetAll();
            foreach (var post in result)
            {
                _whoOwesWhatContext.LoadCollection(post, a => a.Transactions);
            }
            return result;
        }
    }
}
