using CommercialManager.API.Dtos.Categories;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Users;
using CommercialManager.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialManager.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices _services;
        public UsersController(IUsersServices services)
        {
            _services = services;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<UsersDto>>> PostUser([FromBody] UsersCreateDto dto)
        {
            var response = await _services.CreateAsync(dto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<UsersDto>>>>> GetUsersList(
            string searchTerm = "", int page = 1, int pageSize = 0)
        {
            var response = await _services.GetListAsync(searchTerm, page, pageSize);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<UsersActionResponseDto>>> PutUser(Guid id, [FromBody] UsersEditDto dto)
        {
            var reponse = await _services.UpdateAsync(id, dto);

            return StatusCode(reponse.StatusCode, reponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<UsersActionResponseDto>>> DeleteUser(Guid id)
        {
            var response = await _services.DeleteByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
