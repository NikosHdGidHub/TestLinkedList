using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinkedList.ArrayList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList.ArrayList.Tests
{
	[TestClass()]
	public class SimpleArrayListTests
	{

		public SimpleArrayList<int> ctor()
		{
			return new SimpleArrayList<int>(10);
		}

		[TestMethod()]
		public void AppendTest()
		{
			var arr = ctor();
			Assert.IsTrue(arr.IsEmpty);
			arr.Append(10);
			arr.Append(11);
			Assert.AreEqual(2, arr.Count);
			Assert.IsFalse(arr.IsEmpty);
			Assert.AreEqual(10, arr.Size);
			Assert.AreEqual(10, arr.First);
			Assert.AreEqual(11, arr.Last);

		}

		[TestMethod()]
		public void PrependTest()
		{
			var arr = ctor();
			Assert.IsTrue(arr.IsEmpty);
			Assert.AreEqual(10, arr.Size);

			arr.Prepend(-19);
			arr.Prepend(-20);
			Assert.AreEqual(2, arr.Count);
			Assert.IsFalse(arr.IsEmpty);
			Assert.AreEqual(-20, arr.First);
			Assert.AreEqual(-19, arr.Last);
		}

		[TestMethod()]
		public void RemoveTest()
		{
			var arr = ctor();
			Assert.IsTrue(arr.IsEmpty);
			Assert.AreEqual(10, arr.Size);

			arr.Prepend(-19);
			arr.Prepend(-20);//-
			arr.Append(10);
			arr.Append(11);//-
						   //-20,-19,10,11
			arr.RemoveFirst();
			arr.RemoveLast();

			Assert.AreEqual(2, arr.Count);
			Assert.IsFalse(arr.IsEmpty);
			Assert.AreEqual(-19, arr.First);
			Assert.AreEqual(10, arr.Last);

			arr.Clear();
			Assert.IsTrue(arr.IsEmpty);
		}

		[TestMethod()]
		public void IsWriteTest()
		{
			var arr = new SimpleArrayList<int>(5);
			Assert.IsTrue(arr.IsEmpty);
			Assert.AreEqual(5, arr.Size);

			Assert.IsTrue(arr.IsAvailableWrite);
			arr.Append(1);
			arr.Prepend(2);
			arr.Append(3);
			arr.Append(4);
			Assert.IsTrue(arr.IsAvailableWrite);
			arr.Append(5);
			
			Assert.IsFalse(arr.IsAvailableWrite);
			arr.Append(5);
			arr.Append(5);
			arr.RemoveLast();
			Assert.IsTrue(arr.IsAvailableWrite);
		}
		[TestMethod()]
		public void IndexatorTest()
		{
			var arr = ctor();
			Assert.IsTrue(arr.IsEmpty);
			Assert.AreEqual(10, arr.Size);

			for (int i = 0; i < arr.Size; i++)
			{
				arr[i] = i;
			}
			Assert.AreEqual(10, arr.Count);
			Assert.IsFalse(arr.IsEmpty);

			for (int i = 0; i < arr.Size; i++)
			{
				Assert.AreEqual(i, arr[i]);
			}
			



		}
	}
}