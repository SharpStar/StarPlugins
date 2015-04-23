using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Security;
using Star.WebPanel.Nancy;

namespace Star.WebPanel.Modules
{
	public class SecureApiModule : NancyModule
	{
		public SecureApiModule() : base("/secure")
        {
			this.RequiresAuthentication();

			Get["/"] = _ =>
			{
				return "YAY!";
			};
		}
	}
}
