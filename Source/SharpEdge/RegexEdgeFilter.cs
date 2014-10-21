using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SharpEdge.Configuration;

namespace SharpEdge
{
	public class RegexEdgeFilter : IEdgeFilter
	{
		private const string SectionName = "SharpEdge/regexEdgeFilter";

		private ConcurrentDictionary<string, RegexEdgeFilterGroup>
			_groups = new ConcurrentDictionary<string, RegexEdgeFilterGroup>(StringComparer.OrdinalIgnoreCase);

		public RegexEdgeFilter()
		{
			RegexEdgeFilterSection section = (RegexEdgeFilterSection)ConfigurationManager.GetSection(SectionName);

			if (section != null)
			{
				foreach (RegexEdgeFilterGroupElement groupElement in section.Groups)
				{
					List<Regex> include = CreatePatternList(groupElement.Include);
					List<Regex> exclude = CreatePatternList(groupElement.Exclude);

					_groups[groupElement.Name] = new RegexEdgeFilterGroup(include, exclude);
				}
			}
		}

		public bool Include(EdgeCacheRule rule, OriginUrl url)
		{
			RegexEdgeFilterGroup group;

			if (_groups.TryGetValue(rule.Name, out group))
			{
				return group.Include(url);
			}
			
			return false;
		}

		private static List<Regex> CreatePatternList(RegexPatternElementCollection patterns)
		{
			return CreatePatternEnumerable(patterns).ToList();
		}

		private static IEnumerable<Regex> CreatePatternEnumerable(RegexPatternElementCollection patterns)
		{
			foreach (RegexPatternElement pattern in patterns)
			{
				yield return new Regex(pattern.Pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
			}
		}
	}
}
