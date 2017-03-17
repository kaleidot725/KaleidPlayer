using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Collection
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    class CollectionBasedDictionary<TKey, TValue> : KeyedCollection<TKey, KeyValuePair<TKey, TValue>>
    {
        protected override TKey GetKeyForItem(KeyValuePair<TKey, TValue> item)
        {
            return item.Key;
        }
    }
}
