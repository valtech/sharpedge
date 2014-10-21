using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpEdge
{
	sealed class DynamicInvoke
	{
		private object _instance;
		private Type _type;

		public DynamicInvoke(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;

			_type = instance.GetType();
		}

		public object Call(string methodName, params object[] parameters)
		{
			MethodInfo method = _type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

			if (method == null)
			{
				throw new Exception("Could not find method " + method + " on " + _type);
			}

			return method.Invoke(_instance, parameters);
		}
	}
}
