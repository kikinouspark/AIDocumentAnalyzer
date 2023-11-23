using Serilog;

namespace DocumentRecognizerTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogger();

            Log.Information("Application Started");

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-mm-dd} {MachineName} {ThreadId} {Message} {Exception:1} {NewLine}")
                .WriteTo.File(@"log.txt", outputTemplate: "{Timestamp:yyyy-mm-dd} {MachineName} {ThreadId} {Message} {Exception:1} {NewLine}")
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .CreateLogger();
        }
    }
}
