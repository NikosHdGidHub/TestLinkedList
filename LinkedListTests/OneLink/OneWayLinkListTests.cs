using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinkedList.OneLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList.OneLink.Tests
{
	[TestClass()]
	public class OneWayLinkListTests
	{
		private OneWayLinkList<int> GetList() => new OneWayLinkList<int>();
		private OneWayLinkList<int> GetFullList()
		{
			var list = new OneWayLinkList<int>(-1);
			for (int i = 0; i < 5; i++)
			{
				list.Add(i);
			}
			return list;
		}


		[TestMethod()]
		public void AddTest()
		{
			var list = GetList();
			list.Add(1).Add(2);
			var arr = list.ToArray();
			Assert.AreEqual(1, arr[0]);
			Assert.AreEqual(2, arr[1]);
		}

		[TestMethod()]
		public void AddPrevTest()
		{
			var list = GetList();
			list.AddPrev(2).Add(3).AddPrev(1);
			var arr = list.ToArray();
			Assert.AreEqual(1, arr[0]);
			Assert.AreEqual(2, arr[1]);
			Assert.AreEqual(3, arr[2]);
		}

		[TestMethod()]
		public void RemoveFirstTest()
		{
			var list = GetFullList();
			list.RemoveFirst().RemoveFirst();
			var arr = list.ToArray();
			Assert.AreEqual(1, arr[0]);
		}

		[TestMethod()]
		public void ReversTest()
		{
			var list = GetFullList();
			list.Revers();
			var arr = list.ToArray();

			var list2 = GetList();
			list2.Add(1).Add(2);
			list2.Revers();
			var arr2 = list2.ToArray();

			Assert.AreEqual(4, arr[0]);
			Assert.AreEqual(3, arr[1]);
			Assert.AreEqual(2, arr[2]);
			Assert.AreEqual(1, arr[3]);
			Assert.AreEqual(0, arr[4]);
			Assert.AreEqual(-1, arr[5]);


			Assert.AreEqual(2, arr2[0]);
			Assert.AreEqual(1, arr2[1]);

		}

		[TestMethod()]
		public void AddAfterTest()
		{
			var list = GetFullList();
			//-1,0,100,1,2,103,3,4,105
			list.AddAfter(-1, 100).AddAfter(2, 103).AddAfter(4, 105);
			var arr = list.ToArray();
			Assert.AreEqual(100, arr[1]);
			Assert.AreEqual(103, arr[5]);
			Assert.AreEqual(105, arr[8]);
		}

		[TestMethod()]
		public void RemoveElementTest()
		{
			var list = GetFullList();
			//-1,0,1,2,3,4
			//0,1,3
			list.RemoveElement(-1).RemoveElement(4).RemoveElement(2);
			var arr = list.ToArray();
			Assert.AreEqual(0, arr[0]);
			Assert.AreEqual(1, arr[1]);
			Assert.AreEqual(3, arr[2]);
		}

		[TestMethod()]
		public void RemoveAllTest()
		{
			var list = GetFullList();
			list.RemoveAll();
			Assert.IsTrue(list.IsEmpty);
		}

		[TestMethod()]
		public void SortTest()
		{
			var listRand = GetList();
			//-1 до 4
			listRand.Add(2).Add(4).Add(-1).Add(1).Add(3).Add(0);
			listRand.Sort(data=> data);
			var list = GetFullList();
			var arrRand = listRand.ToArray();
			var arr = list.ToArray();
			for (int i = 0; i < arr.Length; i++)
			{
				//Assert.AreEqual(arr[i], arrRand[i]);
			}
		}
	}
}