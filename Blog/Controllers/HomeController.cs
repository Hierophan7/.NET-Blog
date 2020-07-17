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

namespace Blog.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPostService _postService;
		private readonly IMapper _mapper;

		public HomeController(
			IPostService postService,
			IMapper mapper)
		{
			_postService = postService;
			_mapper = mapper;
		}

		public  IActionResult Index()
		{
			//var postedPosts = _postService.GetAllPostedPostsAsync();

			//var postViewDTOs = _mapper.Map<IEnumerable<PostViewDTO>>(postedPosts);

			//return View(postViewDTOs);
			return View();
		}
		
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
