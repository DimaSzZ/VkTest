using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VkTest.Logic.Domain;
using VkTest.Parsistence.dto;

namespace VkTest.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("RegistrationUser")]
    public async Task<IActionResult> RegistrationUser(RegistrationUser user,CancellationToken cancellationToken)
    {
        var result = await _userRepository.AddUser(user,cancellationToken);
        return Ok(result);
    }
    [HttpPost("AuthorizationUser")]
    public async Task<IActionResult> AuthorizationUser(string login,string password,CancellationToken cancellationToken)
    {
        var response = await _userRepository.AuthorizationUser(login,password,cancellationToken);
        return Ok(response);
    }
    [Authorize]
    [HttpPut("DeleteUser")]
    public async Task<IActionResult> DeleteUser(int id,CancellationToken cancellationToken)
    {
        var result = await _userRepository.DeleteUser(id,cancellationToken);
        return Ok(result);
    }
    [Authorize]
    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser(int id,CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetUser(id,cancellationToken);
        return Ok(result);
    }
    [Authorize]
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers(int countUsers,CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetAllUsers(countUsers,cancellationToken);
        return Ok(result);
    }
}