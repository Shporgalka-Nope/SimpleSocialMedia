# Connection Point - Платформа для микроблогинга
![ASP.NET Core](https://img.shields.io/badge/-ASP.NET_Core-512BD4?logo=dotnet) 
[![Main branch tests](https://github.com/Shporgalka-Nope/SimpleSocialMedia/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/Shporgalka-Nope/SimpleSocialMedia/actions/workflows/dotnet-test.yml)
[![Coverage Status](https://coveralls.io/repos/github/Shporgalka-Nope/SimpleSocialMedia/badge.svg)](https://coveralls.io/github/Shporgalka-Nope/SimpleSocialMedia)

[![GitHub Release](https://img.shields.io/github/v/release/Shporgalka-Nope/SimpleSocialMedia?sort=date&display_name=release&style=flat&label=Download%20latest%20release)](https://github.com/Shporgalka-Nope/SimpleSocialMedia/releases/download/Release/Release.build.zip)

Backend - ASP.Net Core (MVC), Identity Core, EF Core

Frontend - Razor представления, HTML, CSS, JavaScript

## Функционал
На данный момент реализован следующий функционал:
  - Регистрация и вход
  - Кастомизация профиля (Описание, картинка профиля)
  - Создание и удаление постов
  - Настройки (Видимость профиля, изменение картинки профиля и описания)
  - Поиск других пользователей с учётом их настроек видимости

## Дорожная карта
Текущая задача: Лайки под постами

Long-term:
  - Функционал: Комментарии под постами
  - Функционал: Панель уведомлений

## Как развернуть?
  1. Скачать последнюю стабильную версию проекта
  2. Распаковать архив
  3. Из папки проекта прописать в консоль следующую команду: ```dotnet ProfileProject.dll``` и перейти по адресу указанному в строке ```info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5249``` (В данном случае ```http://localhost:5249```)

## Известные проблемы
  1. Ошибка подключения к базе данных при запуске сервера (Issue #2) - При попытке запустить сервер, будет выдано исключение "error - 50", разбор проблемы уже был освещён в Issue #2, краткое решение:
     Прописать команды `sqllocaldb stop mssqllocaldb`, `sqllocaldb delete mssqllocaldb` и `sqllocaldb create mssqllocaldb` в консоль и попробовать снова, сервер должен будет запуститься.
