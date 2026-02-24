using MemberSystem.DTOs;
using MemberSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Claims;
using System.Text;
using System.Text;

namespace MemberSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private static List<Users> users = new List<Users>();
		private readonly IConfiguration _config;
		public AuthController(IConfiguration config)
		{
			_config = config; // 取得 appsettings.json 設定
		}

		[HttpPost("register")]
		public IActionResult Register(UserDto dto)
		{
			// 檢查帳號是否存在
			var exist = users.Any(u => u.Username == dto.Username);
			if (exist)
			{
				return BadRequest("使用者已存在");
			}

			var user = new Users
			{
				Username = dto.Username,
				Password = dto.Password
			};
			users.Add(user);


			// 回傳給前端的 DTO（可隱藏敏感資訊）
			return Ok(new { dto.Username });
		}


		[HttpGet("all")]
		public IActionResult GetAllUsers()
		{
			return Ok(users);
		}

		[HttpPost("login")]
		public IActionResult Login(LoginDto loginUser)
		{
			var user = users.FirstOrDefault(u => u.Username == loginUser.Username &&
				u.Password == loginUser.Password);

			if (user == null)
			{
				return BadRequest("帳號或密碼錯誤");
			}

			// 登入成功 → 產生 JWT
			// 從設定檔讀 key
			var keyString = _config["JwtSettings:Key"];
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{	new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, user.Role)   
			};

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddHours(Convert.ToDouble(_config["JwtSettings:TokenValidityInHours"])),
				signingCredentials: creds
			);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return Ok(new { token = tokenString });
		}





		[HttpGet("profile")]
		[Authorize]
		public IActionResult Profile()
		{
			var dto = new ProfileDto
			{
				Username = User.Identity.Name,
				IsAuthenticated = User.Identity.IsAuthenticated,
				Claims = User.Claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value)).ToList()
			};
			return Ok(dto);
		}



		[HttpGet("admin")]
		[Authorize(Roles = "Admin")]
		public IActionResult AdminOnly()
		{
			return Ok("你是 Admin，可以進入此 API");
		}



	}
}
