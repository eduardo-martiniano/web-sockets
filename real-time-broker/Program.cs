using RealTimeBroker.HostedServices;
using RealTimeBroker.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddHostedService<UpdateStockPriceHostedService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicyForDashboard", builder =>
        builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicyForDashboard");
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<BrokerHub>("/brokerhub");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
