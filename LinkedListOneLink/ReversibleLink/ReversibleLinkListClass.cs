using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList.ReversibleLink
{
	public class TwoWayLinkList<T>
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
		private IEnumerable<Item<T>> ItemsPrev
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

		public TwoWayLinkList() : base() { }
		public TwoWayLinkList(T data) => Add(data);
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
		/// Количество элементов
		/// </summary>
		public int Count { get; private set; }
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
		/// <summary>
		/// Правда - содержит один элемент
		/// </summary>
		}
		/// <summary>
		/// Первый
		/// </summary>
		public Item<T> First { get; private set; } = null;
		/// <summary>
		/// Последний
		/// </summary>
		public Item<T> Last { get; private set; } = null;
		/// <summary>
		/// Добавить в конец.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public TwoWayLinkList<T> Add(T data)
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
		public TwoWayLinkList<T> AddPrev(T data)
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
		public TwoWayLinkList<T> RemoveFirst()
		{
			if (!IsEmpty)
			{
				First = ~+First;
				Count--;
			}
			return this;
		}
		/// <summary>
		/// Удалить последний.
		/// </summary>
		/// <returns></returns>
		public TwoWayLinkList<T> RemoveLast()
		{
			if (!IsEmpty)
			{
				Last = !-Last;
				Count--;
			}
			return this;
		}

		/// <summary>
		/// Развернуть лист
		/// </summary>
		/// <returns></returns>
		public TwoWayLinkList<T> Revers()
		{
			if (IsEmpty || IsOneItem) return this;

			if (Count == 2)
			{
				First = ~Last - !First;
				Last = +First;
				return this;
			}

			Item<T> prevItem = null;

			foreach (var item in Items)
			{
				if (prevItem == null)//head
				{
					SleepNullNext(item);
					Last = prevItem = item;
					continue;
				}

				if (+item == null)//tail
				{
					SleepMinusItem(item, prevItem);
					First = item;
					continue;
				}
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
		public TwoWayLinkList<T> AddAfter(T target, T data)
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
					if (item.Next != null)//not Tail
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
		/// Добавить перед
		/// </summary>
		/// <param name="target">Цель.</param>
		/// <param name="data"></param>
		/// <returns></returns>
		public TwoWayLinkList<T> AddBefore(T target, T data)
		{
			if (IsEmpty)
			{
				Add(data);
				return this;
			}
			if (IsOneItem)
			{
				AddPrev(data); return this;
			}
			foreach (var item in ItemsPrev)
			{
				if (item.Data.Equals(target))
				{
					var newItem = new Item<T>(data);
					if (-item != null)//not Tail
					{
						_ = -item + newItem + item;
					}
					else // Tail
					{
						First = newItem - First;
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
		public TwoWayLinkList<T> RemoveElement(T data)
		{
			if (IsEmpty) return this;
			if (IsOneItem) { RemoveAll(); return this; }
			if (First.Data.Equals(data)) { RemoveFirst(); return this; }
			if (Last.Data.Equals(data)) { RemoveLast(); return this; }
			foreach (var item in Items)
			{
				if (item.Data.Equals(data))
				{
					_ = -item + +item;

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


	}
	public class Item<T>
	{
		public Item(T data) => Data = data;
		public Item<T> Prev { get; set; }
		public Item<T> Next { get; set; }
		public T Data { get; set; }
		

		/// <summary>
		/// привязывает левый к правому и возвращает правый
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Item<T> operator +(Item<T> left, Item<T> right)
		{
			if (left == null) throw new ArgumentNullException();
			if (right == null) throw new ArgumentNullException();
			left.Next = right;
			right.Prev = left;
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
			if (right == null) throw new ArgumentNullException();
			left.Next = right;
			right.Prev = left;
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
		/// возвращает Prev
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static Item<T> operator -(Item<T> item)
		{
			if (item == null) throw new ArgumentNullException();

			return item.Prev;
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
		/// <summary>
		/// удаляет Prev, возвращая себя
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static Item<T> operator ~(Item<T> item)
		{
			if (item == null) throw new ArgumentNullException();
			item.Prev = null;
			return item;
		}
	}
}
