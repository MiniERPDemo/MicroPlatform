using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MicroPlatform
{
    public class EntityFactory : IEntityProvider
    {
        private readonly Dictionary<string,IPluginModule> _modules=new Dictionary<string, IPluginModule>();

        private readonly Dictionary<string,EntityType> _types=new Dictionary<string, EntityType>();

        public IEnumerable<EntityType> Types
        {
            get
            {
                foreach (var entityType in _types)
                {
                    yield return entityType.Value;
                }

                foreach (var pluginModule in _modules)
                {
                    foreach (var entityType in pluginModule.Value.EntityTypes)
                    {
                        if (!_types.ContainsKey(entityType.Name))
                        {
                            yield return entityType;
                        }
                    }
                }
            }
        }

        public EntityType CreateType(string typeName)
        {
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));

            var entityType = new EntityType(typeName, this);
            _types[typeName] = entityType;
            return entityType;
        }

        readonly EntityIntIdGenerator _entityIntIdGenerator;

        public EntityFactory()
        {
            _entityIntIdGenerator = new EntityIntIdGenerator();
        }

        public EntityObject CreateItem(EntityType typeName)
        {
            var entityObject = new EntityObject(typeName, _entityIntIdGenerator);
            return entityObject;
        }

        public void RegisterPlugin(IPluginModule pluginModule)
        {
            ValidateModule(pluginModule);

            _modules[pluginModule.PluginType] = pluginModule;

            
        }

        private static void ValidateModule(IPluginModule pluginModule)
        {
            if (pluginModule == null)
                throw new ArgumentNullException(nameof(pluginModule));

            if (string.IsNullOrWhiteSpace(pluginModule.PluginId))
                throw new ArgumentNullException(pluginModule.PluginId);

            if (string.IsNullOrWhiteSpace(pluginModule.PluginType))
                throw new ArgumentNullException(pluginModule.PluginType);
        }

        public IEnumerable<EntityType> GetTypes(string name)
        {
            return _modules.
                Where(module => module.Value != null)
                .SelectMany(module => module.Value.EntityTypes
                    .Where(et=> et.Name == name));
        }
    }

    public interface IPluginModule
    {
        string PluginId { get; }
        string PluginType { get; }

        List<EntityType> EntityTypes { get; }
    }

}
