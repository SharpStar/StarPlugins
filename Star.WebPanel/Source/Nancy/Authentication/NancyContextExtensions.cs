// SharpStar. A Starbound wrapper.
// Copyright (C) 2015 Mitchell Kutchuk
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
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
