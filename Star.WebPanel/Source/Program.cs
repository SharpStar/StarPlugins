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
using Microsoft.Owin.Hosting;

namespace Star.WebPanel
{
	class Program
	{
		static void Main(string[] args)
		{
			//const string url = "http://+:8080";

			//using (WebApp.Start<Startup>(url))
			//{
			//	Console.WriteLine("Running on {0}", url);
			//	Console.WriteLine("Press enter to exit");
			//	Console.ReadLine();
			//}

			StarWeb web = new StarWeb();
			web.Load();

			Console.ReadLine();

			StarWeb.Shutdown();
		}
	}
}
