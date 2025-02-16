using Microsoft.EntityFrameworkCore;
namespace StudyPlannerApplication.Shared;

public static class AppDbContextExtension
{
    public static async Task<int> SaveAndDetachAsync(this DbContext db)
    {
        int res = await db.SaveChangesAsync();
        foreach (var entry in db.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        return res;
    }
}
