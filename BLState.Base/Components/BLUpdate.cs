using Microsoft.AspNetCore.Components;

namespace BLState
{
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

            Store.Subscribe(OnUpdate);
        }

        public void Dispose() => Store.Unsubscribe(OnUpdate);
    }
}
