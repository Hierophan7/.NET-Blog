using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.DTOs.Comment;
using Blog.Entities.Enums;
using Blog.Entities.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
	public class CommentController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ICommentService _commentService;
		private readonly UserManager<User> _userManager;

		public CommentController(
			IMapper mapper,
			UserManager<User> userManager,
			ICommentService commentService)
		{
			_mapper = mapper;
			_userManager = userManager;
			_commentService = commentService;
		}

		[HttpGet]
		public IActionResult CreateComment()
		{
			return PartialView();
		}

		[HttpPost]
		public async Task<IActionResult> CreateComment(CommentCreateDTO commentCreateDTO, int commentStatus)
		{
			if (ModelState.IsValid)
			{
				if (!User.Identity.IsAuthenticated)
					return RedirectToAction("Authenticate", "Account");

				var curUser = User.Identity.Name;
				User user = await _userManager.FindByNameAsync(curUser);

				commentCreateDTO.Created = DateTime.Now;
				commentCreateDTO.UserId = user.Id;

				switch (commentStatus)
				{
					case 1:
						commentCreateDTO.CommentStatus = CommentStatus.Useful;
						break;
					case 2:
						commentCreateDTO.CommentStatus = CommentStatus.Neutral;
						break;
					case 3:
						commentCreateDTO.CommentStatus = CommentStatus.Dangerous;
						break;
				}

				Comment comment = _mapper.Map<Comment>(commentCreateDTO);

				await _commentService.CreateAsync(comment);

				return RedirectToAction("PostDetails", "Post", new { id = commentCreateDTO.PostId });
			}

			return PartialView(commentCreateDTO);
		}
	}
}
