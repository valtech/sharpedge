using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	public class RegexEdgeFilterSection : ConfigurationSection
	{
		[ConfigurationProperty("groups")]
		public RegexEdgeFilterGroupElementCollection Groups
		{
			get
			{
				return (RegexEdgeFilterGroupElementCollection)base["groups"];
			}
		}
	}
}
