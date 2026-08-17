using AddressBook.Business.DTOs.Auth;

namespace AddressBook.Business.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto dto);

        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    }
}
