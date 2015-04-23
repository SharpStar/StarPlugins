using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star.WebPanel.Models
{
	public class ApiPlayer
	{
		public string Name { get; set; }

		public string NameWithoutColor { get; set; }

		public int? AccountId { get; set; }

		public string AccountName { get; set; }
	}
}
