using LinkedList.OneLink;
using System;

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

			var user = new User();

			Console.WriteLine("Проверка стека");
			Console.WriteLine("Money: " + user.Money);
			Console.WriteLine("Положите деньги");
			var money = int.Parse(Console.ReadLine());
			Put(user, money);
			Console.WriteLine();
			Console.WriteLine("Money: " + user.Money);
			Console.WriteLine("Положите деньги");
			money = int.Parse(Console.ReadLine());
			Put(user, money);
			Console.WriteLine();
			Console.WriteLine("Money: " + user.Money);
			Console.WriteLine("Положите деньги");
			money = int.Parse(Console.ReadLine());
			Put(user, money);
			Console.WriteLine();
			user.Invoke();
			Console.WriteLine("Result: " + user.Money);


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
