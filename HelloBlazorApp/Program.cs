using HelloBlazorApp.Components;

List<Person> users = [             // ��������� ������
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
        return Results.BadRequest(new { details = "��� ������ ����� �� ������ 3 � �� ������ 20 ��������" });
    if (user.Age < 1 || user.Age > 110)
        return Results.BadRequest(new { details = "������������ �������" });
    user.Id = Guid.NewGuid().ToString();   //OK
    return Results.Json(user);             //send JSON
});
//-------------6.2 --------WEB API ---------
app.MapGet("/api/users", () => users);

app.MapGet("/api/users/{id}", (string id) =>
{
    // �������� ������������ �� id
    Person? user = users.FirstOrDefault(u => u.Id == id);
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound("������������ �� ������");

    // ���� ������������ ������, ���������� ���
    return Results.Json(user);
});

app.MapDelete("/api/users/{id}", (string id) =>
{
    // �������� ������������ �� id
    Person? user = users.FirstOrDefault(u => u.Id == id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound("������������ �� ������");

    // ���� ������������ ������, ������� ���
    users.Remove(user);
    return Results.Json(user);
});

app.MapPost("/api/users", (Person user) => {

    // ������������� id ��� ������ ������������
    user.Id = Guid.NewGuid().ToString();
    // ��������� ������������ � ������
    users.Add(user);
    return user;
});

app.MapPut("/api/users", (Person userData) => {

    // �������� ������������ �� id
    var user = users.FirstOrDefault(u => u.Id == userData.Id);
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound("������������ �� ������");
    // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������

    user.Age = userData.Age;
    user.Name = userData.Name;
    return Results.Json(user);
});
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();