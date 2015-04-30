﻿// SharpStar. A Starbound wrapper.
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
using Nancy.Security;
using Star.WebPanel.Nancy.Authentication;
using StarLib.Database;
using ServiceStack.OrmLite;
using StarLib.Database.Models;

namespace Star.WebPanel.Nancy
{
	public class StarUserMapper : IUserMapper, IClaimsPrincipalLookup
	{
		public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
		{
			using (StarDb db = new StarDb())
			{
				using (var conn = db.CreateConnection())
				{
					Account acct = conn.Single<Account>(new
					{
						InternalId = identifier
					});

					if (acct == null)
						return null;

					return acct.ToUserIdentity(identifier);
				}
			}
		}

		public Task<StarPrincipal> GetStarPrincipal(Guid identifier)
		{
			using (StarDb db = new StarDb())
			{
				using (var conn = db.CreateConnection())
				{
					Account acct = conn.Single<Account>(new
					{
						InternalId = identifier
					});

					if (acct == null)
						return Task.FromResult<StarPrincipal>(null);

					return Task.FromResult(new StarPrincipal(acct.ToUserIdentity(identifier)));
				}
			}
		}
	}

	public static class AccountExtensions
	{
		public static StarUserIdentity ToUserIdentity(this Account acct, Guid identifier)
		{
			var claims = acct.Permissions != null ? acct.Permissions.Where(p => p.Allowed).Select(p => p.Name).ToList() : new List<string>();
			
			return new StarUserIdentity { Id = acct.Id, Identifier = identifier, UserName = acct.Username,
				Banned = acct.Banned, Claims = claims };
        }
	}
}
