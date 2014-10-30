using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SharpEdge
{
	public sealed class OriginUrl
	{
		private string _path;
		private string _fragment;
		private OriginUrlQuery _query;

		public OriginUrl(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}

			int queryIndex = url.IndexOf('?');

			int fragmentIndex = url.IndexOf('#', queryIndex > 0 ? queryIndex : 0);

			if (fragmentIndex > 0)
			{
				_fragment = url.Substring(fragmentIndex);
			}

			if (queryIndex > 0)
			{
				_path = url.Substring(0, queryIndex);
			}
			else
			{
				if (fragmentIndex > 0)
				{
					_path = url.Substring(0, fragmentIndex);
				}
				else
				{
					_path = url;
				}
			}

			if (_path != null)
			{
				_path = _path.TrimEnd('/');
			}

			_query = new OriginUrlQuery(url);
		}

		public static OriginUrl CurrentUrl(HttpContext context)
		{
			string url = context.Request.RawUrl;

			if (url.StartsWith(HttpRuntime.AppDomainAppVirtualPath))
			{
				url = "~/" + url.Substring(HttpRuntime.AppDomainAppVirtualPath.Length).TrimStart('/');
			}

			return new OriginUrl(url);
		}
		
		/// <summary>
		/// The query string portion of the URL.
		/// </summary>
		public OriginUrlQuery Query
		{
			get
			{
				return _query;
			}
		}

		/// <summary>
		/// The part of the URL before the query string.
		/// </summary>
		public string Path
		{
			get
			{
				return _path ?? "";
			}
			set
			{
				_path = value;
			}
		}

		/// <summary>
		/// The part of the URL after the anchor #.
		/// </summary>
		public string Fragment
		{
			get
			{
				return _fragment ?? "";
			}
			set
			{
				_fragment = value;
			}
		}

		/// <summary>
		/// The part of the URL before the anchor #.
		/// </summary>
		public string PathAndQuery
		{
			get
			{
				return Path + Query;
			}
		}

		/// <summary>
		/// Returns the full URL as a string.
		/// </summary>
		public override string ToString()
		{
			return PathAndQuery + Fragment;
		}

		/// <summary>
		/// Casts the full URL to a string.
		/// </summary>
		public static implicit operator string(OriginUrl url)
		{
			return url.ToString();
		}
	}
}
