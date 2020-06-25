using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class UserService : BaseService<User>, IUserService
	{
		private Repository<User> _repository;

		public UserService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<User>(blogContext);
		}

		public async Task<bool> ChangePassword(Guid userId, string newPassword)
		{
			var user = (await _repository.FindByConditionAsync(u => u.Id == userId)).FirstOrDefault();

			if (user != null)
			{
				byte[] newPasswordHash, newPasswordSalt;

				CreatePasswordHash(newPassword, out newPasswordHash, out newPasswordSalt);

				await _repository.UpdateEntryAsync(user, x => x.PasswordHash, x => x.PasswordSalt);

				return true;
			}

			return false;
		}

		public async Task<bool> ChangePassword(Guid userId, string newPassword, string oldPassword)
		{
			var user = (await _repository.FindByConditionAsync(u => u.Id == userId)).FirstOrDefault();

			if (user != null)
			{
				if (VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
				{
					byte[] newPasswordHash, newPasswordSalt;
					CreatePasswordHash(newPassword, out newPasswordHash, out newPasswordSalt);

					user.PasswordHash = newPasswordHash;
					user.PasswordSalt = newPasswordSalt;

					await _repository.UpdateEntryAsync(user, x => x.PasswordHash, x => x.PasswordSalt);
					return true;
				}
				else return false;
			}
			else
				return false;
		}

		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
			if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
			if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

			using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(storedHash);
			}
		}

		public async Task<User> CreateAsync(User user, string password)
		{
			if (string.IsNullOrWhiteSpace(password))
				throw new Exception("Password is required");

			if ((await _repository.FindByConditionAsync(u => u.Email == user.Email)).Any())
				throw new Exception($@"Email {user.Email} is already taken");

			byte[] passwordHash, passwordSalt;
			CreatePasswordHash(password, out passwordHash, out passwordSalt);

			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;

			_repository.Create(user);
			await _repository.SaveAsync();

			return user;
		}

		public override async Task<User> GetByIdAsync(Guid id)
		{
			User user = (await _repository.FindByConditionAsync(u => u.Id == id)).FirstOrDefault();

			return user;
		}
	}
}
