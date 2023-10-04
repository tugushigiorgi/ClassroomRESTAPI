    using GoogleClassroom.Database;
using GoogleClassroom.Identity;
using GoogleClassroom.Repositories;
using GoogleClassroom.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using GoogleClassroom.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => {
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));

    //Authorization policy
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddXmlSerializerFormatters();
builder.Services.AddScoped<RoomRepositoy>();
builder.Services.AddScoped<PostRepositoy>();
builder.Services.AddScoped<AssigmentRepository>();
builder.Services.AddScoped<StudentAssigmentRepository>();
builder.Services.AddTransient<IJwtService,JwtService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddDbContext<ClassroomDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7185", "http://localhost:7185");
                          policy.SetIsOriginAllowed(origin => true);
                          policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                      });
});



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoogleClassroom", Version = "v1" });
});


builder.Services.AddIdentity<user,userrole>(options => {
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;

})
  .AddEntityFrameworkStores<ClassroomDbContext>()
 .AddDefaultTokenProviders()
 .AddUserStore<UserStore<user, userrole, ClassroomDbContext, Guid>>()
 .AddRoleStore<RoleStore<userrole, ClassroomDbContext, Guid>>()
 ;






builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(options => {
     

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddAuthorization();
 






var app = builder.Build();
 
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Google Classroom v1");
});
 
app.Run();
