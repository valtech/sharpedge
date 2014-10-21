using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpEdge
{
	public interface IEdgeFilter
	{
		bool Include(EdgeCacheRule rule, OriginUrl url);
	}
}
