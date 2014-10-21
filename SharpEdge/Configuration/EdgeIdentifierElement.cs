using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SharpEdge.Configuration
{
	public sealed class EdgeIdentifierElement : ConfigurationElement
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
