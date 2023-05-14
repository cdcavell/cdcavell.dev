using ClassLibrary.Data.Extensions;
using ClassLibrary.Data.Models;
using ClassLibrary.Data.Options;
using ClassLibrary.Mvc.Services.AppSettings;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.EntityFramework.Options;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Claims;
using System.Xml.Linq;
using static Duende.IdentityServer.Models.IdentityResources;
using Client = Duende.IdentityServer.EntityFramework.Entities.Client;

namespace ClassLibrary.Data
{
    /// <summary>
    /// DbContext for Application and IdentityServer data.
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="IConfigurationDbContext" />
    /// <seealso cref="IPersistedGrantDbContext" />
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/25/2022 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 11/13/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/02/2022 | Duende IdentityServer Development |~ 
    /// | Christopher D. Cavell | 1.0.1.0 | 09/05/2022 | Data Access Layer Development |~ 
    /// </revision>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IConfigurationDbContext, IPersistedGrantDbContext
    {
        private readonly ILogger _logger;
        private readonly string _assemblyName;
        private readonly string _userId;

        /// <summary>
        /// IdentityServer Configuration store options.
        /// </summary>
        public ApplicationStoreOptions ApplicationStoreOptions { get => applicationStoreOptions; set => applicationStoreOptions = value; }
        private ApplicationStoreOptions applicationStoreOptions = new();

        /// <summary>
        /// IdentityServer Configuration store options.
        /// </summary>
        public ConfigurationStoreOptions ConfigurationStoreOptions { get => configurationStoreOptions; set => configurationStoreOptions = value; }
        private ConfigurationStoreOptions configurationStoreOptions = new();

        /// <summary>
        /// IdentityServer Operational store options.
        /// </summary>
        public OperationalStoreOptions OperationalStoreOptions { get => operationalStoreOptions; set => operationalStoreOptions = value; }
        private OperationalStoreOptions operationalStoreOptions = new();

        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext&lt;TContext&gt; class.
        /// </summary>
        /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
        /// <seealso cref="IConfigurationDbContext" />
        /// <param name="options">DbContextOptions&lt;ApplicationDbContext&gt;</param>
        /// <param name="logger">ILogger&lt;ApplicationDbContext&gt;</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <method>
        /// ApplicationDbContext(
        ///     DbContextOptions&lt;ApplicationDbContext;&gt; options,
        ///     ILogger&lt;ApplicationDbContext&gt; logger,
        ///     IHttpContextAccessor httpContextAccessor
        /// ) : base(options)
        /// </method>
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ILogger<ApplicationDbContext> logger,
            IHttpContextAccessor httpContextAccessor
        ) : base(options)
        {
            _logger = logger;
            _assemblyName = this.GetType().Assembly.GetParentAssembly().GetName().Name ?? "Unknown Assembly";

            if (httpContextAccessor.HttpContext != null)
            {
                var request = httpContextAccessor.HttpContext.Request;
                var remoteIp = request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                var user = httpContextAccessor.HttpContext.User;

                Claim? idClaim = (user != null) ? user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("email") : null;
                if (idClaim != null)
                    _userId = idClaim.Value ?? (remoteIp ?? "Anonymous");
                else
                    _userId = remoteIp ?? "Anonymous";
            }
            else
            {
                _userId = _assemblyName;
            }

            _userId ??= "Anonymous";

            AuditHistory = Set<AuditHistory>();

            Clients = Set<Client>();
            ClientCorsOrigins = Set<ClientCorsOrigin>();
            IdentityResources = Set<IdentityResource>();
            ApiResources = Set<ApiResource>();
            ApiScopes = Set<ApiScope>();
            IdentityProviders = Set<IdentityProvider>();

            PersistedGrants = Set<PersistedGrant>();
            DeviceFlowCodes = Set<DeviceFlowCodes>();
            Keys = Set<Key>();
            ServerSideSessions = Set<ServerSideSession>();

            Registration = Set<Registration>();
        }

        /// <value>DbSet&lt;AuditHistory&gt;</value>
        public DbSet<AuditHistory> AuditHistory { get; set; }

        /// <value>DbSet&lt;Client&gt;</value>
        public DbSet<Client> Clients { get; set; }

        /// <value>DbSet&lt;ClientCorsOrigin&gt;</value>
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }

        /// <value>DbSet&lt;IdentityResource&gt;</value>
        public DbSet<IdentityResource> IdentityResources { get; set; }

        /// <value>DbSet&lt;ApiResource&gt;</value>
        public DbSet<ApiResource> ApiResources { get; set; }

        /// <value>DbSet&lt;ApiScope&gt;</value>
        public DbSet<ApiScope> ApiScopes { get; set; }

        /// <value>DbSet&lt;IdentityProvider&gt;</value>
        public DbSet<IdentityProvider> IdentityProviders { get; set; }

        /// <value>DbSet&lt;PersistedGrant&gt;</value>
        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        /// <value>DbSet&lt;DeviceFlowCodes&gt;</value>
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        /// <value>DbSet&lt;Key&gt;</value>
        public DbSet<Key> Keys { get; set; }

        /// <value>DbSet&lt;ServerSideSession&gt;</value>
        public DbSet<ServerSideSession> ServerSideSessions { get; set; }

        /// <value>DbSet&lt;AuditHistory&gt;</value>
        public DbSet<Registration> Registration { get; set; }

        /// <summary>
        /// OnModelCreating method
        /// &lt;br /&gt;&lt;br /&gt;
        /// Customize the ASP.NET Identity model and override the defaults if needed.
        /// For example, you can rename the ASP.NET Identity table names and more.
        /// Add your customizations after calling base.OnModelCreating(builder);
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder</param>
        /// <method>OnModelCreating(ModelBuilder modelBuilder)</method>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (ApplicationStoreOptions is null)
            {
                ApplicationStoreOptions = this.GetService<ApplicationStoreOptions>();

                if (ApplicationStoreOptions is null)
                {
                    throw new ArgumentNullException(nameof(ApplicationStoreOptions), "ApplicationStoreOptions must be configured in the DI system.");
                }
            }

            modelBuilder.ConfigureApplicationContext(ApplicationStoreOptions);

            if (ConfigurationStoreOptions is null)
            {
                ConfigurationStoreOptions = this.GetService<ConfigurationStoreOptions>();

                if (ConfigurationStoreOptions is null)
                {
                    throw new ArgumentNullException(nameof(ConfigurationStoreOptions), "ConfigurationStoreOptions must be configured in the DI system.");
                }
            }

            modelBuilder.ConfigureClientContext(ConfigurationStoreOptions);
            modelBuilder.ConfigureResourcesContext(ConfigurationStoreOptions);
            modelBuilder.ConfigureIdentityProviderContext(ConfigurationStoreOptions);

            if (OperationalStoreOptions is null)
            {
                OperationalStoreOptions = this.GetService<OperationalStoreOptions>();

                if (OperationalStoreOptions is null)
                {
                    throw new ArgumentNullException(nameof(OperationalStoreOptions), "OperationalStoreOptions must be configured in the DI system.");
                }
            }

            modelBuilder.ConfigurePersistedGrantContext(OperationalStoreOptions);

            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// OnConfiguring method
        /// </summary>
        /// <param name="optionsBuilder">DbContextOptionsBuilder</param>
        /// <method>OnConfiguring(DbContextOptionsBuilder optionsBuilder)</method>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.LogTo(message => _logger.LogTrace("{message}", message));
                optionsBuilder.EnableDetailedErrors();
            }
        }

        /// <summary>
        /// Checks for any unsaved INSERT, UPDATE, DELETE history.
        /// </summary>
        /// <returns>bool</returns>
        /// <method>HasUnsavedChanges()</method>
        public bool HasUnsavedChanges()
        {
            return ChangeTracker.HasChanges();
        }

        /// <summary>
        /// Override to record all the data change history in a table named ```Audit```, this table 
        /// contains INSERT, UPDATE, DELETE history.
        /// </summary>
        /// <returns>int</returns>
        /// <method>SaveChanges(bool acceptAllChangesOnSuccess = true)</method>
        public override int SaveChanges(bool acceptAllChangesOnSuccess = true)
        {
            // uncomment to have audit records greated than 60 days old removed from dbo.AuditHistory table
            RemoveAuditRecords();

            List<AuditEntry> auditEntries = OnBeforeSaveChanges(_userId);
            int result = base.SaveChanges(acceptAllChangesOnSuccess);
            OnAfterSaveChanges(auditEntries);

            return result;
        }

        /// <summary>
        /// Override to record all the data change history in a table named ```Audit```, this table 
        /// contains INSERT, UPDATE, DELETE history.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task&lt;int&gt;</returns>
        /// <method>SaveChangesAsync(CancellationToken cancellationToken = default)</method>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // uncomment to have audit records greated than 7 days old removed from dbo.AuditHistory table
            RemoveAuditRecords();

            List<AuditEntry> auditEntries = OnBeforeSaveChanges(_userId);
            int result = await base.SaveChangesAsync(true, cancellationToken);
            await OnAfterSaveChangesAsync(auditEntries);

            return result;
        }

        /// <summary>
        /// Save audit entities that have all the modifications and return list of entries 
        /// where the value of some properties are unknown at this step.
        /// &lt;br/&gt;&lt;br/&gt;
        /// https://www.meziantou.net/entity-framework-core-history-audit-table.htm
        /// </summary>
        /// <returns>List&lt;AuditEntry&gt;</returns>
        /// <method>OnBeforeSaveChanges(string userId)</method>
        private List<AuditEntry> OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditHistory || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Metadata.GetTableName() ?? string.Empty,
                    Application = _assemblyName,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    State = entry.State.ToString()
                };
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue ?? string.Empty;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.CurrentValues[propertyName] = property.CurrentValue ?? string.Empty;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OriginalValues[propertyName] = property.OriginalValue ?? string.Empty;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OriginalValues[propertyName] = property.OriginalValue ?? string.Empty;
                                auditEntry.CurrentValues[propertyName] = property.CurrentValue ?? string.Empty;
                            }
                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                AuditHistory.Add(auditEntry.ToAuditHistory());
            }

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        /// <summary>
        /// Save audit entities where the value of some properties were unknown at previous step.
        /// &lt;br/&gt;&lt;br/&gt;
        /// https://www.meziantou.net/entity-framework-core-history-audit-table.htm
        /// </summary>
        /// <method>OnAfterSaveChanges(List&lt;AuditEntry&gt; auditEntries)</method>
        private void OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue ?? string.Empty;
                    }
                    else
                    {
                        auditEntry.CurrentValues[prop.Metadata.Name] = prop.CurrentValue ?? string.Empty;
                    }
                }

                // Save the Audit entry
                AuditHistory.Add(auditEntry.ToAuditHistory());
                SaveChanges();
            }

            return;
        }

        /// <summary>
        /// Save audit entities where the value of some properties were unknown at previous step.
        /// &lt;br/&gt;&lt;br/&gt;
        /// https://www.meziantou.net/entity-framework-core-history-audit-table.htm
        /// </summary>
        /// <method>OnAfterSaveChangesAsync(List&lt;AuditEntry&gt; auditEntries)</method>
        private Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue ?? string.Empty;
                    }
                    else
                    {
                        auditEntry.CurrentValues[prop.Metadata.Name] = prop.CurrentValue ?? string.Empty;
                    }
                }

                // Save the Audit entry
                AuditHistory.Add(auditEntry.ToAuditHistory());
            }

            return SaveChangesAsync();
        }

        /// <summary>
        /// Remove audit records greater than 60 days old
        /// </summary>
        /// <method>RemoveAuditRecords()</method>
        private void RemoveAuditRecords()
        {
            IQueryable<AuditHistory> query = this.AuditHistory.Where(Audit => Audit.ModifiedOn < DateTime.Now.AddDays(-60));
            if (query.Any())
                this.AuditHistory.RemoveRange(query);
        }

        /// <summary>
        /// Method to apply database migrations.
        /// &lt;br/&gt;&lt;br/&gt;
        /// To Initialize Migration: 
        ///  `dotnet ef migrations add InitialCreate_ApplicationDb --context ApplicationDbContext --output-dir Data/Migrations/ApplicationDb`
        /// To Update Migration:     
        ///  `dotnet ef migrations add Update_ApplicationDb_YYYYMMDD --context ApplicationDbContext --output-dir Data/Migrations/ApplicationDb`
        /// &lt;br/&gt;&lt;br/&gt;
        /// EF Core tools reference: https://docs.microsoft.com/en-us/ef/core/cli/dotnet
        /// Install EF Core Tools: `dotnet tool install --global dotnet-ef`
        /// Upgrade EF Core Tools: `dotnet tool update --global dotnet-ef`
        /// Before you can use the tools on a specific project, you'll need to add the Microsoft.EntityFrameworkCore.Design package to it.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;string&gt;&gt;</returns>
        /// <method>Migrate(IApplicationBuilder app)</method>
        public static async Task<IEnumerable<string>> Migrate(IApplicationBuilder app)
        {
            List<string> appliedMigrations = new();

            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            if (scopeFactory != null)
                using (var serviceScope = scopeFactory.CreateScope())
                {
                    var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    if (applicationDbContext != null)
                    {
                        foreach (string migration in applicationDbContext.Database.GetPendingMigrations())
                            appliedMigrations.Add($"Applied Migration: {migration}");

                        await applicationDbContext.Database.MigrateAsync();
                    }
                }

            return appliedMigrations;
        }

        /// <summary>
        /// Load objects to database
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;KeyValuePair&lt;LogLevel, string&gt;&gt;&gt;</returns>
        /// <method>InitializeDatabase(IApplicationBuilder app)</method>
        public static async Task<IEnumerable<KeyValuePair<LogLevel, string>>> InitializeDatabase(IApplicationBuilder app)
        {
            List<KeyValuePair<LogLevel, string>> messageItems = new();

            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            if (scopeFactory != null)
                using (var serviceScope = scopeFactory.CreateAsyncScope())
                {
                    var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    if (applicationDbContext != null)
                    {
                        // Load IdentityResources
                        messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Loading Identity Resources"));
                        foreach (Type type in typeof(Duende.IdentityServer.Models.IdentityResources)
                            .GetNestedTypes()
                            .Where(x => x.IsClass)
                            .ToArray()
                        ) 
                        {
                            IdentityResource? entityResource = await applicationDbContext.IdentityResources
                                .Where(x => x.Name.ToLower().Trim() == type.Name.ToLower().Trim())
                                .FirstOrDefaultAsync();

                            if (entityResource == null)
                            {
                                switch (type.Name)
                                {
                                    case nameof(OpenId):
                                        await applicationDbContext.IdentityResources.AddAsync(new OpenId().ToEntity());
                                        goto case "dbSave";
                                    case nameof(Profile):
                                        await applicationDbContext.IdentityResources.AddAsync(new Profile().ToEntity());
                                        goto case "dbSave";
                                    case nameof(Email):
                                        await applicationDbContext.IdentityResources.AddAsync(new Email().ToEntity());
                                        goto case "dbSave";
                                    case nameof(Phone):
                                        await applicationDbContext.IdentityResources.AddAsync(new Phone().ToEntity());
                                        goto case "dbSave";
                                    case nameof(Address):
                                        await applicationDbContext.IdentityResources.AddAsync(new Address().ToEntity());
                                        goto case "dbSave";
                                    case "dbSave":
                                        await applicationDbContext.SaveChangesAsync();
                                        messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Identity Resource {type.Name} not found, loaded"));
                                        break;
                                    default:
                                        messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Warning, $"Identity Resource {type.Name} not found, not loaded"));
                                        break;
                                }
                            }
                            else
                            {
                                messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Identity Resource {type.Name} found, skipped loading"));
                            }
                        }

                        var appSettingsService = serviceScope.ServiceProvider.GetRequiredService<IAppSettingsService>();

                        //Load Api Scopes
                        List<Duende.IdentityServer.Models.ApiScope> apiScopes = appSettingsService.GetApiScopes();
                        if (apiScopes.Any())
                        {
                            messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Loading Api Scopes"));
                            foreach (Duende.IdentityServer.Models.ApiScope modelApiScope in apiScopes)
                            {
                                ApiScope? entityApiScope = await applicationDbContext.ApiScopes
                                    .Where(x => x.Name.ToLower().Trim() == modelApiScope.Name.ToLower().Trim())
                                    .FirstOrDefaultAsync();

                                if (entityApiScope == null)
                                {
                                    await applicationDbContext.ApiScopes.AddAsync(modelApiScope.ToEntity());
                                    await applicationDbContext.SaveChangesAsync();
                                    messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Api Scope {modelApiScope.Name} not found, loaded"));
                                }
                                else
                                {
                                    messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Api Scope {modelApiScope.Name} found, skipped loading"));
                                }
                            }
                        }

                        //Load Client Resources
                        List<Duende.IdentityServer.Models.Client> clients = appSettingsService.GetClients();
                        if (clients.Any())
                        {
                            messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Loading Client Resources"));
                            foreach (Duende.IdentityServer.Models.Client modelClient in clients)
                            {
                                Client? entityClient = await applicationDbContext.Clients
                                    .Where(x => x.ClientId.ToLower().Trim() == modelClient.ClientId.ToLower().Trim())
                                    .FirstOrDefaultAsync();

                                if (entityClient == null)
                                {
                                    // requires hash secret
                                    foreach (var secret in modelClient.ClientSecrets)
                                        secret.Value = secret.Value.Sha256();

                                    await applicationDbContext.Clients.AddAsync(modelClient.ToEntity());
                                    await applicationDbContext.SaveChangesAsync();
                                    messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Client Resource {modelClient.ClientName} not found, loaded"));
                                }
                                else
                                {
                                    messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Client Resource {modelClient.ClientName} found, skipped loading"));
                                }
                            }
                        }

                        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                        //Load Role Claims
                        List <Mvc.Services.AppSettings.Models.Role> roles = appSettingsService.GetRoles();
                        if (roles.Any())
                        {
                            messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Loading Role Claims"));
                            foreach (Mvc.Services.AppSettings.Models.Role modelRole in roles)
                            {
                                IdentityRole? entityRole = await roleManager.FindByNameAsync(modelRole.Name);
                                if (entityRole == null)
                                {
                                    entityRole = new IdentityRole(modelRole.Name.Clean());
                                    await roleManager.CreateAsync(entityRole);
                                    messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Role {modelRole.Name} not found, loaded"));
                                }
                                else
                                {
                                    messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Role {modelRole.Name} found, skipped loading"));
                                }

                                var entityClaims = await roleManager.GetClaimsAsync(entityRole);
                                foreach (string claimValue in modelRole.Claims)
                                {
                                    Claim? entityClaim = entityClaims
                                        .Where(x => x.Type == JwtClaimTypes.Role)
                                        .Where(x => x.Value == claimValue.Clean())
                                        .FirstOrDefault();

                                    if (entityClaim == null)
                                    {
                                        entityClaim = new Claim(JwtClaimTypes.Role, claimValue.Clean());
                                        await roleManager.AddClaimAsync(entityRole, entityClaim);
                                        messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Role {modelRole.Name} Claim {claimValue} not found, loaded"));
                                    }
                                    else
                                    {
                                        messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Role {modelRole.Name} Claim {claimValue} found, skipped loading"));
                                    }
                                }
                            }
                        }

                        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                        //Load System Administrator
                        Mvc.Services.AppSettings.Models.SysAdmin sysAdmin = appSettingsService.GetSysAdmin();
                        if (!string.IsNullOrEmpty(sysAdmin?.Email.Clean()))
                        {
                            string sysAdminEmail = sysAdmin.Email.Clean();

                            messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Loading System Administrator"));
                            ApplicationUser? entityUser = await userManager.FindByEmailAsync(sysAdminEmail);

                            if (entityUser == null)
                            {
                                entityUser = new()
                                {
                                    Email = sysAdminEmail,
                                    EmailConfirmed = true,
                                    UserName = sysAdminEmail,
                                    NormalizedEmail = sysAdminEmail.ToUpper(),
                                    NormalizedUserName = sysAdminEmail.ToUpper(),
                                    DisplayName = "System Administrator",
                                    Status = UserStatus.Active
                                };

                                IdentityResult identityResult = await userManager.CreateAsync(entityUser);
                                if (!identityResult.Succeeded)
                                    throw new Exception(identityResult.Errors.First().Description);
        
                                entityUser = await userManager.FindByEmailAsync(sysAdminEmail);
                                if (entityUser == null)
                                    throw new Exception($"System Administrator not found, not loaded");

                                messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"System Administrator not found, loaded"));
                            }
                            else
                            {
                                messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"System Administrator found, skipped loading"));
                            }

                            messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"Loading System Administrator Claims"));

                            IList<Claim> entityUserClaims = await userManager.GetClaimsAsync(entityUser);
                            Claim? entityClaim = entityUserClaims.Where(x => x.Type == JwtClaimTypes.Name).FirstOrDefault();
                            if (entityClaim == null)
                            {
                                IdentityResult identityResult = await userManager.AddClaimAsync(entityUser, new Claim(JwtClaimTypes.Name, entityUser.DisplayName));
                                if (!identityResult.Succeeded)
                                    throw new Exception(identityResult.Errors.First().Description);

                                messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"System Administrator Claim {JwtClaimTypes.Name} not found, loaded"));
                            }
                            else
                            {
                                messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"System Administrator Claim {JwtClaimTypes.Name} found, skipped loading"));
                            }

                            entityClaim = entityUserClaims.Where(x => x.Type == JwtClaimTypes.Email).FirstOrDefault();
                            if (entityClaim == null)
                            {
                                IdentityResult identityResult = await userManager.AddClaimAsync(entityUser, new Claim(JwtClaimTypes.Email, entityUser.Email));
                                if (!identityResult.Succeeded)
                                    throw new Exception(identityResult.Errors.First().Description);

                                messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"System Administrator Claim {JwtClaimTypes.Email} not found, loaded"));
                            }
                            else
                            {
                                messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"System Administrator Claim {JwtClaimTypes.Email} found, skipped loading"));
                            }

                            IEnumerable<string> modelSysAdminClaims = roles
                                .Select(x => x.Claims
                                    .Where(x => x.Contains("SysAdmin"))
                                    .Select(x => x.ToString())
                                ).FirstOrDefault() ?? new List<string>();

                            foreach (string modelSysAdminClaim in modelSysAdminClaims)
                            {
                                entityClaim = entityUserClaims
                                    .Where(x => x.Type == JwtClaimTypes.Role)
                                    .Where(x => x.Value == modelSysAdminClaim)
                                    .FirstOrDefault();
                                if (entityClaim == null)
                                {
                                    IdentityResult identityResult = await userManager.AddClaimAsync(entityUser, new Claim(JwtClaimTypes.Role, modelSysAdminClaim.Clean()));
                                    if (!identityResult.Succeeded)
                                        throw new Exception(identityResult.Errors.First().Description);

                                    messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"System Administrator Role Claim {modelSysAdminClaim} not found, loaded"));
                                }
                                else
                                {
                                    messageItems.Add(new KeyValuePair<LogLevel, string>(LogLevel.Information, $"System Administrator Role Claim {modelSysAdminClaim} found, skipped loading"));
                                }
                            }
                        }
                    }
                }

            return messageItems;
        }
    }
}
