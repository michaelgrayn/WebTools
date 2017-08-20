using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : FluentController.FluentController
    {
        private readonly IRepository _repository;

        public ValuesController(IRepository repository)
        {
            _repository = repository;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Action(_repository.GetDefault).ResponseAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Param param)
        {
            return await CheckRequest(param).Action(_repository.Get).ResponseAsync();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Param param)
        {
            CheckRequest(param).Action(_repository.Add);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
