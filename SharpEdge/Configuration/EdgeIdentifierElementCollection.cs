using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SharpEdge.Configuration
{
	[ConfigurationCollection(typeof(EdgeIdentifierElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public sealed class EdgeIdentifierElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new EdgeIdentifierElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((EdgeIdentifierElement)element).TypeName;
		}
	}
}
