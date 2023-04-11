using EventTriangleAPI.Authorization.Presentation;

class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(hostBuilder => 
            hostBuilder.UseStartup<Startup>());
}