using Microsoft.EntityFrameworkCore;

namespace ReadMeter.Api.Data;

public abstract class ReadOnlyDbContext : DbContext
{
    private readonly bool _readOnly;

    protected ReadOnlyDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _readOnly = configuration.GetValue("DatabaseSafety:ReadOnly", true);
    }

    public override int SaveChanges() =>
        _readOnly ? throw ReadOnlyException() : base.SaveChanges();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _readOnly
            ? Task.FromException<int>(ReadOnlyException())
            : base.SaveChangesAsync(cancellationToken);

    private static InvalidOperationException ReadOnlyException() =>
        new("Database đang ở chế độ chỉ đọc.");
}
