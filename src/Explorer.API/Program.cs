using Explorer.API.Startup;
using Explorer.Encounters.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
const string corsPolicy = "_corsPolicy";
builder.Services.ConfigureCors(corsPolicy);
builder.Services.ConfigureAuth();

builder.Services.RegisterModules();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

// Seed podataka za TouristProfiles
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EncountersContext>();
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

    context.SeedData(userRepository);
}

app.Run();

// Required for automated tests
namespace Explorer.API
{
    public partial class Program { }
}