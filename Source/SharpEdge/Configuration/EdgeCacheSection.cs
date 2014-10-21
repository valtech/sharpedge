using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	public sealed class EdgeCacheSection : ConfigurationSection
	{
		[ConfigurationProperty("rules")]
		public EdgeCacheRuleElementCollection Rules
		{
			get
			{
				return (EdgeCacheRuleElementCollection)base["rules"];
			}
		}
	}
}
