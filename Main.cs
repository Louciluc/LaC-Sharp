using System.Net.Security;
using System.Numerics;

namespace LaC_Sharp
{
	public bool hey()
	{

	}
	public class LazyType<T>(Func<T> value) {
		public Func<T> Value = value;

		static public T operator +(LazyType<T> a, LazyType<T> b)
		{
			T o;
			T p;
			T l = o + p;
			return a.Value() + b.Value();
		}

	}

	public class LazyList<T>
	{
		public LazyType<T> 
	}
}
