using System;
using System.Collections.Generic;

namespace MicroPlatform
{
    public class EntityObject
    {
        private readonly Dictionary<string,string> _fieldsKeyValue=new Dictionary<string, string>();

        private readonly EntityType _entityType;

        public EntityObject(EntityType entityType, IEntityIdGenerator entityIdGenerator)
        {
            _entityType = entityType;
            if (entityType == null)
            {
                throw new ArgumentNullException();
            }
            _fieldsKeyValue["id"] = entityIdGenerator.GetNextId(entityType.Name);
        }

        public string GetValue(string fieldKey)
        {
            _entityType.ValidateFieldExist(fieldKey);

            if (!_fieldsKeyValue.ContainsKey(fieldKey))
            {
                _fieldsKeyValue[fieldKey] = _entityType.GetField(fieldKey).GetDefaultValue();
            }

            return _fieldsKeyValue[fieldKey];


        }

        
        public void SetValue(string fieldKey, string fieldValue)
        {
            _entityType
                .ValidateFieldExist(fieldKey)
                .ValidateFieldEditable(fieldKey);
            
            _fieldsKeyValue[fieldKey] = fieldValue;
        }
    }
}