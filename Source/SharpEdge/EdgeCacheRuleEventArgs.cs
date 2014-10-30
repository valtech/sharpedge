using System;
using System.Web;

namespace SharpEdge
{
	public class EdgeCacheRuleEventArgs : EdgeCacheEventArgs
	{
		private EdgeCacheRule _rule;

		public EdgeCacheRuleEventArgs(HttpContext context, EdgeCacheRule rule)
			: base(context)
		{
			_rule = rule;
		}

		public EdgeCacheRule Rule
		{
			get
			{
				return _rule;
			}
		}
	}
}
