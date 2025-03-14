# WriteErace
Десктоп приложение магазина канцелярских принадлежностей «Пиши-стирай».
## Описание
В рамках данного проекта было разработано:
* База данных, которую можно [ИМПОРТИРОВАТЬ](/docs/Import_table.txt) и [ПОСМОТРЕТЬ](/docs/image/image-22.png)
* Десктоп приложение - наглядное отображение пользователю данных туристического агенства.
* Библиотека для расчета графика работы сотрудника [ПОСМОТРЕТЬ](https://github.com/ArrayKat/SF2022UserProject.git)
* 10 Unit-тестов для библиотеки [ПОСМОТРЕТЬ](https://github.com/ArrayKat/SF2022UserProject.git)
* Тестовая докуентация, которая тестирует функционал добавления товаров администратором [ПОСМОТРЕТЬ](/docs/testing-template.docx)
* Итоговый отчет [ПОСМОТРЕТЬ](/docs/ИтоговыйОтчет_УП_КолиниченкоЕС.docx)

## Демо
С предварительным просмотром данного проекта модете ознакомиться на следующей странице:
[Демо проекта](/docs/demo.md)

## Технологии в проекте
Основной стек технологий:
* Среда разработки: Visual Studio Community 2022
* Язык программирования: C#
* Основной фреймврок: Avalonia UI Библиотеки проекта:
* Microsoft.EntityFrameworkCore.Tools, Npgsql.EntityFrameworkCore.PostgreSQL, Microsoft.EntityFrameworkCore.Tools - Позволяют работать с PostgreSQL БД
* ReactivUI.mvvm - Позволяет упростить работу с фреймврок Avalonia Mvvm
* MessageBox.Avalonia - позволяет отображать стандартные уведомления для пользователя
* QuestPDF - nuget пакет, который помогает формировать pdf файлы.


### Процесс установки приложения:
1. Импортируйте базу данных по [ссылке](/docs/Import_table.txt)
2. Откройте Visual Studio Community 2022
3. Через функцию "Клонирование проекта" создать проект. Ссылка: `https://github.com/ArrayKat/WriteErace.git`
4. Необходимо запустить клиентское приложение без отладки. 

### Что планируется добавить:
* Добавление/редактироваание/удаление списка продуктов администратором

## Описание коммитов
| Название | Описание |
|-------------|--------------|
| feat  | добавление новой функциональности     |
| fix    | исправление ошибок |
| docs  | изменения в документации |
| style    | изменения, не влияющие на функциональность, форматирование |
| refactor  | изменение кода без добавления новой функциональности или исправления ошибок |
| test  |добавление тестов или исправление существующих|

## Автор
Я в 
[Gogs](http://gogs.ngknn.ru:3000/ArrayKat2),
[GitHub](https://github.com/ArrayKat)