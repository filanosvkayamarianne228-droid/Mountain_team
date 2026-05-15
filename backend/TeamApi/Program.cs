internal class Program
{
    private static void Main(string[] args)
    {
        // Создаём строитель приложения
        var builder = WebApplication.CreateBuilder(args);

        // Настраиваем CORS (Cross-Origin Resource Sharing) — разрешаем фронтенду обращаться к API
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()  // Разрешаем любые источники
                     .AllowAnyMethod()   // Разрешаем любые HTTP-методы
                     .AllowAnyHeader());  // Разрешаем любые заголовки
        });

        // Строим приложение
        var app = builder.Build();

        // Применяем настройки CORS
        app.UseCors();

        // Данные о команде (массив анонимных объектов)
        var team = new[]
        {
            new { name = "Филановская Марианна", role = "Tech Lead", fact = "Люблю C#" },
            new { name = "Филановская Марианна", role = "Developer", fact = "Пишу на C# с 1 курса" },
            new { name = "Филановская Марианна", role = "QA", fact = "Нахожу баги быстрее всех" }
        };

        // Определяем эндпоинт для получения списка всех участников команды
        app.MapGet("/api/team", () => Results.Ok(team));

        // Определяем эндпоинт для поиска участника по имени
        app.MapGet("/api/team/{name}", (string name) =>
        {
            // Ищем участника, имя которого содержит переданное значение (игнорируем регистр)
            var member = team.FirstOrDefault(m =>
                m.name.Contains(name, StringComparison.OrdinalIgnoreCase));

            // Если участник найден — возвращаем его данные, иначе — ошибку 404
            return member is not null
                ? Results.Ok(member)
                : Results.NotFound(new { error = "Участник не найден" });
        });

        // Запускаем приложение
        app.Run();
    }
}
