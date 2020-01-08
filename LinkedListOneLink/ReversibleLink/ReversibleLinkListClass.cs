using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList.ReversibleLink
{
	public class TwoWayLinkList<T> : IEnumerable
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

		private Item<T> head = null;
		private Item<T> tail = null;
		private Item<T> New(T data) => new Item<T>(data);

		private IEnumerable<Item<T>> Items
		{
			get
			{
				var current = head;
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
				var current = head;
				while (current != null)
				{
					yield return current;
					current = current.Next;
				}
			}
		}

		public TwoWayLinkList() : base() { }
		public TwoWayLinkList(T data) => Add(data);
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

		public bool IsEmpty
		{
			get
			{
				if (head == null && tail == null && Count == 0)
					return true;
				if (head != null && tail != null && Count > 0)
					return false;
				throw new MemberAccessException();
			}
		}
		public int Count { get; private set; }
		public bool IsOneItem
		{
			get
			{
				if (head == tail && Count == 1)
					return true;
				if (IsEmpty)
					return false;
				return false;
			}
		}

		public TwoWayLinkList<T> Add(T data)
		{
			var item = New(data);

			if (IsEmpty)
			{
				head = tail = item;
			}
			else
			{
				tail += item;
			}
			Count++;
			return this;
		}
		public TwoWayLinkList<T> AddPrev(T data)
		{
			var item = New(data);
			if (IsEmpty)
			{
				head = tail = item;
			}
			else
			{
				head = item - head;
			}
			Count++;
			return this;
		}
		public TwoWayLinkList<T> RemoveFirst()
		{
			if (!IsEmpty)
			{
				head = ~+head;
				Count--;
			}
			return this;
		}
		public TwoWayLinkList<T> RemoveLast()
		{
			if (!IsEmpty)
			{
				tail = !-tail;
				Count--;
			}
			return this;
		}

		public TwoWayLinkList<T> Revers()
		{
			if (IsEmpty || IsOneItem) return this;

			if (Count == 2)
			{
				head = ~tail - !head;
				tail = +head;
				return this;
			}

			Item<T> prevItem = null;

			foreach (var item in Items)
			{
				if (prevItem == null)//head
				{
					SleepNullNext(item);
					tail = prevItem = item;
					continue;
				}

				if (+item == null)//tail
				{
					SleepMinusItem(item, prevItem);
					head = item;
					continue;
				}
				SleepMinusItem(item, prevItem);
				prevItem = item;
			}
			InvokeActionStack();
			return this;
		}
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
						tail += newItem;
					}
					Count++;
					break;
				}
			}
			
			return this;
		}
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
						head = newItem - head;
					}
					Count++;
					break;
				}
			}
			return this;
		}

		public TwoWayLinkList<T> RemoveElement(T data)
		{
			if (IsEmpty) return this;
			if (IsOneItem) { RemoveAll(); return this; }
			if (head.Data.Equals(data)) { RemoveFirst(); return this; }
			if (tail.Data.Equals(data)) { RemoveLast(); return this; }
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
		public void RemoveAll()
		{
			head = null;
			tail = null;
			Count = 0;
		}

		public IEnumerator GetEnumerator()
		{
			var current = head;
			while (current != null)
			{
				yield return current.Data;
				current = current.Next;
			}
		}


	}
	internal class Item<T>
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
