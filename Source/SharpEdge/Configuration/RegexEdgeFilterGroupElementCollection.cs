using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	[ConfigurationCollection(typeof(RegexEdgeFilterGroupElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class RegexEdgeFilterGroupElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new RegexEdgeFilterGroupElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RegexEdgeFilterGroupElement)element).Name;
		}
	}
}
