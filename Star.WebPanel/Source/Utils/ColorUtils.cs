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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Star.WebPanel.Utils
{
	public class ColorUtils
	{
		public static string Colorize(string message)
		{
			message = WebUtility.HtmlEncode(message);

			Match match;
			while ((match = Regex.Match(message, @"\^(\w+|#[\da-fA-F]{6});")).Success)
			{
				message = string.Format("{0}<font color='{1}'>{2}</font>",
					WebUtility.HtmlEncode(message.Substring(0, match.Index)), WebUtility.HtmlEncode(match.Groups[1].Value), 
					WebUtility.HtmlEncode(message.Substring(match.Index + match.Length)));
			}

			return message;
		}
	}
}
