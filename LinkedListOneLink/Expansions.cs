using LinkedList.OneLink;
using LinkedList.ReversibleLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
	public static class Expansions
	{
		public static OneWayLinkList<T> ToOneWayLinkList<T>(this T[] array)
		{
			var list = new OneWayLinkList<T>();
			foreach (var item in array)
			{
				list.Add(item);
			}
			return list;
		}
		public static TwoWayLinkList<T> ToTwoWayLinkList<T>(this T[] array)
		{
			var list = new TwoWayLinkList<T>();
			foreach (var item in array)
			{
				list.Add(item);
			}
			return list;
		}
	}
}
