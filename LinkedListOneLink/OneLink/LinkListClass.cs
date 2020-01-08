using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList.OneLink
{
	public class OneWayLinkList<T> : IEnumerable
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


		public OneWayLinkList(T data) => Add(data);
		public OneWayLinkList() { }

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
		public int Count { get; private set; } = 0;

		public OneWayLinkList<T> Sort<Tkey>(Func<T, Tkey> compare)
		{

			return null;
		}

		public OneWayLinkList<T> Add(T data)
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
		public OneWayLinkList<T> AddPrev(T data)
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
		public OneWayLinkList<T> RemoveFirst()
		{
			if (!IsEmpty)
			{
				head = +head;
				Count--;
			}
			return this;
		}
		public OneWayLinkList<T> Revers()
		{
			if (IsEmpty || IsOneItem) return this;
			if (Count == 2)
			{
				head = tail - !head;
				tail = +head;
				return this;
			}

			Item<T> prevItem = null;
			foreach (var item in Items)
			{
				if (prevItem == null)//head
				{
					//!item
					SleepNullNext(item);
					tail = prevItem = item;
					continue;
				}
				if (+item == null)//tail
				{
					//item - prevItem
					SleepMinusItem(item, prevItem);
					head = item;
					continue;
				}
				//item - prevItem
				SleepMinusItem(item, prevItem);

				prevItem = item;
			}
			InvokeActionStack();
			return this;
		}
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
						tail += newItem;
					}
					Count++;
					break;
				}
			}
			return this;
		}

		public OneWayLinkList<T> RemoveElement(T data)
		{
			if (IsEmpty) return this;
			if (IsOneItem) { RemoveAll(); return this; }
			if (head.Data.Equals(data)) { RemoveFirst(); return this; }
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
						tail = !item;
					}
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
