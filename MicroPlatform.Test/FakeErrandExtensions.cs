using System.Collections.Generic;

namespace MicroPlatform.Test
{
    public class FakeErrandExtensions : IPluginModule
    {
        public string PluginId { get; } = "EAEF1608-876A-4C5C-8D7E-65633A8A5C74";
        public string PluginType { get; } = "Errand";

        public List<EntityType> EntityTypes =>
            new List<EntityType>()
            {
                GetErrandType()
            };


        private EntityType GetErrandType()
        {
            var entityType = new EntityType("Errand",null);
            entityType.AddField(new EntityTypeFieldItem()
            {
                FieldId = "Price",
                FieldDescription = "Цена",
                FieldType = "int"
            });
            return entityType;
        }
    }
}