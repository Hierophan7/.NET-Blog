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


		public PostController(
			IMapper mapper,
			IPostService postService,
			ITagService tagService,
			ICategoryService categoryService,
			IPictureService pictureService,
			ILanguageService languageService,
			UserManager<User> userManager)
		{
			_mapper = mapper;
			_postService = postService;
			_tagService = tagService;
			_categoryService = categoryService;
			_pictureService = pictureService;
			_languageService = languageService;
			_userManager = userManager;
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

			var category = await _categoryService.GetByIdAsync(post.CategoryId);
			var categoryViewDTO = _mapper.Map<CategoryViewDTO>(category);

			var user = await _userManager.FindByIdAsync(post.UserId.ToString());
			var userViewDTO = _mapper.Map<UserViewDto>(user);

			var postViewDTO = _mapper.Map<PostViewDTO>(post);

			postViewDTO.CategoryViewDTO = categoryViewDTO;
			postViewDTO.UserViewDto = userViewDTO;

			return View(postViewDTO);
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> DeletePost(Guid id)
		{
			var deleteStatus = await _postService.DeleteAsync(id);

			if (deleteStatus)
				return RedirectToAction("AllArchivedPosts", "Post");

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

				var category = await _categoryService.GetByIdAsync(post.CategoryId);
				var categoryViewDTO = _mapper.Map<CategoryViewDTO>(category);

				var user = await _userManager.FindByIdAsync(post.UserId.ToString());
				var userViewDTO = _mapper.Map<UserViewDto>(user);

				postViewDTO.CategoryViewDTO = categoryViewDTO;
				postViewDTO.UserViewDto = userViewDTO;

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

				var category = await _categoryService.GetByIdAsync(post.CategoryId);
				var categoryViewDTO = _mapper.Map<CategoryViewDTO>(category);

				var user = await _userManager.FindByIdAsync(post.UserId.ToString());
				var userViewDTO = _mapper.Map<UserViewDto>(user);

				postViewDTO.UserViewDto = userViewDTO;
				postViewDTO.CategoryViewDTO = categoryViewDTO;

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

				var category = await _categoryService.GetByIdAsync(post.CategoryId);
				var categoryViewDTO = _mapper.Map<CategoryViewDTO>(category);

				var userViewDTO = _mapper.Map<UserViewDto>(user);

				postViewDTO.UserViewDto = userViewDTO;
				postViewDTO.CategoryViewDTO = categoryViewDTO;

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

				var category = await _categoryService.GetByIdAsync(post.CategoryId);
				var categoryViewDTO = _mapper.Map<CategoryViewDTO>(category);

				var userViewDTO = _mapper.Map<UserViewDto>(user);

				postViewDTO.UserViewDto = userViewDTO;
				postViewDTO.CategoryViewDTO = categoryViewDTO;

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

				var category = await _categoryService.GetByIdAsync(post.CategoryId);
				var categoryViewDTO = _mapper.Map<CategoryViewDTO>(category);

				var userViewDTO = _mapper.Map<UserViewDto>(user);

				postViewDTO.UserViewDto = userViewDTO;
				postViewDTO.CategoryViewDTO = categoryViewDTO;

				if (postViewDTO.Text.Length > 300)
					postViewDTO.Text = postViewDTO.Text.Substring(0, 300);

				postViewDTOs.Add(postViewDTO);
			}

			return View(postViewDTOs);
		}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> ZipPost(Guid id)
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
		public async Task<IActionResult> UnzipPost(Guid id)
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
