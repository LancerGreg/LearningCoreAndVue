using System;

namespace backend.Repositories
{
    public class AppDbRepository : IDisposable
    {
        protected AppDbContext dbContext { get; }

        private bool _disposed;

        protected AppDbRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    dbContext.Dispose();
                _disposed = true;
            }
        }

        ~AppDbRepository() => Dispose(false);
    }
}
