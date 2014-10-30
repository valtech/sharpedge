using System;
using System.Web;
using System.Web.Caching;

namespace SharpEdge
{
	public class OutputCacheEdgeStore : IEdgeStore
	{
		private const string DateTimeFormat = "dd/MMM/yyyy HH:mm:ss";
		private const string DependencyKey = "_!XEdgeDependency";

		void IEdgeStore.Clear(EdgeCacheRule rule)
		{
			UpdateCacheDependency(DependencyKey);
		}

		private void UpdateCacheDependency(string name)
		{
			HttpRuntime.Cache.Insert(name, new object(), null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
		}

		void IEdgeStore.Initialize(EdgeCacheRule rule)
		{
			UpdateCacheDependency(DependencyKey);
			
			rule.ResolveRequestCache += OnResolveRequestCache;

			rule.UpdateRequestCache += OnUpdateRequestCache;
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
				response.Write("\r\n<!--Served from edge at " + TimeStamp + "-->");

				response.AddHeader("X-Edge-Modified", TimeStamp);
			}

			object rawResponse = invoker.Call("GetSnapshot");

			string cacheKey = keyBuilder.GetResponseCacheKey(rule.CacheKeyStamp(context));

			CacheDependency dependency = new CacheDependency(null, new string[] { DependencyKey });

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
