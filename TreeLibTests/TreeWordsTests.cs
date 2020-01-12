using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeLib.Tests
{
	[TestClass()]
	public class TreeWordsTests
	{
		private TreeWords<int> GetTree()
		{
			var dict = new TreeWords<int>();

			dict.Add("apple", 1);
			dict.Add("application", 2);
			dict.Add("фрукт 1", 3);
			return dict;
		}
		[TestMethod()]
		public void AddTest()
		{
			var dict = new TreeWords<int>();

			dict.Add("apple", 1);
			dict.Add("яблоко", 2);
			dict.Add("фрукт 1", 3);
			Assert.AreEqual(3, dict.Count);
		}

		[TestMethod()]
		public void RemoveTest()
		{
			var dict = GetTree();

			dict.Remove("apple");

			Assert.AreEqual(2, dict.Count);

			dict.Remove("фрукт 1"); 
			dict.Remove("application");

			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod()]
		public void ContainsTest()
		{
			var dict = GetTree();

			Assert.IsTrue(dict.Contains("apple"));
			Assert.IsTrue(dict.Contains("фрукт 1"));
			Assert.IsTrue(dict.Contains("application"));
			Assert.IsFalse(dict.Contains("фрукт 2"));
		}

		[TestMethod()]
		public void TryGetValueTest()
		{
			var dict = GetTree();

			var f1 = dict.TryGetValue("apple", out int value1);
			var f2 = dict.TryGetValue("фрукт 1", out int value2);
			var f22 = dict.TryGetValue("application", out int value22);
			var f3 = dict.TryGetValue("фрукт 2", out int value3);

			Assert.IsTrue(f1);
			Assert.IsTrue(f2);
			Assert.IsTrue(f22);
			Assert.IsFalse(f3);

			Assert.AreEqual(1, value1);
			Assert.AreEqual(3, value2);
			Assert.AreEqual(2, value22);
		}

		[TestMethod()]
		public void SearchTest()
		{
			var dict = GetTree();
			var set1 = dict.SearchByPrefix("");
			var set2 = dict.SearchByPrefix("appl");

			var set3 = dict.Search("");
			var set4 = dict.Search("appl");

			Assert.IsTrue(set1.Contains("application"));
			Assert.IsTrue(set1.Contains("фрукт 1"));
			Assert.IsTrue(set1.Contains("apple"));
			Assert.AreEqual(3, set1.Count);

			Assert.IsTrue(set2.Contains("ication"));
			Assert.IsTrue(set2.Contains("e"));
			Assert.AreEqual(2, set2.Count);

			Assert.IsTrue(set3.Contains("application"));
			Assert.IsTrue(set3.Contains("фрукт 1"));
			Assert.IsTrue(set3.Contains("apple"));
			Assert.AreEqual(3, set3.Count);

			Assert.IsTrue(set4.Contains("application"));
			Assert.IsTrue(set4.Contains("apple"));
			Assert.AreEqual(2, set4.Count);

		}
	}
}