using GraphQLserver.Services;
using HotChocolate.Resolvers;

namespace GraphQLserver.Middleware
{
    public class GraphQLTenantMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly ITenantIdResolverService _tenantIdResolver;

        public GraphQLTenantMiddleware(FieldDelegate next, ITenantIdResolverService tenantIdResolver)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _tenantIdResolver = tenantIdResolver ?? throw new ArgumentNullException(nameof(tenantIdResolver));
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
           
            if (!context.ContextData.TryGetValue("HttpContext", out var httpContextObj) || httpContextObj is not HttpContext httpContext)
            {
                throw new GraphQLException(new Error("HttpContext is missing in ContextData").WithCode("MISSING_HTTP_CONTEXT"));
            }
            var tenantId = _tenantIdResolver.TenantId;
            if (!tenantId.HasValue)
            {
                throw new GraphQLException(new Error("Tenant ID is missing or invalid in the header").WithCode("INVALID_TENANT"));
            }
            context.ContextData["TenantId"] = tenantId.Value;
            await _next(context);
        }
    }
}