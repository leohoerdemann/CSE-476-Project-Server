using FirebaseAdmin;

public class Program
{
    public static void Main(string[] args)
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile("path/to/serviceAccountKey.json")
        });

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.MapGet("/", () => "Hello World!");


        app.Run();
    }
}