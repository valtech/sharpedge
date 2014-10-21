using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SharpEdge.Configuration
{
	[ConfigurationCollection(typeof(EdgeFilterElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public sealed class EdgeFilterElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new EdgeFilterElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((EdgeFilterElement)element).TypeName;
		}
	}
}
