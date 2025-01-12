using Microsoft.AspNetCore.Components;

namespace Tasker.Web.Data.Services
{
    public class Refresh
    {
        public delegate Task Action2();
        public static event Action2 OnChange = null!;

        public static async Task CallRequestRefresh()
        {
            if (OnChange != null)
            {
                await OnChange.Invoke();
            }
        }
    }
}
