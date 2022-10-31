using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Project.Infrastructure
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<Domain.Aggregates.DeviceAggregate.Device> Devices => Set<Domain.Aggregates.DeviceAggregate.Device>();
        public DbSet<Domain.Aggregates.DeviceAggregate.Notification> DeviceNotifications => Set<Domain.Aggregates.DeviceAggregate.Notification>();
        public DbSet<Domain.Aggregates.NotificationAggregate.Notification> Notifications => Set<Domain.Aggregates.NotificationAggregate.Notification>();
        public DbSet<Domain.Aggregates.NotificationAggregate.Device> NotificationDevices => Set<Domain.Aggregates.NotificationAggregate.Device>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityConfigurations.DeviceAggregate.DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.DeviceAggregate.NotificationConfiguration());

            modelBuilder.ApplyConfiguration(new EntityConfigurations.NotificationAggregate.NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.NotificationAggregate.DeviceConfiguration());
        }

        private IDbContextTransaction? _currentTransaction;

        public IDbContextTransaction CurrentTransaction => _currentTransaction!;

        public bool HasActiveTransaction => _currentTransaction != null;

        public void Initialize()
        {
            ChangeTracker.Clear();
            DisposeCurrentTransaction();
        }

        public bool SameActiveTransaction(IDbContextTransaction dbContextTransaction)
        {
            if (HasActiveTransaction)
                return dbContextTransaction == CurrentTransaction;
            return false;
        }

        public virtual async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);

            return _currentTransaction;
        }

        public virtual async Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                DisposeCurrentTransaction();
            }
        }

        public virtual async Task RollbackAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_currentTransaction != null)
                    await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                DisposeCurrentTransaction();
            }
        }

        private void DisposeCurrentTransaction()
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}