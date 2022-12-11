using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using MySql.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using SKU_MetaverseApi.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<MemberContext>(options =>
//    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEntityFrameworkMySQL().AddDbContext<MemberContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEntityFrameworkMySQL().AddDbContext<AttendanceContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
