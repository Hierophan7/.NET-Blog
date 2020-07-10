using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abbott.Entities.Dtos.Account;
using AutoMapper;
using Blog.Entities.DTOs;
using Blog.Entities.DTOs.Account;
using Blog.Entities.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Controllers
{
	[Authorize(Roles = "Admin, SuperAdmin")]
	public class AdminController : Controller
	{
		private IUserService _userService;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;
		private readonly RoleManager<AppRole> _roleManager;

		public AdminController(IMapper mapper,
			IUserService userService,
			UserManager<User> userManager,
			RoleManager<AppRole> roleManager)
		{
			_userManager = userManager;
			_userService = userService;
			_roleManager = roleManager;
			_mapper = mapper;
		}

		public async Task<IActionResult> Users()
		{
			var users = await _userService.GetAllAsync();

			var usersToView = _mapper.Map<IEnumerable<UserViewDto>>(users);

			return View(usersToView);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteUser(Guid id)
		{

			var deleteStatus = await _userService.DeleteAsync(id);

			if (deleteStatus)
				return RedirectToAction("_ViewAllUsers");

			return NotFound();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateUser(Guid id)
		{
			if (id == Guid.Empty)
			{
				return NotFound();
			}

			var user = await _userManager.FindByIdAsync(id.ToString());

			var roles = await _roleManager.Roles.ToListAsync();

			var userToView = _mapper.Map<UserUpdateDto>(user);

			var rolesInUser = await _userManager.GetRolesAsync(user);

			userToView.RolesInCurrentUser = rolesInUser.ToList();
			userToView.AllRoles = roles;

			//SelectList list = new SelectList(roles);

			if (userToView == null)
			{
				return NotFound();
			}
			//ViewBag.Roles = list;
			return View(userToView);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto, List<string> roles)
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

				return RedirectToAction("Users", "Admin");
			}

			return View(userUpdateDto);
		}
	}
}
