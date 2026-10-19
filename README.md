# Домашняя библиотека

Веб-приложение для учёта домашней библиотеки: список книг с поиском и постраничным выводом, карточка книги, создание, изменение и удаление записей. Оглавление книги редактируется в HTML-редакторе и хранится в XML-поле базы данных. Весь доступ к данным выполняется через хранимые процедуры и функции PostgreSQL.

## Технологии

* ASP.NET Core MVC (.NET 10), Razor-представления
* Dapper + Npgsql для вызова процедур и функций
* PostgreSQL 14 или новее (OUT-параметры процедур появились в версии 14)
* Bootstrap 5 для оформления, CKEditor 5 как HTML-редактор (подключается с CDN)

## Требования

* .NET SDK 10
* PostgreSQL 14+
* Доступ в интернет при работе с редактором оглавления (CKEditor загружается с CDN)
## Быстрый старт

**1. Создайте базу данных и выполните скрипты** из папки `Database` по порядку:

```bash
psql -U postgres -c "CREATE DATABASE familylibrary;"
psql -U postgres -d familylibrary -f Database/Schema.sql
psql -U postgres -d familylibrary -f Database/Routines.sql
psql -U postgres -d familylibrary -f Database/Seed.sql    # необязательно: 15 тестовых книг
```
Скрипты можно выполнить и в pgAdmin (Query Tool): откройте файл и нажмите F5.

**2. Укажите строку подключения.** В `appsettings.json` стоит заглушка `INSERT\_PASSWORD\_HERE`. Замените её на пароль своего пользователя PostgreSQL одним из способов:

* отредактировать `appsettings.json` (строка `ConnectionStrings:DataBaseConnection`);
* или сохранить значение в секретах пользователя, чтобы пароль не попал в репозиторий:

```bash
dotnet user-secrets set "ConnectionStrings:DataBaseConnection" "Host=localhost;Database=familylibrary;Username=postgres;Password=ВАШ\_ПАРОЛЬ"
```

* или задать переменную окружения `ConnectionStrings\_\_DataBaseConnection`.

**3. Запустите приложение**
