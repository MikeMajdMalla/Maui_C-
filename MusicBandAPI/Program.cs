using Microsoft.IdentityModel.Tokens;
using MusicBandAPI.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEntityFrameworkSqlite().AddDbContext<CountryContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy("countryPolicy", builder =>
builder.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()));
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("MyPolicy");

app.MapControllers() .RequireCors("countryPolicy"); ;

app.Run();
