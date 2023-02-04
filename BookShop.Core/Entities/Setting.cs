using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
	public class Setting : Entity
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}
}
