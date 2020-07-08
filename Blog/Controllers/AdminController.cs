using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abbott.Entities.Dtos.Account;
using AutoMapper;
using Blog.Entities.DTOs.Account;
using Blog.Entities.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

		public AdminController(IMapper mapper,
			IUserService userService,
			UserManager<User> userManager)
		{
			_userManager = userManager;
			_userService = userService;
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
				return RedirectToAction("_ViewAllUsers", "Admin");

			return NotFound();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateUser(Guid id)
		{
			if (id == Guid.Empty)
			{
				return NotFound();
			}

			var user = await _userService.GetByIdAsync(id);

			var userToView = _mapper.Map<UserUpdateDto>(user);

			if (userToView == null)
			{
				return NotFound();
			}

			return View(userToView);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
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

				return RedirectToAction("_ViewAllUsers", "Admin");
			}

			return View(userUpdateDto);
		}
	}
}
