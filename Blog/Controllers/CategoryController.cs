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
	}
}
