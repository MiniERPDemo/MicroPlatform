using System.Collections.Generic;

namespace MicroPlatform
{
    public interface IEntityIdGenerator
    {
        string GetNextId(string entityType);
    }

    public class EntityIntIdGenerator : IEntityIdGenerator
    {
        readonly Dictionary<string, int> _entitieIds;

        public EntityIntIdGenerator()
        {
            _entitieIds = new Dictionary<string, int>();
        }

        public string GetNextId(string entityType)
        {
            if (_entitieIds.ContainsKey(entityType))
            {
                _entitieIds[entityType]++;
                return _entitieIds[entityType].ToString();
            }
            else
            {
                _entitieIds[entityType] = 1;
                return _entitieIds[entityType].ToString();
            }
        }
    }
}