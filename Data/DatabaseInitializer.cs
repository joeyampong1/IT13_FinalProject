using Microsoft.EntityFrameworkCore;
using IT_13FinalProject.Data;

namespace IT_13FinalProject.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
}
