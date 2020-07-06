using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Models
{
	public partial class RoleClaim : IdentityRoleClaim<Guid>
	{

	}

	public partial class UserLogin : IdentityUserLogin<Guid>
	{

	}

	public partial class UserClaim : IdentityUserClaim<Guid>
	{

	}

	public partial class UserRole : IdentityUserRole<Guid>
	{

	}

	public partial class UserToken : IdentityUserToken<Guid>
	{

	}

	public partial class AppRole : IdentityRole<Guid>
	{

	}
}
