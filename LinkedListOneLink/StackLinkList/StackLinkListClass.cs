﻿using System;
using System.Collections.Generic;

namespace LinkedList.StackLink
{
	public class StackLinkList<T>
	{
		private Item<T> First = null;
		
		public StackLinkList(T[] array) => Push(array);
		public StackLinkList() { }


		#region Публичные методы и свойства


		/// <summary>
		/// Достать несколько
		/// </summary>
		/// <param name="Count">Количество</param>
		/// <returns></returns>
		public IEnumerable<T> Pop(int count)
		{
			if (Count < 0) throw new IndexOutOfRangeException(nameof(count));
			for (int i = 0; i < count; i++)
			{
				if (IsEmpty) yield break;
				yield return Pop();
			}
		}
		/// <summary>
		/// Достать все
		/// </summary>
		/// <returns></returns>
		public IEnumerable<T> PopAll()
		{
			while (!IsEmpty)
			{
				yield return Pop();
			}
		}

		/// <summary>
		/// Пустой список.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				if (First == null && Count == 0)
					return true;
				if (First != null && Count > 0)
					return false;
				throw new MemberAccessException();
			}
		}
		/// <summary>
		/// Правда - содержит один элемент
		/// </summary>
		public bool IsOneItem
		{
			get
			{
				if (First != null && Count == 1)
					return true;
				if (IsEmpty)
					return false;
				return false;
			}
		}
		/// <summary>
		/// Количество элементов
		/// </summary>
		public int Count { get; private set; } = 0;

		/// <summary>
		/// Посмотреть
		/// </summary>
		public T Peek()
		{
			return First.Data;
		}
		/// <summary>
		/// Добавить несколько.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public StackLinkList<T> Push(T[] data)
		{
			foreach (var item in data)
			{
				Push(item);
			}
			return this;
		}
		/// <summary>
		/// Добавить.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public StackLinkList<T> Push(T data)
		{
			var item = new Item<T>(data);
			if (IsEmpty)
			{
				First = item;
			}
			else
			{
				First = item - First;
			}
			Count++;
			return this;
		}
		/// <summary>
		/// Удалить первый.
		/// </summary>
		/// <returns></returns>
		public T Pop()
		{
			var peekData = First.Data;
			if (!IsEmpty)
			{
				First = +First;
				Count--;
			}
			return peekData;
		}
		/// <summary>
		/// Удалить все
		/// </summary>
		public void RemoveAll()
		{
			First = null;
			Count = 0;
		}


		#endregion

	}
	public class Item<T>
	{
		public T Data { get; set; }
		public Item(T data) => Data = data;
		public Item<T> Next { get; set; }

		#region Operation

		/// <summary>
		/// привязывает левый к правому и возвращает правый
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Item<T> operator +(Item<T> left, Item<T> right)
		{
			if (left == null) throw new ArgumentNullException();
			left.Next = right;
			return right;
		}
		/// <summary>
		/// привязывает левый к правому и возвращает левый
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Item<T> operator -(Item<T> left, Item<T> right)
		{
			if (left == null) throw new ArgumentNullException();
			left.Next = right;
			return left;
		}
		/// <summary>
		/// возвращает Next
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static Item<T> operator +(Item<T> item)
		{
			if (item == null) throw new ArgumentNullException();

			return item.Next;
		}
		/// <summary>
		/// удаляет Next, возвращая себя
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static Item<T> operator !(Item<T> item)
		{
			if (item == null) throw new ArgumentNullException();
			item.Next = null;
			return item;
		}
		#endregion
	}
}
