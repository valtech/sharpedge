using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpEdge
{
	public class RegexEdgeFilterGroup
	{
		private List<Regex> _include;
		private List<Regex> _exclude;

		public RegexEdgeFilterGroup(List<Regex> include, List<Regex> exclude)
		{
			_include = include;
			_exclude = exclude;
		}

		public bool Include(OriginUrl url)
		{
			if (IsMatch(_include, url, false))
			{
				return !IsMatch(_exclude, url, false);
			}

			return false;
		}

		private static bool IsMatch(List<Regex> patterns, string url, bool defaultValue)
		{
			foreach (Regex regex in patterns)
			{
				if (regex.Match(url).Success)
				{
					return true;
				}
			}

			return defaultValue;
		}
	}
}
