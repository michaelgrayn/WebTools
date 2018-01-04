// MvcTools.Tests.Document.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MongoDb
{
    using MongoDB.Bson;
    using MvcTools.Domain.MongoDb;

    public class Document : MongoDbDocument
    {
        public override ObjectId Id
        {
            get => base.Id;
            set
            {
                if (value == default) value = ObjectId.GenerateNewId();
                base.Id = value;
            }
        }

        public int Value { get; set; }
    }
}
