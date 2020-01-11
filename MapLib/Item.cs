using System.Collections.Generic;

namespace MapLib
{

	internal class Item<TKey, TValue>
	{
		public TValue Value { get; set; }
		public TKey Key { get; set; }
		public List<CollisionData> Collisions = new List<CollisionData>();

		public Item(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public struct CollisionData
		{
			public CollisionData(TKey key, TValue value)
			{
				Key = key;
				Value = value;
			}

			public TKey Key { get; set; }
			public TValue Value { get; set; }

		}
	}
}
