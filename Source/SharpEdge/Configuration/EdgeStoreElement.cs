using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	public sealed class EdgeStoreElement : ConfigurationElement
	{
		[ConfigurationProperty("type", IsRequired = true, IsKey = true)]
		public string TypeName
		{
			get
			{
				return (string)base["type"];
			}
		}
	}
}
