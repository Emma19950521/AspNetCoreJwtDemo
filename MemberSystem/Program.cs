using MemberSystem.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 加入 Controller
builder.Services.AddControllers();

// 加入 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 JWT 設定
// 讀取 key
var keyString = builder.Configuration["JwtSettings:Key"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = key,
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true
		};
	});



builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "MemberSystem API", Version = "v1" });

	//  新增 Bearer 認證
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "請輸入 'Bearer {token}'"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
			},
			new string[] {}
		}
	});
});




var app = builder.Build();

// 啟用 Swagger
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//註冊Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

// 🔹 啟用認證中介軟體
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();