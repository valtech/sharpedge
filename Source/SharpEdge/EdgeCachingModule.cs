using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpEdge
{
	public sealed class EdgeCacheModule : IHttpModule
	{
		private static object Locker = new object();
		private static EdgeCacheConfiguration ConfigurationSingleton;

		void IHttpModule.Dispose()
		{
			
		}

		void IHttpModule.Init(HttpApplication application)
		{
			EdgeCache edgeCache = EdgeCache.Current;

			if (ConfigurationSingleton == null)
			{
				lock (Locker)
				{
					if (ConfigurationSingleton == null)
					{
						ConfigurationSingleton = new EdgeCacheConfiguration();

						edgeCache.AddRules(ConfigurationSingleton.EnumerateRules());
					}
				}
			}

			application.PostAuthorizeRequest += (sender, e) =>
			{
				edgeCache.OnPostAuthorizeRequest(new EdgeCacheEventArgs(application.Context));
			};

			application.PostMapRequestHandler += (sender, e) =>
			{
				edgeCache.OnPostMapRequestHandler(new EdgeCacheEventArgs(application.Context));
			};

			application.PreSendRequestHeaders += (sender, e) =>
			{
				edgeCache.OnPreSendRequestHeaders(new EdgeCacheEventArgs(application.Context));
			};

			application.ResolveRequestCache += (sender, e) =>
			{
				edgeCache.OnResolveRequestCache(new EdgeCacheEventArgs(application.Context));
			};

			application.UpdateRequestCache += (sender, e) =>
			{
				edgeCache.OnUpdateRequestCache(new EdgeCacheEventArgs(application.Context));
			};
		}
	}
}
