using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	[ConfigurationCollection(typeof(EdgeStoreElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public sealed class EdgeStoreElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new EdgeStoreElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((EdgeStoreElement)element).TypeName;
		}
	}
}
