using System;
using System.Collections;

namespace LinkedList.ReversibleLink
{
	public class TwoWayLinkList<T> : IEnumerable
	{
		private Item<T> head = null;
		private Item<T> tail = null;
		private Item<T> New(T data) => new Item<T>(data);


		private void BruteForceNext(Predicate<Item<T>> action)
		{
			var current = head;
			while (current != null)
			{
				if (action(current)) break;
				current = current.Next;
			}
		}
		private void BruteForcePrev(Predicate<Item<T>> action)
		{
			var current = tail;
			while (current != null)
			{
				if (action(current)) break;
				current = current.Prev;
			}
		}


		public TwoWayLinkList() : base() { }
		public TwoWayLinkList(T data) => Add(data);
		public T[] ToArray()
		{
			var array = new T[Count];
			var iter = 0;
			BruteForceNext(item =>
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
				item.Prev = tail;
				tail.Next = item;
				tail = item;
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
				head.Prev = item;
				item.Next = head;
				head = item;
			}
			Count++;
			return this;
		}
		public TwoWayLinkList<T> RemoveFirst()
		{
			if (!IsEmpty)
			{
				head = head.Next;
				head.Prev = null;
				Count--;
			}
			return this;
		}
		public TwoWayLinkList<T> RemoveLast()
		{
			if (!IsEmpty)
			{
				tail = tail.Prev;
				tail.Next = null;
				Count--;
			}
			return this;
		}

		public TwoWayLinkList<T> Revers()
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
			BruteForceNext(itemRef =>
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
		public TwoWayLinkList<T> AddAfter(T target, T data)
		{
			if (IsEmpty || IsOneItem)
			{
				Add(data);
				return this;
			}
			BruteForceNext(item =>
			{
				if (item.Data.Equals(target))
				{
					var newItem = new Item<T>(data);
					if (item.Next != null)//not Tail
					{
						var next = item.Next;
						item.Next = newItem;
						newItem.Next = next;
						newItem.Prev = item;
						next.Prev = newItem;
					}
					else // Tail
					{
						newItem.Prev = tail;
						tail.Next = newItem;
						tail = newItem;
					}
					Count++;
					return true;
				}
				return false;
			});
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
			BruteForcePrev(item =>
			{
				if (item.Data.Equals(target))
				{
					var newItem = new Item<T>(data);
					if (item.Prev != null)//not Tail
					{
						var prev = item.Prev;
						item.Prev = newItem;
						newItem.Prev = prev;
						newItem.Next = item;
						prev.Next = newItem;
					}
					else // Tail
					{
						head.Prev = newItem;
						newItem.Next = head;
						head = newItem;
					}
					Count++;
					return true;
				}
				return false;
			});
			return this;
		}

		public TwoWayLinkList<T> RemoveElement(T data)
		{
			if (IsEmpty) return this;
			if (IsOneItem) { RemoveAll(); return this; }
			if (head.Data.Equals(data)) { RemoveFirst(); return this; }
			if (tail.Data.Equals(data)) { RemoveLast(); return this; }
			BruteForceNext(itemRef =>
			{
				if (itemRef.Data.Equals(data))
				{
					var prevItem = itemRef.Prev;
					var nextItem = itemRef.Next;

					prevItem.Next = nextItem;
					nextItem.Prev = prevItem;

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
		public Item(T data) => Data = data;

		public Item<T> Prev { get; set; }
		public Item<T> Next { get; set; }
		public T Data { get; set; }
		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
