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
using Star.WebPanel.Hubs;
using Star.WebPanel.Utils;
using StarLib.Events.Packets;
using StarLib.Logging;
using StarLib.Packets;

namespace Star.WebPanel.Star.PacketEvents
{
	public class ConnectionEvents
	{
        private readonly IHubContext _hub = GlobalHost.ConnectionManager.GetHubContext<StarHub>();

        [PacketEvent(PacketType.ConnectionSuccess, PacketEventType.AfterSent)]
		public void OnConnectionSuccess(PacketEvent evt)
		{
			if (evt.Proxy.Player != null)
			{
				_hub.Clients.Group("chat").playerJoined(ColorUtils.Colorize(evt.Proxy.Player.Name));
			}
		}

		[PacketEvent(PacketType.ClientDisconnect)]
		public void OnDisconnection(PacketEvent evt)
		{
			if (evt.Proxy.Player != null)
			{
				_hub.Clients.Group("chat").playerLeft(ColorUtils.Colorize(evt.Proxy.Player.Name));
			}
		}

	}
}
