using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpEdge
{
	public sealed class OriginUrlEdgeIdentifier : IEdgeIdentifier
	{
		private const string Html5Device = "HTML5";

		public string GetKey(HttpContext context)
		{
			HttpRequest request = context.Request;

			StringBuilder key = new StringBuilder();

			OriginUrl url = OriginUrl.CurrentUrl(context);

			key.Append(request.HttpMethod);
			key.Append(";");
			key.Append(url.Path);
			key.Append(";");
			key.Append(GetDeviceKey(context));

			return key.ToString().ToLowerInvariant();
		}

		private string GetDeviceKey(HttpContext context)
		{
			string browserName = context.Request.Browser.Browser;

			if (String.Equals(browserName, "IE", StringComparison.InvariantCultureIgnoreCase))
			{
				return Html5Device;
			}
			else if (String.Equals(browserName, "Firefox", StringComparison.InvariantCultureIgnoreCase))
			{
				return Html5Device;
			}
			else if (browserName.IndexOf("Safari", StringComparison.InvariantCultureIgnoreCase) != -1)
			{
				return Html5Device;
			}
			else if (browserName.IndexOf("Explorer", StringComparison.InvariantCultureIgnoreCase) != -1 && browserName.IndexOf("Internet", StringComparison.InvariantCultureIgnoreCase) != -1)
			{
				return Html5Device;
			}
			else if (String.Equals(browserName, "Chrome", StringComparison.InvariantCultureIgnoreCase))
			{
				return Html5Device;
			}
			else if (String.Equals(browserName, "Opera", StringComparison.InvariantCultureIgnoreCase))
			{
				return Html5Device;
			}

			return "";
		}
	}
}
