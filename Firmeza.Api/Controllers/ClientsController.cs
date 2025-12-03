using AutoMapper;
using Firmeza.Api.DTOs;
using Firmeza.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ClientsController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(
        UserManager<AppUser> userManager, IMapper mapper, ILogger<ClientsController> logger)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/clients
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Getting all clients...");

            // Obtener todos los usuarios con rol "Client"
            var clients = await _userManager.GetUsersInRoleAsync("Client");

            _logger.LogInformation($"Found {clients.Count} client(s).");

            var clientDtos = _mapper.Map<IEnumerable<ClientDto>>(clients);
            return Ok(clientDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting clients");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET api/clients/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "Client not found" });

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Client"))
            return NotFound(new { message = "User is not a client" });

        var clientDto = _mapper.Map<ClientDto>(user);
        return Ok(clientDto);
    }

    // POST api/clients/{id}/assign-role
    [HttpPost("{id}/assign-role")]
    public async Task<IActionResult> AssignClientRole(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Client"))
            return BadRequest(new { message = "User already has Client role" });

        var result = await _userManager.AddToRoleAsync(user, "Client");
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = $"Client role assigned to {user.FullName}" });
    }
}
