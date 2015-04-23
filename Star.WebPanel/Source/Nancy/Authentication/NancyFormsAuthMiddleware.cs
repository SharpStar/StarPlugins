using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Nancy.Authentication.Forms;
using Nancy.Cookies;

namespace Star.WebPanel.Nancy.Authentication
{
	//https://github.com/damianh/Nancy.Authentication.Forms.Owin
	public class NancyFormsAuthMiddleware : OwinMiddleware
	{
		private const string ServerUser = "server.User";
		private readonly IClaimsPrincipalLookup _claimsPrincipalLookup;
		private readonly FormsAuthenticationConfiguration _formsAuthenticationConfiguration;

		public NancyFormsAuthMiddleware(OwinMiddleware next,
			FormsAuthenticationConfiguration formsAuthenticationConfiguration,
			IClaimsPrincipalLookup claimsPrincipalLookup) : base(next)
		{
			_formsAuthenticationConfiguration = formsAuthenticationConfiguration;
			_claimsPrincipalLookup = claimsPrincipalLookup;
		}

		public override async Task Invoke(IOwinContext context)
		{
			var requestHeaders = ((IDictionary<string, string[]>)context.Environment["owin.RequestHeaders"]);
			if (!requestHeaders.ContainsKey("Cookie"))
			{
				await Next.Invoke(context);
				return;
			}
			NancyCookie authCookie = GetFormsAuthCookies(requestHeaders["Cookie"]).SingleOrDefault();
			if (authCookie == null)
			{
				await Next.Invoke(context);
				return;
			}

			string user = FormsAuthentication.DecryptAndValidateAuthenticationCookie(authCookie.Value,
				_formsAuthenticationConfiguration);
			
			Guid userId;
			if (Guid.TryParse(user, out userId))
			{
				StarUserIdentity ident = context.Request.User != null ? context.Request.User.Identity as StarUserIdentity : null;
				
				//we don't want to keep checking the database if we already have the user authenticated
				if (ident != null && ident.Identifier == userId)
				{
					await Next.Invoke(context);

					return;
				}
				
				ClaimsPrincipal claimsPrincipal = await _claimsPrincipalLookup.GetClaimsPrincial(userId);

				context.Request.User = claimsPrincipal;
				//if (context.Environment.ContainsKey(ServerUser))
				//{
				//	context.Environment[ServerUser] = claimsPrincipal;
				//}
				//else
				//{
				//	context.Environment.Add(ServerUser, claimsPrincipal);
				//}
			}

			await Next.Invoke(context);
		}

		private IEnumerable<NancyCookie> GetFormsAuthCookies(IEnumerable<string> cookieHeaders)
		{
			return cookieHeaders
				.Select(h => h.Split(';'))
				.Select(header =>
					header.Select(c =>
					{
						string[] pair = c.Split('=');
						return new NancyCookie(pair[0].Trim(), pair[1]);
					})
					.SingleOrDefault(c => c.Name == FormsAuthentication.FormsAuthenticationCookieName));
		}
	}
}
