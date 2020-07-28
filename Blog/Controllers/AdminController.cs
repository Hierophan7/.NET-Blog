using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abbott.Entities.Dtos.Account;
using AutoMapper;
using Blog.Entities.DTOs;
using Blog.Entities.DTOs.Account;
using Blog.Entities.DTOs.Picture;
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
		private readonly IPictureService _pictureService;

		public AdminController(IMapper mapper,
			IUserService userService,
			UserManager<User> userManager,
			RoleManager<AppRole> roleManager,
			IPictureService pictureService)
		{
			_pictureService = pictureService;
			_userManager = userManager;
			_userService = userService;
			_roleManager = roleManager;
			_mapper = mapper;
		}

		public async Task<IActionResult> Users()
		{
			var users = await _userService.GetAllUsersAsync();

			List<UserViewDto> userViewDtos = new List<UserViewDto>();

			foreach (var user in users)
			{
				// Gets a list of role names the specified user belongs to
				var rolesInUser = await _userManager.GetRolesAsync(user);

				var userViewDTO = _mapper.Map<UserViewDto>(user);

				//var avatar = await _pictureService.GetAvatarAsync(user.Id);

				//if (avatar != null)
				//{
				//	var avatarViewDTO = _mapper.Map<PictureViewDTO>(avatar);
				//	userViewDTO.AvatarViewDTO = avatarViewDTO;
				//}
				//else
				//	userViewDTO.AvatarViewDTO = null;

				// User has only one role, so when we get roles list, we take zero [0] element from it.
				userViewDTO.RoleName = rolesInUser[0].ToString();

				userViewDtos.Add(userViewDTO);
			}

			return View(userViewDtos);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteUser(Guid id)
		{
			var deleteStatus = await _userService.DeleteAsync(id);

			if (deleteStatus)
				return RedirectToAction("Users", "Admin");

			return NotFound();
		}
	}
}
