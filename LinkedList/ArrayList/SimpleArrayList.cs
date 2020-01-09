using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList.ArrayList
{
	public class SimpleArrayList<T> : IEnumerable<T>
	{
		private HashSet<T> a;
		private readonly T[] array;
		private int headIndex = -1;
		private int tailIndex = -1;
		private int LastIndex => Size - 1;
		private void TailNext()
		{


			if (IsEmpty) throw new ArithmeticException("Массив не должен быть пустым, выполняя этот метод");
			if (!IsAvailableWrite) throw new ArithmeticException("Массив не должен быть переполненным, выполняя этот метод");

			if (tailIndex < LastIndex)
			{
				tailIndex++;
				return;
			}
			if (tailIndex == LastIndex)
			{
				tailIndex = 0;
				return;
			}
			throw new MemberAccessException();
		}
		private void TailPrev()
		{
			if (IsEmpty) throw new ArithmeticException("Массив не должен быть пустым, выполняя этот метод");
			if (Count == 1) throw new ArithmeticException("Массив не должен иметь 1 элемент, выполняя этот метод");

			if (tailIndex > 0)
			{
				tailIndex--;
				return;
			}
			if (tailIndex == 0)
			{
				tailIndex = LastIndex;
				return;
			}
			throw new MemberAccessException();
		}
		private void HeadNext()
		{
			if (IsEmpty) throw new ArithmeticException("Массив не должен быть пустым, выполняя этот метод");
			if (Count == 1) throw new ArithmeticException("Массив не должен иметь 1 элемент, выполняя этот метод");

			if (headIndex < LastIndex)
			{
				headIndex++;
				return;
			}
			if (headIndex == LastIndex)
			{
				headIndex = 0;
				return;
			}
			throw new MemberAccessException();
		}
		private void HeadPrev()
		{
			if (IsEmpty) throw new ArithmeticException("Массив не должен быть пустым, выполняя этот метод");
			if (!IsAvailableWrite) throw new ArithmeticException("Массив не должен быть переполненным, выполняя этот метод");

			if (headIndex > 0)
			{
				headIndex--;
				return;
			}
			if (headIndex == 0)
			{
				headIndex = LastIndex;
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
		private int MathIndex(int index)
		{
			if (index < 0 || index > LastIndex)
				throw new IndexOutOfRangeException();
			var result = index + headIndex;
			if (result > LastIndex)
				result -= LastIndex;
			return result;
		}
		private IEnumerable<T> BruteForse()
		{
			for (int i = 0; i < Size; i++)
			{
				yield return array[MathIndex(i)];
			}
		}

		public T this[int index]
		{
			get => array[MathIndex(index)];
			set
			{
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
		public int Size => array?.Length??0;
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

		public SimpleArrayList(int size)
		{
			array = new T[size];
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
