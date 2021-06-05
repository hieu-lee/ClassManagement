using ClassManagement.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ClassManagement.Services
{

    public class SessionService
    {
        private readonly ProtectedLocalStorage localStorage;
        public bool LoggedState => !string.IsNullOrWhiteSpace(UsernameState);
#nullable enable
        public string? UsernameState { get; set; }
#nullable disable

        public SessionService(ProtectedLocalStorage localStorage)
        {
            this.localStorage = localStorage;
        }

        public async Task SignInAsync(Account account)
        {
            var task = localStorage.SetAsync("MEDUv1UsernameState", account.Username);
            UsernameState = account.Username;
            await task;
        }

        public async Task SignUpAsync(Account account)
        {
            var task = localStorage.SetAsync("MEDUv1UsernameState", account.Username);
            UsernameState = account.Username;
            await task;
        }

        public async Task SignOutAsync()
        {
            var task = localStorage.DeleteAsync("MEDUv1UsernameState");
            UsernameState = null;
            await task;
        }
    }
}
