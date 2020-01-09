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




	[TestClass()]
	public class FastArrayListTests
	{

		public FastArrayList<int> ctor()
		{
			return new FastArrayList<int>(10);
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
			var arr = new FastArrayList<int>(5);
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
				arr.Append(i);
			}
			Assert.AreEqual(10, arr.Count);
			Assert.IsFalse(arr.IsEmpty);

			for (int i = 0; i < arr.Size; i++)
			{
				Assert.AreEqual(i, arr[i]);
			}
			arr.Clear();
			arr.Append(3);
			arr.Append(4);
			arr.Append(5);
			arr.Append(6);
			arr.Append(7);
			arr.Prepend(2);
			arr.Prepend(1);
			arr.Prepend(0);

			arr.Append(8);
			arr.Append(9);

			for (int i = 0; i < arr.Size; i++)
			{
				Assert.AreEqual(i, arr[i]);
			}

		}
		[TestMethod()]
		public void RemoveTest2()
		{
			var arr = ctor();
			Assert.IsTrue(arr.IsEmpty);
			Assert.AreEqual(10, arr.Size);
			arr.Clear();

			arr.Append(3);
			arr.Append(4);
			arr.Append(5);
			arr.Append(6);
			arr.Append(7);

			arr.Prepend(2);
			arr.Prepend(1);
			arr.Prepend(0);

			arr.Append(8);
			arr.Append(9);
			//0,1,2,3,4,5,6,7,8,9
			Assert.AreEqual(10, arr.Count);
			Assert.IsFalse(arr.IsEmpty);
			//0,1,2,4,5,6,7,8,9
			arr.RemoveAt(3);			
			Assert.AreEqual(9, arr.Count);
			Assert.AreEqual(0, arr[0]);
			Assert.AreEqual(1, arr[1]);
			Assert.AreEqual(2, arr[2]);
			Assert.AreEqual(4, arr[3]);
			Assert.AreEqual(5, arr[4]);
			Assert.AreEqual(6, arr[5]);
			Assert.AreEqual(7, arr[6]);
			Assert.AreEqual(8, arr[7]);
			Assert.AreEqual(9, arr[8]);
			//0,1,2,4,5,6,8,9
			arr.RemoveAt(6);
			Assert.AreEqual(8, arr.Count);
			Assert.AreEqual(0, arr[0]);
			Assert.AreEqual(1, arr[1]);
			Assert.AreEqual(2, arr[2]);
			Assert.AreEqual(4, arr[3]);
			Assert.AreEqual(5, arr[4]);
			Assert.AreEqual(6, arr[5]);
			Assert.AreEqual(8, arr[6]);
			Assert.AreEqual(9, arr[7]);
		}
		[TestMethod()]
		public void RemoveTest3()
		{
			var arr = ctor();
			Assert.IsTrue(arr.IsEmpty);
			Assert.AreEqual(10, arr.Size);
			arr.Clear();

			arr.Append(3);
			arr.Append(4);
			arr.Append(5);
			arr.Append(6);
			arr.Append(7);

			arr.Prepend(2);
			arr.Prepend(1);
			arr.Prepend(0);

			arr.Append(8);
			arr.Append(9);
			//0,1,2,3,4,5,6,7,8,9
			Assert.AreEqual(10, arr.Count);
			Assert.IsFalse(arr.IsEmpty);
			//0,1,2,4,5,6,7,8,9
			arr.Remove(3);
			Assert.AreEqual(9, arr.Count);
			Assert.AreEqual(0, arr[0]);
			Assert.AreEqual(1, arr[1]);
			Assert.AreEqual(2, arr[2]);
			Assert.AreEqual(4, arr[3]);
			Assert.AreEqual(5, arr[4]);
			Assert.AreEqual(6, arr[5]);
			Assert.AreEqual(7, arr[6]);
			Assert.AreEqual(8, arr[7]);
			Assert.AreEqual(9, arr[8]);
			//0,1,2,4,5,6,8,9
			arr.Remove(7);
			Assert.AreEqual(8, arr.Count);
			Assert.AreEqual(0, arr[0]);
			Assert.AreEqual(1, arr[1]);
			Assert.AreEqual(2, arr[2]);
			Assert.AreEqual(4, arr[3]);
			Assert.AreEqual(5, arr[4]);
			Assert.AreEqual(6, arr[5]);
			Assert.AreEqual(8, arr[6]);
			Assert.AreEqual(9, arr[7]);
		}
	}
}