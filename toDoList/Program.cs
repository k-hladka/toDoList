using toDoList.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString = "Data Source=ABC.PQR.GCXYZ.IT.SQL;Initial Catalog=DBName;User ID=User;Password=***;TrustServerCertificate=true; Data Source=Kateryna;Initial Catalog=ToDoList;Integrated Security=True;";
builder.Services.AddScoped<INoteRepository, NoteRepository>(provider => new NoteRepository(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Note}/{action=Index}/{id?}");

app.Run();
