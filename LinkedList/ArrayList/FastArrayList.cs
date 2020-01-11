using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;

namespace LinkedList.ArrayList
{

	public class FastArrayList<T> : IEnumerable<T>
	{


		private readonly T[] array;
		private readonly int[] hashArray;
		/// <summary>
		/// хранит позицию в головы массива в hashArray
		/// </summary>
		private int headIndex = -1;
		/// <summary>
		/// хранит позицию в хвоста массива в hashArray
		/// </summary>
		private int tailIndex = -1;

		/// <summary>
		/// Length - 1
		/// </summary>
		private int MaxIndex => Size - 1;

		#region Private Methods
		private int FindIndex = -1;
		private async Task FindRecAsync(T data, int start, int end)
		{
			if (FindIndex != -1) return;
			await Task.Run(()=>FindRec(data, start, end));
			return;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="start"></param>
		/// <param name="end">не включительно</param>
		/// <param name="data"></param>
		private void FindRec(T data, int start, int end)
		{
			var count = end - start;			
			
			for (int i = start; i < end; i++)
			{
				if (FindIndex != -1) break;
				if (this[i].Equals(data))
				{
					//Console.WriteLine("Find!");
					FindIndex = i;
					break;
				}
			}			
		}
		private int GetNextIndex(int tIndex)
		{			
			if (tIndex < MaxIndex)
			{
				return tIndex + 1;
			}
			if (tIndex == MaxIndex)
			{
				return 0;
			}
			throw new MemberAccessException();
		}
		private int GetPrevIndex(int tIndex)
		{
			if (tIndex > 0)
				return tIndex - 1;

			if (tIndex == 0)
				return MaxIndex;

			throw new MemberAccessException();
		}

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


		private int MathIndex(int index)
		{
			return (index + headIndex) % Size;
		}
		private int MathIndex2(int index)
		{
			index += headIndex;
			if (index > MaxIndex)
				index -= Size;
			return index;
		}
		private void Cut(int indexS)
		{
			var index = MathIndex(indexS);
			if (indexS > Count / 2) //+
			{
				hashArray[tailIndex] = index;
				for (int i = index; i != tailIndex; )
				{
					hashArray[i] = GetNextIndex(hashArray[i]);
					i = GetNextIndex(i);
				}
				tailIndex = GetPrevIndex(tailIndex);
			}
			else//-
			{
				hashArray[headIndex] = index;
				for (int i = index; i != headIndex; )
				{
					hashArray[i] = GetPrevIndex(hashArray[i]);
					i = GetPrevIndex(i);
				}
				headIndex = GetNextIndex(headIndex);
			}
		}

		#endregion

		#region Public Property

		public T First
		{
			get
			{
				if (!IsEmpty)
					return array[hashArray[headIndex]];
				throw new NullReferenceException("Массив пустой");
			}
		}
		public T Last
		{
			get
			{
				if (!IsEmpty)
					return array[hashArray[tailIndex]];
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
				return array[hashArray[MathIndex(index)]];
			}
			set
			{
				if (!IsValidIndex(index))
					throw new IndexOutOfRangeException();


				array[hashArray[MathIndex(index)]] = value;
				return;
			}
		}

		public FastArrayList(int size)
		{
			array = new T[size];

			hashArray = new int[Size];
			for (int i = 0; i < Size; i++)
			{
				hashArray[i] = i;
			}
		}

		#region Public Methods

		public void Append(T data)
		{
			if (!IsAvailableWrite) return;
			if (IsEmpty)
			{
				SetStartOprions(data); return;
			}

			tailIndex = GetNextIndex(tailIndex);

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
			headIndex = GetPrevIndex(headIndex);
			array[headIndex] = data;

			Count++;
		}
		public void RemoveFirst()
		{
			if (IsEmpty) return;
			if (Count == 1) { Clear(); return; }

			//HeadNext();
			headIndex = GetNextIndex(headIndex);
			Count--;
		}
		public void RemoveLast()
		{
			if (IsEmpty) return;
			if (Count == 1) { Clear(); return; }

			//TailPrev();
			tailIndex = GetPrevIndex(tailIndex);
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


			var SearchIndex = 0;
			var flag = false;
			ForEach((item, index) =>
			{
				if (item.Equals(data))
				{
					flag = true;
					SearchIndex = index;
				}
				return flag;
			});
			if (flag)
			{
				Cut(SearchIndex);
				Count--;
			}

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

			Cut(index);
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
			var index = headIndex;
			for (int i = 0; i < Count; i++)
			{
				if (action(array[hashArray[index]], i)) return;
				index = GetNextIndex(index);
			}
		}
		private IEnumerable<T> BruteForse()
		{
			var index = headIndex;
			for (int i = 0; i < Count; i++)
			{
				yield return array[hashArray[index]];
				index = GetNextIndex(index);
			}
		}

	}
}
