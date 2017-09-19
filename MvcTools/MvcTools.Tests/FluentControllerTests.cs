// MvcTools.MvcTools.Tests.FluentControllerTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ResultTypes;

    public class Fail : IValidatable
    {
        public bool Validate()
        {
            return false;
        }
    }

    [TestClass]
    public class FluentControllerTests : FluentControllerBase
    {
        [TestMethod]
        public async Task TestParameterMainActionSuccessAsync()
        {
            var parameter = new NoValidation();
            var response = await CheckRequest(parameter).Action(async s => await Task.FromResult(s)).Success(Xml).ResponseAsync();
            Assert.IsInstanceOfType(response, typeof(XmlResult));
        }

        [TestMethod]
        public async Task TestMultipleActions()
        {
            // since there is no success call the default will be used
            var response = await Action(() => Task.FromResult("Failure"))
                .Action(async x => await Task.Delay(500))
                .Action(async x => await Task.Delay(500))
                .Error(Json).ResponseAsync();
            Assert.IsInstanceOfType(response, typeof(EmptyResult));
        }

        [TestMethod]
        public async Task TestFailure()
        {
            var p = new Fail();
            var response = await CheckRequest(p).Action(async x => await Task.Delay(100)).Error(e => new AcceptedResult()).ResponseAsync();
            Assert.IsInstanceOfType(response, typeof(AcceptedResult));
        }
    }
}
