namespace AddressBook.Business.Interfaces
{
    public interface ICurrentUserService
    {
        int GetUserId();
        string GetUserEmail();
    }
}
