namespace BLState
{
    public abstract class BLStoreBase
    {
        private event Action? onChange = null;

        public void Subscribe(Action onStoreUpdate) =>
            onChange += onStoreUpdate;

        public void Unsubscribe(Action onStoreUpdate) =>
            onChange -= onStoreUpdate;

        protected void InvokeUpdates() =>
            onChange?.Invoke();
    }
}
