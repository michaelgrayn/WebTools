// MvcTools.Tests.FluentControllerTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MvcTools.FluentController;
    using MvcTools.ResultTypes;

    public class Fail : IValidatable
    {
        public static void SetDefaults()
        {
            FluentControllerBase.DefaultError = new BadRequestResult();
            FluentControllerBase.DefaultSuccess = new EmptyResult();
            var content = new EncodedContentResult { Encoding = Encoding.UTF8 };
            content.Content = content.Encoding.ToString();
        }

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
            var parameter = new List<int> { 1, 2, 3 };
            var response = await RequestParameter(parameter).Action(async s => await Task.FromResult(s)).Success(Xml).ResponseAsync();
            Assert.IsInstanceOfType(response, typeof(XmlResult));
        }

        [TestMethod]
        public async Task TestMultipleActions()
        {
            // since there is no success call the default will be used
            Fail.SetDefaults();
            var response = await Action(() => Task.FromResult("Failure"))
                .Action(async x => await Task.Delay(500))
                .Action(async x => await Task.Delay(500))
                .Error(Json).ResponseAsync();
            Assert.IsInstanceOfType(response, typeof(EmptyResult));
        }

        [TestMethod]
        public async Task TestFailure()
        {
            var response = await RequestParameter(new Fail())
                .Action(async x => await Task.Delay(100))
                .Error(e => new AcceptedResult()).ResponseAsync();
            Assert.IsInstanceOfType(response, typeof(AcceptedResult));
        }
    }
}
