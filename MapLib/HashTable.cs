using System;
using System.Collections.Generic;
using System.Linq;

namespace MapLib
{
    public class CollisionStats
    {
        public int CollisionsCount { get; set; }
        public int FoundTimes { get; set; } = 1;
        public double Percent { get; set; }
    }
    public class CollisionsDataStats
    {
        public int MaxCollisions { get; private set; }

        public Dictionary<int, CollisionStats> Stats { get; private set; }
        public CollisionsDataStats(int max, Dictionary<int, CollisionStats> dict)
        {
            Stats = dict;
            MaxCollisions = max;
        }
    }

    public class HashTable<Tkey, TValue>
    {
        #region Private Fields
        private int size = 100;
        private Item<Tkey, TValue>[] items;
        private readonly List<Tkey> keys;
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
            size = (int)(size * 1.59) + keys.Count;
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
        public HashTable() : this(100) { }
        public HashTable(int size)
        {
            if (size < 1) throw new Exception("Размер должен быть больше 0: size = " + size);

            this.size = size;
            items = new Item<Tkey, TValue>[size];
            keys = new List<Tkey>();
        }
        public TValue this[Tkey key]
        {
            get
            {
                return GetValue(key);
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
        public CollisionsDataStats GetStatsCollisions()
        {
            var max = 0;
            var dict = new Dictionary<int, CollisionStats>();

            foreach (var item in items)
            {
                if (item == null) continue;

                var count = item.Collisions.Count;
                if (count > max)
                    max = count;

                if (dict.TryGetValue(count, out CollisionStats stats))
                {
                    stats.FoundTimes++;
                }
                else
                {
                    dict.Add(count, new CollisionStats() { CollisionsCount = count });
                }
            }
            foreach (var item in dict)
            {
                dict.TryGetValue(item.Key, out CollisionStats stats);
                stats.Percent = stats.FoundTimes / (double)Count * 100.0;
            }
            return new CollisionsDataStats(max, dict);
        }
        public void Add(Tkey key, TValue value)
        {
            if (Count >= size) UpSizeHash();

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
        public TValue GetValue(Tkey key)
        {
            if (!keys.Contains(key)) throw new Exception($"Ключа @{key}@ не существует");

            var hash = GetHash(key);
            var data = items[hash];
            var collisionsCount = data.Collisions.Count;
            if (data.Key.Equals(key))
            {
                return data.Value;
            }
            else
            {
                for (int i = 0; i < collisionsCount; i++)
                {
                    if (data.Collisions[i].Key.Equals(key))
                    {
                        return data.Collisions[i].Value;
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
        #endregion


    }
}
