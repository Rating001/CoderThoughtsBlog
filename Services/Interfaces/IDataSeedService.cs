namespace CoderThoughtsBlog.Services.Interfaces
{
    public interface IDataSeedService
    {
        //Seed a few roles into the system
        Task SeedRolesAsync();

        //Seed a few users into the system
        Task SeedUsersAsync();
    }
}
