using HouseRentSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

//Adding DbContext and Identity methods
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity(builder.Configuration);



builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
    //To protect from CSRF attacks
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

//Adding services
builder.Services.AddApplicationServices();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//new way to protect URL
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name:"House Details",
        pattern: "/House/Details/{id}/{information}",
        defaults: new {Controller = "House", Action="Details"}
        );
    app.MapDefaultControllerRoute();
    app.MapRazorPages();

});


await app.RunAsync();
