using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.DTOs.Category;
using Blog.Entities.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;

namespace Blog.Controllers
{
	
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ICategoryService _categoryService;


		public CategoryController(IMapper mapper,
			ICategoryService categoryService)
		{
			_categoryService = categoryService;
			_mapper = mapper;
		}

		public IActionResult CreateCategory()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory(CategoryCreateDTO categoryCreateDTO)
		{
			if (ModelState.IsValid)
			{
				var category = _mapper.Map<Category>(categoryCreateDTO);

				await _categoryService.CreateAsync(category);

				return RedirectToAction("Index", "Home");
			}

			return View(categoryCreateDTO);
		}
		public async Task<IActionResult> ViewAllCategories()
		{
			var categories = await _categoryService.GetAllAsync();

			List<CategoryViewDTO> categoryViewDTOs = _mapper.Map<List<CategoryViewDTO>>(categories);

			return View(categoryViewDTOs);
		}

		[HttpPost]
		public async Task<IActionResult> SelectCategory(string Prefix)
		{
			var categories = await _categoryService.GetByPrefixAsync(Prefix);

			List<CategoryViewDTO> categoryViewDTOs = _mapper.Map<List<CategoryViewDTO>>(categories);
			
			var json = JsonSerializer.Serialize(categoryViewDTOs);

			return View(json);
		}

		[Route("api/category")]
		[Produces("application/json")]
		[HttpGet("search")]
		public async Task<IActionResult> Search()
		{
			try
			{
				string Prefix = HttpContext.Request.Query["term"].ToString();
				var categories = await _categoryService.GetByPrefixAsync(Prefix);
				List<CategoryViewDTO> categoryViewDTOs = _mapper.Map<List<CategoryViewDTO>>(categories);
				return Ok(categoryViewDTOs);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
