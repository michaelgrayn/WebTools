﻿// MvcTools.Tests.Document.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MonogDb
{
    using MongoDb;
    using MongoDB.Bson;

    // ReSharper disable All
    public class Document : MongoDbDocument<Document>
    {
        public override ObjectId Id
        {
            get => base.Id;
            set => base.Id = value;
        }

        public int Value { get; set; }
    }
}