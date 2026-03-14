using Common.CommandHandler;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvoiceService.Domain.CreateInvoicePDF
{
    public record CreateInvoicePDFCommand(Guid OrderId);

    public class CreateInvoicePDFHandler : ICommandHandler<CreateInvoicePDFCommand>
    {
        public Task HandleAsync(CreateInvoicePDFCommand command)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            
            byte[] body = Document.Create(container =>
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
                            
                            column.Item().Text($"Order Number: {command.OrderId}")
                                .FontSize(16)
                                .AlignCenter();
                            
                            column.Item().PaddingTop(20)
                                .Text("Thank you for your order!")
                                .FontSize(24)
                                .AlignCenter();
                        });
                });
            }).GeneratePdf();
            
            return Task.CompletedTask;
        }
    }
}
