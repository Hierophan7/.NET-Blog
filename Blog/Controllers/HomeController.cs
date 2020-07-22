using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.DTOs.Post;
using Blog.Entities.Models;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

		public async Task<IActionResult> Index(string searchString)
		{
			List<PostViewDTO> postViewDTOs = new List<PostViewDTO>();

			IEnumerable<Post> posts = new List<Post>();

			if (searchString != null)
			{
				posts = await _postService.SearchAsync(searchString);
			}
			else
			{
				posts = await _postService.GetAllPostedPostsAsync();
			}

			if(searchString != null && posts.Count() == 0)
			{
				return PartialView("NoResults");
			}

			foreach (var post in posts)
			{
				var postViewDTO = _mapper.Map<PostViewDTO>(post);

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
