using System;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public interface IUserRepository
    {
        Task InsertEntity(string role, string id, string fullName);
    }
}
