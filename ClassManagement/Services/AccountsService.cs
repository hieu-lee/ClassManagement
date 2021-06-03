using ClassManagement.Data;
using ClassManagement.Models;
using System.Threading.Tasks;

namespace ClassManagement.Services
{
    public class AccountsService
    {
        private readonly AppDbContext dbContext;
        private readonly SessionService sessionService;
        public AccountsService (AppDbContext dbContext, SessionService sessionService)
        {
            this.sessionService = sessionService;
            this.dbContext = dbContext;
        }
        //public async Task<Account> GetAccount (string username)
        //{
        //    var res = await dbContext.Accounts.Where(s => s.Username == username).FirstOrDefaultAsync();
        //    return res;
        //}
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

        public async Task<ServiceResult> SignUpAsync(string accUsername, string accPassword)
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
            var task = sessionService.SignUpAsync(newAccount);
            dbContext.Accounts.Add(newAccount);
            await dbContext.SaveChangesAsync();
            await task;
            return new() { success = true, svAccount = newAccount };
        }

        public async Task<ServiceResult> SignInAsync(string username, string password)
        {
            var Account = await dbContext.Accounts.FindAsync(username);
            if (Account is null || Account.Password != password)
            {
                return new() { success = false, err = "Wrong username/password" };
            }
            await sessionService.SignInAsync(Account);
            return new() { success = true };
        }

        public async Task<ServiceResult> SignOutAsync()
        {
            await sessionService.SignOutAsync();
            return new() { success = true };
        }

        public bool CheckLength(string password)
        {
            return (password.Length >= 6 && password.Length <= 15);
        }
    }
}
