using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapLib.Tests
{
	[TestClass()]
	public class HashTableTests
	{
		private const int Size = 100;
		private HashTable<int, string> HashTable { get; set; }
		public void UpdateHashTable()
		{
			var hashTable = new HashTable<int, string>(Size);
			hashTable.Add(0, "Нуль");
			hashTable.Add(1, "Один");
			hashTable.Add(113, "Три");
			hashTable.Add(13, "Четыре");
			hashTable.Add(14, "Пять");
			hashTable.Add(6, "Шесть");
			HashTable = hashTable;
		}

		[TestMethod()]
		public void AddTest()
		{
			UpdateHashTable();
			HashTable.Add(1222013,"Collision");
			Assert.AreEqual(5, HashTable.Count);

		}
		[TestMethod()]
		public void RemoveTest()
		{
			UpdateHashTable();

			HashTable.Remove(1);
			HashTable.Remove(113);
			HashTable.Remove(13);
			HashTable.Remove(6);

			Assert.AreEqual(2, HashTable.Count);
		}
		[TestMethod()]
		public void UpSizeTest()
		{
			var asss = new HashTable<string, string>(1);
			GC.Collect();
			for (int i = 0; i < 1500000; i++)
			{
				asss.Add("Key " + i, "Value " + i);
			}
			var stats = asss.GetStatsCollisions();
			var max = stats.MaxCollisions;
			var dict = stats.Stats;
			dict.TryGetValue(0, out Stats collisionStats_0);
			dict.TryGetValue(1, out Stats collisionStats_1);
			var PERCENT0 = Math.Round(collisionStats_0.Percent,2);
			var PERCENT1 = Math.Round(collisionStats_1.Percent,2);
			for (int i = 0; i < 1561; i++)
			{
				Assert.AreEqual("Value " + i, asss["Key " + i]);
			}
			
		}
		[TestMethod()]
		public void GetSetTest()
		{
			var hashT = new HashTable<string, int>(10);

			hashT.Add("1", 1);
			hashT.Add("2", 2);
			hashT.Add("3", 3);
			hashT.Add("4", 4);
			hashT.Add("5", 5);

			Assert.AreEqual(1, hashT["1"]);
			Assert.AreEqual(2, hashT["2"]);
			Assert.AreEqual(3, hashT["3"]);
			Assert.AreEqual(4, hashT["4"]);
			Assert.AreEqual(5, hashT["5"]);

			hashT["1"] = 99992;
			hashT["2"] = 99992;

			Assert.AreEqual(99992, hashT["1"]);
			Assert.AreEqual(99992, hashT["2"]);
		}
	}


}