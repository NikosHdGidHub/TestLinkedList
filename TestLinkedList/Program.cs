using LinkedList.OneLink;
using LinkedList.ReversibleLink;
using LinkedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestLinkedList
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Hello");

			Console.WriteLine("OneList");
			var list1 = new OneWayLinkList<int>();
			
			for (int i = 0; i < 10; i++)
			{
				list1.Add(i);
			}
			list1.AddPrev(-1);
			list1.AddAfter(9, 10).AddAfter(0, -99).Add(11)//.AddBefore(0,-100).AddBefore(11,12).AddPrev(90).Add(111);
			; Console.WriteLine();
			foreach (var item in list1)
			{
				Console.WriteLine(item);
			}
			
			Console.ReadLine();
		}
	}
}
