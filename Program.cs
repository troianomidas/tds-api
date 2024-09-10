using System.Reflection;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Integrations.Queues;
using WebApi.Integrations.Serverless;
using WebApi.Persistence;
using WebApi.Services.Common.Behaviours;
using WebApi.Services.Common.Security;
using Microsoft.AspNetCore.ResponseCompression;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Npgsql"),
            builder =>
            {
                builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
            })
        .UseSnakeCaseNamingConvention());
        
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddScoped<IQueue, StorageQueue>();
builder.Services.AddScoped<IDelivery3Serverless, Delivery3Serverless>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
});

builder.Services.AddResponseCompression(opts =>
{
    opts.EnableForHttps = true;
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();
app.UseResponseCompression();

var supportedCultures = new[] { "pt-BR" };

RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures("pt-BR")
    .SetDefaultCulture(supportedCultures[0]);

app.UseRequestLocalization(localizationOptions);

app.Run();