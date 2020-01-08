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
			
			Console.ReadLine();
		}
		
	}

	public class User
	{
		public void Put( int money)
		{
			Invoke(() =>
			{
				Money += money;
			});

		}
		private StackLinkList<Action> stack = new StackLinkList<Action>();
		public string Name { get; set; }
		public int Money { get; set; }

		public void Invoke(int n)
		{
			foreach (var action in stack - n)
			{
				action.Invoke();
			}
		}
		public void Invoke()
		{
			foreach (var action in !stack)
			{
				action.Invoke();
			}
		}
		public void Invoke(Action action) => stack += action;
	}
}
