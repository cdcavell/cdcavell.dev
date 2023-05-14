using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary.Data
{
    /// <summary>
    /// Static class used to backup current application SQLLite database.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 09/06/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public static class ApplicationDbBackup
    {
        /// <summary>
        /// Backup current application SQLLite database. Method will retain the latest three copies of database.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <method>Run(IApplicationBuilder app)</method>
        public static void Run(IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            if (scopeFactory != null)
                using (var serviceScope = scopeFactory.CreateScope())
                {
                    var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    string dataSource = applicationDbContext.Database.GetDbConnection().DataSource;
                    string appDataDirectory = Path.GetDirectoryName(dataSource) ?? string.Empty;
                    if (!string.IsNullOrEmpty(appDataDirectory))
                    {
                        DateTime now = DateTime.UtcNow;
                        string backupDirectory = Path.Combine(appDataDirectory, "Backup");
                        if (!Directory.Exists(backupDirectory))
                            Directory.CreateDirectory(backupDirectory);

                        string backupFile = BackupFile(now, backupDirectory);
                        if (HasBackupForToday(backupFile)) return;

                        string backupConnectionString = string.Format("data source={0}", backupFile);
                        using (var backupConnection = new SqliteConnection(backupConnectionString))
                        using (var sourceConnection = new SqliteConnection(applicationDbContext.Database.GetConnectionString()))
                        {
                            backupConnection.Open();
                            sourceConnection.Open();
                            sourceConnection.BackupDatabase(backupConnection, "main", "main");
                            sourceConnection.Close();
                            backupConnection.Close();
                        }

                        DirectoryInfo info = new DirectoryInfo(backupDirectory);
                        FileInfo[] files = info.GetFiles().OrderByDescending(p => p.CreationTime).Skip(3).ToArray();
                        foreach (FileInfo file in files)
                            file.Delete();
                    }
                }
        }

        private static string BackupFile(DateTime now, string backupDirectory)
        {
            return Path.Combine(backupDirectory, string.Format("{0}.db", (now.ToString("yyyy-MM-dd_hh-mm-ss"))));
        }

        private static bool HasBackupForToday(string backupFile)
        {
            return File.Exists(backupFile);
        }
    }
}
