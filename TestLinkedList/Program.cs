using LinkedList.OneLink;
using LinkedList.ReversibleLink;
using LinkedList.StackLink;
using System;

namespace TestLinkedList
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Hello");

			var list = new StackLinkList<int>(new[] { 1, 2, 3, 4, 5 });
			foreach (var item in list.Pop(3))
			{
				Console.WriteLine(item);
			}
			foreach (var item in list.PopAll())
			{
				Console.WriteLine(item);
			}


			Console.ReadLine();
		}
		public static void Put(User user, int money)
		{
			var iter = 0;
			user.Invoke(() =>
			{
				
				Console.WriteLine(iter++);
				user.Money += money;
			});

		}
	}

	public class User
	{
		
		private Action stack = null;
		public string Name { get; set; }
		public int Money { get; set; }

		public void Invoke() => stack();
		public void Invoke(Action action) => stack += action;
	}
}
