using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Collection
{
    public class CollectionBasedDictionary<TKey, TValue> : KeyedCollection<TKey, KeyValuePair<TKey, TValue>>
    {
        protected override TKey GetKeyForItem(KeyValuePair<TKey, TValue> item)
        {
            return item.Key;
        }
    }
}
