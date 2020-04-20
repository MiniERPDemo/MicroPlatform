﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MicroPlatform.Test
{
    public class EntityExtensionsTests
    {
        [Test]
        public void TestCreateAdditionalEntity()
        {
            var entityFactory = new EntityFactory();
            var errandType = entityFactory.CreateType("Errand");

            entityFactory.RegisterPlugin(new FakeIssueExtensions());

            errandType.AddField(new EntityTypeFieldItem()
            {
                FieldId = "name",
                FieldDescription = "Название",
                FieldType = "string"
            });

            var errandItem1 = entityFactory.CreateItem(errandType);
            errandItem1.SetValue("name", "Проект");

            var types = entityFactory.Types.ToList();
            Assert.AreEqual(2,types.Count());

            Assert.AreEqual("Errand",types[0].Name);
            Assert.AreEqual("Issue",types[1].Name);
        }


        [Test]
        public void TestExtendExistingEntity()
        {
            var entityFactory = new EntityFactory();
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

            Assert.AreEqual("int",types[0].GetField("Price").FieldType);
        }


    }

    public class FakeErrandExtensions : IPluginModule
    {
        public string PluginId { get; } = "EAEF1608-876A-4C5C-8D7E-65633A8A5C74";
        public string PluginType { get; } = "Errand";

        public List<EntityType> EntityTypes =>
            new List<EntityType>()
            {
                GetErrandType()
            };


        private EntityType GetErrandType()
        {
            var entityType = new EntityType("Errand",null);
            entityType.AddField(new EntityTypeFieldItem()
            {
                FieldId = "Price",
                FieldDescription = "Цена",
                FieldType = "int"
            });
            return entityType;
        }
    }

    public class FakeIssueExtensions : IPluginModule
    {
        public string PluginId { get; } = "EAEF1608-876A-4C5C-8D7E-65633A8A5C74";
        public string PluginType { get; } = "Issue";

        public List<EntityType> EntityTypes =>
            new List<EntityType>()
            {
                GetErrandType()
            };


        private EntityType GetErrandType()
        {
            var entityType=new EntityType("Issue",null);
            entityType.AddField(new EntityTypeFieldItem()
            {
                FieldId = "Price",
                FieldDescription = "Цена",
                FieldType = "int"
            });
            return entityType;
        }
    }
}