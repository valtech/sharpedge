using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpEdge
{
	public sealed class EdgeCacheEventArgs : EventArgs
	{
		private HttpContext _context;

		public EdgeCacheEventArgs(HttpContext context)
		{
			_context = context;
		}

		public HttpContext Context
		{
			get
			{
				return _context;
			}
		}
	}
}
