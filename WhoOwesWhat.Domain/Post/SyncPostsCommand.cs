using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Post
{
    public class SyncPostsCommand : ISyncPostsCommand
    {
        private readonly ISyncPostCommand _syncPostCommand;

        public SyncPostsCommand(
            ISyncPostCommand syncPostCommand
            )
        {
            _syncPostCommand = syncPostCommand;
        }

        public void SyncPosts(string username, List<SyncPostModel> syncPostModels)
        {
            foreach (var syncPostModel in syncPostModels)
            {
                _syncPostCommand.SyncPost(username, syncPostModel);
            }
        }
    }



}
