using System.Text;
using FluentValidation;
using KosKu.Api.Middleware;
using KosKu.Application.Extensions;
using KosKu.Application.Features.Auth.DTOs;
using KosKu.Application.Features.Auth.Validators;
using KosKu.Application.Features.Bookings.DTOs;
using KosKu.Application.Features.Bookings.Validators;
using KosKu.Application.Features.Fines.DTOs;
using KosKu.Application.Features.Fines.Validators;
using KosKu.Application.Features.Maintenance.DTOs;
using KosKu.Application.Features.Maintenance.Validators;
using KosKu.Application.Features.Payments.DTOs;
using KosKu.Application.Features.Payments.Validators;
using KosKu.Application.Features.Rentals.DTOs;
using KosKu.Application.Features.Rentals.Validators;
using KosKu.Application.Features.Rooms.DTOs;
using KosKu.Application.Features.Rooms.Validators;
using KosKu.Application.Features.RoomTypes.DTOs;
using KosKu.Application.Features.RoomTypes.Validators;
using KosKu.Application.Features.Users.DTOs;
using KosKu.Application.Features.Users.Validators;
using KosKu.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

// ─── Bootstrap Serilog ──────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Memulai KosKu.Api...");

    var builder = WebApplication.CreateBuilder(args);

    // ─── Serilog ────────────────────────────────────────────────────────────
    builder.Host.UseSerilog((context, services, config) =>
        config.ReadFrom.Configuration(context.Configuration)
              .ReadFrom.Services(services)
              .Enrich.FromLogContext());

    // ─── Infrastructure & Application Services ──────────────────────────────
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplicationServices();

    // ─── FluentValidation ───────────────────────────────────────────────────
    builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
    builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
    builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
    builder.Services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordRequestValidator>();
    builder.Services.AddScoped<IValidator<CreateRoomTypeRequest>, CreateRoomTypeRequestValidator>();
    builder.Services.AddScoped<IValidator<UpdateRoomTypeRequest>, UpdateRoomTypeRequestValidator>();
    builder.Services.AddScoped<IValidator<CreateRoomRequest>, CreateRoomRequestValidator>();
    builder.Services.AddScoped<IValidator<UpdateRoomRequest>, UpdateRoomRequestValidator>();
    builder.Services.AddScoped<IValidator<CreateBookingRequest>, CreateBookingRequestValidator>();
    builder.Services.AddScoped<IValidator<ExtendRentalRequest>, ExtendRentalRequestValidator>();
    builder.Services.AddScoped<IValidator<CreatePaymentRequest>, CreatePaymentRequestValidator>();
    builder.Services.AddScoped<IValidator<CreateFineRequest>, CreateFineRequestValidator>();
    builder.Services.AddScoped<IValidator<CreateMaintenanceRequest>, CreateMaintenanceRequestValidator>();

    // ─── JWT Authentication ──────────────────────────────────────────────────
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"]
        ?? throw new InvalidOperationException("JWT SecretKey tidak dikonfigurasi.");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization();

    // ─── Controllers ────────────────────────────────────────────────────────
    // builder.Services.AddControllers();
    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Mengubah SEMUA Enum di C# agar otomatis dibaca dan dikirim sebagai STRING
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });

    // ─── Swagger / OpenAPI ───────────────────────────────────────────────────
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "KosKu API",
            Version = "v1",
            Description = "Backend API untuk aplikasi manajemen kos KosKu",
            Contact = new OpenApiContact { Name = "KosKu Team" }
        });

        // JWT Auth di Swagger
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Masukkan JWT token"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
    });

    // ─── Health Check ────────────────────────────────────────────────────────
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<KosKu.Infrastructure.Persistence.AppDbContext>(
            name: "mysql",
            tags: ["db", "sql", "mysql"]);

    // ─── CORS ────────────────────────────────────────────────────────────────
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
    });

    // ─── Build App ───────────────────────────────────────────────────────────
    var app = builder.Build();

    // ─── Middleware Pipeline ─────────────────────────────────────────────────
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "KosKu API v1");
            c.RoutePrefix = "swagger"; // Swagger di root
        });
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

    Log.Information("KosKu.Api berhasil dijalankan. Swagger tersedia di http://localhost:5160/swagger/index.html");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "KosKu.Api gagal dijalankan.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;
