using newOne.Models;
using Task = System.Threading.Tasks.Task;

namespace newOne.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        /// <summary>
        /// get users by Ids
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetByUserIds(IEnumerable<int> Ids);

        /// <summary>
        /// Get users by team name
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUserByTeam(string teamName);

        /// <summary>
        /// Get users by role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUserByRole(string role);

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUser(int userId);

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateUser(User user);

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task AddUser(User user);
    }
}
