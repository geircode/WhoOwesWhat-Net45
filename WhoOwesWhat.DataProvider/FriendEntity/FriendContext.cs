using System.Linq;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider.FriendEntity
{
    public interface IFriendContext
    {
        Entity.Friend GetFriend(int friendId);
    }

    public class FriendContext : IFriendContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public FriendContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public Friend GetFriend(int friendId)
        {
            return _whoOwesWhatContext.GetFriendSqlRepository().GetAll().SingleOrDefault(a => a.FriendId == friendId);
        }
    }
}
