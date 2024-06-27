using System;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.GroupEntity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.Domain.DTO;
using Group = WhoOwesWhat.DataProvider.Entity.Group;
using Post = WhoOwesWhat.DataProvider.Entity.Post;

namespace WhoOwesWhat.DataProvider.PostEntity
{
    public class PostCommand : IPostCommand
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IPostDataProviderLogic _postDataProviderLogic;
        private readonly IPersonContext _personContext;
        private readonly IGroupContext _groupContext;
        private readonly IPostContext _postContext;

        public PostCommand(IWhoOwesWhatContext whoOwesWhatContext, ILog log
            , IPostDataProviderLogic postDataProviderLogic
            , IPersonContext personContext
            , IGroupContext groupContext
            , IPostContext postContext
            )
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _postDataProviderLogic = postDataProviderLogic;
            _personContext = personContext;
            _groupContext = groupContext;
            _postContext = postContext;
        }

        /// <summary>
        /// Will either create a new Post or Undelete an existing post
        /// </summary>
        /// <param name="post"></param>
        public void SavePost(Domain.DTO.Post post)
        {
            var createdByPerson = _personContext.GetPersonByPersonGuid(post.CreatedByPersonGuid);
            var lastUpdatedByPerson = _personContext.GetPersonByPersonGuid(post.LastUpdatedByPersonGuid);
            Guard.NotNull(() => createdByPerson, createdByPerson);
            Guard.NotNull(() => lastUpdatedByPerson, lastUpdatedByPerson);

            Group group = null;
            if (post.GroupGuid.HasValue)
            {
                group = _groupContext.GetGroupByGroupGuid(post.GroupGuid.Value);
                Guard.NotNull(() => group, group);
            }

            Post postDb = _postContext.GetPostByPostGuid(post.PostGuid);
            if (postDb == null)
            {
                postDb = new Entity.Post();
                postDb.PostGuid = post.PostGuid;
                postDb.CreatedBy = createdByPerson;

                _postContext.Add(postDb);
            }

            postDb.PurchaseDate = post.PurchaseDate;
            postDb.Description = post.Description;
            postDb.TotalCost = post.TotalCost;
            postDb.Iso4217CurrencyCode = post.Iso4217CurrencyCode;
            postDb.Version = post.Version;
            postDb.VersionUpdated = post.VersionUpdated;
            postDb.IsDeleted = post.IsDeleted;
            postDb.Comment = post.Comment;
            postDb.LastUpdated = post.LastUpdated;
            postDb.Created = post.Created;

            postDb.Group = group;
            postDb.LastUpdatedBy = lastUpdatedByPerson;

            ValidatePostEntity(postDb);
            _whoOwesWhatContext.SaveChanges();
        }

        private void ValidatePostEntity(Post post)
        {
            Guard.NotNull(() => post.PostGuid, post.PostGuid);
            Guard.NotNull(() => post.PurchaseDate, post.PurchaseDate);
            Guard.NotNull(() => post.Description, post.Description);
            Guard.NotNull(() => post.TotalCost, post.TotalCost);
            Guard.NotNull(() => post.Iso4217CurrencyCode, post.Iso4217CurrencyCode);
            Guard.NotNull(() => post.Version, post.Version);
            Guard.NotNull(() => post.VersionUpdated, post.VersionUpdated);
            Guard.NotNull(() => post.IsDeleted, post.IsDeleted);
            if (post.Group != null)
            {
                Guard.NotNull(() => post.Group.GroupGuid, post.Group.GroupGuid);    
            }
            
            Guard.NotNull(() => post.LastUpdatedBy, post.LastUpdatedBy);
            Guard.NotNull(() => post.LastUpdated, post.LastUpdated);
            Guard.NotNull(() => post.Created, post.Created);
            Guard.NotNull(() => post.CreatedBy, post.CreatedBy);
        }

        public void DeletePost(Guid postGuid)
        {
            var postDb = _whoOwesWhatContext.GetPostSqlRepository().GetAll().FirstOrDefault(a => a.PostGuid == postGuid);

            if (postDb == null)
            {
                throw new PostCommandException("Unable to find the Post to delete");
            }

            postDb.IsDeleted = true;


            _whoOwesWhatContext.SaveChanges();


        }

        public void UnDeletePost(Guid postGuid)
        {
            var postDb = _whoOwesWhatContext.GetPostSqlRepository().GetAll().FirstOrDefault(a => a.PostGuid == postGuid);

            if (postDb == null)
            {
                throw new PostCommandException("Unable to find the Post to undelete");
            }

            postDb.IsDeleted = false;


            _whoOwesWhatContext.SaveChanges();
        }

        public class PostCommandException : Exception
        {
            public PostCommandException(string message)
                : base(message)
            {
            }
        }
    }
}
