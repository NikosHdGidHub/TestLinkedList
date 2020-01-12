using System;
using System.Collections.Generic;

namespace TreeLib
{
	public class TreeWords<T>
	{
		private readonly Node<T> root = new Node<T>();
		private readonly HashSet<string> hashKeyWords = new HashSet<string>();
		#region Public
		public int Count => hashKeyWords.Count;
		public void Add(string keyWord ,T data)
		{
			_ = keyWord ?? throw new ArgumentNullException(nameof(keyWord));

			root.AddChild(keyWord, data);
			hashKeyWords.Add(keyWord);
		}
		public void Remove(string keyWord)
		{
			_ = keyWord ?? throw new ArgumentNullException(nameof(keyWord));

			root.RemoveChild(keyWord);
			hashKeyWords.Remove(keyWord);
		}
		public bool Contains(string keyWord)
		{
			_ = keyWord ?? throw new ArgumentNullException(nameof(keyWord));

			return hashKeyWords.Contains(keyWord);
		}
		public bool TryGetValue(string keyWord, out T value)
		{
			_ = keyWord ?? throw new ArgumentNullException(nameof(keyWord));

			value = default;
			if (!Contains(keyWord)) return false;

			value = root.GetValue(keyWord);
			return true;
		}
		public HashSet<string> SearchByPrefix(string subString)
		{
			_ = subString ?? throw new ArgumentNullException(nameof(subString));

			return root.GetAllWords(subString);
		}
		public HashSet<string> Search(string subString)
		{
			_ = subString ?? throw new ArgumentNullException(nameof(subString));

			return root.GetAllWords(subString, string.Copy(subString));
		}
		#endregion
	}
}
