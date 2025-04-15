using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Users;

namespace CommercialManager.API.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<ResponseDto<UsersDto>> CreateAsync(UsersCreateDto dto);
        Task<ResponseDto<UsersActionResponseDto>> DeleteByIdAsync(Guid id);
        Task<ResponseDto<PaginationDto<List<UsersDto>>>> GetListAsync(string searchTerm = "", int page = 1, int pageSize = 0);
        Task<ResponseDto<UsersActionResponseDto>> UpdateAsync(Guid id, UsersEditDto dto);
    }
}
