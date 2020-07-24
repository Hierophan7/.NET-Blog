using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Blog.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Entities.DTOs.Tag;
using Blog.Services.Interfaces;

namespace Blog.Controllers
{
	[Authorize]
	public class TagController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ITagService _tagService;

		public TagController(IMapper mapper,
			ITagService tagService)
		{
			_mapper = mapper;
			_tagService = tagService;
		}

		[HttpGet]
		public IActionResult CreateTag()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateTag(TagCreateDTO tagCreateDTO)
		{
			if (ModelState.IsValid && !string.IsNullOrEmpty(tagCreateDTO.Name))
			{
				var tag = _mapper.Map<Tag>(tagCreateDTO);
				await _tagService.CreateAsync(tag);
				return RedirectToAction("Index", "Home");
			}
			return View(tagCreateDTO);
		}
	}
}
