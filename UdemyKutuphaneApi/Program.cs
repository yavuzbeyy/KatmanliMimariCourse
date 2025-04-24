using KutuphaneCore.Entities;
using KutuphaneDataAccess;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.MapProfile;
using KutuphaneServis.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
*/

builder.Services.AddDbContext<DatabaseConnection>();
builder.Services.AddAutoMapper(typeof(MapProfile));

//Servis Implamantasyonlarý
//AddTransient: Her istekte yeni bir instance oluþturur.
//AddScoped: Her istekte bir instance oluþturur ve isteðin sonunda yok eder.
builder.Services.AddScoped<IGenericRepository<Author>,Repository<Author>>();
builder.Services.AddScoped<IGenericRepository<Book>, Repository<Book>>();
builder.Services.AddScoped<IGenericRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
