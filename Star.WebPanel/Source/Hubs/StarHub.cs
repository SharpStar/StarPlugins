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
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Star.WebPanel.Nancy;
using Star.WebPanel.Utils;
using StarLib;
using StarLib.Database;
using StarLib.Database.Models;
using StarLib.Extensions;
using StarLib.Logging;
using StarLib.Server;

namespace Star.WebPanel.Hubs
{
	[HubName("StarHub")]
	public class StarHub : Hub
	{

		public void sendMessage(string message)
		{
			if (Context.User == null)
			{
				Clients.Caller.authError();

				return;
			}
			
			StarUserIdentity ident = (StarUserIdentity)Context.User.Identity;
			if (ident.Banned)
			{
				Clients.Caller.banned();

				return;
			}
			
			using (StarDb db = new StarDb())
			{
				Ban ban = db.GetBanByAccount(ident.Id);
				
				if (ban != null)
				{
					Clients.Caller.banned();

					return;
				}
			}

			Parallel.ForEach(StarMain.Instance.Server.Proxies, proxy =>
			{
				proxy.SendChatMessage(Context.User.Identity.Name, message);
			});

			Clients.Group("chat").chatReceived(ColorUtils.Colorize(Context.User.Identity.Name), ColorUtils.Colorize(message));
		}

		public void joinChat()
		{
			if (IsBanned())
			{
				Clients.Caller.banned();

				return;
			}

			Groups.Add(Context.ConnectionId, "chat");
		}

		private bool IsBanned()
		{
            if (Context.User == null)
                return false;

			StarUserIdentity ident = (StarUserIdentity)Context.User.Identity;

			return ident.Banned;
		}
	}
}
