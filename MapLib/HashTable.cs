using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Linq;

namespace MapLib
{
    public class Stats
    {
        public int CollisionsCount { get; set; }
        public int FoundTimes { get; set; } = 1;
        public double Percent { get; set; }
    }
    public class CollisionsStats
    {
        /// <summary>
        /// Максимальное количество коллизий на один хеш
        /// </summary>
        public int MaxCollisions { get; }
        /// <summary>
        /// Выделено памяти
        /// </summary>
        public int AllocatedMemory { get; private set; }
        /// <summary>
        /// Занято памяти
        /// </summary>
        public int MemotyOccupied { get; private set; }
        public double PercentMemory { get; private set; }

        public Dictionary<int, Stats> Stats { get; private set; }
        public CollisionsStats(int max, Dictionary<int, Stats> dict, int locatedMem, int occupied)
        {
            Stats = dict;
            MaxCollisions = max;
            AllocatedMemory = locatedMem;
            MemotyOccupied = occupied;
            PercentMemory = Math.Round(occupied / (double)locatedMem * 100.0,2);
        }
    }

    public class HashTable<Tkey, TValue> : IEnumerable<KeyValuePair<Tkey, TValue>>
    {
        #region Private Fields
        private int size = 1;
        private Item<Tkey, TValue>[] items;
        private readonly HashSet<Tkey> keys;
        #endregion
        #region Private Methods
        private int GetHash(Tkey key)
        {
            return Math.Abs(key.GetHashCode() % size);
        }
        private void AddItem(Tkey key, TValue value, int hash)
        {
            if (items[hash] == null)
            {
                items[hash] = new Item<Tkey, TValue>(key, value);
                Count++;
            }
            else
            {
                items[hash].Collisions.Add(new Item<Tkey, TValue>.CollisionData(key, value));
            }
        }
        private void UpSizeHash()
        {
            //size = (int)(size * 2);
            //size = (int)(size * 2 - size * (Count / (double)keys.Count));
            size = size + Count;
            //size = (int)(size + size * (Count / (double)keys.Count) );
            var oldArrayItems = items;
            items = new Item<Tkey, TValue>[size];
            keys.Clear();
            Count = 0;
            for (int i = 0; i < oldArrayItems.Length; i++)
            {
                var data = oldArrayItems[i];
                //перехеширование
                if (data != null)
                {
                    AddItem(data.Key, data.Value, GetHash(data.Key));
                    keys.Add(data.Key);
                    foreach (var item in data.Collisions)
                    {
                        AddItem(item.Key, item.Value, GetHash(item.Key));
                        keys.Add(item.Key);
                    }
                }
            }
        }
        private void DownSizeHash()
        {
            if (size < 10) return;
            size = size / 2;
            var newHash = new Item<Tkey, TValue>[size];
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {

                }
            }
            items = newHash;
        }
        #endregion
        public HashTable() : this(1) { }
        public HashTable(int size)
        {
            if (size < 1) throw new Exception("Размер должен быть больше 0: size = " + size);

            this.size = size;
            items = new Item<Tkey, TValue>[size];
            keys = new HashSet<Tkey>(size);            
        }
        public TValue this[Tkey key]
        {
            get
            {
                TryGetValue(key, out TValue val);
                return val;
            }
            set
            {
                SetValue(key, value);
            }
        }
        #region Public Properties
        public int Count { get; private set; } = 0;

        #endregion
        #region Public Methods
        public CollisionsStats GetStatsCollisions()
        {
            var max = 0;
            var dict = new Dictionary<int, Stats>();

            foreach (var item in items)
            {
                if (item == null) continue;

                var count = item.Collisions.Count;
                if (count > max)
                    max = count;

                if (dict.TryGetValue(count, out Stats stats))
                {
                    stats.FoundTimes++;
                }
                else
                {
                    dict.Add(count, new Stats() { CollisionsCount = count });
                }
            }
            foreach (var item in dict)
            {
                dict.TryGetValue(item.Key, out Stats stats);
                stats.Percent = stats.FoundTimes / (double)Count * 100.0;
            }
            return new CollisionsStats(max, dict, size, Count);
        }
        public void Add(Tkey key, TValue value)
        {
            if (keys.Count >= size) UpSizeHash();
            //if ((Count / (double)size) > 0.8) UpSizeHash();
            if (keys.Contains(key)) throw new Exception($"Ключ @{key}@ уже существует");

            keys.Add(key);

            var hash = GetHash(key);
            if (hash < 0) throw new Exception($"Отрицательный ключ @{key}@");

            AddItem(key, value, hash);
        }
        public bool Search(Tkey key)
        {
            return keys.Contains(key);
        }
        public void Remove(Tkey key)
        {
            if (!keys.Contains(key)) throw new Exception($"Ключа @{key}@ не существует");

            keys.Remove(key);

            var hash = GetHash(key);
            var data = items[hash];
            var collisionsCount = data.Collisions.Count;
            if (data.Key.Equals(key))
            {
                if (collisionsCount > 0)
                {
                    //перемещаем последнего конкурента в начало и удаляем его
                    var lastCollision = data.Collisions.Last();
                    data.Key = lastCollision.Key;
                    data.Value = lastCollision.Value;
                    data.Collisions.RemoveAt(collisionsCount - 1);
                }
                else
                {
                    items[hash] = null;
                    Count--;
                }
            }
            else
            {
                for (int i = 0; i < collisionsCount; i++)
                {
                    if (data.Collisions[i].Key.Equals(key))
                    {
                        data.Collisions.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public bool TryGetValue(Tkey key, out TValue value)
        {
            value = default;
            if (!keys.Contains(key)) return false;

            var hash = GetHash(key);
            var data = items[hash];
            var collisionsCount = data.Collisions.Count;
            if (data.Key.Equals(key))
            {
                value = data.Value;
                return true;
            }
            else
            {
                for (int i = 0; i < collisionsCount; i++)
                {
                    if (data.Collisions[i].Key.Equals(key))
                    {
                        value = data.Collisions[i].Value;
                        return true;
                    }
                }
            }
            throw new Exception("Элемент не доступен из-за не правильной логики приложения");
        }
        public void SetValue(Tkey key, TValue value)
        {
            if (!keys.Contains(key)) throw new Exception($"Ключа @{key}@ не существует");

            var hash = GetHash(key);
            var data = items[hash];
            var collisionsCount = data.Collisions.Count;
            if (data.Key.Equals(key))
            {
                data.Value = value;
            }
            else
            {
                for (int i = 0; i < collisionsCount; i++)
                {
                    if (data.Collisions[i].Key.Equals(key))
                    {
                        data.Collisions[i] = new Item<Tkey, TValue>.CollisionData(key, value);
                    }
                }
            }
        }

        public IEnumerator<KeyValuePair<Tkey, TValue>> GetEnumerator()
        {
            foreach (var key in keys)
            {
                var value = this[key];
                yield return new KeyValuePair<Tkey, TValue>(key, value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion


    }
}
