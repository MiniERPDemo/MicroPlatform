using System;
using System.Reflection.PortableExecutable;
using NUnit.Framework;

namespace MicroPlatform.Test
{
    public class EntityTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCreateIntId()
        {
            var entityFactory = new EntityFactory();
            var errandType=entityFactory.CreateType("Errand");
            var issueType=entityFactory.CreateType("Issue");
            
            var errandItem1 = entityFactory.CreateItem(errandType);
            var errandItem2 = entityFactory.CreateItem(errandType);
            var issueItem1 = entityFactory.CreateItem(issueType);

            Assert.AreEqual("1",errandItem1.GetValue("id"));
            Assert.AreEqual("2",errandItem2.GetValue("id"));
            Assert.AreEqual("1",issueItem1.GetValue("id"));
        }

        [Test]
        public void TestCreateEditableField()
        {
            var entityFactory = new EntityFactory();
            var errandType = entityFactory.CreateType("Errand");
            errandType.AddField(new EntityTypeItem()
            {
                TypeId = "name",
                TypeDescription = "Название"
            });

            var errandItem1 = entityFactory.CreateItem(errandType);
            errandItem1.SetValue("name", "Проект");
            Assert.AreEqual("Проект", errandItem1.GetValue("name"));
        }


        [Test]
        public void TestReadonlyField()
        {
            var entityFactory = new EntityFactory();
            var errandType = entityFactory.CreateType("Errand");
            
            var errandItem1 = entityFactory.CreateItem(errandType);

            Assert.Throws<FieldAccessException>(() =>
            {
                errandItem1.SetValue("id", "2");
            });
        }



    }

    
}