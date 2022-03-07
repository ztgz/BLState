namespace BLState;

public static class BLStoreBaseExtensions
{
    public static void Update<T>(this T store, Action<T> obj)
        where T : BLStoreBase
    {
        obj.Invoke(store);
        store.InvokeUpdates();
    }

    public static async Task UpdateAsync<T>(this T store, Func<T, Task> obj)
        where T : BLStoreBase
    {
        await obj.Invoke(store);
        store.InvokeUpdates();
    }
}
