using Law.DAL.Data;
using Law.DAL.Repository.IRepository; 
using Law.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Law.BL.Services.IServices;
using Law.BL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LawAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConStr")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IIssueService, IssueService>();  
builder.Services.AddScoped<IIssueFileService, IssueFileService>();  
builder.Services.AddScoped<ICourtService, CourtService>();
builder.Services.AddScoped<ICenterService, CenterService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IPartyService, PartyService>();
builder.Services.AddScoped<ICourtSessionService, CourtSessionService>();    
builder.Services.AddScoped<IReceiptService, ReceiptService>();    
builder.Services.AddScoped<IPaymentService, PaymentService>();    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
