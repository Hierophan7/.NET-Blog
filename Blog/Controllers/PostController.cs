using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.DTOs.Post;
using Blog.Entities.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
	public class PostController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IPostService _postService;

		public PostController( 
			IMapper mapper,
			IPostService postService)
		{
			_mapper = mapper;
			_postService = postService;
		}

		//public async Task<IActionResult> CreatePost(PostCreateDTO postCreateDTO)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var newPost = _mapper.Map<Post>(postCreateDTO);


		//	}

		//}
	}
}
