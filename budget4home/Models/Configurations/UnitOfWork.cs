using System.Threading.Tasks;

namespace budget4home.Models.Configurations
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begin transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit
        /// </summary>
        Task<int> CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public void BeginTransaction()
        {
            // Method intentionally left empty.
        }

        /// <inheritdoc />
        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}