using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList.OneLink
{
	public class OneWayLinkList<T> 
	{
		#region StackFunc
		private Action actionStack = null;
		private void InvokeActionStack()
		{
			actionStack();
		}
		/// <summary>
		/// !item
		/// </summary>
		/// <param name="item"></param>
		private void SleepNullNext(Item<T> item)
		{
			actionStack += () => _ = !item;
		}
		/// <summary>
		/// leftItem - rightItem
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		private void SleepMinusItem(Item<T> left, Item<T> right)
		{
			actionStack += () => _ = left - right;
		}
		#endregion


		private Item<T> New(T data) => new Item<T>(data);
		private IEnumerable<Item<T>> Items
		{
			get
			{
				var current = First;
				while (current != null)
				{
					yield return current;
					current = current.Next;
				}
			}
		}


		public OneWayLinkList(T data) => Add(data);
		public OneWayLinkList() { }

		#region Публичные методы и свойства

		/// <summary>
		/// Преобразовать в массив.
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			var array = new T[Count];
			var iter = 0;
			foreach (var item in Items)
			{
				array[iter++] = item.Data;
			}
			return array;
		}
		/// <summary>
		/// Пустой список.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				if (First == null && Last == null && Count == 0)
					return true;
				if (First != null && Last != null && Count > 0)
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
				if (First == Last && Count == 1)
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
		/// Первый
		/// </summary>
		public Item<T> First { get; private set; } = null;
		/// <summary>
		/// Последний
		/// </summary>
		public Item<T> Last { get; private set; } = null;


		public OneWayLinkList<T> Sort<Tkey>(Func<T, Tkey> compare)
		{

			return null;
		}
		/// <summary>
		/// Добавить в конец.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public OneWayLinkList<T> Add(T data)
		{
			var item = New(data);

			if (IsEmpty)
			{
				First = Last = item;
			}
			else
			{
				Last += item;
			}
			Count++;
			return this;
		}
		/// <summary>
		/// Добавить перед первым.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public OneWayLinkList<T> AddPrev(T data)
		{
			var item = New(data);
			if (IsEmpty)
			{
				First = Last = item;
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
		public OneWayLinkList<T> RemoveFirst()
		{
			if (!IsEmpty)
			{
				First = +First;
				Count--;
			}
			return this;
		}
		/// <summary>
		/// Развернуть лист
		/// </summary>
		/// <returns></returns>
		public OneWayLinkList<T> Revers()
		{
			if (IsEmpty || IsOneItem) return this;
			if (Count == 2)
			{
				First = Last - !First;
				Last = +First;
				return this;
			}

			Item<T> prevItem = null;
			foreach (var item in Items)
			{
				if (prevItem == null)//head
				{
					//!item
					SleepNullNext(item);
					Last = prevItem = item;
					continue;
				}
				if (+item == null)//tail
				{
					//item - prevItem
					SleepMinusItem(item, prevItem);
					First = item;
					continue;
				}
				//item - prevItem
				SleepMinusItem(item, prevItem);

				prevItem = item;
			}
			InvokeActionStack();
			return this;
		}
		/// <summary>
		/// Добавить после
		/// </summary>
		/// <param name="target">Цель.</param>
		/// <param name="data"></param>
		/// <returns></returns>
		public OneWayLinkList<T> AddAfter(T target, T data)
		{
			if (IsEmpty || IsOneItem)
			{
				Add(data);
				return this;
			}
			foreach (var item in Items)
			{
				if (item.Data.Equals(target))
				{
					var newItem = new Item<T>(data);
					if (+item != null)//not Tail
					{
						_ = item + (newItem - +item);
					}
					else // Tail
					{
						Last += newItem;
					}
					Count++;
					break;
				}
			}
			return this;
		}
		/// <summary>
		/// Удалить первое вхождение
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public OneWayLinkList<T> RemoveElement(T data)
		{
			if (IsEmpty) return this;
			if (IsOneItem) { RemoveAll(); return this; }
			if (First.Data.Equals(data)) { RemoveFirst(); return this; }
			foreach (var item in Items)
			{
				var possibleTarget = +item;
				if (possibleTarget.Data.Equals(data))
				{
					if (+possibleTarget != null)
					{
						_ = item + +possibleTarget;
					}
					else
					{
						Last = !item;
					}
					Count--;
					break;
				}
			}
			return this;
		}
		/// <summary>
		/// Удалить все
		/// </summary>
		public void RemoveAll()
		{
			First = null;
			Last = null;
			Count = 0;
		}
		
		/// <summary>
		/// Перебор всех данных
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			var current = First;
			while (current != null)
			{
				yield return current.Data;
				current = current.Next;
			}
		}

		#endregion

	}
	public class Item<T>
	{
		public T Data { get; set; }
		public Item(T data) => Data = data;
		public Item<T> Next { get; set; }

		
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
	}
}
