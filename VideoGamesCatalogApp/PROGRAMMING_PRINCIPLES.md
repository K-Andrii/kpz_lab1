# Аналіз принципів програмування у проєкті VideoGamesCatalogApp
Даний проєкт реалізований як веб-каталог відеоігор на базі ASP.NET Core MVC

### SoC (Separation of Concerns) — Розділення відповідальності
Проєкт чітко слідує архітектурі MVC, де логіка даних, інтерфейс користувача та керування запитами ізольовані в окремих директоріях:
* **Models**: Містять лише опис структур даних та правил валідації. [Переглянути папку Models](https://github.com/K-Andrii/kpz_lab1/tree/main/VideoGamesCatalogApp/Models)
* **Views**: Відповідають виключно за відображення інтерфейсу. [Переглянути папку Views](https://github.com/K-Andrii/kpz_lab1/tree/main/VideoGamesCatalogApp/Views)
* **Controllers**: Обробляють вхідні HTTP-запити та взаємодіють з контекстом бази даних. [Переглянути папку Controllers](https://github.com/K-Andrii/kpz_lab1/tree/main/VideoGamesCatalogApp/Controllers)

### Dependency Injection (Впровадження залежностей)
Проєкт використовує вбудований DI-контейнер ASP.NET Core для передачі контексту бази даних у контролери. Це робить компоненти незалежними від конкретних реалізацій та полегшує тестування.
* **Доказ у коді**: Контекст `VideoGamesCatalogContext` ін'єктується через конструктор контролера. [Приклад у GamesController.cs (рядок 17)](https://github.com/K-Andrii/kpz_lab1/blob/main/VideoGamesCatalogApp/Controllers/GamesController.cs#L17)

### DRY (Don't Repeat Yourself)
Уникаю потоврення коду шляхом використання спільних макетів та часткових представлень:
* **Layouts**: Спільна структура сайту (меню, футер, підключення стилів) винесена в один файл. [Див. _Layout.cshtml](https://github.com/K-Andrii/kpz_lab1/blob/main/VideoGamesCatalogApp/Views/Shared/_Layout.cshtml)
* **Validation Partials**: Скрипти валідації на стороні клієнта винесені в окремий файл для повторного використання на всіх сторінках з формами. [Див. _ValidationScriptsPartial.cshtml](https://github.com/K-Andrii/kpz_lab1/blob/main/VideoGamesCatalogApp/Views/Shared/_ValidationScriptsPartial.cshtml)

### Data Annotations (Декларативна валідація)
Замість ручних перевірок `if-else`, ми використовуємо атрибути валідації безпосередньо в моделях. Це дозволяє автоматично перевіряти дані через `ModelState`.
* **Доказ у коді**: Атрибути `[Required]`, `[StringLength]` та інші в моделі користувача. [Див. Models/User.cs](https://github.com/K-Andrii/kpz_lab1/blob/main/VideoGamesCatalogApp/Models/User.cs)

### Strongly-typed Views (Строга типізація)
Представлення використовують директиву `@model`, що забезпечує контроль типів під час розробки та підтримку IntelliSense.
* **Приклад**: Сторінка відображення деталей гри працює безпосередньо з об'єктом моделі `Game`. [Див. Views/Games/Details.cshtml](https://github.com/K-Andrii/kpz_lab1/blob/main/VideoGamesCatalogApp/Views/Games/Details.cshtml)

### KISS (Keep It Simple, Stupid)
Логіка в CRUD-контролерах залишена максимально зрозумілою. Пряму взаємодія з Entity Framework Core без створення зайвих рівнів абстракції там, де це не вимагається складністю задачі.
* **Приклад**: Простий метод отримання списку в [GenresController.cs](https://github.com/K-Andrii/kpz_lab1/blob/main/VideoGamesCatalogApp/Controllers/GenresController.cs)
