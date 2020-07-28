using System;
using System.Collections.Generic;
using System.Text;
using Blog.Entities.DTOs.Account;
using Blog.Entities.DTOs.Post;

namespace Blog.Entities.DTOs
{
	public class UserPostsDTO
	{
		public UserViewDto UserViewDto { get; set; }

		public List<PostViewDTO> PostViewDTOs { get; set; }
	}
}
