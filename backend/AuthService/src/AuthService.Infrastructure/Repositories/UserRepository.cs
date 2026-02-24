using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : Generics<User>, IUserRepository
    {
        private readonly DBContext context;
        public UserRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this.context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}