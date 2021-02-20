using System.Collections.Specialized;

namespace DataContainer.Utils
{
    public class CollectionChangedListener
    {
        private readonly INotifyCollectionChanged _source;

        public CollectionChangedListener(INotifyCollectionChanged source)
        {
            _source = source;

            _source.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LastChange = e;
        }

        public NotifyCollectionChangedEventArgs LastChange { get; set; }
    }
}
