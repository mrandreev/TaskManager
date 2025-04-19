using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TaskManager.Api.ExceptionHandlers;
using TaskManager.Application;
using TaskManager.Application.DomainEvents;
using TaskManager.Application.Messaging.Interfaces;
using TaskManager.Application.Messaging.Messages;
using TaskManager.Application.Repositories;
using TaskManager.Infrastructure.Messaging;
using TaskManager.Persistence.DbContexts;
using TaskManager.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("Default"));
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(dataSource);

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITaskItemReadRepository, TaskItemReadRepository>();
builder.Services.AddTransient<ITaskItemWriteRepository, TaskItemWriteRepository>();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

builder.Services.AddScoped<IMessageBus, RabbitMqMessageBus>();
builder.Services.AddScoped<IMessageHandler<TaskItemCompletedMessage>, TaskItemCompletedMessageHandler>();
builder.Services.AddHostedService<RabbitMqMessageListener<TaskItemCompletedMessage>>();

builder.Services.AddValidatorsFromAssemblyContaining<TaskManagerApplicationModule>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(TaskManagerApplicationModule).Assembly);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
