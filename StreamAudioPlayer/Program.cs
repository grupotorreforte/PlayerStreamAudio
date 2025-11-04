using Serilog;
using StreamAudioPlayer.Services;

namespace StreamAudioPlayer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs\\radio-monitor-.log", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();

            Observability.Iniciar();
            Log.Information("Aplicação iniciada.");

            Application.Run(new FrmPrincipal());
        }
    }
}