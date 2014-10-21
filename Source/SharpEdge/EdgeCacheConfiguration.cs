using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using SharpEdge.Configuration;

namespace SharpEdge
{
	sealed class EdgeCacheConfiguration
	{
		private const string SectionName = "sharpEdge";

		public IEnumerable<EdgeCacheRule> EnumerateRules()
		{
			EdgeCacheSection section = GetSection();

			List<EdgeCacheRule> rules = new List<EdgeCacheRule>();

			if (section != null)
			{
				foreach (EdgeCacheRuleElement ruleElement in section.Rules)
				{
					EdgeCacheRule rule = new EdgeCacheRule(ruleElement.Name, ruleElement.Duration, ruleElement.Debug);

					foreach (EdgeFilterElement filterElement in ruleElement.Filters)
					{
						rule.AddFilter(InstantiateFromTypeName<IEdgeFilter>(filterElement.TypeName));
					}

					foreach (EdgeIdentifierElement identifierElement in ruleElement.Identifiers)
					{
						rule.AddIdentifier(InstantiateFromTypeName<IEdgeIdentifier>(identifierElement.TypeName));
					}

					foreach (EdgeStoreElement storeElement in ruleElement.Stores)
					{
						rule.AddStore(InstantiateFromTypeName<IEdgeStore>(storeElement.TypeName));
					}

					rules.Add(rule);
				}
			}

			return rules;
		}

		private static T InstantiateFromTypeName<T>(string typeName)
		{
			Type type = Type.GetType(typeName, true);

			return (T)Activator.CreateInstance(type);
		}

		private static EdgeCacheSection GetSection()
		{
			return (EdgeCacheSection)ConfigurationManager.GetSection(SectionName);
		}
	}
}
