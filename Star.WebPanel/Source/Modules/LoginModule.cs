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
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Authentication.Forms;
using StarLib.Database;
using StarLib.Database.Models;
using StarLib.Security;

namespace Star.WebPanel.Modules
{
	public class LoginModule : NancyModule
	{
		public LoginModule() : base("/login")
		{
			Get["/"] = _ =>
			{
				if (Context.CurrentUser != null)
					return Response.AsRedirect("~/");

				return View["Index"];
			};

			Post["/"] = p =>
			{
				string username = Context.Request.Form.username;
				string password = Context.Request.Form.password;

				using (StarDb db = new StarDb())
				{
					Account account = db.GetAccountByUsername(username);

					if (account == null)
						return View["Index"];

					string hash = StarSecurity.GenerateHash(account.Username, password, Encoding.UTF8.GetBytes(account.PasswordSalt));

					if (account.PasswordHash == hash)
						return this.LoginAndRedirect(account.InternalId);
				}

				return View["Index"];
			};

		}
	}
}
