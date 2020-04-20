using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MicroPlatform
{
    public class EntityType
    {
        private readonly IEntityProvider _entityProvider;
        private readonly Dictionary<string,EntityTypeFieldItem> _fields=new Dictionary<string, EntityTypeFieldItem>();

        public EntityType(string name, IEntityProvider entityProvider)
        {
            _entityProvider = entityProvider;
            Name = name;
            _fields["id"] = new EntityTypeFieldItem(true)
            {
                FieldId = "id",
                FieldDescription = "Идентификатор",
            };
        }

        public string Name { get; }

        public bool HasField(string fieldName)
        {
            return _fields.ContainsKey(fieldName);
        }

        public string[] PrimitiveTypes = new[]
        {
            "int",
            "string",
            "date"
        };

        public void AddField(EntityTypeFieldItem entityTypeFieldItem)
        {
            ValidateEntityType(entityTypeFieldItem);

            _fields.Add(entityTypeFieldItem.FieldId,entityTypeFieldItem);
        }

        private void ValidateEntityType(EntityTypeFieldItem entityTypeFieldItem)
        {
            if (entityTypeFieldItem == null)
                throw new ArgumentNullException(nameof(entityTypeFieldItem));

            if (string.IsNullOrWhiteSpace(entityTypeFieldItem.FieldId))
                throw new ArgumentNullException(nameof(entityTypeFieldItem.FieldId));

            if (string.IsNullOrWhiteSpace(entityTypeFieldItem.FieldDescription))
                throw new ArgumentNullException(nameof(entityTypeFieldItem.FieldDescription));

            if (!PrimitiveTypes.Contains(entityTypeFieldItem.FieldType))
            {
                throw new ArgumentException($"Тип {entityTypeFieldItem.FieldType} не известен");
            }
        }

        public EntityTypeFieldItem GetField(string fieldKey)
        {
            if (_fields.ContainsKey(fieldKey))
            {
                return _fields[fieldKey];
            }

            if (_entityProvider != null)
            {
                var extendedTypes = (_entityProvider.GetTypes(this.Name));
                foreach (var extendedType in extendedTypes)
                {
                    if (extendedType.HasField(fieldKey))
                    {
                        return extendedType.GetField(fieldKey);
                    }
                }
            }

            throw new ArgumentException($"У сущности {Name} нет поля {fieldKey}");
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

    public interface IEntityProvider
    {
        IEnumerable<EntityType> GetTypes(string name);
    }

    public class EntityTypeFieldItem
    {
        public bool IsReadOnly { get; }

        public EntityTypeFieldItem(bool isReadOnly=false)
        {
            IsReadOnly = isReadOnly;
        }

        public string FieldId { get; set; }
        
        public string FieldDescription { get; set; }
        public string FieldType { get; set; }
    }
}
