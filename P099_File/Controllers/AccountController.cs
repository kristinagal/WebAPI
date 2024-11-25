using Microsoft.AspNetCore.Mvc;
using P099_File.Models;
using P099_File.Services;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IJwtService _jwtService;

    public AccountController(IAccountService accountService, IJwtService jwtService)
    {
        _accountService = accountService;
        _jwtService = jwtService;
    }

    // Sign-up Endpoint
    [HttpPost("signup")]
    public IActionResult SignUp([FromBody] SignUpRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password cannot be empty.");
        }

        try
        {
            var account = _accountService.SignupNewAccount(request.Username, request.Password);
            return Created("api/account/signup", new { account.Username });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Login Endpoint
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password cannot be empty.");
        }

        var isAuthenticated = _accountService.Login(request.Username, request.Password, out string role);
        if (!isAuthenticated)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = _jwtService.GetJwtToken(request.Username, role);
        return Ok(new { Token = token });
    }

}

// DTOs for Sign-Up and Login Requests
public class SignUpRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
