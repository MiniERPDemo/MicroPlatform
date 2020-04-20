using System.Linq;
using NUnit.Framework;

namespace MicroPlatform.Test
{
    public class EntityEventTest
    {
        [Test]
        public void TestEventField()
        {
            var entityFactory = new EntityFactory();
            
            EntityChangedEvent entityChanged=null;

            entityFactory.EntityValueChanged += (s, e) => { entityChanged = e; };
            
            var errandType = entityFactory.CreateType("Errand");

            entityFactory.RegisterPlugin(new FakeErrandExtensions());

            errandType.AddField(new EntityTypeFieldItem()
            {
                FieldId = "name",
                FieldDescription = "Название",
                FieldType = "string"
            });

            var errandItem1 = entityFactory.CreateItem(errandType);
            errandItem1.SetValue("name", "Проект");

            
            Assert.AreEqual("Errand", entityChanged?.Target?.EntityType?.Name);
            Assert.AreEqual("name", entityChanged.FieldKey);
            Assert.AreEqual("", entityChanged.OldValue);
            Assert.AreEqual("Проект", entityChanged.NewValue);
        }

        
    }
}