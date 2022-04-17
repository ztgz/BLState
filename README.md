# BLState Simple State Management For Blazor
Simple source generator library for working with global state and updates in Blazor. Declare stores and its values with attributes.

## Prerequisites
Blazor WASM or Server project based on .NET 6

## How to use BLState
### Download package
- Download the package trough nuget store  
- Or add via CLI - **dotnet add package BLState**  
- Or add a reference directly in the project file
`<PackageReference Include="BLState" Version="0.2.1" />`

**Note: If you are using Visual Studio you probably have to close and re-open Visual Studio after downloading the package to let intellisense catch up.**
The reason is that this library uses source generators and Visual Studio might not find the files until first restart of the software.

### Register Services
First register BLState so the stores so they can be used with dependency injection. *It's good to know that each store register with a scoped lifetime*
```
using BLState;
builder.Services.AddBLStore();
```

### Add a Store
Create a **public partial** class and anotate it with the attribute [BLStore]. For each value in the store add a private field with the annotaion BLValue.
```
using BLState;

...

[BLStore]
public partial class CountStore
{
    [BLValue]
    private int count = 1;
    
    [BLValue]
    private int _multiplier = 3;
}
```
The example above creates a store named CountStore with the properties Count and Multiplier.

### Use in a component
To use in a component   
1. Inject the store (don't forget to add a using directive to where you have defined your store).
2. Add a BLUpdate component to subscribe to changes in a specific store.  This component requires two arguments. A store it will listen to and a OnUpdate action that will be triggerd when an update occurs.
3. Access the values in the store by the generated property names.
4. Update a value, this change will be reflected in all components that uses the store.
```
@inject CountStore CountStore

<BLUpdate Store="CountStore" OnUpdate="StateHasChanged" /> 
<p>Current count: @CountStore.Count</p>  

<button @onclick="MultiplyValue">Multiply</button>

@code {  
   private void MultiplyValue()
   {
       // Sets a new value to the count filed in CountStore
       CountStore.Count *= CountStore.Multiplier;
   }
}
```

### Trigger update on reference type
When making a change to a reference type without changing the actual reference the store needs to be told that a update occured. To do this use the Update or UpdateAsync method.  

Example a store with a list.
```
[BLStore]
public partial class UserStore
{
    [BLValue]private List<string> names = new List<string>();
}
```
Then in the compoenent
```
using BLState;
@inject UserStore UserStore
...

UserStore.Update(x => x.Names.Add("Scott"));

//Or the async variant
await UserStore.UpdateAsync(async x => x.Names.Add(await GetName()));
```



To manually trigger that the store has been updated call the method InvokeUpdates on the store.
```
UserStore.Names.Add("Scott");
UserStore.InvokeUpdates();
```

### Custom property names
For the need of custom property names of the values in the store add a string to the BLValue attribute
`[BLValue(PropertyName = "CustomName")] private string secretName = "";`. The public property will in this case be named CustomName.
