using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.DTOs.Comment;
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
		public async Task<IActionResult> CreateComment(CommentCreateDTO commentCreateDTO)
		{
			if (ModelState.IsValid)
			{
				User user = new User();

				if (!User.Identity.IsAuthenticated)
					return RedirectToAction("Authenticate", "Account");

				var curUser = User.Identity.Name;
				if (curUser != null)
					user = await _userManager.FindByNameAsync(curUser);

				commentCreateDTO.Created = DateTime.Now;
				commentCreateDTO.UserId = user.Id;

				Comment comment = _mapper.Map<Comment>(commentCreateDTO);

				await _commentService.CreateAsync(comment);

				return RedirectToAction("PostDetails", "Post", new { id = commentCreateDTO.PostId });
			}

			return PartialView(commentCreateDTO);
		}

		//[HttpGet]
		//public async Task<IActionResult> GetComments(Guid id)
		//{
		//	if (id != Guid.Empty)
		//	{
		//		// id it's post's Id
		//		var comments = await _commentService.GetCommentsForPost(id);

		//		List<CommentViewDTO> commentViewDTOs = _mapper.Map<List<CommentViewDTO>>(comments);

		//		return PartialView(commentViewDTOs);
		//	}

		//	return NotFound();
		//}
	}
}
