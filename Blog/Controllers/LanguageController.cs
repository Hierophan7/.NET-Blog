using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Blog.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Entities.DTOs.Language;
using Blog.Services.Interfaces;

namespace Blog.Controllers
{
	[Authorize]

	public class LanguageController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ILanguageService _languageService;

		public LanguageController(IMapper mapper,
			ILanguageService languageService)
		{
			_mapper = mapper;
			_languageService = languageService;
		}

		[HttpGet]
		public IActionResult CreateLanguage()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateLanguage(LanguageCreateDTO languageCreateDTO)
		{
			if (ModelState.IsValid && !string.IsNullOrEmpty(languageCreateDTO.LanguageName))
			{
				var language = _mapper.Map<Language>(languageCreateDTO);
				await _languageService.CreateAsync(language);
				return RedirectToAction("Index", "Home");
			}
			return View(languageCreateDTO);
		}
	}
}
