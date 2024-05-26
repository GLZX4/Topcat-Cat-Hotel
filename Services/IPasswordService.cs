namespace Topcat_Cat_Hotel.Services
{
    public interface IPasswordService
    {
        (string HashedPassword, byte[] Salt) HashPassword(string password);
        bool VerifyPassword(string enteredPassword, string storedHash, byte[] salt);
    }
}
