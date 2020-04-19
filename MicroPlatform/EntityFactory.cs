using System.Text;

namespace MicroPlatform
{
    public class EntityFactory
    {
        public EntityType CreateType(string typeName)
        {
            var entityType = new EntityType(typeName);
            return entityType;
        }

        readonly EntityIntIdGenerator _entityIntIdGenerator;

        public EntityFactory()
        {
            _entityIntIdGenerator = new EntityIntIdGenerator();
        }

        public EntityObject CreateItem(EntityType typeName)
        {
            return new EntityObject(typeName, _entityIntIdGenerator);
        }
    }
}
