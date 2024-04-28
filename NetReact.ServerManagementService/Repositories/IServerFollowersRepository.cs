using Models;

namespace NetReact.ServerManagementService.Repositories
{
    public interface IServerFollowersRepository
    {
        Task<string> Add(ServerFollower follower);
        Task<bool> Delete(string id);
        Task<IEnumerable<ServerFollower>> GetFollowedServers(string userId);
        Task<IEnumerable<ServerFollower>> GetFollowedUsers(string serverId);
        Task<bool> GetIsUserFollowingServer(string userId, string serverId);
    }
}