using System.Security.Claims;

namespace Multitenancy.Tenants;

public interface ITenantProvider
{
	TenantInfo GetCurrentTenantInfo();
}

public class TenantProvider : ITenantProvider
{
	private readonly TenantInfo _tenantInfo;

	public TenantProvider(IHttpContextAccessor accessor)
	{
		var userIdValue = accessor.HttpContext?.User.FindFirstValue("user-id");

		Guid? userId = Guid.TryParse(userIdValue, out var guid) ? guid : null;
		Guid? headerTenantId = null;

		if (accessor.HttpContext?.Request.Headers.TryGetValue("X-TenantId", out var headerGuid) is true)
		{
			headerTenantId = Guid.Parse(headerGuid.ToString());
		}

		_tenantInfo = new TenantInfo(userId, headerTenantId);
	}

	public TenantInfo GetCurrentTenantInfo() => _tenantInfo;
}

// public class TenantProvider : ITenantProvider
// {
// 	private readonly TenantInfo _tenantInfo;
//
// 	public TenantProvider(IHttpContextAccessor accessor)
// 	{
// 		var userIdValue = accessor.HttpContext?.User.FindFirstValue("user-id");
// 		var tenantIdValue = accessor.HttpContext?.User.FindFirstValue("tenant-id");
//
// 		Guid? userId = Guid.TryParse(userIdValue, out var guid) ? guid : null;
// 		Guid? tenantId = Guid.TryParse(tenantIdValue, out guid) ? guid : null;
//
// 		_tenantInfo = new TenantInfo(userId, tenantId);
// 	}
//
// 	public TenantInfo GetCurrentTenantInfo() => _tenantInfo;
// }
