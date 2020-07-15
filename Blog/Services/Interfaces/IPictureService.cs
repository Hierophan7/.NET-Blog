using System;
using System.Threading.Tasks;
using Blog.Entities.Models;

namespace Blog.Services.Interfaces
{
	public interface IPictureService : IBaseService<Picture>
	{
		Task<Picture> GetAvatarAsync(Guid userId);
		
	}
}
