namespace UniCore.Application.Entity
{
    public class Tenant
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string ConnectionString { get; set; } = default!;
        public bool IsActive { get; set; }
    }

    public interface ITenantSetter
    {
        Tenant? CurrentTenant { get; set; }
    }

    public interface ITenantGetter
    {
        Tenant? CurrentTenant { get; }
        string? TenantId => CurrentTenant?.Id;
    }

    public class TenantProvider : ITenantSetter, ITenantGetter
    {
        public Tenant? CurrentTenant { get; set; }
    }

}
