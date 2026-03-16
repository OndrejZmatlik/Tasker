using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Globalization;
using Tasker.Web.Components;
using Tasker.Web.Components.Account;
using Tasker.Web.Data;
using Tasker.Web.Data.Classes;
using Tasker.Web.Data.Services;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.OpenApi;

namespace Tasker.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            if (builder.Environment.IsProduction())
            {
                var cert = CreateSelfSignedCertificate();
                builder.WebHost.ConfigureKestrel((context, options) =>
                {
                    options.ConfigureEndpointDefaults(t =>
                    {
                        t.UseHttps(cert);
                    });
                    options.ConfigureHttpsDefaults(httpsOptions =>
                    {
                        httpsOptions.ServerCertificate = cert;
                        httpsOptions.ServerCertificateSelector = (connectionContext, name) => cert;
                    });
                });
            }
            // Add services to the container.
            builder.Services.AddRazorComponents()
                            .AddInteractiveServerComponents();
            builder.Services.AddMudServices();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasker API", Version = "v1" });
            });
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<TasksService>();
            builder.Services.AddTransient<Refresh>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("cs-CZ");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("cs-CZ");
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddOpenIdConnect("FarHive SSO", displayName: "FarHive SSO", options =>
                {
                    options.Authority = builder.Configuration["Authentication:FarHive:Authority"] ?? string.Empty;
                    options.ClientId = builder.Configuration["Authentication:FarHive:ClientId"] ?? string.Empty;
                    options.ClientSecret = builder.Configuration["Authentication:FarHive:ClientSecret"] ?? string.Empty;
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.Scope.Add("email");
                    options.CallbackPath = builder.Configuration["Authentication:FarHive:SignInPath"] ?? string.Empty;
                    options.SignedOutCallbackPath = builder.Configuration["Authentication:FarHive:LogOutPath"] ?? string.Empty;
                })
                .AddIdentityCookies();
            if (builder.Environment.IsDevelopment())
            {
                var connectionString = builder.Configuration.GetConnectionString("Development") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                    options.UseNpgsql(connectionString));
            }
            else
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                    options.UseNpgsql(connectionString));

            }
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
            builder.Services.AddDataProtection()
            .PersistKeysToDbContext<ApplicationDbContext>();
            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var identitySeedData = new IdentitySeedData(scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(), scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
            await identitySeedData.InitializeRoles();
            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //app.UseStaticFiles();
            app.MapStaticAssets();
            app.UseAntiforgery();
            string[] supportedCultures = ["cs-CZ"];
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseRequestLocalization(localizationOptions);
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.MapControllers();
            // Add additional endpoints required by the Identity /Account Razor components.
            //app.MapAdditionalIdentityEndpoints();
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasker API v1");
                options.RoutePrefix = "api";
            });
            app.Run();
        }

        private static X509Certificate2 CreateSelfSignedCertificate()
        {
            var subject = new X500DistinguishedName("CN=WrongCert");

            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(subject, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DataEncipherment, false));
                SubjectAlternativeNameBuilder builder = new SubjectAlternativeNameBuilder();
                builder.AddDnsName("WrongCert");
                request.CertificateExtensions.Add(builder.Build());
                request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") },
                    false));
                var cert = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddYears(1)));
                return new X509Certificate2(cert.Export(X509ContentType.Pfx, "heslo"), "heslo", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
        }
    }
}
