using Microsoft.Extensions.Primitives;

namespace GraphQLserver.Services
{
    public class HeaderTenantIdResolverService : ITenantIdResolverService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<HeaderTenantIdResolverService> _logger;
        private readonly string _headerName = "x-tenant";

        public HeaderTenantIdResolverService(IHttpContextAccessor context, ILogger<HeaderTenantIdResolverService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int? TenantId
        {
            get
            {
                try
                {
                    string? tenantValue = GetHeaderValue(_headerName);

                    if (!string.IsNullOrEmpty(tenantValue))
                    {
                        if (int.TryParse(tenantValue, out int tenantId))
                        {
                            return tenantId;
                        }
                        else
                        {
                            _logger.LogError("Invalid tenant ID format in header: {TenantValue}", tenantValue);
                            throw new FormatException($"HeaderTenantIdResolverService: '{tenantValue}' is not a valid tenant ID.");
                        }
                    }

                    _logger.LogDebug("Header '{HeaderName}' is missing. Returning default tenant ID.", _headerName);
                    return default;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while resolving the tenant ID.");
                    throw;
                }
            }
        }

        private string? GetHeaderValue(string headerName)
        {
            if (_context?.HttpContext?.Request.Headers.TryGetValue(headerName, out StringValues value) == true)
            {
                return value.ToString();
            }
            return null;
        }
    }
}
