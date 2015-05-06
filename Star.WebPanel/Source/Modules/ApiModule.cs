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
using Star.WebPanel.Models;
using StarLib;
using StarLib.Starbound;

namespace Star.WebPanel.Modules
{
    public class ApiModule : NancyModule
    {
        public ApiModule() : base("/api")
        {
            Get["/playersonline"] = _ => Response.AsJson(new
            {
                Players = StarMain.Instance.Server.Proxies.Count
            });

            Get["/players"] = p =>
            {
                var apiPlayers = StarMain.Instance.Server.Proxies.Where(x => x.Player != null).Select(x => x.Player).Select(ToApiPlayer);

                return Response.AsJson(apiPlayers);
            };

            Get["/player/{name}"] = p =>
            {
                var plrsWithName = StarMain.Instance.Server.Proxies.Where(x => x.Player != null && x.Player.Name != null 
                    && x.Player.Name.Equals(p.name, StringComparison.OrdinalIgnoreCase));

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
                AccountId = player.Account != null ? player.Account.Id : (int?)null,
                Uuid = player.Uuid.Id
            };
        }

    }
}