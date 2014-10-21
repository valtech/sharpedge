using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	public class RegexEdgeFilterGroupElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base["name"];
			}
		}

		[ConfigurationProperty("include")]
		public RegexPatternElementCollection Include
		{
			get
			{
				return (RegexPatternElementCollection)base["include"];
			}
		}

		[ConfigurationProperty("exclude")]
		public RegexPatternElementCollection Exclude
		{
			get
			{
				return (RegexPatternElementCollection)base["exclude"];
			}
		}
	}
}
