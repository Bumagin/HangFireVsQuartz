namespace Extentions;

public class QueryExample
{
    public void Example()
    {
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), UserName = "JohnDoe", Email = "johndoe@example.com" },
            new User { Id = Guid.NewGuid(), UserName = "JaneDoe", Email = "janedoe@example.com" },
            new User { Id = Guid.NewGuid(), UserName = "Alice", Email = "alice@example.com" },
        }.AsQueryable();

        var userIds = new List<Guid>();
        
        string userNameToFilter = "JohnDoe";
        DateTime? startDate = DateTime.Now;
        DateTime? endDate = DateTime.Now.AddDays(10);
        
        var filteredUsers = users
            .WhereIf(!string.IsNullOrWhiteSpace(userNameToFilter), 
                u => u.UserName.ToLower().Contains(userNameToFilter.ToLower()))
            .WhereIf(startDate.HasValue,
                u => u.CreationTime >= startDate)
            .WhereIf(endDate.HasValue,
                u => u.CreationTime <= endDate)
            .WhereIf(!userIds.IsNullOrEmpty(),
                u => userIds.Contains(u.Id));
    }
}