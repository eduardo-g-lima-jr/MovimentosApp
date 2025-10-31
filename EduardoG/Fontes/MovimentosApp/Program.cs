using MovimentosApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// registra as dependências do repositório
builder.Services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();
builder.Services.AddScoped<DapperMovimentoRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movimentos}/{action=Index}/{id?}");

app.Run();
