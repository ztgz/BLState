namespace BLState.Models
{
    internal class BLGeneratorModels
    {
        internal class BLStoreModel
        {
            internal BLStoreModel(string name, string theNamespace)
            {
                Name = name;
                Namespace = theNamespace;
            }

            public string Name { get; set; }
            public string Namespace { get; set; }
        }
    }
}
