using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpEdge
{
	public sealed class ClientCacheEdgeStore : IEdgeStore
	{
		void IEdgeStore.Clear(EdgeCacheRule rule)
		{
			
		}

		void IEdgeStore.Initialize(EdgeCacheRule rule)
		{
			//Needs to be here otherwise won't get added to snapshot for output cached versions
			rule.PreSendRequestHeaders += OnPreSendRequestHeaders;
		}

		private void OnPreSendRequestHeaders(object sender, EdgeCacheEventArgs e)
		{
			EdgeCacheRule rule = (EdgeCacheRule)sender;

			HttpContext context = e.Context;

			int minutes = rule.Duration;

			HttpResponse response = context.Response;

			HttpCachePolicy policy = response.Cache;

			policy.SetMaxAge(TimeSpan.FromMinutes(minutes));
			policy.SetCacheability(HttpCacheability.ServerAndPrivate);
			policy.SetSlidingExpiration(true);
		}
	}
}
