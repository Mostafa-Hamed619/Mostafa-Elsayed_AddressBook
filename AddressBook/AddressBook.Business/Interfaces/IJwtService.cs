namespace AddressBook.Business.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email);
    }
}
