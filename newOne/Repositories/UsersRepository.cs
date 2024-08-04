using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using newOne.Models;
using newOne.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace newOne.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly newOneContext _dbContext;
        public UsersRepository(newOneContext db)
        {
            _dbContext = db;
        }

        public async Task<IEnumerable<User>> GetByUserIds(IEnumerable<int> Ids)
        {
            var dbItems = await _dbContext.Users
                .AsQueryable()
                .Where(u => Ids.Contains(u.UserId))
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<User>> GetUserByTeam(string teamName)
        {
            var dbItems = await _dbContext.Users
                .AsQueryable()
                .Where(u => teamName.Contains(u.Team))
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<User>> GetUserByRole(string role)
        {
            var dbItems = await _dbContext.Users
                .AsQueryable()
                .Where(u => role.Contains(u.Role))
                .ToListAsync();

            return dbItems;
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.UserId == userId);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("userInDb not found");
            }
        }

        public async Task UpdateUser(User user)
        {
            var userId = user.UserId;
            var userInDb = await _dbContext.Users.FirstOrDefaultAsync(d => d.UserId == userId);

            if (userInDb != null)
            {
                _dbContext.Update(userInDb);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("userInDb not found");
            }
        }

        public async Task AddUser(User user)
        {
            
            var userInDb = await GetByUserIds(new List<int> { user.UserId });
            if(userInDb.Any())
            {
                throw new Exception("userInDb already exists");
            }

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
