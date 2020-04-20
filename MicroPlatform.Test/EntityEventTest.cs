using System.Linq;
using NUnit.Framework;

namespace MicroPlatform.Test
{
    public class EntityEventTest
    {
        [Test]
        public void TestExtendExistingEntity()
        {
            var entityFactory = new EntityFactory();
            
            entityFactory.EntityValueChanged += (s, e) =>
            {

            };
            
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

            var types = entityFactory.Types.ToList();

            Assert.AreEqual(1, types.Count());
            Assert.AreEqual("Errand", types[0].Name);
            Assert.AreEqual("int", types[0].GetField("Price").FieldType);
        }

        
    }
}