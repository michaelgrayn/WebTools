// MvcTools.Tests.Document.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MongoDb
{
    using Domain.MongoDb;
    using MongoDB.Bson;

    public class Document : MongoDbDocument
    {
        public override ObjectId Id
        {
            get => base.Id;
            set => base.Id = value;
        }

        public int Value { get; set; }
    }
}
