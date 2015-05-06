using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Security;

namespace Star.WebPanel.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule()
        {
            this.RequiresClaims(new[] { "admin" });

            Get["/"] = _ => View["Index"];

        }
    }
}
