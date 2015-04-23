using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
namespace Star.WebPanel.Nancy
{
	public class StarApiUserMapper : IUserMapper
	{
		public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
		{
			string apiKey = identifier.ToString();

			if (apiKey != StarWeb.WebConfig.ApiKey)
				return null;

			return new StarUserIdentity { UserName = "admin" };
		}
	}
}
