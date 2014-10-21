using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace SharpEdge
{
	public sealed class EdgeCache
	{
		private static object Locker = new object();
		private List<EdgeCacheRule> _rules;
		private static EdgeCache EdgeCacheSingleton;

		public EdgeCache(IEnumerable<EdgeCacheRule> rules)
		{
			if (rules != null)
			{
				_rules = rules.ToList();
			}
			else
			{
				_rules = new List<EdgeCacheRule>();
			}
		}

		public EdgeCache()
			: this(null)
		{

		}

		public IEnumerable<EdgeCacheRule> Rules()
		{
			return _rules;
		}

		public void AddRules(IEnumerable<EdgeCacheRule> rules)
		{
			LocklessConcurrentAddRangeToList(ref _rules, rules);
		}

		private static void LocklessConcurrentAddRangeToList<T>(ref List<T> list, IEnumerable<T> value)
		{
			List<T> copy;
			List<T> original;

			do
			{
				original = list;

				copy = new List<T>(original);

				copy.AddRange(value);
			}
			while (Interlocked.CompareExchange<List<T>>(ref list, copy, original) != original);
		}

		public void Clear()
		{
			foreach (EdgeCacheRule rule in _rules)
			{
				rule.Clear();
			}
		}

		public void Clear(string name)
		{
			foreach (EdgeCacheRule rule in _rules)
			{
				if (String.Equals(rule.Name, name, StringComparison.InvariantCultureIgnoreCase))
				{
					rule.Clear();
				}
			}
		}

		public static EdgeCache Current
		{
			get
			{
				if (EdgeCacheSingleton == null)
				{
					lock (Locker)
					{
						if (EdgeCacheSingleton == null)
						{
							EdgeCacheSingleton = new EdgeCache();
						}
					}
				}

				return EdgeCacheSingleton;
			}
		}

		internal void OnPostAuthorizeRequest(EdgeCacheEventArgs e)
		{
			foreach (EdgeCacheRule rule in _rules)
			{
				rule.OnPostAuthorizeRequest(e);
			}
		}

		internal void OnPostMapRequestHandler(EdgeCacheEventArgs e)
		{
			foreach (EdgeCacheRule rule in _rules)
			{
				rule.OnPostMapRequestHandler(e);
			}
		}

		internal void OnPreSendRequestHeaders(EdgeCacheEventArgs e)
		{
			foreach (EdgeCacheRule rule in _rules)
			{
				rule.OnPreSendRequestHeaders(e);
			}
		}

		internal void OnResolveRequestCache(EdgeCacheEventArgs e)
		{
			foreach (EdgeCacheRule rule in _rules)
			{
				rule.OnResolveRequestCache(e);
			}
		}

		internal void OnUpdateRequestCache(EdgeCacheEventArgs e)
		{
			foreach (EdgeCacheRule rule in _rules)
			{
				rule.OnUpdateRequestCache(e);
			}
		}
	}
}
