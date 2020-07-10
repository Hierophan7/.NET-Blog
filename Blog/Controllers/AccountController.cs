using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abbott.Entities.Dtos.Account;
using AutoMapper;
using Blog.Common;
using Blog.Entities.DTOs.Account;
using Blog.Entities.Models;
using Blog.Helpers;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Controllers
{
	public class AccountController : Controller
	{
		private IUserService _userService;
		private readonly IMapper _mapper;
		private readonly AppSettings _appSettings;
		private readonly IEmailService _emailService;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<AppRole> _roleManager;

		public AccountController(
			IUserService userService,
			IMapper mapper,
			IOptions<AppSettings> appSettings,
			IEmailService emailService,
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			RoleManager<AppRole> roleManager)
		{
			_userService = userService;
			_appSettings = appSettings.Value;
			_mapper = mapper;
			_emailService = emailService;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		[HttpGet]
		public IActionResult Authenticate(string returnUrl = null)
		{
			return View(new UserAuthenticateDto { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public async Task<IActionResult> Authenticate(UserAuthenticateDto userAuthenticateDto)
		{
			if (ModelState.IsValid)
			{
				var user = await _userService.GetByEmailAsync(userAuthenticateDto.Email);

				var result =
					await _signInManager.PasswordSignInAsync(user, userAuthenticateDto.Password, userAuthenticateDto.RememberMe, false);

				if (result.Succeeded)
				{
					// проверяем, принадлежит ли URL приложению
					if (!string.IsNullOrEmpty(userAuthenticateDto.ReturnUrl) && Url.IsLocalUrl(userAuthenticateDto.ReturnUrl))
					{
						return Redirect(userAuthenticateDto.ReturnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "Неправильный логин и (или) пароль");
				}
			}
			return View(userAuthenticateDto);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
		{

			if (ModelState.IsValid)
			{
				var user = _mapper.Map<User>(userRegisterDto);

				// добавляем пользователя
				var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

				if (result.Succeeded)
				{
					var currentUser = await _userManager.FindByEmailAsync(user.Email);
					await _userManager.AddToRoleAsync(currentUser, "User");

					await _emailService.SendAsync(SeccessRegisterSettings.subject,
							SeccessRegisterSettings.CteateMessage(userRegisterDto), userRegisterDto.Email);

					// установка куки
					await _signInManager.SignInAsync(user, false);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(userRegisterDto);
		}

		[HttpGet]
		public async Task<IActionResult> LogOff()
		{
			// delete authentication cookies
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
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

			var feedBack = Url.Action(
				"NewPassword",
				"Account",
				new { userEmail = email },
				protocol: HttpContext.Request.Scheme);

			await _emailService.SendAsync(PasswordRecoverySettings.subject,
				PasswordRecoverySettings.GetMessageWihtFeedBack(feedBack), email);

			return RedirectToAction("Authenticate", "Account");
		}

		[HttpGet]
		public async Task<IActionResult> NewPassword(string userEmail)
		{
			var existUser = await _userService.GetByEmailAsync(userEmail);

			var userNewPasswordDto = _mapper.Map<UserNewPasswordDto>(existUser);

			return View(userNewPasswordDto);
		}

		[HttpPost]
		public async Task<IActionResult> NewPassword(UserNewPasswordDto userNewPasswordDto)
		{
			if (ModelState.IsValid && userNewPasswordDto.Id != Guid.Empty && userNewPasswordDto.NewPassword.Equals(userNewPasswordDto.NewPasswordConfirm))
			{
				var user = await _userManager.FindByIdAsync(userNewPasswordDto.Id.ToString());

				if (user != null)
				{
					var _passwordValidator =
				HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
					var _passwordHasher =
						HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

					IdentityResult result =
						await _passwordValidator.ValidateAsync(_userManager, user, userNewPasswordDto.NewPassword);

					if (result.Succeeded)
					{
						user.PasswordHash = _passwordHasher.HashPassword(user, userNewPasswordDto.NewPassword);

						await _userManager.UpdateAsync(user);

						await _emailService.SendAsync(ChangePasswordSettings.subject,
				ChangePasswordSettings.GetMessage(userNewPasswordDto.Email, userNewPasswordDto.NewPassword), userNewPasswordDto.Email);

						return RedirectToAction("Authenticate", "Account");
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Користувач незнайдений");
				}
			}
			else
			{
				return Content("Model state isn't valid");
			}
			return View(userNewPasswordDto);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			var nameOfCurrentUser = HttpContext.User.Identity.Name;

			var currentUser = await _userManager.FindByNameAsync(nameOfCurrentUser);

			var userChangePasswordDto = _mapper.Map<UserChangePasswordDto>(currentUser);

			return View(userChangePasswordDto);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ChangePassword(UserChangePasswordDto userChangePasswordDto)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(userChangePasswordDto.Id.ToString());

				if (user != null)
				{
					IdentityResult result =
						await _userManager.ChangePasswordAsync(user, userChangePasswordDto.OldPassword, userChangePasswordDto.NewPassword);

					if (result.Succeeded)
					{
						await _emailService.SendAsync(ChangePasswordSettings.subject,
				ChangePasswordSettings.GetMessage(userChangePasswordDto.Email, userChangePasswordDto.NewPassword), userChangePasswordDto.Email);

						return RedirectToAction("Index", "Home");
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Користувач незнайдений");
				}
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Неправильно введені дані");
			}
			return View(userChangePasswordDto);
		}

		[HttpGet]
		public async Task<IActionResult> Update(Guid id)
		{
			User user = new User();

			if (id == Guid.Empty)
			{
				var userName = User.Identity.Name;
				user = await _userManager.FindByNameAsync(userName);
			}
			else
			{
				user = await _userManager.FindByIdAsync(id.ToString());
			}

			if (user == null)
				return NotFound();

			var roles = await _roleManager.Roles.ToListAsync();

			var userToView = _mapper.Map<UserUpdateDto>(user);

			var rolesInUser = await _userManager.GetRolesAsync(user);

			userToView.RolesInCurrentUser = rolesInUser.ToList();
			userToView.AllRoles = roles;

			if (userToView == null)
			{
				return NotFound();
			}

			return View(userToView);
		}

		[HttpPost]
		public async Task<IActionResult> Update(UserUpdateDto userUpdateDto, List<string> roles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var user = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());

					if (user != null)
					{
						user.UserName = userUpdateDto.UserName;
						user.Email = userUpdateDto.Email;
						user.FacebookLink = userUpdateDto.FacebookLink;
						user.InstagramLink = userUpdateDto.InstagramLink;
						user.LinkedInLink = userUpdateDto.LinkedInLink;
						user.TwitterLink = userUpdateDto.TwitterLink;
						user.YoutubeLink = userUpdateDto.YoutubeLink;
					}

					// получем список ролей пользователя
					var userRoles = await _userManager.GetRolesAsync(user);
					// получаем все роли
					var allRoles = _roleManager.Roles.ToList();
					// получаем список ролей, которые были добавлены
					var addedRoles = roles.Except(userRoles);
					// получаем роли, которые были удалены
					var removedRoles = userRoles.Except(roles);

					await _userManager.AddToRolesAsync(user, addedRoles);
					await _userManager.RemoveFromRolesAsync(user, removedRoles);
					await _userManager.UpdateAsync(user);
				}
				catch (DbUpdateException ex)
				{
					return Content(ex.Message);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}

				return RedirectToAction("Index", "Home");
			}

			return View(userUpdateDto);
		}
	}
}
