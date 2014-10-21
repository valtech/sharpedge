using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	public class RegexPatternElement : ConfigurationElement
	{
		[ConfigurationProperty("pattern", IsRequired = true, IsKey = true)]
		public string Pattern
		{
			get
			{
				return (string)base["pattern"];
			}
		}
	}
}
