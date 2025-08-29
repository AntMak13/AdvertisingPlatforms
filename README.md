# AdvertisingPlatforms

Простой веб-сервис на C#, который хранит рекламные площадки в оперативной памяти и возвращает их для заданной локации.

## Требования

- .NET 8 SDK
- Visual Studio / Rider / или терминал с поддержкой dotnet CLI
- (Опционально) Postman для тестирования API

## Запуск проекта

1. Клонируйте репозиторий: git clone https://github.com/ВашПользователь/AdvertisingPlatforms.git
    Затем выполните: cd AdvertisingPlatforms

2. В терминале из папки проекта запустите сервер: dotnet run --project AdvertisingPlatforms
    По умолчанию сервер доступен на: http://localhost:5000 (https://localhost:5001)

3. Откройте Swagger для тестирования API: http://localhost:5000/swagger

## Использование API

### POST /api/advertising/upload
- Загружает файл с рекламными площадками.
- Пример curl: curl -F "file=@places.txt" http://localhost:5000/api/advertising/upload

### GET /api/advertising/search?location={location}
- Получает список рекламодателей для заданной локации.
- Пример curl: curl "http://localhost:5000/api/advertising/search?location=/ru/svrd/revda

## Юнит-тесты

1. Перейдите в проект тестов: cd AdvertisingPlatforms.Tests

2. Запустите тесты: dotnet test