using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Entities.Models;

namespace Blog.Services.Interfaces
{
	public interface ICommentService : IBaseService<Comment>
	{
		//Task<IEnumerable<Comment>> GetCommentsForPost(Guid id);
	}
}
