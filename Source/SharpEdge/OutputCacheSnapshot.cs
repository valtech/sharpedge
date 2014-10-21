using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpEdge
{
	sealed class OutputCacheSnapshot
	{
		private object _response;
		private DateTime _date;

		public OutputCacheSnapshot(object response)
		{
			_response = response;
			_date = DateTime.Now;
		}

		public object Response
		{
			get
			{
				return _response;
			}
		}

		public DateTime Date
		{
			get
			{
				return _date;
			}
		}

		public int TotalMinutes
		{
			get
			{
				TimeSpan span = DateTime.Now - _date;

				return (int)span.TotalMinutes;
			}
		}

		public int TotalSeconds
		{
			get
			{
				TimeSpan span = DateTime.Now - _date;

				return (int)span.TotalSeconds;
			}
		}

		public string Description
		{
			get
			{
				TimeSpan span = DateTime.Now - _date;

				if (span.TotalMinutes > 60)
				{
					return String.Format("Cached at edge {0} hour(s) and {1} minute(s) ago", (int)Math.Floor(span.TotalHours), (int)span.Minutes);
				}
				else if (span.TotalSeconds > 60)
				{
					return String.Format("Cached at edge {0} minute(s) and {1} second(s) ago", (int)Math.Floor(span.TotalMinutes), (int)span.Seconds);
				}
				
				return String.Format("Cached at edge {0} second(s) ago", (int)span.TotalSeconds);
			}
		}
	}
}
