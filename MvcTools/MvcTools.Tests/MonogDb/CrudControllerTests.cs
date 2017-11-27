﻿// MvcTools.Tests.CrudControllerTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MonogDb
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDb;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using ResultTypes;

    [TestClass]
    public class CrudControllerTests
    {
        private const string Database = "Test";
        private const string Collection = "CrudController";

        [TestMethod]
        public async Task TestGetDocumentsAsync()
        {
            var controller = new InternalController(new MongoClient(), Database, Collection);
            var documents = await controller.GetDocuments(FilterDefinition<Document>.Empty.CrudFilter());
            Assert.IsInstanceOfType(documents, typeof(JsonStringResult));
        }

        [TestMethod]
        public async Task TestPostDocumentAsync()
        {
            var controller = new InternalController(new MongoClient(), Database, Collection);
            var result = await controller.PostDocument(new Document { Id = ObjectId.GenerateNewId() });
            Assert.IsInstanceOfType(result, typeof(JsonStringResult));
            result = await controller.PostDocument(new Document { Id = ObjectId.Empty });
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task TestPutDocumentAsync()
        {
            var controller = new InternalController(new MongoClient(), Database, Collection);
            var result = await controller.PutDocument(new Document { Id = ObjectId.GenerateNewId() });
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            result = await controller.PutDocument(new Document { Id = ObjectId.Empty });
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task TestDefaultPutDocumentAsync()
        {
            var controller = new DefaultCrudControllerBase(new MongoClient(), Database, Collection);
            var result = await controller.PutDocument(new Document { Id = ObjectId.GenerateNewId() });
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        public async Task TestDeleteDocumentAsync()
        {
            var controller = new InternalController(new MongoClient(), Database, Collection);
            var document = new Document { Id = ObjectId.GenerateNewId() };
            await controller.PostDocument(document);
            var result = await controller.DeleteDocument(document.Id);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            result = await controller.DeleteDocument(ObjectId.Empty);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}