using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class ComplaintService : BaseService<Complaint>, IComplaintService
	{
		private Repository<Complaint> _repository;

		public ComplaintService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<Complaint>(blogContext);
		}

		public async override Task<Complaint> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(c => c.Id == id)).FirstOrDefault();
		}
	}
}
