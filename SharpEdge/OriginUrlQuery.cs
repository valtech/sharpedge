using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace SharpEdge
{
	public sealed class OriginUrlQuery
	{
		private NameValueCollection _parameters;

		public OriginUrlQuery()
		{
		}

		public OriginUrlQuery(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}

			int queryIndex = url.IndexOf('?');

			if (queryIndex > 0)
			{
				int fragmentIndex = url.IndexOf('#', queryIndex);

				if (fragmentIndex > 0)
				{
					_parameters = HttpUtility.ParseQueryString(url.Substring(queryIndex + 1, fragmentIndex - queryIndex - 1));
				}
				else
				{
					_parameters = HttpUtility.ParseQueryString(url.Substring(queryIndex + 1));
				}
			}
		}

		public NameValueCollection Parameters
		{
			get
			{
				if (_parameters == null)
				{
					_parameters = new NameValueCollection();
				}

				return _parameters;
			}
		}

		public override string ToString()
		{
			if (Parameters.Count > 0)
			{
				StringBuilder builder = new StringBuilder();

				builder.Append("?");

				List<string> keyValuePairs = new List<string>();

				foreach (string key in Parameters.Keys)
				{
					string[] values = Parameters.GetValues(key);

					foreach (string value in values)
					{
						if (!String.IsNullOrEmpty(value))
						{
							string keyValuePair = String.Format("{0}={1}", key, HttpUtility.UrlEncode(value));

							keyValuePairs.Add(keyValuePair);
						}
					}
				}

				keyValuePairs.Sort(StringComparer.InvariantCultureIgnoreCase);

				foreach (string keyValuePair in keyValuePairs)
				{
					if (builder.Length > 1)
					{
						builder.Append("&");
					}

					builder.Append(keyValuePair);
				}

				return builder.ToString();
			}

			return "";
		}
	}
}
