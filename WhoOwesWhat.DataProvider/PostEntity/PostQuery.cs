using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.PostEntity
{
    public class PostQuery : IPostQuery
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IPostDataProviderLogic _postDataProviderLogic;

        public PostQuery(IWhoOwesWhatContext whoOwesWhatContext, ILog log, IPostDataProviderLogic postDataProviderLogic)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _postDataProviderLogic = postDataProviderLogic;
        }

        /// <summary>
        /// Get the last version of a Post. MMF: Foreløpig, så tar jeg ikke vare på alle versjoner, men kun den siste.
        /// </summary>
        /// <param name="postGuid"></param>
        /// <returns></returns>
        public Post GetPostByPostGuid(Guid postGuid)
        {
            var postDb = _whoOwesWhatContext.GetPostSqlRepository().GetAll().SingleOrDefault(a => a.PostGuid == postGuid);
            if (postDb != null)
            {
                _whoOwesWhatContext.LoadProperty(postDb, a => a.CreatedBy);
                _whoOwesWhatContext.LoadProperty(postDb, a => a.LastUpdatedBy);
                _whoOwesWhatContext.LoadProperty(postDb, a => a.Group);
                _whoOwesWhatContext.LoadCollection(postDb, a => a.Transactions);
                foreach (var transaction in postDb.Transactions)
                {
                    _whoOwesWhatContext.LoadProperty(transaction, a => a.Person);
                }
            }

            return postDb == null ? null : _postDataProviderLogic.MapToDomain(postDb);
        }


        public List<Post> GetPostsCreatedByAndUpdatedAfter(Guid personGuid, DateTime? lastSynchronizedToServer)
        {
            //Get all Posts created By personGuid
            var postDb = _whoOwesWhatContext.GetPostSqlRepository().GetAll().Where(a => a.CreatedBy.PersonGuid == personGuid);
            //_log.Info(_whoOwesWhatContext.GetDatabase().Connection.ConnectionString);
            if (lastSynchronizedToServer.HasValue)
            {
                postDb = postDb.Where(a => a.VersionUpdated > lastSynchronizedToServer);
            }

            foreach (var post in postDb)
            {
                _whoOwesWhatContext.LoadProperty(post, a => a.Group);
                _whoOwesWhatContext.LoadProperty(post, a => a.CreatedBy);
                _whoOwesWhatContext.LoadProperty(post, a => a.LastUpdatedBy);
                _whoOwesWhatContext.LoadCollection(post, a => a.Transactions);
                foreach (var transaction in post.Transactions)
                {
                    _whoOwesWhatContext.LoadProperty(transaction, a => a.Person);
                }
            }

            return postDb.Select(_postDataProviderLogic.MapToDomain).ToList();
        }
    }

}
