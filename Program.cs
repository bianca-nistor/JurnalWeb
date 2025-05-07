var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession(); // <-- sesiune ad?ugat? aici

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // <-- ?i aici activ?m sesiunea

app.MapRazorPages();
app.Run();
