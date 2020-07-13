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
				return RedirectToAction("Users", "Admin");

			return NotFound();
		}
	}
}
