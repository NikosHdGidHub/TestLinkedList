using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList.ArrayList
{
	
	internal struct IndexingPathing
	{
		public int Prev;
		public int Next;
	}
	internal static class ExtendClass
	{
		public static void Connect(this IndexingPathing[] pathes, int indexLeft, int indexRight)
		{
			pathes[indexLeft].Next = indexRight;
			pathes[indexRight].Prev = indexLeft;
		}
		public static void Connect2(this IndexingPathing[] pathes, int indexLeft, int indexTarget, int indexRight)
		{
			pathes.Connect(indexLeft, indexTarget);
			pathes.Connect(indexTarget, indexRight);
		}
		public static void PasteBefore(this IndexingPathing[] pathes, int indexTarget, int indexRight)
		{
			var indexLeft = pathes[indexRight].Prev;
			pathes.Connect2(indexLeft, indexTarget, indexRight);
		}
		public static void Cut(this IndexingPathing[] pathes, int indexTarget)
		{
			var indexRight = pathes[indexTarget].Next;
			var indexLeft = pathes[indexTarget].Prev;
			pathes.Connect(indexLeft, indexRight);
		}
	}
	public class FastArrayList<T> : IEnumerable<T>
	{
		

		private T[] array;
		private readonly IndexingPathing[] pathes;
		private int headIndex = -1;
		private int tailIndex = -1;

		/// <summary>
		/// Length - 1
		/// </summary>
		private int MaxIndex => Size - 1;

		#region Private Methods

		private bool IsValidIndex(int index)
		{
			if (index >= 0 && index < Count)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Служит для настройки списка под 1 элемент
		/// </summary>
		private void SetStartOprions(T data)
		{
			array[0] = data;
			headIndex = 0;
			tailIndex = 0;
			Count = 1;
		}

		private void FormatingRoadArray(int i)
		{
			if (i == 0)
			{
				pathes[i].Prev = MaxIndex;
			}
			else
			{
				pathes[i].Prev = i - 1;
			}
			if (i == MaxIndex)
			{
				pathes[i].Next = 0;
			}
			else
				pathes[i].Next = i + 1;
		}
		private void FormatingArray()
		{
			var newArray = new T[Size];
			var headI = headIndex;
			for (int i = 0; i < Size; i++)
			{
				if (i < Count)
				{
					newArray[i] = array[headI];
					headI = pathes[headI].Next;
				}
				FormatingRoadArray(i);
			}

			array = newArray;
			
			
			IsFormated = true;
			headIndex = 0;
			tailIndex = Count - 1;
		}

		private int MathIndex(int index)
		{
			var result = index + headIndex;
			if (result > MaxIndex)
				result -= Size;
			return result;
		}

		private void CutRoad(int indexRoad)
		{
			if (indexRoad == headIndex || indexRoad == tailIndex)
				throw new ArithmeticException();

			pathes.Cut(indexRoad);
			pathes.PasteBefore(indexRoad, headIndex);
			IsFormated = false;
		}
		#endregion

		#region Public Property

		public bool IsFormated { get; private set; }
		public T First
		{
			get
			{
				if (!IsEmpty)
					return array[headIndex];
				throw new NullReferenceException("Массив пустой");
			}
		}
		public T Last
		{
			get
			{
				if (!IsEmpty)
					return array[tailIndex];
				throw new NullReferenceException("Массив пустой");
			}
		}
		public int Count { get; private set; } = 0;
		/// <summary>
		/// Length
		/// </summary>
		public int Size => array?.Length ?? 0;
		public bool IsEmpty
		{
			get => Count == 0;
			//{

			//	//if (headIndex == -1 && tailIndex == -1 && Count == 0)
			//	//	return true;
			//	//if (headIndex > -1 && tailIndex > -1 && Count > 0)
			//	//	return false;
			//	//throw new MemberAccessException();
			//}
		}
		public bool IsAvailableWrite
		{
			get
			{
				if (Count < Size)
					return true;
				return false;
			}
		}

		#endregion

		public T this[int index]
		{
			get
			{
				if (!IsFormated) FormatingArray();
				return array[MathIndex(index)];
			}
			set
			{
				if (!IsFormated) FormatingArray();
				var mathIndex = MathIndex(index);
				if (IsEmpty || mathIndex == Count)
				{
					Append(value);
					return;
				}
				if (mathIndex < Count)
				{
					array[mathIndex] = value;
					return;
				}
				throw new IndexOutOfRangeException();
			}
		}

		public FastArrayList(int size)
		{
			array = new T[size];

			pathes = new IndexingPathing[Size];
			for (int i = 0; i < Size; i++)
			{
				FormatingRoadArray(i);
			}
			IsFormated = true;
		}

		#region Public Methods

		public void Append(T data)
		{
			if (!IsAvailableWrite) return;
			if (IsEmpty)
			{
				SetStartOprions(data); return;
			}

			//TailNext();
			tailIndex = pathes[tailIndex].Next;

			array[tailIndex] = data;

			Count++;
		}
		public void Prepend(T data)
		{
			if (!IsAvailableWrite) return;
			if (IsEmpty)
			{
				SetStartOprions(data); return;
			}

			//HeadPrev();
			headIndex = pathes[headIndex].Prev;
			array[headIndex] = data;

			Count++;
		}
		public void RemoveFirst()
		{
			if (IsEmpty) return;
			if (Count == 1) { Clear(); return; }

			//HeadNext();
			headIndex = pathes[headIndex].Next;
			Count--;
		}
		public void RemoveLast()
		{
			if (IsEmpty) return;
			if (Count == 1) { Clear(); return; }

			//TailPrev();
			tailIndex = pathes[tailIndex].Prev;
			Count--;
		}
		public void Remove(T data)
		{
			if (First.Equals(data))
			{
				RemoveFirst();
				return;
			}
			if (Last.Equals(data))
			{
				RemoveLast();
				return;
			}

			var searchIndex = headIndex;
			var flag = false;
			ForEach((item, index) =>
			{
				if (item.Equals(data))
				{
					flag = true;
					searchIndex = index;
				}
				return flag;
			});

			if (flag) CutRoad(searchIndex);
			Count--;
		}
		public void RemoveAt(int index)
		{
			if (!IsValidIndex(index)) throw new IndexOutOfRangeException();

			if (0 == index)
			{
				RemoveFirst();
				return;
			}

			if (Count - 1 == index)
			{
				RemoveLast();
				return;
			}

			index = MathIndex(index);
			if (IsFormated) CutRoad(index);
			else
			{
				var headI = headIndex;
				for (int i = 0; i < index; i++)
				{
					headI = pathes[headI].Next;
				}
				CutRoad(headI);
			}
			Count--;
		}
		public void Clear()
		{
			headIndex = -1;
			tailIndex = -1;
			Count = 0;
		}

		#endregion

		public IEnumerator<T> GetEnumerator()
		{
			return BruteForse().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return BruteForse().GetEnumerator();
		}

		public void ForEach(Func<T, int, bool> action)
		{
			var headI = headIndex;
			for (int i = 0; i < Count; i++)
			{
				if (action(array[headI], headI)) return;
				headI = pathes[headI].Next;
			}
		}
		private IEnumerable<T> BruteForse()
		{
			var headI = headIndex;
			for (int i = 0; i < Count; i++)
			{
				yield return array[headI];
				headI = pathes[headI].Next;
			}
		}

	}
}
