using LinkedList.StackLink;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LinkedList.StackLink.Tests
{
	[TestClass()]
	public class StackLinkListClassTests
	{
		private StackLinkList<int> GetList() => new StackLinkList<int>();
		private StackLinkList<int> GetFullList() => new StackLinkList<int>(new int[] { 5, 4, 3, 2, 1 });

		[TestMethod()]
		public void PeekTest()
		{
			var list = GetFullList();
			Assert.AreEqual(1, list.Peek());
			Assert.IsTrue(!list.IsEmpty);
		}

		[TestMethod()]
		public void PushTest()
		{
			var list = GetList();
			list.Push(11);
			Assert.AreEqual(11, list.Peek());
			Assert.IsTrue(!list.IsEmpty);
		}

		[TestMethod()]
		public void PopTest()
		{
			var list = GetFullList();
			Assert.AreEqual(1, list.Pop());
			Assert.AreEqual(2, list.Peek());
			Assert.IsTrue(!list.IsEmpty);
		}

		[TestMethod()]
		public void RemoveAllTest()
		{
			var list = GetFullList();
			list.RemoveAll();
			Assert.IsTrue(list.IsEmpty);
		}

		[TestMethod()]
		public void PushArrayTest()
		{
			var list = GetList();
			list.Push(new int[] { 3, 2, 1 });
			Assert.IsTrue(!list.IsEmpty);
			Assert.AreEqual(1, list.Peek());
		}

		[TestMethod()]
		public void PopArrTest()
		{
			var list = GetFullList();
			foreach(var item in list.Pop(3));
			Assert.AreEqual(4, list.Peek());
			Assert.AreEqual(2, list.Count);
			foreach (var item in list.PopAll());
			Assert.IsTrue(list.IsEmpty);
		}
	}
}