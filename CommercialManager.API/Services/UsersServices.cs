using AutoMapper;
using CommercialManager.API.Constants;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Categories;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Users;
using CommercialManager.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommercialManager.API.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly IMapper _mapper;
        private readonly CommercialDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public UsersServices(IMapper mapper, CommercialDbContext context, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }

        
        // Create user
        public async Task<ResponseDto<UsersDto>> CreateAsync(UsersCreateDto dto)
        {
            var userEntity = _mapper.Map<UserEntity>(dto);

            if (userEntity == null)
            {
                return new ResponseDto<UsersDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Datos invalidos",
                    Data = null
                };
            }

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return new ResponseDto<UsersDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = "Registro encontrado",
                Data = _mapper.Map<UsersDto>(userEntity)
            };
        }

        // Read users
        public async Task<ResponseDto<PaginationDto<List<UsersDto>>>> GetListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0)
        {
            pageSize = pageSize == 0 ? PAGE_SIZE: pageSize;
            int startIndex = (page - 1) * pageSize;

            IQueryable<UserEntity> userQuery = _context.Users;

            if(!string.IsNullOrEmpty(searchTerm))
            {
                userQuery = userQuery
                    .Where(x => (x.FirstName + " " + x.LastName + " "+ x.DNI + " " + x.Email).Contains(searchTerm));
            }

            int totalRows = userQuery.Count();

            var userEntity = await userQuery
                .OrderBy(x => x.DNI).Skip(startIndex).Take(pageSize).ToListAsync();


            return new ResponseDto<PaginationDto<List<UsersDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<UsersDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT && page < (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = _mapper.Map<List<UsersDto>>(userEntity)
                }
            };
        }

        // Update users
        public async Task<ResponseDto<UsersActionResponseDto>> UpdateAsync(Guid id,UsersEditDto dto)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userEntity == null)
            {
                return new ResponseDto<UsersActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Registro no encontrado",
                    Data = null
                };
            }

            _mapper.Map<UsersEditDto, UserEntity>(dto, userEntity);

            _context.Users.Update(userEntity);

            await _context.SaveChangesAsync();

            return new ResponseDto<UsersActionResponseDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = "Registro editado correctamente",
                Data = _mapper.Map<UsersActionResponseDto>(userEntity)
            };
        }

        // Delete async
        public async Task<ResponseDto<UsersActionResponseDto>> DeleteByIdAsync(Guid id)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);

            if (userEntity == null)
            {
                return new ResponseDto<UsersActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "No se encontro el registro",
                    Data = null
                };
            }

            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();

            return new ResponseDto<UsersActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro Eliminado Correctamente",
                Data = _mapper.Map<UsersActionResponseDto>(userEntity)
            };
        }
    }
}
