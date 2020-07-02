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
	//[Route("api/[controller]")]
	public class AccountController : Controller
	{
		private IUserService _userService;
		private readonly IMapper _mapper;
		private readonly AppSettings _appSettings;
		private readonly IRoleService _roleService;
		private readonly IEmailService _emailService;

		public AccountController(
			IUserService userService,
			IMapper mapper,
			IOptions<AppSettings> appSettings,
			IRoleService roleService,
			IEmailService emailService)
		{
			_userService = userService;
			_appSettings = appSettings.Value;
			_mapper = mapper;
			_roleService = roleService;
			_emailService = emailService;
		}

		[HttpGet]
		public IActionResult Authenticate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Authenticate(UserAuthenticateDto userAuthenticateDto)
		{
			var user = await _userService.AuthenticateAsync(userAuthenticateDto.Email, userAuthenticateDto.Password);


			if (user == null)
			{
				return BadRequest(new { message = "К сожалению, логин или пароль неверны" });
			}

			var userRole = await _roleService.GetByIdAsync(user.RoleId);

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Role, userRole.RoleName)
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

				await _userService.CreateAsync(user, userRegisterDto.Password);

				return RedirectToAction("Authenticate", "Account");
			}
			else
				return BadRequest(new { message = "Enter Email and  Password or write all required data for user" });
		}

		[HttpGet]
		public IActionResult SendLinkForChengePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendLinkForChengePassword(string email)
		{
			var user = await _userService.GetByEmailAsync(email);

			if (user == null)
			{
				return BadRequest(new { message = $"User with {email} email wasn't find" });
			}

			

			var callBack = Url.Action(
				"NewPassword",
				"Account",
				new { userEmail = email },
				protocol: HttpContext.Request.Scheme);

			await _emailService.SendAsync("Відновлення паролю",
			 $"Для відновлення паролю, передіть за посиланням <a href='{callBack}'>Travel blog</a>", email);

			return RedirectToAction("Authenticate", "Account");
		}
		 
		[HttpGet]
		public async Task<IActionResult> NewPassword(string userEmail)
		{
			var existUser = await _userService.GetByEmailAsync(userEmail);

			UserNewPasswordDto userNewPasswordDto = new UserNewPasswordDto()
			{
				Id = existUser.Id,
				UserEmail = existUser.Email
			};

			return View(userNewPasswordDto);
		}

		[HttpPost]
		public async Task<IActionResult> NewPassword(UserNewPasswordDto user)
		{
			if (ModelState.IsValid && user.Id != Guid.Empty && user.NewPassword.Equals(user.NewPasswordConfirm))
			{
				bool isPasswordChanged = await _userService.ChangePassword(user.Id, user.NewPassword);

				if (isPasswordChanged)
					return RedirectToAction("Authenticate", "Account");
				else
					return BadRequest(new { message = "User didn't find in DB" });
			}
			else
			{
				return BadRequest(new { message = "Model state isn't valid" });
			}
		}
	}
}
