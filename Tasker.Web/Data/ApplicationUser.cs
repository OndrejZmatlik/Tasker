using Microsoft.AspNetCore.Identity;

namespace Tasker.Web.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {
    }

}
