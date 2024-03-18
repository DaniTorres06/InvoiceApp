using InvoiceBusiness;
using InvoiceBusiness.Interfaces;
using InvoiceData;
using InvoiceData.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IInvoiceBusiness, InvoicesBusiness>();
builder.Services.AddTransient<IInvoiceData, InvoicesData>();

builder.Services.AddTransient<IItemBusiness, ItemBusiness>();
builder.Services.AddTransient<IItemData, ItemData>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("NuevaPolitica");

app.MapControllers();

app.Run();
