using System;
using System.Collections.Generic;

namespace MicroPlatform
{
    public class EntityObject
    {
        private readonly IEntityChanged _entityChanged;
        private readonly Dictionary<string,string> _fieldsKeyValue=new Dictionary<string, string>();

        public EntityType EntityType { get; }

        public EntityObject(EntityType entityType, IEntityIdGenerator entityIdGenerator, IEntityChanged entityChanged)
        {
            _entityChanged = entityChanged;
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

            var field = EntityType.GetField(fieldKey);
            var oldValue = _fieldsKeyValue.ContainsKey(fieldKey) ? _fieldsKeyValue[fieldKey] : field.GetDefaultValue();
            
            _fieldsKeyValue[fieldKey] = fieldValue;

            _entityChanged?.OnEntityValueChanged(new EntityChangedEvent()
            {
                Target = this,
                FieldKey = fieldKey,
                FieldName = field.FieldDescription,
                OldValue = oldValue,
                NewValue = fieldValue
            });
        }
    }
}