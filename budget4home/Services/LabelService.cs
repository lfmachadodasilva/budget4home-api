using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.Models;

namespace budget4home.Services
{
    public interface ILabelService
    {
        IList<LabelModel> GetAll();
        Task<LabelModel> AddAsync(LabelModel obj);
    }

    public class LabelService : ILabelService
    {
        private readonly Context _context;

        public LabelService(Context context)
        {
            _context = context;
        }

        public IList<LabelModel> GetAll()
        {
            return _context.Labels.ToList();
        }

        public async Task<LabelModel> AddAsync(LabelModel obj)
        {
            var added = await _context.AddAsync(obj);
            if (added != null)
            {
                await _context.SaveChangesAsync();
            }
            return added.Entity;
        }
    }
}