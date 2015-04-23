using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Star.WebPanel.Nancy.Authentication
{
	public class StarPrincipal : IPrincipal
	{

		public IIdentity Identity { get; set; }


		public bool IsInRole(string role)
		{
			return false;
		}

		public StarPrincipal(IIdentity identity)
		{
			Identity = identity;
		}
	}
}
