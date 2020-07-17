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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Controllers
{
	public class PostController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IPostService _postService;
		private readonly ITagService _tagService;
		private readonly ICategoryService _categoryService;
		private readonly IPictureService _pictureService;


		public PostController(
			IMapper mapper,
			IPostService postService)
		{
			_mapper = mapper;
			_postService = postService;
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
			if (ModelState.IsValid && !string.IsNullOrEmpty(postCreateDTO.Title) && !string.IsNullOrEmpty(postCreateDTO.Text))
			{
				var category = _mapper.Map<Category>(postCreateDTO.CategoryCreateDTO);

				var newCategory = await _categoryService.CreateAsync(category);

				List<SelectListItem> selectListItems = new List<SelectListItem>();
				var post = _mapper.Map<Post>(postCreateDTO);

				post.CategoryId = newCategory.Id;

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

		[Authorize(Roles = "SuperAdmin")]
		[HttpPost]
		public async Task<IActionResult> DeletePost(Guid id)
		{
			var deleteStatus = await _postService.DeleteAsync(id);

			if (deleteStatus)
				return RedirectToAction("Users", "Admin");

			return NotFound();
		}

		//public async Task<IActionResult> ViewPost(PostViewDTO postViewDTO)
		//{
		//	return View(postViewDTO);
		//}

		public async Task<IActionResult> ViewAllPosts()
		{
			var posts = await _postService.GetAllAsync();

			List<PostViewDTO> postViewDtos = new List<PostViewDTO>();

			foreach (var post in posts)
			{
				var postViewDTO = _mapper.Map<PostViewDTO>(post);

				//pictures;

				postViewDtos.Add(postViewDTO);
			}
			return View(postViewDtos);
		}

		//View all by post status;

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> CreateTag(TagCreateDTO tagCreateDTO)
		{
			if (ModelState.IsValid && !string.IsNullOrEmpty(tagCreateDTO.TagName))
			{
				var tag = _mapper.Map<Tag>(tagCreateDTO);
				await _tagService.CreateAsync(tag);
				return RedirectToAction("Index", "Home");
			}
			return View(tagCreateDTO);
		}

		//[HttpPost]
		//public async Task<IActionResult> ViewTag(TagViewDTO tagViewDTO)
		//{
		//	return View(tagViewDTO);
		//}

		[Authorize(Roles = "SuperAdmin, Admin")]
		[HttpPost]
		public async Task<IActionResult> CreateCategory(CategoryCreateDTO categoryCreateDTO)
		{
			if (ModelState.IsValid && !string.IsNullOrEmpty(categoryCreateDTO.CategoryName))
			{
				var category = _mapper.Map<Category>(categoryCreateDTO);
				await _categoryService.CreateAsync(category);
				return RedirectToAction("Index", "Home");
			}
			return View(categoryCreateDTO);
		}

		//[HttpPost]
		//public async Task<IActionResult> ViewCategory(CategoryViewDTO categoryViewDTO)
		//{
		//	return View(categoryViewDTO);
		//}

	}
}
