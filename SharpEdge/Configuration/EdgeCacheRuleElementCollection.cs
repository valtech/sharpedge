using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	[ConfigurationCollection(typeof(EdgeCacheRuleElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public sealed class EdgeCacheRuleElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new EdgeCacheRuleElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((EdgeCacheRuleElement)element).Name;
		}
	}
}
