using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharpEdge.Configuration
{
	public sealed class EdgeCacheRuleElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base["name"];
			}
		}

		[ConfigurationProperty("duration", DefaultValue = 60)]
		public int Duration
		{
			get
			{
				return (int)base["duration"];
			}
		}

		[ConfigurationProperty("debug", DefaultValue = false)]
		public bool Debug
		{
			get
			{
				return (bool)base["debug"];
			}
		}

		[ConfigurationProperty("identifiers")]
		public EdgeIdentifierElementCollection Identifiers
		{
			get
			{
				return (EdgeIdentifierElementCollection)base["identifiers"];
			}
		}

		[ConfigurationProperty("filters")]
		public EdgeFilterElementCollection Filters
		{
			get
			{
				return (EdgeFilterElementCollection)base["filters"];
			}
		}

		[ConfigurationProperty("stores")]
		public EdgeStoreElementCollection Stores
		{
			get
			{
				return (EdgeStoreElementCollection)base["stores"];
			}
		}
	}
}
