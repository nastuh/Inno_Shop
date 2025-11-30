using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        private readonly object _lock = new();

        public Task<User?> GetByIdAsync(Guid id)
        {
            lock (_lock)
            {
                return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
            }
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            lock (_lock)
            {
                return Task.FromResult(_users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
            }
        }

        public Task<List<User>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_users.Where(u => u.IsActive).ToList());
            }
        }

        public Task AddAsync(User user)
        {
            lock (_lock)
            {
                _users.Add(user);
                return Task.CompletedTask;
            }
        }

        public Task UpdateAsync(User user)
        {
            lock (_lock)
            {
                var existing = _users.FirstOrDefault(u => u.Id == user.Id);
                if (existing != null)
                {
                    _users.Remove(existing);
                    _users.Add(user);
                }
                return Task.CompletedTask;
            }
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            lock (_lock)
            {
                return Task.FromResult(_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
            }
        }
    }
}