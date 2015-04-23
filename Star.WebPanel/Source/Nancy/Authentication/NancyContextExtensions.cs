using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Owin;

namespace Star.WebPanel.Nancy.Authentication
{
	//https://github.com/damianh/Nancy.Authentication.Forms.Owin
	public static class NancyContextExtensions
	{
		private const string ServerUser = "server.User";

		public static ClaimsPrincipal GetClaimsPrincipal(this NancyContext context)
		{
			var environment = context.Items[NancyMiddleware.RequestEnvironmentKey] as IDictionary<string, object>;
			if (environment == null || !environment.ContainsKey(ServerUser))
			{
				return null;
			}
			return environment[ServerUser] as ClaimsPrincipal;
		}
	}
}
