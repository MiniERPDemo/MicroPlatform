using System;
using System.Collections.Generic;

namespace MicroPlatform
{
    public class EntityObject
    {
        private readonly Dictionary<string,string> _fieldsKeyValue=new Dictionary<string, string>();

        public EntityType EntityType { get; }

        public EntityObject(EntityType entityType, IEntityIdGenerator entityIdGenerator)
        {
            EntityType = entityType;
            if (entityType == null)
            {
                throw new ArgumentNullException();
            }
            _fieldsKeyValue["id"] = entityIdGenerator.GetNextId(entityType.Name);
        }

        public string GetValue(string fieldKey)
        {
            EntityType.ValidateFieldExist(fieldKey);

            if (!_fieldsKeyValue.ContainsKey(fieldKey))
            {
                _fieldsKeyValue[fieldKey] = EntityType.GetField(fieldKey).GetDefaultValue();
            }

            return _fieldsKeyValue[fieldKey];


        }

        
        public void SetValue(string fieldKey, string fieldValue)
        {
            EntityType
                .ValidateFieldExist(fieldKey)
                .ValidateFieldEditable(fieldKey);
            
            _fieldsKeyValue[fieldKey] = fieldValue;
        }
    }
}