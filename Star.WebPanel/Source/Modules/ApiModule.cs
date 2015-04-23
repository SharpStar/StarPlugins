using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Star.WebPanel.Models;
using StarLib;
using StarLib.Starbound;

namespace Star.WebPanel.Modules
{
	public class ApiModule : NancyModule
	{
		public ApiModule() : base("/api")
		{
			Get["/playersonline"] = _ =>
			{
				return Response.AsJson(new
				{
					Players = StarMain.Instance.Server.Proxies.Count
				});
			};

			Get["/players"] = p =>
			{
				var apiPlayers = new List<ApiPlayer>();

				foreach (Player player in StarMain.Instance.Server.Proxies.Where(x => x.Player != null).Select(x => x.Player))
				{
					apiPlayers.Add(ToApiPlayer(player));
				}

				return Response.AsJson(apiPlayers);
			};

			Get["/player/{name}"] = p =>
			{
				var plrsWithName = StarMain.Instance.Server.Proxies.Where(x => x.Player != null && x.Player.Name.Equals(p.name, StringComparison.OrdinalIgnoreCase));

				return Response.AsJson(plrsWithName.Select(x => ToApiPlayer(x.Player)));
			};
		}

		private static ApiPlayer ToApiPlayer(Player player)
		{
			return new ApiPlayer
			{
				Name = player.Name,
				NameWithoutColor = player.NameWithoutColor,
				AccountName = player.Account != null ? player.Account.Username : string.Empty,
				AccountId = player.Account != null ? player.Account.Id : (int?)null
			};
        }

	}
}