using System;
using System.Web;
using System.Web.Caching;

namespace SharpEdge
{
	public class OutputCacheEdgeStore : IEdgeStore
	{
		private const string DateTimeFormat = "dd/MMM/yyyy HH:mm:ss";
		private const string DependencyKeyPrefix = "_!XEdgeDependency:OutputCacheEdgeStore";

		void IEdgeStore.Clear(EdgeCacheRule rule)
		{
			UpdateCacheDependency(rule);
		}

		private void UpdateCacheDependency(EdgeCacheRule rule)
		{
			HttpRuntime.Cache.Insert(DependencyKeyFromRule(rule), new object(), null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
		}

		void IEdgeStore.Initialize(EdgeCacheRule rule)
		{
			UpdateCacheDependency(rule);
			
			rule.ResolveRequestCache += OnResolveRequestCache;

			rule.UpdateRequestCache += OnUpdateRequestCache;
		}

		private static string DependencyKeyFromRule(EdgeCacheRule rule)
		{
			return String.Format("{0}/{1}", DependencyKeyPrefix, rule.Name);
		}

		private void OnResolveRequestCache(object sender, EdgeCacheEventArgs e)
		{
			EdgeCacheRule rule = (EdgeCacheRule)sender;

			HttpContext context = e.Context;

			HttpResponse response = context.Response;

			HttpRequest request = context.Request;

			OutputCacheKeyBuilder keyBuilder = new OutputCacheKeyBuilder(context);

			OutputCacheSnapshot snapshot = null;

			foreach (string cacheKey in keyBuilder.EnumerateRequestCacheKeys(rule.CacheKeyStamp(context)))
			{
				snapshot = (OutputCacheSnapshot)HttpRuntime.Cache[cacheKey];

				if (snapshot != null)
				{
					break;
				}
			}

			if (snapshot != null && snapshot.Response != null)
			{
				bool sendBody = !String.Equals(request.HttpMethod, "HEAD", StringComparison.InvariantCultureIgnoreCase);

				new DynamicInvoke(response)
					.Call("UseSnapshot", snapshot.Response, sendBody);

				response.AddHeader("X-Edge", snapshot.Description);

				OnPreSendSnapshot(new EdgeCacheRuleEventArgs(e.Context, rule));

				context.ApplicationInstance.CompleteRequest();
			}
		}

		protected virtual void OnPreSendSnapshot(EdgeCacheRuleEventArgs args)
		{

		}

		private void OnUpdateRequestCache(object sender, EdgeCacheEventArgs e)
		{
			EdgeCacheRule rule = (EdgeCacheRule)sender;

			HttpContext context = e.Context;

			HttpResponse response = context.Response;

			HttpRequest request = context.Request;

			OutputCacheKeyBuilder keyBuilder = new OutputCacheKeyBuilder(context);

			if (response.StatusCode != 200)
			{
				return;
			}

			DynamicInvoke invoker = new DynamicInvoke(response);

			if (!(bool)invoker.Call("IsBuffered"))
			{
				return;
			}

			if (rule.Debug)
			{
				response.Write("\r\n<!--Served from edge at " + TimeStamp + " by rule " + rule.Name + "-->");

				response.AddHeader("X-Edge-Modified", TimeStamp);
				response.AddHeader("X-Edge-Rule", rule.Name);
			}

			object rawResponse = invoker.Call("GetSnapshot");

			string cacheKey = keyBuilder.GetResponseCacheKey(rule.CacheKeyStamp(context));

			CacheDependency dependency = new CacheDependency(null, new string[] { DependencyKeyFromRule(rule) });

			OutputCacheSnapshot snapshot = new OutputCacheSnapshot(rawResponse);

			DateTime expiry = rule.AbsoluteExpiry;

			HttpRuntime.Cache.Add(cacheKey, snapshot, dependency, expiry, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
		}

		private static string TimeStamp
		{
			get
			{
				return DateTime.Now.ToString(DateTimeFormat);
			}
		}
	}
}
