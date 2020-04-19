using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MicroPlatform
{
    public class EntityType
    {
        private readonly Dictionary<string,EntityTypeItem> _fields=new Dictionary<string, EntityTypeItem>();

        public EntityType(string name)
        {
            Name = name;
            _fields["id"] = new EntityTypeItem(true)
            {
                TypeId = "id",
                TypeDescription = "Идентификатор",
            };
        }

        public string Name { get; }

        public bool HasField(string fieldName)
        {
            return _fields.ContainsKey(fieldName);
        }


        public void AddField(EntityTypeItem entityTypeItem)
        {
            if (entityTypeItem == null) 
                throw new ArgumentNullException(nameof(entityTypeItem));

            if(string.IsNullOrWhiteSpace(entityTypeItem.TypeId))
                throw new ArgumentNullException(nameof(entityTypeItem.TypeId));

            if (string.IsNullOrWhiteSpace(entityTypeItem.TypeDescription))
                throw new ArgumentNullException(nameof(entityTypeItem.TypeDescription));

            _fields.Add(entityTypeItem.TypeId,entityTypeItem);
        }

        public EntityTypeItem GetField(string fieldKey)
        {
            return _fields[fieldKey];
        }
        public EntityType ValidateFieldExist(string fieldKey)
        {
            if (!this.HasField(fieldKey))
            {
                throw new ArgumentException($"Поле {fieldKey} не задано для сущности {Name}");
            }

            return this;
        }

        public void ValidateFieldEditable(string fieldKey)
        {
            ValidateFieldExist(fieldKey);
            if (_fields[fieldKey].IsReadOnly)
            {
                throw new FieldAccessException($"Поле {this.Name}.{fieldKey} только для чтения");
            }
        }
    }

    public class EntityTypeItem
    {
        public bool IsReadOnly { get; }

        public EntityTypeItem(bool isReadOnly=false)
        {
            IsReadOnly = isReadOnly;
        }

        public string TypeId { get; set; }
        
        public string TypeDescription { get; set; }
    }
}
