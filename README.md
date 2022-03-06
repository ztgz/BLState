# BLState Simple State Management For Blazor
Simple source generator library for working with global state and updates in Blazor. Declare stores and its values with attributes.

## Prerequisites
Blazor WASM or Server project based on .NET 6

## How to use BLState
### Download package
- Download the package trough nuget store  
- Or add via CLI - **dotnet add package BLState**  
- Or add a reference directly in the project file
`<PackageReference Include="BLState" Version="0.1.0" />`

**Note: If you are using Visual Studio you probably have to close and re-open Visual Studio after downloading the package to let intellisense catch up.**
The reason is that this library uses source generators and Visual Studio might not find the files until first restart of the software.

### Register Services
First register the stores so they can be used with dependency injection. *Each store registers with a scoped lifetime*
```
using BLState;
builder.Services.InitializeBLStore();
```

### Add a Store
Create a **partial** class and anotate it with the attribute [BLStore]. For each value in the store add a private field with the annotaion BLValue.
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

### Custom property names
For the need of custom property names of the values in the store add a string to the BLValue attribute
`[BLValue(PropertyName = "CustomName")] private string secretName = "";`. The public property will in this case be named CustomName.
