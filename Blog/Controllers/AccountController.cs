using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abbott.Entities.Dtos.Account;
using AutoMapper;
using Blog.Common;
using Blog.Entities.DTOs.Account;
using Blog.Entities.DTOs.Picture;
using Blog.Entities.Models;
using Blog.Helpers;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
		private readonly IPictureService _pictureService;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public AccountController(
			IUserService userService,
			IMapper mapper,
			IOptions<AppSettings> appSettings,
			IEmailService emailService,
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			RoleManager<AppRole> roleManager,
			IPictureService pictureService,
			IWebHostEnvironment webHostEnvironment)
		{
			_pictureService = pictureService;
			_userService = userService;
			_appSettings = appSettings.Value;
			_mapper = mapper;
			_emailService = emailService;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_webHostEnvironment = webHostEnvironment;
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
				var user = await _userManager.FindByEmailAsync(userAuthenticateDto.Email);

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
		public IActionResult PasswordRestoringLink()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> PasswordRestoringLink(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);

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
			var existUser = await _userManager.FindByEmailAsync(userEmail);

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
					var _passwordValidator = HttpContext
						.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;

					var _passwordHasher =
						HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

					IdentityResult result =
						await _passwordValidator.ValidateAsync(_userManager, user, userNewPasswordDto.NewPassword);

					if (result.Succeeded)
					{
						user.PasswordHash = _passwordHasher.HashPassword(user, userNewPasswordDto.NewPassword);

						await _userManager.UpdateAsync(user);

						await _emailService.SendAsync(ChangePasswordSettings.subject,
							ChangePasswordSettings.GetMessage(userNewPasswordDto.Email,
							userNewPasswordDto.NewPassword), userNewPasswordDto.Email);

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

		//[Authorize]
		//[HttpGet]
		//public async Task<IActionResult> ChangePassword()
		//{
		//	var nameOfCurrentUser = HttpContext.User.Identity.Name;

		//	var currentUser = await _userManager.FindByNameAsync(nameOfCurrentUser);

		//	if (currentUser == null)
		//		return NotFound();

		//	var userChangePasswordDto = _mapper.Map<UserChangePasswordDto>(currentUser);

		//	return PartialView(userChangePasswordDto);
		//}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ChangePassword(UserChangePasswordDto userChangePasswordDto)
		{
			if (ModelState.IsValid)
			{
				var nameOfCurrentUser = HttpContext.User.Identity.Name;

				var user = await _userManager.FindByNameAsync(nameOfCurrentUser);

				if (user != null)
				{
					IdentityResult result =
						await _userManager.ChangePasswordAsync(user, userChangePasswordDto.OldPassword, userChangePasswordDto.NewPassword);

					if (result.Succeeded)
					{
						await _emailService.SendAsync(ChangePasswordSettings.subject,
							ChangePasswordSettings.GetMessage(user.Email, userChangePasswordDto.NewPassword),
							user.Email);

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
			return PartialView(userChangePasswordDto);
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

			// Get the user's prifile picture
			var avatar = await _pictureService.GetAvatarAsync(user.Id);

			var roles = await _roleManager.Roles.ToListAsync();

			var userToView = _mapper.Map<UserUpdateDto>(user);

			if (avatar != null)
			{
				var avatarViewDTO = _mapper.Map<PictureViewDTO>(avatar);

				userToView.AvatarViewDTO = avatarViewDTO;
			}

			var rolesInUser = await _userManager.GetRolesAsync(user);

			userToView.RolesInCurrentUser = rolesInUser.ToList();
			userToView.AllRoles = roles;

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

					// Get the user's prifile picture
					var currentAvatar = await _pictureService.GetAvatarAsync(user.Id);

					if (userUpdateDto.Avatar != null)
					{
						Picture avatar = new Picture();
						
						if (currentAvatar != null)
							// Delete the current profile pictures
							await _pictureService.DeleteAsync(currentAvatar.Id);

						AddAvatar(userUpdateDto.Avatar, user.Id, out avatar);

						await _pictureService.CreateAsync(avatar);
					}

					if (user != null)
					{
						user.UserName = userUpdateDto.UserName;
						user.Email = userUpdateDto.Email;
						user.FacebookLink = userUpdateDto.FacebookLink;
						user.InstagramLink = userUpdateDto.InstagramLink;
						user.LinkedInLink = userUpdateDto.LinkedInLink;
						user.TwitterLink = userUpdateDto.TwitterLink;
						user.YoutubeLink = userUpdateDto.YoutubeLink;
						user.AutomaticEmailNotification = userUpdateDto.AutomaticEmailNotification;
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

		public void AddAvatar(IFormFile formFile, Guid userId, out Picture avatar)
		{
			var pictureName = Guid.NewGuid() + "_" + formFile.FileName;

			// Path to the profile picture
			var path = Path.Combine(_webHostEnvironment.WebRootPath, "Files", "Avatars", pictureName);

			// Save the profile picture into WebRootPath/Files/Avatars
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				formFile.CopyTo(fileStream);
			}

			avatar = new Picture { PictureName = pictureName, PicturePath = path, UserId = userId };
		}
	}
}
