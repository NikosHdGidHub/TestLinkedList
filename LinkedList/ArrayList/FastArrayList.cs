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
		private void TailNext()
		{
			if (IsEmpty) throw new ArithmeticException("Массив не должен быть пустым, выполняя этот метод");
			if (!IsAvailableWrite) throw new ArithmeticException("Массив не должен быть переполненным, выполняя этот метод");

			if (tailIndex < Size)
			{
				tailIndex = pathes[tailIndex].Next;
				return;
			}
			throw new MemberAccessException();
		}
		private void TailPrev()
		{
			if (IsEmpty) throw new ArithmeticException("Массив не должен быть пустым, выполняя этот метод");
			if (Count == 1) throw new ArithmeticException("Массив не должен иметь 1 элемент, выполняя этот метод");

			if (tailIndex > -1)
			{
				tailIndex = pathes[tailIndex].Prev;
				return;
			}
			throw new MemberAccessException();
		}
		private void HeadNext()
		{
			if (IsEmpty) throw new ArithmeticException("Массив не должен быть пустым, выполняя этот метод");
			if (Count == 1) throw new ArithmeticException("Массив не должен иметь 1 элемент, выполняя этот метод");

			if (headIndex < Size)
			{
				headIndex = pathes[headIndex].Next;
				return;
			}
			throw new MemberAccessException();
		}
		private void HeadPrev()
		{
			if (IsEmpty) throw new ArithmeticException("Массив не должен быть пустым, выполняя этот метод");
			if (!IsAvailableWrite) throw new ArithmeticException("Массив не должен быть переполненным, выполняя этот метод");

			if (headIndex > -1)
			{
				headIndex = pathes[headIndex].Prev;
				return;
			}
			throw new MemberAccessException();
		}
		/// <summary>
		/// Служит для настройки списка под 1 элемент
		/// </summary>
		private void SetStartOprions()
		{
			headIndex = 0;
			tailIndex = 0;
			Count = 1;
		}
		/// <summary>
		/// Служит для настройки списка под 1 элемент
		/// </summary>
		private void SetStartOprions(T data)
		{
			array[0] = data;
			SetStartOprions();
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
		}

		private int MathIndex(int index)
		{
			if (index < 0 || index > MaxIndex)
				throw new IndexOutOfRangeException();
			var result = index + headIndex;
			if (result > MaxIndex)
				result -= MaxIndex;
			return result;
		}
		private int FindIndex(int index)
		{
			var headI = headIndex;
			for (int i = 0; i < index; i++)
			{
				headI = pathes[headI].Next;
			}
			return headI;
		}

		private void CutRoad(int indexRoad)
		{
			if (indexRoad == headIndex || indexRoad == tailIndex || indexRoad > MaxIndex || indexRoad < 0)
				throw new ArithmeticException();

			pathes.Cut(indexRoad);
			pathes.PasteBefore(indexRoad, headIndex);
			IsFormated = false;
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
			get
			{
				if (headIndex == -1 && tailIndex == -1 && Count == 0)
					return true;
				if (headIndex > -1 && tailIndex > -1 && Count > 0)
					return false;
				throw new MemberAccessException();
			}
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
		public void Append(T data)
		{
			if (!IsAvailableWrite) return;
			if (IsEmpty)
			{
				SetStartOprions(data); return;
			}

			TailNext();
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

			HeadPrev();
			array[headIndex] = data;

			Count++;
		}
		public void RemoveFirst()
		{
			if (IsEmpty) return;
			if (Count == 1) { Clear(); return; }

			HeadNext();
			Count--;
		}
		public void RemoveLast()
		{
			if (IsEmpty) return;
			if (Count == 1) { Clear(); return; }

			TailPrev();
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

			var headI = headIndex;
			var flag = false;
			for (int i = 0; i < Count; i++)
			{
				if (array[headI].Equals(data))
				{
					flag = true;
					break;
				}
				headI = pathes[headI].Next;
			}

			if (flag) CutRoad(headI);
			Count--;
		}
		public void RemoveAt(int index)
		{
			if (headIndex == index)
			{
				RemoveFirst();
				return;
			}
			if (tailIndex == index)
			{
				RemoveLast();
				return;
			}
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

		public IEnumerator<T> GetEnumerator()
		{
			return BruteForse().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return BruteForse().GetEnumerator();
		}


	}
}
