using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpEdge
{
	public interface IEdgeStore
	{
		void Clear(EdgeCacheRule rule);

		void Initialize(EdgeCacheRule rule);
	}
}
