using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.IO;
using Windows.Storage.Pickers;
using Windows.Storage;
using System;
using WinRT.Interop;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace csc13001_plant_pos.View
{
    public sealed partial class BillPage : Page
    {
        public BillViewModel ViewModel { get; }

        public BillPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<BillViewModel>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string orderId)
            {
                await ViewModel.LoadOrderAsync(orderId);
            }
        }

        private async void ExportPDFButton_Click(object sender, RoutedEventArgs e)
        {
            using (PdfDocument document = new PdfDocument())
            {
                document.PageSettings.Orientation = PdfPageOrientation.Portrait;
                document.PageSettings.Margins.All = 20;
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;
                string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Fonts", "Arial.ttf");
                using (FileStream fontStream = new FileStream(fontPath, FileMode.Open, FileAccess.Read))
                {
                    PdfFont titleFont = new PdfTrueTypeFont(fontStream, 20);
                    PdfFont subHeadingFont = new PdfTrueTypeFont(fontStream, 14);
                    PdfFont bodyFont = new PdfTrueTypeFont(fontStream, 12);
                    PdfFont watermarkFont = new PdfTrueTypeFont(fontStream, 40);
                    // Watermark
                    PdfGraphicsState state = graphics.Save();
                    graphics.SetTransparency(0.25f);
                    graphics.RotateTransform(-45);
                    graphics.DrawString("PlanPos", watermarkFont, new PdfSolidBrush(new PdfColor(150, 150, 150)), new Syncfusion.Drawing.PointF(-150, 400));
                    graphics.Restore(state);
                    // Tiêu đề
                    graphics.DrawString("Hoá đơn", titleFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(240, 20));
                    // Thông tin đơn hàng
                    float y = 60;
                    graphics.DrawString($"Mã hoá đơn: {ViewModel.OrderId}", bodyFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(20, y));
                    y += 20;
                    graphics.DrawString($"Nhân viên: {ViewModel.StaffName}", bodyFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(20, y));
                    y += 20;
                    graphics.DrawString($"Khách hàng: {ViewModel.CustomerName}", bodyFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(20, y));
                    y += 20;
                    graphics.DrawString($"Ngày: {ViewModel.OrderDate:dd/MM/yyyy HH:mm:ss}", bodyFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(20, y));
                    y += 30;
                    // Đường phân cách
                    graphics.DrawRectangle(new PdfSolidBrush(new PdfColor(126, 151, 173)), new Syncfusion.Drawing.RectangleF(0, y, graphics.ClientSize.Width, 20));
                    graphics.DrawString("Chi tiết đơn hàng", subHeadingFont, PdfBrushes.White, new Syncfusion.Drawing.PointF(10, y + 4));
                    y += 20;
                    // Bảng sản phẩm
                    PdfGrid pdfGrid = new PdfGrid();
                    pdfGrid.Columns.Add(3);
                    pdfGrid.Headers.Add(1);
                    // Tiêu đề bảng
                    PdfGridRow header = pdfGrid.Headers[0];
                    header.Cells[0].Value = "Sản phẩm";
                    header.Cells[1].Value = "Số lượng";
                    header.Cells[2].Value = "Giá bán";
                    // Định dạng tiêu đề
                    PdfGridCellStyle headerStyle = new PdfGridCellStyle
                    {
                        BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173)),
                        TextBrush = PdfBrushes.White,
                        Font = subHeadingFont
                    };
                    header.ApplyStyle(headerStyle);
                    // Dữ liệu sản phẩm
                    foreach (var item in ViewModel.OrderItems)
                    {
                        PdfGridRow row = pdfGrid.Rows.Add();
                        row.Cells[0].Value = item.Product.Name;
                        row.Cells[1].Value = item.Quantity.ToString();
                        row.Cells[2].Value = $"{item.SalePrice:N0} đ";
                    }
                    // Định dạng ô
                    PdfGridCellStyle cellStyle = new PdfGridCellStyle
                    {
                        Font = bodyFont,
                        Borders = { Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.7f) }
                    };
                    foreach (PdfGridRow row in pdfGrid.Rows)
                    {
                        row.ApplyStyle(cellStyle);
                        row.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                        row.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    }
                    // Vẽ bảng
                    PdfGridLayoutResult gridResult = pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(20, y));
                    y = gridResult.Bounds.Bottom + 20;
                    // Tổng tiền
                    float pageWidth = page.GetClientSize().Width; // Lấy chiều rộng trang
                    float padding = 20; // Khoảng cách từ mép phải
                    float GetRightAlignedX(string text, PdfFont font)
                    {
                        float textWidth = font.MeasureString(text).Width;
                        return pageWidth - textWidth - padding;
                    }
                    // Vẽ text với căn lề phải
                    string text1 = $"Tổng sản phẩm ({ViewModel.OrderItems.Count}): {ViewModel.TotalPrice:N0} đ";
                    graphics.DrawString(text1, bodyFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(GetRightAlignedX(text1, bodyFont), y));
                    y += 20;
                    string text2 = $"Giảm giá ({ViewModel.DiscountRate}%): {ViewModel.DiscountAmount:N0} đ";
                    graphics.DrawString(text2, bodyFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(GetRightAlignedX(text2, bodyFont), y));
                    y += 20;
                    string text3 = $"Tổng tiền: {ViewModel.FinalPrice:N0} đ";
                    graphics.DrawString(text3, bodyFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(GetRightAlignedX(text3, bodyFont), y));
                    y += 20;
                    string text4 = "Cảm ơn quý khách!";
                    graphics.DrawString(text4, subHeadingFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(GetRightAlignedX(text4, subHeadingFont), y));   
                    // Lưu file PDF
                    FileSavePicker savePicker = new FileSavePicker
                    {
                        SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                        SuggestedFileName = $"HoaDon_{ViewModel.OrderId}",
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

                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "Thành công",
                            Content = "Hoá đơn đã được xuất thành công!",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };
                        await dialog.ShowAsync();
                    }
                } 
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}