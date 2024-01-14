using HelloBlazorApp.Components;

List<Person> users = [             // начальные данные
    new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
];

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();

Console.WriteLine("-------------------------");
Console.WriteLine("-------------------------");

foreach (var service in builder.Services)
{
 //   Console.WriteLine(service.ServiceType);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
//-------------6.1 --------GET, POST ---------
app.MapGet("/time", () => DateTime.Now.ToShortTimeString());
app.MapPost("/user", (Person user) => {
    if (user.Name.Length < 3 || user.Name.Length > 20)
        return Results.BadRequest(new { details = "»м€ должно иметь не меньше 3 и не больше 20 символов" });
    if (user.Age < 1 || user.Age > 110)
        return Results.BadRequest(new { details = "Ќекорректный возраст" });
    user.Id = Guid.NewGuid().ToString();   //OK
    return Results.Json(user);             //send JSON
});
//-------------6.2 --------WEB API ---------
app.MapGet("/api/users", () => users);

app.MapGet("/api/users/{id}", (string id) =>
{
    // получаем пользовател€ по id
    Person? user = users.FirstOrDefault(u => u.Id == id);
    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound("ѕользователь не найден");

    // если пользователь найден, отправл€ем его
    return Results.Json(user);
});

app.MapDelete("/api/users/{id}", (string id) =>
{
    // получаем пользовател€ по id
    Person? user = users.FirstOrDefault(u => u.Id == id);

    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound("ѕользователь не найден");

    // если пользователь найден, удал€ем его
    users.Remove(user);
    return Results.Json(user);
});

app.MapPost("/api/users", (Person user) => {

    // устанавливаем id дл€ нового пользовател€
    user.Id = Guid.NewGuid().ToString();
    // добавл€ем пользовател€ в список
    users.Add(user);
    return user;
});

app.MapPut("/api/users", (Person userData) => {

    // получаем пользовател€ по id
    var user = users.FirstOrDefault(u => u.Id == userData.Id);
    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound("ѕользователь не найден");
    // если пользователь найден, измен€ем его данные и отправл€ем обратно клиенту

    user.Age = userData.Age;
    user.Name = userData.Name;
    return Results.Json(user);
});
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();