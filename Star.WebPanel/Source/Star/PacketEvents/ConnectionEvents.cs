using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Star.WebPanel.Hubs;
using StarLib.Events.Packets;
using StarLib.Packets;

namespace Star.WebPanel.Star.PacketEvents
{
	public class ConnectionEvents
	{
		[PacketEvent(PacketType.ConnectionSuccess, PacketEventType.AfterSent)]
		public void OnConnectionSuccess(PacketEvent evt)
		{			
			if (evt.Proxy.Player != null)
			{
				var hub = GlobalHost.ConnectionManager.GetHubContext<StarHub>();

				hub.Clients.All.playerJoined(evt.Proxy.Player.Name);
			}
		}

		[PacketEvent(PacketType.ClientDisconnect)]
		public void OnDisconnection(PacketEvent evt)
		{
			if (evt.Proxy.Player != null)
			{
				var hub = GlobalHost.ConnectionManager.GetHubContext<StarHub>();

				hub.Clients.All.playerLeft(evt.Proxy.Player.Name);
			}
		}

	}
}
