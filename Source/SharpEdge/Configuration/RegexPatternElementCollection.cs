using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	[ConfigurationCollection(typeof(RegexPatternElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class RegexPatternElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new RegexPatternElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RegexPatternElement)element).Pattern;
		}
	}
}
