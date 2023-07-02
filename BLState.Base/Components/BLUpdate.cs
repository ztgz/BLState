using Microsoft.AspNetCore.Components;

namespace BLState;

public class BLUpdate : ComponentBase, IDisposable
{
    [Parameter]
    [EditorRequired]
    public BLStoreBase Store { get; set; } = null!;

    [Parameter] 
    [EditorRequired]
    public Action OnUpdate { get; set; } = null!;


    protected override void OnInitialized()
    {
        if (Store is null)
            throw new ArgumentNullException(nameof(Store));
        if (OnUpdate is null)
            throw new ArgumentNullException("Paramter OnUpdate cannot be null");

        SubscribeToEvents(Store, OnUpdate);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (_store != Store || _onUpdate != OnUpdate)
        {
            UnsubscribeToEvents();
            SubscribeToEvents(Store, OnUpdate);
        }
    }

    private BLStoreBase _store = default!;
    private Action _onUpdate = default!;
    private void SubscribeToEvents(BLStoreBase store, Action onUpdate)
    {
        _store = store;
        _onUpdate = onUpdate;
        _store.Subscribe(_onUpdate);
    }

    private void UnsubscribeToEvents() => _store.Unsubscribe(_onUpdate);
    
    public void Dispose() => UnsubscribeToEvents();
}
