namespace Finance_Tracker.Infrastructure.Services.Contracts
{
    public interface IPasswordHasherService
    {
        string Hash(string password);
        bool Verify(string hashedPassword, string providedPassword);
    }
}