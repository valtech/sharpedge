using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;

namespace SharpEdge
{
	public sealed class EdgeCacheRule
	{
		private const string BaseRequestItemsCacheKey = "_!XEdgeIncludeUrl";

		private string _name;
		private int _duration;
		private List<IEdgeFilter> _filters = new List<IEdgeFilter>();
		private List<IEdgeIdentifier> _stamps = new List<IEdgeIdentifier>();
		private List<IEdgeStore> _stores = new List<IEdgeStore>();
		private bool _debug;
	    private readonly string _requestItemsCacheKey;

		public EdgeCacheRule(string name, int duration, bool debug)
		{
			_name = name;
			_duration = duration;
			_debug = debug;
		    _requestItemsCacheKey = BaseRequestItemsCacheKey + name;
		}

		internal void OnPostAuthorizeRequest(EdgeCacheEventArgs e)
		{
			if (PostAuthorizeRequest != null)
			{
				if (IncludeUrl(e.Context))
				{
					PostAuthorizeRequest(this, e);
				}
			}
		}

		internal void OnPostMapRequestHandler(EdgeCacheEventArgs e)
		{
			if (PostMapRequestHandler != null)
			{
				if (IncludeUrl(e.Context))
				{
					PostMapRequestHandler(this, e);
				}
			}
		}

		internal void OnPreSendRequestHeaders(EdgeCacheEventArgs e)
		{
			if (PreSendRequestHeaders != null)
			{
				if (IncludeUrl(e.Context))
				{
					PreSendRequestHeaders(this, e);
				}
			}
		}

		internal void OnResolveRequestCache(EdgeCacheEventArgs e)
		{
			if (ResolveRequestCache != null)
			{
				if (IncludeUrl(e.Context))
				{
					ResolveRequestCache(this, e);
				}
			}
		}

		internal void OnUpdateRequestCache(EdgeCacheEventArgs e)
		{
			if (UpdateRequestCache != null)
			{
				if (IncludeUrl(e.Context))
				{
					UpdateRequestCache(this, e);
				}
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public bool Debug
		{
			get
			{
				return _debug;
			}
		}

		private static bool IsCachable(HttpContext context)
		{
			bool isPostBack = String.Equals(context.Request.HttpMethod, "POST", StringComparison.InvariantCultureIgnoreCase);

			return !isPostBack;
		}

		public event EventHandler<EdgeCacheEventArgs> PostAuthorizeRequest;

		public event EventHandler<EdgeCacheEventArgs> PostMapRequestHandler;

		public event EventHandler<EdgeCacheEventArgs> PreSendRequestHeaders;

		public event EventHandler<EdgeCacheEventArgs> ResolveRequestCache;

		public event EventHandler<EdgeCacheEventArgs> UpdateRequestCache;

		public int Duration
		{
			get
			{
				return _duration;
			}
		}

		public void AddFilter(IEdgeFilter filter)
		{
			LocklessConcurrentAddToList(ref _filters, filter);
		}

		public void AddStore(IEdgeStore store)
		{
			store.Initialize(this);

			LocklessConcurrentAddToList(ref _stores, store);
		}

		public void AddIdentifier(IEdgeIdentifier stamp)
		{
			LocklessConcurrentAddToList(ref _stamps, stamp);
		}

		private static void LocklessConcurrentAddToList<T>(ref List<T> list, T value)
		{
			List<T> copy;
			List<T> original;

			do
			{
				original = list;

				copy = new List<T>(original);

				copy.Add(value);
			}
			while (Interlocked.CompareExchange<List<T>>(ref list, copy, original) != original);
		}

		public DateTime AbsoluteExpiry
		{
			get
			{
				return DateTime.Now.Add(TimeSpan.FromMinutes(Duration));
			}
		}

		public OriginUrl Url(HttpContext context)
		{
			return OriginUrl.CurrentUrl(context);
		}

		public string CacheKeyStamp(HttpContext context)
		{
			StringBuilder builder = new StringBuilder();

			foreach (IEdgeIdentifier stamp in Stamps())
			{
				string key = stamp.GetKey(context);

				if (key == null)
				{
					key = "";
				}

				if (builder.Length > 0)
				{
					builder.Append(":");
				}

				builder.Append(key);
			}

			return builder.ToString();
		}

		public void Clear()
		{
			foreach (IEdgeStore filter in Stores())
			{
				filter.Clear(this);
			}
		}

		internal bool IncludeUrl(HttpContext context)
		{
			if (!IsCachable(context))
			{
				return false;
			}

			OriginUrl url = Url(context);

            // Method can be called mutliple times in a request,
            // so cache "Include" result for this rule.
            object o = context.Items[_requestItemsCacheKey];

			if (o != null)
			{
				return o.Equals(true);
			}

			bool include = false;

			foreach (IEdgeFilter filter in Filters())
			{
				if (filter.Include(this, url))
				{
					include = true;

					break;
				}
			}

            context.Items[_requestItemsCacheKey] = include;

			return include;
		}

		private IEnumerable<IEdgeStore> Stores()
		{
			return _stores;
		}

		private IEnumerable<IEdgeFilter> Filters()
		{
			return _filters;
		}

		private IEnumerable<IEdgeIdentifier> Stamps()
		{
			return _stamps;
		}
	}
}
