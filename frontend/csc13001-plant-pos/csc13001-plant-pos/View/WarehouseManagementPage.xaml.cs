using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using WinRT.Interop;

namespace csc13001_plant_pos.View
{
    public sealed partial class WarehouseManagementPage : Page
    {
        public WarehouseManagementViewModel ViewModel { get; }

        public WarehouseManagementPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<WarehouseManagementViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadInventoryOrdersAsync();
        }

        private void AddStockReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddStockReceiptPage));
        }

        private async void ExportPDFButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int inventoryId)
            {
                // Tìm phiếu nhập tương ứng
                var inventory = ViewModel.FilteredInventoryOrders.FirstOrDefault(i => i.InventoryId == inventoryId);
                if (inventory == null)
                {
                    await ShowErrorDialog("Không tìm thấy phiếu nhập.");
                    return;
                }

                // Tạo PDF
                using (PdfDocument document = new PdfDocument())
                {
                    document.PageSettings.Orientation = PdfPageOrientation.Portrait;
                    document.PageSettings.Margins.All = 30;
                    PdfPage page = document.Pages.Add();
                    PdfGraphics graphics = page.Graphics;

                    // Đường dẫn font
                    string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Fonts", "Arial.ttf");
                    using (FileStream fontStream = new FileStream(fontPath, FileMode.Open, FileAccess.Read))
                    {
                        // Định nghĩa các font
                        PdfTrueTypeFont logoFont = new PdfTrueTypeFont(fontStream, 28, PdfFontStyle.Bold);
                        PdfTrueTypeFont titleFont = new PdfTrueTypeFont(fontStream, 24, PdfFontStyle.Bold);
                        PdfTrueTypeFont headingFont = new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold);
                        PdfTrueTypeFont normalFont = new PdfTrueTypeFont(fontStream, 12);
                        PdfTrueTypeFont boldFont = new PdfTrueTypeFont(fontStream, 12, PdfFontStyle.Bold);
                        PdfTrueTypeFont labelFont = new PdfTrueTypeFont(fontStream, 10);
                        PdfTrueTypeFont watermarkFont = new PdfTrueTypeFont(fontStream, 40, PdfFontStyle.Bold);

                        // Định nghĩa các màu
                        PdfColor brandColor = new PdfColor(0, 122, 51); // Màu xanh lá đậm
                        PdfColor lightGreen = new PdfColor(240, 248, 240); // Màu xanh lá nhạt
                        PdfColor grayColor = new PdfColor(150, 150, 150);
                        PdfColor borderColor = new PdfColor(200, 200, 200);

                        // Lấy kích thước trang
                        float pageWidth = page.GetClientSize().Width;
                        float pageHeight = page.GetClientSize().Height;
                        float currentY = 30;

                        // Watermark
                        PdfGraphicsState state = graphics.Save();
                        graphics.SetTransparency(0.08f);
                        graphics.RotateTransform(-40);
                        graphics.DrawString("Plant POS", watermarkFont, new PdfSolidBrush(grayColor), new Syncfusion.Drawing.PointF(-150, 450));
                        graphics.Restore(state);

                        // ----- HEADER -----
                        graphics.DrawString("Plant POS", logoFont, new PdfSolidBrush(brandColor),
                                            new Syncfusion.Drawing.PointF(pageWidth / 2 - 70, currentY));
                        currentY += 35;

                        graphics.DrawString("PHIẾU NHẬP KHO", titleFont, PdfBrushes.Black,
                                            new Syncfusion.Drawing.PointF(pageWidth / 2 - 100, currentY));
                        currentY += 40;

                        // ----- THÔNG TIN PHIẾU NHẬP -----
                        float infoBoxWidth = pageWidth - 60;
                        float infoBoxHeight = 100;
                        graphics.DrawRectangle(new PdfPen(borderColor, 1),
                                              new Syncfusion.Drawing.RectangleF(30, currentY, infoBoxWidth, infoBoxHeight));

                        float infoLeftX = 45;
                        float infoStartY = currentY + 15;

                        // Mã phiếu nhập
                        graphics.DrawString("Mã phiếu nhập:", labelFont, PdfBrushes.Gray,
                                            new Syncfusion.Drawing.PointF(infoLeftX, infoStartY));
                        graphics.DrawString(inventory.InventoryId.ToString(), boldFont, PdfBrushes.Black,
                                            new Syncfusion.Drawing.PointF(infoLeftX, infoStartY + 20));

                        // Ngày nhập
                        graphics.DrawString("Ngày nhập:", labelFont, PdfBrushes.Gray,
                                            new Syncfusion.Drawing.PointF(infoLeftX, infoStartY + 45));
                        graphics.DrawString(inventory.PurchaseDate.ToString("dd/MM/yyyy HH:mm:ss"), boldFont, PdfBrushes.Black,
                                            new Syncfusion.Drawing.PointF(infoLeftX, infoStartY + 65));

                        // Nhà cung cấp
                        float infoRightX = pageWidth / 2 + 30;
                        graphics.DrawString("Nhà cung cấp:", labelFont, PdfBrushes.Gray,
                                            new Syncfusion.Drawing.PointF(infoRightX, infoStartY));
                        graphics.DrawString(inventory.Supplier ?? "N/A", boldFont, PdfBrushes.Black,
                                            new Syncfusion.Drawing.PointF(infoRightX, infoStartY + 20));

                        currentY += infoBoxHeight + 20;

                        // ----- SEPARATOR -----
                        graphics.DrawLine(new PdfPen(brandColor, 2), new Syncfusion.Drawing.PointF(30, currentY),
                                          new Syncfusion.Drawing.PointF(pageWidth - 30, currentY));
                        currentY += 20;

                        // ----- DANH SÁCH SẢN PHẨM -----
                        graphics.DrawString("CHI TIẾT SẢN PHẨM", headingFont, new PdfSolidBrush(brandColor),
                                            new Syncfusion.Drawing.PointF(30, currentY));
                        currentY += 25;

                        PdfGrid pdfGrid = new PdfGrid();
                        pdfGrid.Columns.Add(4);
                        pdfGrid.Headers.Add(1);

                        PdfGridRow header = pdfGrid.Headers[0];
                        header.Cells[0].Value = "Sản phẩm";
                        header.Cells[1].Value = "SL";
                        header.Cells[2].Value = "Giá nhập";
                        header.Cells[3].Value = "Thành tiền";

                        PdfGridCellStyle headerStyle = new PdfGridCellStyle
                        {
                            BackgroundBrush = new PdfSolidBrush(brandColor),
                            TextBrush = PdfBrushes.White,
                            Font = headingFont
                        };
                        header.ApplyStyle(headerStyle);

                        header.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        header.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        header.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        header.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);

                        pdfGrid.Columns[0].Width = 220;
                        pdfGrid.Columns[1].Width = 50;
                        pdfGrid.Columns[2].Width = 100;
                        pdfGrid.Columns[3].Width = 100;

                        foreach (var item in inventory.InventoryItems)
                        {
                            PdfGridRow row = pdfGrid.Rows.Add();
                            row.Cells[0].Value = item.Product.Name;
                            row.Cells[1].Value = item.Quantity.ToString();
                            row.Cells[2].Value = $"{item.PurchasePrice:N0} ₫";
                            row.Cells[3].Value = $"{item.TotalItemPrice:N0} ₫";
                        }

                        PdfGridCellStyle cellStyle = new PdfGridCellStyle
                        {
                            Font = normalFont,
                            Borders = { Bottom = new PdfPen(borderColor, 0.7f) }
                        };

                        PdfGridCellStyle boldCellStyle = new PdfGridCellStyle
                        {
                            Font = boldFont,
                            Borders = { Bottom = new PdfPen(borderColor, 0.7f) }
                        };

                        foreach (PdfGridRow row in pdfGrid.Rows)
                        {
                            row.ApplyStyle(cellStyle);
                            row.Cells[3].Style.Font = boldFont;

                            row.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                            row.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                            row.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                            row.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        }

                        PdfGridLayoutResult gridResult = pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(30, currentY));
                        currentY = gridResult.Bounds.Bottom + 30;

                        // ----- TỔNG TIỀN -----
                        float summaryBoxWidth = 250;
                        float summaryBoxHeight = 50;
                        float summaryBoxX = pageWidth - summaryBoxWidth - 30;

                        graphics.DrawRectangle(new PdfSolidBrush(lightGreen),
                                              new Syncfusion.Drawing.RectangleF(summaryBoxX, currentY, summaryBoxWidth, summaryBoxHeight));
                        graphics.DrawRectangle(new PdfPen(borderColor, 1),
                                              new Syncfusion.Drawing.RectangleF(summaryBoxX, currentY, summaryBoxWidth, summaryBoxHeight));

                        float summaryTextX = summaryBoxX + 15;
                        float summaryValueX = summaryBoxX + summaryBoxWidth - 15;
                        float summaryY = currentY + 15;

                        graphics.DrawString("Tổng thanh toán:", boldFont, PdfBrushes.Black,
                                            new Syncfusion.Drawing.PointF(summaryTextX, summaryY));
                        PdfStringFormat rightAlign = new PdfStringFormat { Alignment = PdfTextAlignment.Right };
                        graphics.DrawString($"{inventory.TotalPrice:N0} ₫", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold),
                                            new PdfSolidBrush(brandColor),
                                            new Syncfusion.Drawing.RectangleF(summaryTextX, summaryY, summaryBoxWidth - 30, 20), rightAlign);

                        // ----- FOOTER -----
                        float footerY = pageHeight - 70;
                        graphics.DrawLine(new PdfPen(grayColor, 0.5f), new Syncfusion.Drawing.PointF(30, footerY),
                                          new Syncfusion.Drawing.PointF(pageWidth - 30, footerY));
                        footerY += 15;

                        string thankYouText = "Cảm ơn nhà cung cấp!";
                        SizeF thankYouSize = headingFont.MeasureString(thankYouText);
                        graphics.DrawString(thankYouText, headingFont, PdfBrushes.Black,
                                            new Syncfusion.Drawing.PointF((pageWidth - thankYouSize.Width) / 2, footerY));
                        footerY += 20;

                        string contactText = "Plant POS - Hệ thống quản lý bán hàng chuyên nghiệp";
                        SizeF contactSize = normalFont.MeasureString(contactText);
                        graphics.DrawString(contactText, normalFont, new PdfSolidBrush(grayColor),
                                            new Syncfusion.Drawing.PointF((pageWidth - contactSize.Width) / 2, footerY));

                        // Lưu file PDF
                        FileSavePicker savePicker = new FileSavePicker
                        {
                            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                            SuggestedFileName = $"PhieuNhap_{inventory.InventoryId}",
                        };
                        savePicker.FileTypeChoices.Add("PDF File", new[] { ".pdf" });
                        IntPtr hwnd = WindowNative.GetWindowHandle((Application.Current as App)?.GetMainWindow());
                        InitializeWithWindow.Initialize(savePicker, hwnd);

                        StorageFile file = await savePicker.PickSaveFileAsync();

                        if (file != null)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                document.Save(memoryStream);
                                memoryStream.Position = 0;

                                byte[] pdfBytes = memoryStream.ToArray();
                                IBuffer buffer = WindowsRuntimeBuffer.Create(pdfBytes, 0, pdfBytes.Length, pdfBytes.Length);
                                await FileIO.WriteBufferAsync(file, buffer);
                            }

                            await ShowSuccessDialog("Phiếu nhập đã được xuất thành công!");
                        }
                    }
                }
            }
        }

        private async Task ShowErrorDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Lỗi",
                Content = message,
                CloseButtonText = "OK",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async Task ShowSuccessDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Thành công",
                Content = message,
                CloseButtonText = "OK",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}