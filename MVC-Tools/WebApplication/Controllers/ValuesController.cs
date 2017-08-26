using FluentController;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : FluentControllerBase
    {
        private readonly IRepository _repository;

        public ValuesController(IRepository repository)
        {
            _repository = repository;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = new Param { Index = id };
            return await CheckRequest(p).Action(_repository.Delete).ResponseAsync();
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await
                 Action(_repository.GetDefault)
                .Success(model => Xml(new Param { Value = model }))
                .Error((e, vr) => new BadRequestResult())
                .ResponseAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = new Param { Index = id };
            return await
                 CheckRequest(p)
                 .Action(_repository.Get)
                .Success(model => Json(new Param { Index = id, Value = model }))
                .ResponseAsync();
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Param param)
        {
            return await CheckRequest(param).Action(_repository.Add).ResponseAsync();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]string value)
        {
            var p = new Param { Index = id, Value = value };
            return await CheckRequest(p).Action(_repository.Update).ResponseAsync();
        }
    }
}
