﻿using System;
using System.Collections;

namespace LinkedList.OneLink
{
	public class OneWayLinkList<T> : IEnumerable
	{
		

		private Item<T> head = null;
		private Item<T> tail = null;

		private Item<T> New(T data) => new Item<T>(data);
		private void BruteForce(Predicate<Item<T>> action)
		{
			var current = head;
			while (current != null)
			{
				if (action(current)) break;
				current = current.Next;
			}
		}


		public OneWayLinkList(T data) => Add(data);
		public OneWayLinkList() { }

		public T[] ToArray()
		{
			var array = new T[Count];
			var iter = 0;
			BruteForce(item =>
			{
				array[iter++] = item.Data;
				return false;
			});
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
				var buffer = head;
				head = tail;
				tail = buffer;
				head.Next = tail;
				tail.Next = null;
				return this;
			}

			Item<T> prevItem = null;
			BruteForce(itemRef =>
			{
				var item = (Item<T>)itemRef.Clone();
				if (prevItem == null)//head
				{
					tail = prevItem = item;
					tail.Next = null;
					return false;
				}
				if (item.Next == null)//tail
				{
					item.Next = prevItem;
					head = item;
					return true;
				}
				item.Next = prevItem;



				prevItem = item;
				return false;
			});
			return this;
		}
		public OneWayLinkList<T> AddAfter(T target, T data)
		{
			if (IsEmpty || IsOneItem)
			{
				Add(data);
				return this;
			}
			BruteForce(item =>
			{
				if (item.Data.Equals(target))
				{
					var newItem = new Item<T>(data);
					if (item.Next != null)//not Tail
					{
						var next = item.Next;
						item.Next = newItem;
						newItem.Next = next;
					}
					else // Tail
					{
						tail = tail.Next = newItem;
					}
					Count++;
					return true;
				}
				return false;
			});
			return this;
		}
		
		public OneWayLinkList<T> RemoveElement(T data)
		{
			if (IsEmpty) return this;
			if (IsOneItem) { RemoveAll(); return this; }
			if (head.Data.Equals(data)) { RemoveFirst(); return this; }
			BruteForce(itemRef =>
			{
				if (itemRef.Next.Data.Equals(data))
				{
					if (itemRef.Next.Next != null)
					{
						itemRef.Next = itemRef.Next.Next;						
					}
					else
					{
						itemRef.Next = null;
						tail = itemRef;
					}
					Count--;
					return true;
				}
				return false;
			});
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
	internal class Item<T> : ICloneable
	{
		public T Data { get; set; }
		public Item(T data) => Data = data;
		public Item<T> Next { get; set; }

		public object Clone()
		{
			return MemberwiseClone();
		}
		/// <summary>
		/// привязывает левый к правому и возвращает правый
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Item<T> operator +(Item<T> left, Item<T> right)
		{
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
			return item.Next;
		}
	}
}
