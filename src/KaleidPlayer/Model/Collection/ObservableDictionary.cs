using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.ComponentModel;

namespace kaleidot725.Collection
{
    class ObservableDictionary<TKey, TValue> : CollectionBasedDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;
        protected virtual event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }

            remove
            {
                PropertyChanged -= value;
            }
        }

        protected const string IndexerName = "Item[]";
        protected const string CountName = "Count";

        protected override void InsertItem(int index, KeyValuePair<TKey, TValue> item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            OnPropertyChanged(IndexerName);
            OnPropertyChanged(CountName);
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            OnPropertyChanged(IndexerName);
            OnPropertyChanged(CountName);
        }

        protected override void SetItem(int index, KeyValuePair<TKey, TValue> item)
        {
            var oldItem = this[index];
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
            OnPropertyChanged(IndexerName);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(IndexerName);
            OnPropertyChanged(CountName);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
