using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Models;
using Blog.Services.Interfaces;
using AutoMapper;
using Blog.Entities.DTOs.Post;
using Microsoft.AspNetCore.Identity;
using Blog.Entities.Models;
using Blog.Entities.DTOs.Category;
using Blog.Entities.DTOs.Account;

namespace Blog.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPostService _postService;
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly ICategoryService _categoryService;

		public HomeController(
			IPostService postService,
			UserManager<User> userManager,
			ICategoryService categoryService,
			IMapper mapper)
		{
			_postService = postService;
			_userManager = userManager;
			_categoryService = categoryService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			List<PostViewDTO> postViewDTOs = new List<PostViewDTO>();

			var posts = await _postService.GetAllPostedPostsAsync();

			foreach (var post in posts)
			{
				var postViewDTO = _mapper.Map<PostViewDTO>(post);

				if (postViewDTO.Text.Length > 300)
					postViewDTO.Text = postViewDTO.Text.Substring(0, 300);

				postViewDTOs.Add(postViewDTO);
			}

			return View(postViewDTOs);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
