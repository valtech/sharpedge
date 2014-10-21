using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpEdge
{
	public sealed class OutputCacheKeyBuilder
	{
		private HttpContext _context;

		public OutputCacheKeyBuilder(HttpContext context)
		{
			_context = context;
		}

		public string GetResponseCacheKey(string stamp)
		{
			HttpResponse response = _context.Response;

			string encoding = (string)new DynamicInvoke(response)
				.Call("GetHttpHeaderContentEncoding");

			return BuildKey(stamp, encoding);
		}

		public IEnumerable<string> EnumerateRequestCacheKeys(string stamp)
		{
			foreach (string encoding in EnumerateAcceptEncodings())
			{
				yield return BuildKey(stamp, encoding);
			}

			yield return BuildKey(stamp, null);
		}

		private IEnumerable<string> EnumerateAcceptEncodings()
		{
			string vary = _context.Request.Headers["Accept-Encoding"];

			if (!String.IsNullOrEmpty(vary))
			{
				string[] parts = vary.Split(';', ',');

				foreach (string part in parts)
				{
					string encoding = part.Trim();

					if (encoding.Length > 0)
					{
						yield return part;
					}
				}
			}
		}

		private string BuildKey(string stamp, string encoding)
		{
			if (encoding == null)
			{
				encoding = "";
			}

			return String.Format("EDGE:{0}\\ENC:{1}",
				stamp.ToLowerInvariant(),
				encoding.ToLowerInvariant()
			);
		}
	}
}
