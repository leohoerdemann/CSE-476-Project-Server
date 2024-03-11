using FirebaseAdmin;
using Google.Cloud.Firestore;
public class Program
{

    public static FirestoreDb db;

    public static void Main(string[] args)
    {
        db = FirestoreDb.Create("cse476-hoerdema");

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

    public static async void InsertRecord(string user, double time)
    {
        CollectionReference collection = db.Collection("main");
        await collection.AddAsync(new { User = user, Time = time, Day = DateTime.Today});
    }


    public static async Task<List<Dictionary<string, object>>> GetRecordsUser(string user)
    {
        QuerySnapshot snapshot = await db.Collection("main").WhereEqualTo("User", user).OrderBy("Time").GetSnapshotAsync();
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            list.Add(document.ToDictionary());
        }
        return list;
    }


    public static async Task<List<Dictionary<string, object>>> GetRecordsUser(string user, DateTime date)
    {
        QuerySnapshot snapshot = await db.Collection("main").WhereEqualTo("User", user).WhereEqualTo("Day", date).OrderBy("Time").GetSnapshotAsync();
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            list.Add(document.ToDictionary());
        }
        return list;
    }


    public static async Task<List<Dictionary<string, object>>> GetRecordsDate(DateTime date)
    {
        QuerySnapshot snapshot = await db.Collection("main").WhereEqualTo("Day", date).OrderBy("Time").GetSnapshotAsync();
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            list.Add(document.ToDictionary());
        }
        return list;
    }


    public static async Task<List<Dictionary<string, object>>> GetRecords()
    {
        QuerySnapshot snapshot = await db.Collection("main").OrderBy("Time").GetSnapshotAsync();
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            list.Add(document.ToDictionary());
        }
        return list;
    }

    public async void updateRecord(string user, double time, DateTime day)
    {
        // update the record for a users time on day
        Query snapshot = db.Collection("main").WhereEqualTo("User", user).WhereEqualTo("Day", day);
        QuerySnapshot query = await snapshot.GetSnapshotAsync();
        DocumentSnapshot document = query.Documents[0];
        Dictionary<string, object> data = document.ToDictionary();
        data["Time"] = time;
        await document.Reference.SetAsync(data);
    }


    public async void clearRecords()
    {
        // clear all records
        QuerySnapshot snapshot = await db.Collection("main").GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            await document.Reference.DeleteAsync();
        }
    }

    public async void clearRecordsUser(string user)
    {
        // clear all records for a user
        QuerySnapshot snapshot = await db.Collection("main").WhereEqualTo("User", user).GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            await document.Reference.DeleteAsync();
        }
    }
}