using Common.CommandHandler;
using Common.IntegrationEvents;
using InvoiceService.Infrastructure.Storage;
using MassTransit;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvoiceService.Domain.CreateInvoicePDF
{
    public record CreateInvoicePDFCommand(Guid OrderId);

    public class CreateInvoicePDFHandler(IPdfStorageService pdfStorageService, IPublishEndpoint publishEndpoint) : ICommandHandler<CreateInvoicePDFCommand>
    {
        public async Task HandleAsync(CreateInvoicePDFCommand command)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var fileName = $"invoice-{command.OrderId}.pdf";

            byte[] body = GenerateInvoicePdf(command.OrderId);
            var url = await pdfStorageService.UploadPdfAsync(body, fileName);
            await publishEndpoint.Publish(new InvoiceCreatedEvent(command.OrderId));
        }

        private byte[] GenerateInvoicePdf(Guid orderId)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(20);

                            column.Item().Text("Invoice")
                                .FontSize(32)
                                .Bold()
                                .AlignCenter();

                            column.Item().Text($"Order Number: {orderId}")
                                .FontSize(16)
                                .AlignCenter();

                            column.Item().PaddingTop(20)
                                .Text("Thank you for your order!")
                                .FontSize(24)
                                .AlignCenter();
                        });
                });
            }).GeneratePdf();
        }
    }
}

