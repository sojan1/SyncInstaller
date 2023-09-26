using Dotmim.Sync;
using Dotmim.Sync.Enumerations;
using Dotmim.Sync.SqlServer;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Security.Policy;
using System.Configuration;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using Microsoft.Data.SqlClient;
using Dotmim.Sync.Sqlite;
using System.Runtime.CompilerServices;

namespace SyncInstaller
{
    public partial class frmProvision : Form
    {
        List<string> systemTables = new List<string>();
        List<string> divisionTables = new List<string>();
        List<string> projectTables = new List<string>();


        string connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
        string clientAPIURL = ConfigurationSettings.AppSettings["ClientAPIURL"];
        string defaultScope = ConfigurationSettings.AppSettings["DefaultScope"];

        SqlSyncChangeTrackingProvider serverProvider = new SqlSyncChangeTrackingProvider("Data Source=GEROC-LT4\\SQLEXPRESS01; Initial Catalog=CORE4; Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=False;");
        SqliteSyncProvider clientProvider = new SqliteSyncProvider(@"Data Source=C:\\Users\\sojan.GEROC\\Documents\\Projects\\Dotmim.Sync-master\\SyncInstaller1\\CoreDatabase.db");


        public frmProvision()
        {
            InitializeComponent();

        }
 
        private static List<string> AddToList(string path)
        {
            List<string> tables = new List<string>();
            foreach (var tablename in File.ReadLines(path))
            {
                tables.Add(tablename.Trim());
            }
            return tables;
        }


        private async void button4_Click(object sender, EventArgs e)
        {

            var options = new SyncOptions
            {
                BatchDirectory = Path.Combine(SyncOptions.GetDefaultUserBatchDirectory(), "client")
            };

            var agent = new SyncAgent(clientProvider, serverProvider, options);
            agent.SessionStateChanged += Agent_SessionStateChanged;


            var progress = new SynchronousProgress<ProgressArgs>(
                  pa => Console.WriteLine($"{pa.ProgressPercentage:p}\t {pa.Message}"));

            var parameters2 = new SyncParameters
                {
                    { "CLNT_ID","D1" },
                    { "PROJ_ID","MASTER" }
                };
            var s2 = await agent.SynchronizeAsync("v0", SyncType.Normal, parameters2, progress);
            Console.WriteLine(s2);
        }


        private async void btnDemo2a_Click(object sender, EventArgs e)
        {
            var options = new SyncOptions
            {
                BatchDirectory = Path.Combine(SyncOptions.GetDefaultUserBatchDirectory(), "client")
            };

            var agent = new SyncAgent(clientProvider, serverProvider, options);
            agent.SessionStateChanged += Agent_SessionStateChanged;


            var progress = new SynchronousProgress<ProgressArgs>(
                  pa => Console.WriteLine($"{pa.ProgressPercentage:p}\t {pa.Message}"));

            var parameters2 = new SyncParameters
                {
                    { "CLNT_ID","D1" },
                    { "PROJ_ID","Demo 2a" }
                };
            var s2 = await agent.SynchronizeAsync("v0", SyncType.Normal, parameters2, progress);
            Console.WriteLine(s2);
        }



        private void Progress_ProgressChanged(object? sender, ProgressArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Agent_SessionStateChanged(object? sender, SyncSessionState e)
        {
            //throw new NotImplementedException();
        }


    }
}



