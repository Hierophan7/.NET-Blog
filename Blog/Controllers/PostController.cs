using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blog.Entities.DTOs.Post;
using Blog.Entities.DTOs.Tag;
using Blog.Entities.DTOs.Category;
using Blog.Entities.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Blog.Entities.DTOs.Language;
using Blog.Entities.DTOs.Account;
using Blog.Entities.Enums;
using Blog.Common;

namespace Blog.Controllers
{
	public class PostController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IPostService _postService;
		private readonly ITagService _tagService;
		private readonly ICategoryService _categoryService;
		private readonly IPictureService _pictureService;
		private readonly ILanguageService _languageService;
		private readonly UserManager<User> _userManager;
		private readonly IUserService _userService;
		private readonly IAutomaticEmailNotificationService _automaticEmailNotificationService;
		private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

		public PostController(
			IMapper mapper,
			IPostService postService,
			ITagService tagService,
			ICategoryService categoryService,
			IPictureService pictureService,
			ILanguageService languageService,
			UserManager<User> userManager,
			IUserService userService,
			IAutomaticEmailNotificationService automaticEmailNotificationService,
			IRazorViewToStringRenderer razorViewToStringRenderer)
		{
			_mapper = mapper;
			_postService = postService;
			_tagService = tagService;
			_categoryService = categoryService;
			_pictureService = pictureService;
			_languageService = languageService;
			_userManager = userManager;
			_userService = userService;
			_automaticEmailNotificationService = automaticEmailNotificationService;
			_razorViewToStringRenderer = razorViewToStringRenderer;
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpGet]
		public IActionResult CreatePost()
		{
			return View();
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> CreatePost(PostCreateDTO postCreateDTO)
		{
			if (ModelState.IsValid)
			{
				var userName = User.Identity.Name;

				var user = await _userManager.FindByNameAsync(userName);

				var categories = await _categoryService.GetAllAsync();
				var languages = await _languageService.GetAllAsync();

				var category = categories.First();
				var language = languages.First();

				var post = _mapper.Map<Post>(postCreateDTO);

				post.UserId = user.Id;
				post.CreationData = DateTime.Now;
				post.PostStatus = PostStatus.Posted;

				if (category != null && language != null)
				{
					post.CategoryId = category.Id;
					post.LanguageId = language.Id;
				}

				await _postService.CreateAsync(post);

				var usersForNotification = await _userService.GetUsersForNitificationAsync();

				if (usersForNotification != null)
				{
					var usersVoewDTO = _mapper.Map<List<UserViewDto>>(usersForNotification);
					var postViewDTO = _mapper.Map<PostViewDTO>(post);
					var userViewDTO = _mapper.Map<UserViewDto>(user);

					if (postViewDTO.Text.Length > 300)
						postViewDTO.Text = postViewDTO.Text.Substring(0, 300);

					postViewDTO.UserViewDto = userViewDTO;

					var viewPostLink = Url.Action(
						"PostDetails",
						"Post",
						new { id = postViewDTO.Id },
						protocol: HttpContext.Request.Scheme);

					var stringForm = await _razorViewToStringRenderer.RenderToStringAsync("Post/PostForEmailNotification", postViewDTO);

					await _automaticEmailNotificationService.SentAutomaticNotificationAsync(EmailNotificationSettings.subject,
						EmailNotificationSettings.CteateMessage(postViewDTO, stringForm, viewPostLink), usersVoewDTO);
				}

				return RedirectToAction("Index", "Home");
			}
			return View(postCreateDTO);
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpGet]
		public async Task<IActionResult> UpdatePostAsync(Guid postId)
		{

			if (postId == Guid.Empty)
				return NotFound();

			var post = await _postService.GetByIdAsync(postId);

			if (post == null)
				return NotFound();

			var postViewDTO = _mapper.Map<PostViewDTO>(post);

			//pictures;

			return View(postViewDTO);
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> UpdatePost(PostUpdateDTO postUpdateDto)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var postToUpdate = _mapper.Map<Post>(postUpdateDto);

					postToUpdate.ModifiedDate = DateTime.Now;

					await _postService.UpdateAsync(postToUpdate);


				}
				catch (DbUpdateException ex)
				{
					return Content(ex.Message);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}

				return RedirectToAction("Index", "Home");
			}

			return View(postUpdateDto);
		}

		public async Task<IActionResult> PostDetails(Guid id)
		{
			var post = await _postService.GetByIdAsync(id);

			var postViewDTO = _mapper.Map<PostViewDTO>(post);

			return View(postViewDTO);
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> DeletePost(Guid id)
		{
			if (id != Guid.Empty)
			{
				var deleteStatus = await _postService.DeleteAsync(id);

				if (deleteStatus)
					return RedirectToAction("AllArchivedPosts", "Post");
			}

			return NotFound();
		}

		[Authorize(Roles = "SuperAdmin")]
		[HttpGet]
		public async Task<IActionResult> AllArchivedPosts()
		{
			List<PostViewDTO> postViewDTOs = new List<PostViewDTO>();

			var posts = await _postService.GetAllArchivedPostsAsync();

			foreach (var post in posts)
			{
				var postViewDTO = _mapper.Map<PostViewDTO>(post);

				if (postViewDTO.Text.Length > 300)
					postViewDTO.Text = postViewDTO.Text.Substring(0, 300);

				postViewDTOs.Add(postViewDTO);
			}

			return View(postViewDTOs);
		}

		[Authorize(Roles = "SuperAdmin")]
		[HttpGet]
		public async Task<IActionResult> AllDrafts()
		{
			List<PostViewDTO> postViewDTOs = new List<PostViewDTO>();

			var posts = await _postService.GetAllDraftsAsync();

			foreach (var post in posts)
			{
				var postViewDTO = _mapper.Map<PostViewDTO>(post);

				if (postViewDTO.Text.Length > 300)
					postViewDTO.Text = postViewDTO.Text.Substring(0, 300);

				postViewDTOs.Add(postViewDTO);
			}
			 
			return View(postViewDTOs);
		}


		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAllUserPosts()
		{
			var userName = User.Identity.Name;

			var user = await _userManager.FindByNameAsync(userName);

			List<PostViewDTO> postViewDTOs = new List<PostViewDTO>();

			var posts = await _postService.GetAllUserPostedPostsAsync(user.Id);

			foreach (var post in posts)
			{
				var postViewDTO = _mapper.Map<PostViewDTO>(post);

				if (postViewDTO.Text.Length > 300)
					postViewDTO.Text = postViewDTO.Text.Substring(0, 300);

				postViewDTOs.Add(postViewDTO);
			}

			return View(postViewDTOs);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAllUserDrafts()
		{
			var userName = User.Identity.Name;

			var user = await _userManager.FindByNameAsync(userName);

			List<PostViewDTO> postViewDTOs = new List<PostViewDTO>();

			var posts = await _postService.GetAllUserDraftsAsync(user.Id);

			foreach (var post in posts)
			{
				var postViewDTO = _mapper.Map<PostViewDTO>(post);

				if (postViewDTO.Text.Length > 300)
					postViewDTO.Text = postViewDTO.Text.Substring(0, 300);

				postViewDTOs.Add(postViewDTO);
			}

			return View(postViewDTOs);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAllUserArcihedPosts()
		{
			var userName = User.Identity.Name;

			var user = await _userManager.FindByNameAsync(userName);

			List<PostViewDTO> postViewDTOs = new List<PostViewDTO>();

			var posts = await _postService.GetAllUserArchivedPostsAsync(user.Id);

			foreach (var post in posts)
			{
				var postViewDTO = _mapper.Map<PostViewDTO>(post);

				if (postViewDTO.Text.Length > 300)
					postViewDTO.Text = postViewDTO.Text.Substring(0, 300);

				postViewDTOs.Add(postViewDTO);
			}

			return View(postViewDTOs);
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> Archive(Guid id)
		{
			var post = await _postService.GetByIdAsync(id);

			if (post != null)
			{
				post.PostStatus = PostStatus.Archive;

				await _postService.UpdateAsync(post);

				if (User.IsInRole("SuperAdmin"))
				{
					return RedirectToAction("Index", "Home");
				}

				return RedirectToAction("GetAllUserPosts", "Post");
			}

			return NotFound();
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> Unarchive(Guid id)
		{
			var post = await _postService.GetByIdAsync(id);

			if (post != null)
			{
				post.PostStatus = PostStatus.Posted;

				await _postService.UpdateAsync(post);

				if (User.IsInRole("SuperAdmin"))
				{
					return RedirectToAction("Index", "Home");
				}

				return RedirectToAction("GetAllUserArcihedPosts", "Post");
			}

			return NotFound();
		}
	}
}
