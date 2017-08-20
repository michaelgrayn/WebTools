using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public interface IRepository
    {
        Task Add(Param p);
        Task<string> Get(Param p);
        Task<string> GetDefault();
    }

    public class Repository : IRepository
    {
        private static readonly IList<string> Values = new List<string> { "value0", "value1", "value2" };

        public async Task Add(Param p)
        {
            await Task.Run(() => Values.Add(p.Value));
        }

        public async Task<string> Get(Param p)
        {
            return await Task.FromResult(Values[p.Int]);
        }

        public async Task<string> GetDefault()
        {
            return await Task.FromResult(Values.FirstOrDefault());
        }
    }
}