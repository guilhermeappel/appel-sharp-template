using Appel.SharpTemplate.Infrastructure.Data;
using Appel.SharpTemplate.TestsCommon.Constants;

namespace Appel.SharpTemplate.TestsCommon.Data;

public static class DatabaseSetup
{
    public static void SeedData(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var user1 = MockEntityHelper.GetUserEntity(UserTestConstants.Entity.User1.EXTERNAL_ID, UserTestConstants.Entity.User1.EMAIL, UserTestConstants.Entity.User1.PASSWORD);
            var user2 = MockEntityHelper.GetUserEntity(UserTestConstants.Entity.User2.EXTERNAL_ID, UserTestConstants.Entity.User2.EMAIL, UserTestConstants.Entity.User2.PASSWORD);

            context.Users.AddRange(user1, user2);
            context.SaveChanges();
        }
    }
}
