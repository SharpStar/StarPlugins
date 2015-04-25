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
using Microsoft.Owin.Cors;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Cryptography;
using Owin;
using Star.WebPanel.Nancy;
using Star.WebPanel.Nancy.Authentication;

namespace Star.WebPanel
{
	public class Startup
	{
		public static readonly FormsAuthenticationConfiguration FormsConfiguration = new FormsAuthenticationConfiguration();

		public void Configuration(IAppBuilder app)
		{
			var configuration = new HubConfiguration { EnableDetailedErrors = true };

			StaticConfiguration.DisableErrorTraces = false;


			string pass = StarWeb.WebConfig.AuthPassword;
			byte[] salt = Convert.FromBase64String(StarWeb.WebConfig.AuthSalt);

			var cryptoConfig = new CryptographyConfiguration(new RijndaelEncryptionProvider(new PassphraseKeyGenerator(pass, salt)),
															new DefaultHmacProvider(new PassphraseKeyGenerator(pass, salt)));
			
			FormsConfiguration.RedirectUrl = "~/login";
			FormsConfiguration.CryptographyConfiguration = cryptoConfig;

			app.UseCors(CorsOptions.AllowAll);
			app.UseNancyAuth(FormsConfiguration, new StarUserMapper());
			app.MapSignalR("/star", configuration);
			app.UseNancy(opt =>
			{
				//opt.PerformPassThrough = context => context.Response.StatusCode == HttpStatusCode.NotFound;
				opt.Bootstrapper = new Bootstrapper();
			});

		}
	}
}
