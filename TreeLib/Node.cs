using System;
using System.Collections.Generic;

namespace TreeLib
{
	internal class Node<TValue>
	{
		//public char Symbol { get; }
		public bool IsEndWord { get; private set; }
		public TValue Data { get; private set; }
		public Dictionary<char, Node<TValue>> Children { get; } = new Dictionary<char, Node<TValue>>();


		#region Private
		private void SendErrorApp(string mes)
		{
			throw new Exception("Не правильная работа приложения: " + mes);
		}
		private void SetEndWord(TValue data)
		{
			if (IsEndWord) throw new Exception("Ключевое слово уже существует");

			Data = data;
			IsEndWord = true;
		}
		private void DeleteEndWord()
		{
			IsEndWord = false;
		}
		private HashSet<string> GetSubWords(string previusStr = "")
		{
			if (Children.Count == 0) return null;

			previusStr = string.Copy(previusStr);
			HashSet<string> result = new HashSet<string>();

			foreach (var item in Children)
			{
				var child = item.Value;
				var keyChar = item.Key;
				var previusStrChild = previusStr + keyChar;
				var childSubWords = child.GetSubWords(previusStrChild);
				//если ребенок был крайним
				if (childSubWords == null)
				{
					//если он был концом слова
					if (child.IsEndWord)
					{
						result.Add(previusStrChild);
						continue;
					}
					//здесь можно удалять элемент при желании
				}
				//если ребенок не был крайним, то он возвращает множество обрывков
				//объединяем полученные обрывки с нашим множеством
				result.UnionWith(childSubWords);
			}
			return result;
		}
		#endregion
		public void AddChild(string subWord, TValue data)
		{
			if (string.IsNullOrWhiteSpace(subWord)) return;

			var charKey = subWord[0];
			//проверить наличие текущего символа в детях
			if (!Children.TryGetValue(charKey, out Node<TValue> child))
			{
				child = new Node<TValue>();
				Children.Add(charKey, child);
			}
			//рекуррентный вызов
			if (subWord.Length > 1)
			{
				child.AddChild(subWord.Substring(1), data);
				return;
			}

			//остался последний символ -> выход из рекурсии			
			child.SetEndWord(data);
		}
		internal HashSet<string> GetAllWords(string subString)
		{
			return GetAllWords(subString, "");
		}
		internal HashSet<string> GetAllWords(string subString, string prefix)
		{
			//Если строка еще содержит символы, то вызываем рекуррентно у детей
			if (!string.IsNullOrWhiteSpace(subString))
			{
				var charKey = subString[0];
				//проверить наличие текущего символа в детях
				if (!Children.TryGetValue(charKey, out Node<TValue> child))
				{
					return null;
				}
				//рекуррентный вызов
				if (subString.Length > 1)
				{
					return child.GetAllWords(subString.Substring(1), prefix);
				}
				//остался последний символ -> выход из рекурсии	
				return child.GetSubWords(prefix);
			}
			//Если строка пустая, то начинаем поиск с этого элемента
			return GetSubWords(prefix);
		}

		internal TValue GetValue(string subWord)
		{
			if (string.IsNullOrWhiteSpace(subWord))
				throw new ArgumentNullException(subWord);

			var charKey = subWord[0];
			//проверить наличие текущего символа в детях
			if (!Children.TryGetValue(charKey, out Node<TValue> child))
			{
				SendErrorApp("Искомый ключ должен существовать");
			}
			//рекуррентный вызов
			if (subWord.Length > 1)
			{
				return child.GetValue(subWord.Substring(1));
			}

			//остался последний символ -> выход из рекурсии			
			if (!child.IsEndWord) SendErrorApp("Ребенок должен был быть последнем элементом");

			return child.Data;
		}

		internal void RemoveChild(string subWord)
		{
			if (string.IsNullOrWhiteSpace(subWord))
				throw new ArgumentNullException(subWord);

			var charKey = subWord[0];
			//проверить наличие текущего символа в детях
			if (!Children.TryGetValue(charKey, out Node<TValue> child))
			{
				SendErrorApp("Удаляемый ключ должен существовать");
			}
			//рекуррентный вызов
			if (subWord.Length > 1)
			{
				child.RemoveChild(subWord.Substring(1));
				return;
			}

			//остался последний символ -> выход из рекурсии			
			child.DeleteEndWord();
		}
	}
}
