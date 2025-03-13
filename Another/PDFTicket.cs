using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteErace.Models;

namespace WriteErace.Another
{
    internal class PDFTicket
    {
        Order _currentOrder;

        public PDFTicket(Order order) {
            _currentOrder = order;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public void CreatePDF()
        {
            Document
                .Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(14));

                        page.Header()
                                .Text($"Заказ № {_currentOrder.Id}")
                                .FontSize(24).Bold().AlignCenter();

                        // Основное содержимое
                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(column =>
                            {
                                column.Spacing(10);
                                // Заказчик
                                column.Item().Text($"Заказчик: {_currentOrder.IdClientNavigation?.FIO ?? "Гость"}");

                                // Дата заказа
                                column.Item().Text($"Дата заказа: {_currentOrder.DateOrder?.ToShortDateString()}");
                                // Пункт выдачи
                                column.Item().Text($"Пункт выдачи: {_currentOrder.IdAddressNavigation.Name}");

                                // Состав заказа
                                column.Item().Text("Состав заказа:").Bold();
                                foreach (var product in _currentOrder.OrderProducts)
                                {
                                    column.Item().Text($" - {product.ArticleProductsNavigation.NameProducts} {product.CountProducts} {product.ArticleProductsNavigation.Unit}");
                                }

                                column.Item().Text("Код получения:").FontSize(20).Bold().AlignCenter();
                                column.Item().Text($"{_currentOrder.Code}").FontSize(20).Bold().AlignCenter();

                                // Сумма заказа и скидка
                                column.Item().Text($"Сумма заказа: {_currentOrder.TotalSumm} руб.");
                                column.Item().Text($"Сумма скидки: {_currentOrder.TotalSummDiscount} руб.");
                                column.Item().Text($"Итоговая сумма заказа: {_currentOrder.TotalSumm - _currentOrder.TotalSummDiscount} руб.").Bold();

                                // Срок доставки
                                column.Item().Text($"Срок доставки: {_currentOrder.DeliveryDuration} дня");
                            });

                        // Подвал
                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Страница ");
                                x.CurrentPageNumber();
                            });
                    });
                })
                .GeneratePdf($"pdf_files/{_currentOrder.Id}.pdf");
        }
    }
}
