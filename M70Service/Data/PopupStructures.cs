namespace M70Service.Data.Popups
{
    /* readonly Pair structure to populate dropdown lists*/
    public readonly struct PopupItem<T>
    {
        public T Item { get; }
        public string Name { get; }

        public PopupItem(T item, string name) {
            Item = item;
            Name = name;
        }
    }
}
