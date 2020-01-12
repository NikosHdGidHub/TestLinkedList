using System;
using System.Collections.Generic;

namespace TreeLib
{
	public class TreeWords<T>
	{
		/// <summary>
		/// Корневой элемент
		/// </summary>
		private readonly Node<T> root = new Node<T>();
		/// <summary>
		/// Список существующих слов
		/// </summary>
		private readonly HashSet<string> hashKeyWords = new HashSet<string>();

		#region Public
		/// <summary>
		/// Количество слов в дереве
		/// </summary>
		public int Count => hashKeyWords.Count;
		/// <summary>
		/// Добавить новое слово и данные к нему
		/// </summary>
		/// <param name="keyWord">Слово</param>
		/// <param name="data">Данные</param>
		public void Add(string keyWord ,T data)
		{
			_ = keyWord ?? throw new ArgumentNullException(nameof(keyWord));

			root.AddChild(keyWord, data);
			hashKeyWords.Add(keyWord);
		}
		/// <summary>
		/// Удалить слово из дерева
		/// </summary>
		/// <param name="keyWord"></param>
		public void Remove(string keyWord)
		{
			_ = keyWord ?? throw new ArgumentNullException(nameof(keyWord));

			root.RemoveChild(keyWord);
			hashKeyWords.Remove(keyWord);
		}
		/// <summary>
		/// Проверяет существует ли слово в дереве
		/// </summary>
		/// <param name="keyWord"></param>
		/// <returns></returns>
		public bool Contains(string keyWord)
		{
			_ = keyWord ?? throw new ArgumentNullException(nameof(keyWord));

			return hashKeyWords.Contains(keyWord);
		}
		/// <summary>
		/// Проверяет существует ли слово в дереве, и возвращает его данные
		/// </summary>
		/// <param name="keyWord"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(string keyWord, out T value)
		{
			_ = keyWord ?? throw new ArgumentNullException(nameof(keyWord));

			value = default;
			if (!Contains(keyWord)) return false;

			value = root.GetValue(keyWord);
			return true;
		}
		/// <summary>
		/// Возвращает множество обрывков слов после префикса
		/// </summary>
		/// <param name="prefix">префикс</param>
		/// <returns></returns>
		public HashSet<string> SearchByPrefix(string prefix)
		{
			_ = prefix ?? throw new ArgumentNullException(nameof(prefix));

			return root.GetAllWords(prefix);
		}
		/// <summary>
		/// Возвращает множество слов начинающиеся на subString
		/// </summary>
		/// <param name="subString">начало слова</param>
		/// <returns></returns>
		public HashSet<string> Search(string subString)
		{
			_ = subString ?? throw new ArgumentNullException(nameof(subString));

			return root.GetAllWords(subString, string.Copy(subString));
		}
		#endregion
	}
}
