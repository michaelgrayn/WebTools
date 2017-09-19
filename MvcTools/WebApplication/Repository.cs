// MvcTools.WebApplication.Repository.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace WebApplication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository
    {
        Task Add(Param p);

        Task Delete(Param p);

        Task<string> Get(Param p);

        Task<string> GetDefault();

        Task Update(Param p);
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class Repository : IRepository
    {
        private static readonly IList<string> Values = new List<string> { "value0", "value1", "value2" };

        public async Task Add(Param p)
        {
            await Task.Run(() => Values.Add(p.Value));
        }

        public async Task Delete(Param p)
        {
            await Task.Run(() => Values.RemoveAt(p.Index));
        }

        public async Task<string> Get(Param p)
        {
            return await Task.FromResult(Values[p.Index]);
        }

        public async Task<string> GetDefault()
        {
            return await Task.FromResult(Values.FirstOrDefault());
        }

        public async Task Update(Param p)
        {
            await Task.Run(() => Values[p.Index] = p.Value);
        }
    }
}
