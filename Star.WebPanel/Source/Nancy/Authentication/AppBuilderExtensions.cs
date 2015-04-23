using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Authentication.Forms;
using Owin;

namespace Star.WebPanel.Nancy.Authentication
{
	//https://github.com/damianh/Nancy.Authentication.Forms.Owin
	public static class AppBuilderExtensions
	{
		public static IAppBuilder UseNancyAuth(this IAppBuilder builder, FormsAuthenticationConfiguration formsAuthenticationConfiguration, IClaimsPrincipalLookup claimsPrincipalLookup)
		{
			builder.Use(typeof(NancyFormsAuthMiddleware), formsAuthenticationConfiguration, claimsPrincipalLookup);
			return builder;
		}
	}
}
