using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WAFido2.Pages
{
    public class dashboardModel : PageModel
    {
        public void OnGet(string username)
        {
            this.Username = username;
        }

        public string Username { get; set; }
    }
}
