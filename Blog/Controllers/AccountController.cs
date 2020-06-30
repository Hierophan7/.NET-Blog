using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.DTOs.Account;
using Blog.Entities.Models;
using Blog.Helpers;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Controllers
{
	public class AccountController : Controller
	{
		private IUserService _userService;
		private readonly IMapper _mapper;
		private readonly AppSettings _appSettings;
		private readonly IRoleService _roleService;


		public AccountController(
			IUserService userService,
			IMapper mapper,
			IOptions<AppSettings> appSettings,
			IRoleService roleService)
		{
			_userService = userService;
			_appSettings = appSettings.Value;
			_mapper = mapper;
			_roleService = roleService;
		}

		[HttpGet]
		public IActionResult Authenticate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Authenticate([FromBody] UserAuthenticateDto userAuthenticateDto)
		{
			var user = await _userService.AuthenticateAsync(userAuthenticateDto.Email, userAuthenticateDto.Password);

			if (user == null)
			{
				return BadRequest(new { message = "К сожалению, логин или пароль неверны" });
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Role, user.Role.RoleName)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);

			return Ok(
				new
				{
					Id = user.Id,
					Email = user.Email,
					UserName = user.UserName,
					Token = tokenString
				});
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
		{
			if (ModelState.IsValid && !string.IsNullOrEmpty(userRegisterDto.Email) && !string.IsNullOrEmpty(userRegisterDto.Password))
			{
				var user = _mapper.Map<User>(userRegisterDto);

				var role = await _roleService.GetRoleByName("User");

				user.RoleId = role.Id;

				var newUser = await _userService.CreateAsync(user, userRegisterDto.Password);

				return RedirectToAction("Authenticate", "Account");
			}
			else
				return BadRequest(new { message = "Enter Email and  Password or write all required data for user" });
		}
	}
}
