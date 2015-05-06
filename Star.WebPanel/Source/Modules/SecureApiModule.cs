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
using Nancy.Authentication.Stateless;
using Nancy.ModelBinding;
using Nancy.Security;
using Star.WebPanel.Nancy;
using StarLib;
using StarLib.Extensions;
using StarLib.Logging;
using StarLib.Server;
using StarLib.Starbound;

namespace Star.WebPanel.Modules
{
    public class SecureApiModule : NancyModule
    {
        private static readonly StarApiUserMapper UserValidator = new StarApiUserMapper();

        public SecureApiModule() : base("/secure")
        {
            StatelessAuthentication.Enable(this, new StatelessAuthenticationConfiguration(ctx =>
            {
                if (!ctx.Request.Query.apikey.HasValue)
                    return null;

                Guid guid;
                if (!Guid.TryParse(ctx.Request.Query.apiKey, out guid))
                    return null;

                return UserValidator.GetUserFromIdentifier(guid, ctx);
            }));

            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                return HttpStatusCode.OK;
            };

            Post["kick"] = p =>
            {
                ApiKick kick = this.Bind<ApiKick>();

                StarProxy proxy = StarMain.Instance.Server.Proxies.Where(x => x.Player != null).SingleOrDefault(x => x.Player.Uuid.Id == kick.Uuid);
                proxy.Kick(kick.Reason ?? string.Empty);

                return Response.AsJson(new ApiResult { Success = true });
            };

            Post["warp"] = p =>
            {
                ApiWarp warp = this.Bind<ApiWarp>();

                StarProxy warpTo = StarMain.Instance.Server.Proxies.SingleOrDefault(x => x.Player != null && x.Player.Uuid.Id == warp.WarpTo);

                if (warpTo == null || warpTo.Player == null)
                    return Response.AsJson(new ApiResult { Error = "Could not find the player to warp to!" });

                Parallel.ForEach(warp.WarpPlayers, x =>
                {
                    StarProxy warpPlr = StarMain.Instance.Server.Proxies.SingleOrDefault(a => a.Player != null && a.Player.Uuid.Id == x);

                    if (warpPlr != null)
                        warpPlr.Player.WarpToPlayer(warpTo.Player);
                });

                return Response.AsJson(new ApiResult { Success = true });
            };
        }

        public class ApiWarp
        {
            public string WarpTo { get; set; }

            public string[] WarpPlayers { get; set; }
        }

        public class ApiKick
        {
            public string Uuid { get; set; }

            public string Reason { get; set; }
        }

        public class ApiResult
        {
            public string Error { get; set; }

            public bool Success { get; set; }
        }
    }
    
}
