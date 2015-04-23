using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Star.WebPanel.Nancy.Authentication
{
	//https://github.com/damianh/Nancy.Authentication.Forms.Owin
	public interface IClaimsPrincipalLookup
	{
		Task<StarPrincipal> GetStarPrincipal(Guid identifier);
	}
}
