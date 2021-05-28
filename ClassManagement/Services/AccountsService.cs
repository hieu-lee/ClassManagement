using ClassManagement.Data;
using ClassManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement.Services;

namespace ClassManagement.Services
{
    public class AccountsService
    {
        private readonly AppDbContext dbContext;
        public AccountsService (AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Account> GetAccount (string password)
        {
            var res = await dbContext.Accounts.Where(s => s.Password == password).FirstOrDefaultAsync();
            return res;
        }
        //public async Task<ServiceResult> CreateNewAcc(Account NewAccount)
        //{
        //    var Account = dbContext.Accounts.Find(NewAccount.Username);
        //    if (Account is not null)
        //    {
        //        return new() { success = false, err = "Username already existed" };
        //    }
        //    dbContext.Accounts.Add(NewAccount);
        //    await dbContext.SaveChangesAsync();
        //    return new() { success = true };
        //}

        public async Task<ServiceResult> CreateNewAccountAsync (string accUsername, string accPassword)
        {
            var account = await dbContext.Accounts.FindAsync(accUsername);
            if (account is not null)
            {
                return new() { success = false, err = "Username already existed" };
            }
            Account newAccount = new()
            {
                Username = accUsername,
                Password = accPassword,
            };
            dbContext.Accounts.Add(newAccount);
            await dbContext.SaveChangesAsync();
            return new() { success = true, svAccount = newAccount };
        }

        public async Task<ServiceResult> CheckValid (string username, string password)
        {
            var Account = await dbContext.Accounts.FindAsync(username);
            if (Account is null || Account.Password != password)
            {
                return new() { success = false, err = "Wrong username/password" };
            }
            return new() { success = true, svAccount = Account };
        }

        public bool CheckLength(string password)
        {
            return (password.Length >= 6 && password.Length <= 15);
        }
    }
}
