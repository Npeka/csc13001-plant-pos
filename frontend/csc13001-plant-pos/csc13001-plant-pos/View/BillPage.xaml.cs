using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Printing;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using WinRT.Interop;

namespace csc13001_plant_pos.View
{
    public sealed partial class BillPage : Page
    {
        public BillViewModel ViewModel { get; }

        private PrintDocument printDocument = null;
        private IPrintDocumentSource printDocumentSource = null;
        private List<UIElement> printPreviewPages = new List<UIElement>();
        private DispatcherQueue dispatcherQueue;
        private PrintManager printManager; // Khai báo biến instance

        public BillPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<BillViewModel>();
            dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            Loaded += BillPage_Loaded;
            Unloaded += BillPage_Unloaded;
        }

        private void BillPage_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterForPrinting();
        }

        private void BillPage_Unloaded(object sender, RoutedEventArgs e)
        {
            UnregisterForPrinting();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string orderId)
            {
                await ViewModel.LoadOrderAsync(orderId);
            }
        }

        private void RegisterForPrinting()
        {
            var mainWindow = (Application.Current as App)?.GetMainWindow();
            if (mainWindow == null)
                return;

            var hWnd = WindowNative.GetWindowHandle(mainWindow);
            printManager = PrintManagerInterop.GetForWindow(hWnd); // Gán cho biến instance
            printManager.PrintTaskRequested += PrintTask_Requested;

            printDocument = new PrintDocument();
            printDocumentSource = printDocument.DocumentSource;
            printDocument.Paginate += PrintDocument_Paginate;
            printDocument.GetPreviewPage += PrintDocument_GetPreviewPage;
            printDocument.AddPages += PrintDocument_AddPages;
        }

        private void UnregisterForPrinting()
        {
            if (printDocument != null)
            {
                printDocument.Paginate -= PrintDocument_Paginate;
                printDocument.GetPreviewPage -= PrintDocument_GetPreviewPage;
                printDocument.AddPages -= PrintDocument_AddPages;
                printDocument = null;
            }

            if (printManager != null)
            {
                printManager.PrintTaskRequested -= PrintTask_Requested;
                printManager = null;
            }
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (PrintManager.IsSupported())
            {
                try
                {
                    var mainWindow = (Application.Current as App)?.GetMainWindow();
                    if (mainWindow == null)
                        throw new InvalidOperationException("Không thể truy cập cửa sổ chính.");

                    var hWnd = WindowNative.GetWindowHandle(mainWindow);
                    await PrintManagerInterop.ShowPrintUIForWindowAsync(hWnd);
                }
                catch
                {
                    ContentDialog noPrintingDialog = new ContentDialog
                    {
                        Title = "Lỗi in",
                        Content = "Không thể in vào lúc này. Vui lòng kiểm tra máy in hoặc thử lại.",
                        CloseButtonText = "OK",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };
                    await noPrintingDialog.ShowAsync();
                }
            }
            else
            {
                ContentDialog noPrintingDialog = new ContentDialog
                {
                    Title = "Không hỗ trợ in",
                    Content = "Thiết bị này không hỗ trợ in.",
                    CloseButtonText = "OK",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };
                await noPrintingDialog.ShowAsync();
            }
        }

        private void PrintTask_Requested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            PrintTask printTask = args.Request.CreatePrintTask("In hoá đơn", PrintTaskSourceRequested);
            printTask.Completed += PrintTask_Completed;
        }

        private void PrintTaskSourceRequested(PrintTaskSourceRequestedArgs args)
        {
            args.SetSource(printDocumentSource);
        }

        private void PrintTask_Completed(PrintTask sender, PrintTaskCompletedEventArgs args)
        {
            string message = args.Completion switch
            {
                PrintTaskCompletion.Failed => "In thất bại.",
                PrintTaskCompletion.Canceled => "Huỷ in.",
                _ => "In hoàn tất."
            };

            dispatcherQueue.TryEnqueue(async () =>
            {
                if (this.XamlRoot != null)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Thông báo",
                        Content = message,
                        CloseButtonText = "OK",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            });
        }

        void PrintDocument_Paginate(object sender, PaginateEventArgs e)
        {
            printPreviewPages.Clear();
            PrintTaskOptions printingOptions = e.PrintTaskOptions;
            PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

            // Tạo layout in với thiết kế hiện đại hơn
            Grid printLayout = new Grid
            {
                Width = pageDescription.PageSize.Width,
                Height = pageDescription.PageSize.Height,
                Padding = new Thickness(30)
            };

            printLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Header
            printLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Thông tin đơn hàng
            printLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Separator
            printLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Tiêu đề sản phẩm
            printLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Danh sách sản phẩm
            printLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Tổng tiền
            printLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Footer

            // --- HEADER ---
            StackPanel headerPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            };

            TextBlock logoText = new TextBlock
            {
                Text = "Plant POS",
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green)
            };

            TextBlock titleText = new TextBlock
            {
                Text = "HOÁ ĐƠN BÁN HÀNG",
                FontSize = 24,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };

            headerPanel.Children.Add(logoText);
            headerPanel.Children.Add(titleText);
            Grid.SetRow(headerPanel, 0);
            printLayout.Children.Add(headerPanel);

            // --- THÔNG TIN ĐƠN HÀNG ---
            Border infoBorder = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.LightGray),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 20),
                CornerRadius = new CornerRadius(5)
            };

            Grid infoGrid = new Grid();
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Left Column - Order Info
            StackPanel leftPanel = new StackPanel { Margin = new Thickness(0, 0, 10, 0) };
            TextBlock orderIdLabel = CreateInfoLabel("Mã hoá đơn:");
            TextBlock orderIdValue = CreateInfoValue(ViewModel.OrderId.ToString());
            TextBlock orderDateLabel = CreateInfoLabel("Ngày:");
            TextBlock orderDateValue = CreateInfoValue(ViewModel.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"));

            leftPanel.Children.Add(orderIdLabel);
            leftPanel.Children.Add(orderIdValue);
            leftPanel.Children.Add(orderDateLabel);
            leftPanel.Children.Add(orderDateValue);

            // Right Column - Customer & Staff Info
            StackPanel rightPanel = new StackPanel { Margin = new Thickness(10, 0, 0, 0) };
            TextBlock staffLabel = CreateInfoLabel("Nhân viên:");
            TextBlock staffValue = CreateInfoValue(ViewModel.StaffName);
            TextBlock customerLabel = CreateInfoLabel("Khách hàng:");
            TextBlock customerValue = CreateInfoValue(ViewModel.CustomerName);

            rightPanel.Children.Add(staffLabel);
            rightPanel.Children.Add(staffValue);
            rightPanel.Children.Add(customerLabel);
            rightPanel.Children.Add(customerValue);

            Grid.SetColumn(leftPanel, 0);
            Grid.SetRow(leftPanel, 0);
            Grid.SetColumn(rightPanel, 1);
            Grid.SetRow(rightPanel, 0);

            infoGrid.Children.Add(leftPanel);
            infoGrid.Children.Add(rightPanel);
            infoBorder.Child = infoGrid;
            Grid.SetRow(infoBorder, 1);
            printLayout.Children.Add(infoBorder);

            // --- SEPARATOR ---
            Border separator = new Border
            {
                Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.DarkGreen),
                Height = 2,
                Margin = new Thickness(0, 0, 0, 15)
            };
            Grid.SetRow(separator, 2);
            printLayout.Children.Add(separator);

            // --- TIÊU ĐỀ SẢN PHẨM ---
            Grid productHeaderGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 5)
            };
            productHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) }); // Tên sản phẩm
            productHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Số lượng
            productHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Đơn giá
            productHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Thành tiền

            TextBlock productNameHeader = CreateColumnHeader("Sản phẩm");
            TextBlock quantityHeader = CreateColumnHeader("Số lượng");
            TextBlock priceHeader = CreateColumnHeader("Đơn giá");
            TextBlock totalHeader = CreateColumnHeader("Thành tiền");

            quantityHeader.HorizontalAlignment = HorizontalAlignment.Center;
            priceHeader.HorizontalAlignment = HorizontalAlignment.Center;
            totalHeader.HorizontalAlignment = HorizontalAlignment.Center; 

            Grid.SetColumn(productNameHeader, 0);
            Grid.SetColumn(quantityHeader, 1);
            Grid.SetColumn(priceHeader, 2);
            Grid.SetColumn(totalHeader, 3);

            productHeaderGrid.Children.Add(productNameHeader);
            productHeaderGrid.Children.Add(quantityHeader);
            productHeaderGrid.Children.Add(priceHeader);
            productHeaderGrid.Children.Add(totalHeader);

            Grid.SetRow(productHeaderGrid, 3);
            printLayout.Children.Add(productHeaderGrid);

            // --- DANH SÁCH SẢN PHẨM ---
            ScrollViewer scrollViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Margin = new Thickness(0, 0, 0, 15)
            };

            StackPanel productsPanel = new StackPanel();

            foreach (var item in ViewModel.OrderItems)
            {
                Grid productRow = new Grid
                {
                    Margin = new Thickness(0, 5, 0, 5)
                };
                productRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                productRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                productRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                productRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

                TextBlock productName = new TextBlock
                {
                    Text = item.Product.Name,
                    TextWrapping = TextWrapping.Wrap
                };

                TextBlock quantity = new TextBlock
                {
                    Text = item.Quantity.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                TextBlock price = new TextBlock
                {
                    Text = $"{item.SalePrice:N0} ₫",
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                TextBlock itemTotal = new TextBlock
                {
                    Text = $"{(item.Quantity * item.SalePrice):N0} ₫",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.SemiBold
                };

                Grid.SetColumn(productName, 0);
                Grid.SetColumn(quantity, 1);
                Grid.SetColumn(price, 2);
                Grid.SetColumn(itemTotal, 3);

                productRow.Children.Add(productName);
                productRow.Children.Add(quantity);
                productRow.Children.Add(price);
                productRow.Children.Add(itemTotal);

                productsPanel.Children.Add(productRow);

                // Separator line
                if (ViewModel.OrderItems.IndexOf(item) < ViewModel.OrderItems.Count - 1)
                {
                    productsPanel.Children.Add(new Border
                    {
                        Height = 1,
                        Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.LightGray),
                        Margin = new Thickness(0, 5, 0, 5)
                    });
                }
            }

            scrollViewer.Content = productsPanel;
            Grid.SetRow(scrollViewer, 4);
            printLayout.Children.Add(scrollViewer);

            // --- TỔNG TIỀN ---
            Border summaryBorder = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.LightGray),
                Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                    Microsoft.UI.ColorHelper.FromArgb(20, 0, 100, 0)),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 15),
                CornerRadius = new CornerRadius(5)
            };

            StackPanel summaryPanel = new StackPanel();

            Grid subtotalRow = CreateSummaryRow($"Tổng sản phẩm ({ViewModel.TotalItem}):", $"{ViewModel.TotalPrice:N0} ₫");
            Grid discountRow = CreateSummaryRow($"Giảm giá ({ViewModel.DiscountRate}%):", $"{ViewModel.DiscountAmount:N0} ₫");

            Border finalSeparator = new Border
            {
                Height = 1,
                Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.DarkGray),
                Margin = new Thickness(0, 10, 0, 10)
            };

            Grid finalRow = CreateSummaryRow("Tổng thanh toán:", $"{ViewModel.FinalPrice:N0} ₫", true);

            summaryPanel.Children.Add(subtotalRow);
            summaryPanel.Children.Add(discountRow);
            summaryPanel.Children.Add(finalSeparator);
            summaryPanel.Children.Add(finalRow);

            summaryBorder.Child = summaryPanel;
            Grid.SetRow(summaryBorder, 5);
            printLayout.Children.Add(summaryBorder);

            // --- FOOTER ---
            StackPanel footerPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 15, 0, 0)
            };

            TextBlock thankYouBlock = new TextBlock
            {
                Text = "Cảm ơn quý khách đã mua hàng!",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            TextBlock contactBlock = new TextBlock
            {
                Text = "Plant POS - Hệ thống quản lý bán hàng chuyên nghiệp",
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };

            footerPanel.Children.Add(thankYouBlock);
            footerPanel.Children.Add(contactBlock);
            Grid.SetRow(footerPanel, 6);
            printLayout.Children.Add(footerPanel);

            printPreviewPages.Add(printLayout);
            printDocument.SetPreviewPageCount(printPreviewPages.Count, PreviewPageCountType.Final);
        }

        // Helper methods
        private TextBlock CreateInfoLabel(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 12,
                Opacity = 0.7,
                Margin = new Thickness(0, 5, 0, 0)
            };
        }

        private TextBlock CreateInfoValue(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 10)
            };
        }

        private TextBlock CreateColumnHeader(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 5, 0)
            };
        }

        private Grid CreateSummaryRow(string label, string value, bool isFinal = false)
        {
            Grid row = new Grid();
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            TextBlock labelBlock = new TextBlock
            {
                Text = label,
                FontSize = isFinal ? 16 : 14,
                FontWeight = isFinal ? FontWeights.Bold : FontWeights.Normal,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock valueBlock = new TextBlock
            {
                Text = value,
                FontSize = isFinal ? 18 : 14,
                FontWeight = isFinal ? FontWeights.Bold : FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Grid.SetColumn(labelBlock, 0);
            Grid.SetColumn(valueBlock, 1);

            row.Children.Add(labelBlock);
            row.Children.Add(valueBlock);

            return row;
        }

        private void PrintDocument_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            printDocument.SetPreviewPage(e.PageNumber, printPreviewPages[e.PageNumber - 1]);
        }

        private void PrintDocument_AddPages(object sender, AddPagesEventArgs e)
        {
            foreach (var page in printPreviewPages)
            {
                printDocument.AddPage(page);
            }
            printDocument.AddPagesComplete();
        }

        private async void ExportPDFButton_Click(object sender, RoutedEventArgs e)
        {
            using (PdfDocument document = new PdfDocument())
            {
                // Thiết lập trang
                document.PageSettings.Orientation = PdfPageOrientation.Portrait;
                document.PageSettings.Margins.All = 30;
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;

                // Chuẩn bị font chữ
                string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Fonts", "Arial.ttf");
                using (FileStream fontStream = new FileStream(fontPath, FileMode.Open, FileAccess.Read))
                {
                    // Định nghĩa các loại font
                    PdfTrueTypeFont logoFont = new PdfTrueTypeFont(fontStream, 28, PdfFontStyle.Bold);
                    PdfTrueTypeFont titleFont = new PdfTrueTypeFont(fontStream, 24, PdfFontStyle.Bold);
                    PdfTrueTypeFont headingFont = new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold);
                    PdfTrueTypeFont normalFont = new PdfTrueTypeFont(fontStream, 12);
                    PdfTrueTypeFont boldFont = new PdfTrueTypeFont(fontStream, 12, PdfFontStyle.Bold);
                    PdfTrueTypeFont labelFont = new PdfTrueTypeFont(fontStream, 10);
                    PdfTrueTypeFont watermarkFont = new PdfTrueTypeFont(fontStream, 40, PdfFontStyle.Bold);

                    // Định nghĩa các màu
                    PdfColor brandColor = new PdfColor(0, 122, 51); // Màu xanh lá đậm
                    PdfColor lightGreen = new PdfColor(240, 248, 240); // Màu xanh lá nhạt cho background
                    PdfColor grayColor = new PdfColor(150, 150, 150);
                    PdfColor borderColor = new PdfColor(200, 200, 200);

                    // Lấy kích thước trang để tính toán
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
                    // Logo và tiêu đề
                    graphics.DrawString("Plant POS", logoFont, new PdfSolidBrush(brandColor),
                                        new Syncfusion.Drawing.PointF(pageWidth / 2 - 70, currentY));
                    currentY += 35;

                    graphics.DrawString("HOÁ ĐƠN BÁN HÀNG", titleFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF(pageWidth / 2 - 120, currentY));
                    currentY += 40;

                    // ----- THÔNG TIN ĐƠN HÀNG -----
                    // Vẽ border
                    float infoBoxWidth = pageWidth - 60;
                    float infoBoxHeight = 100;
                    graphics.DrawRectangle(new PdfPen(borderColor, 1),
                                          new Syncfusion.Drawing.RectangleF(30, currentY, infoBoxWidth, infoBoxHeight));

                    // Thông tin đơn hàng - cột trái
                    float infoLeftX = 45;
                    float infoStartY = currentY + 15;

                    // Mã hoá đơn
                    graphics.DrawString("Mã hoá đơn:", labelFont, PdfBrushes.Gray,
                                        new Syncfusion.Drawing.PointF(infoLeftX, infoStartY));
                    graphics.DrawString(ViewModel.OrderId.ToString(), boldFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF(infoLeftX, infoStartY + 20));

                    // Ngày
                    graphics.DrawString("Ngày:", labelFont, PdfBrushes.Gray,
                                        new Syncfusion.Drawing.PointF(infoLeftX, infoStartY + 45));
                    graphics.DrawString(ViewModel.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"), boldFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF(infoLeftX, infoStartY + 65));

                    // Thông tin đơn hàng - cột phải
                    float infoRightX = pageWidth / 2 + 30;

                    // Nhân viên
                    graphics.DrawString("Nhân viên:", labelFont, PdfBrushes.Gray,
                                        new Syncfusion.Drawing.PointF(infoRightX, infoStartY));
                    graphics.DrawString(ViewModel.StaffName, boldFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF(infoRightX, infoStartY + 20));

                    // Khách hàng
                    graphics.DrawString("Khách hàng:", labelFont, PdfBrushes.Gray,
                                        new Syncfusion.Drawing.PointF(infoRightX, infoStartY + 45));
                    graphics.DrawString(ViewModel.CustomerName, boldFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF(infoRightX, infoStartY + 65));

                    currentY += infoBoxHeight + 20;

                    // ----- SEPARATOR -----
                    graphics.DrawLine(new PdfPen(brandColor, 2), new Syncfusion.Drawing.PointF(30, currentY),
                                      new Syncfusion.Drawing.PointF(pageWidth - 30, currentY));
                    currentY += 20;

                    // ----- DANH SÁCH SẢN PHẨM -----
                    // Tiêu đề bảng
                    graphics.DrawString("CHI TIẾT SẢN PHẨM", headingFont, new PdfSolidBrush(brandColor),
                                        new Syncfusion.Drawing.PointF(30, currentY));
                    currentY += 25;

                    // Tạo bảng sản phẩm
                    PdfGrid pdfGrid = new PdfGrid();
                    pdfGrid.Columns.Add(4);
                    pdfGrid.Headers.Add(1);

                    // Tiêu đề cột
                    PdfGridRow header = pdfGrid.Headers[0];
                    header.Cells[0].Value = "Sản phẩm";
                    header.Cells[1].Value = "SL";
                    header.Cells[2].Value = "Đơn giá";
                    header.Cells[3].Value = "Thành tiền";

                    // Định dạng tiêu đề
                    PdfGridCellStyle headerStyle = new PdfGridCellStyle
                    {
                        BackgroundBrush = new PdfSolidBrush(brandColor),
                        TextBrush = PdfBrushes.White,
                        Font = headingFont
                    };
                    header.ApplyStyle(headerStyle);

                    // Căn giữa cho tiêu đề
                    header.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                    header.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                    header.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                    header.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);

                    // Cài đặt chiều rộng cột
                    pdfGrid.Columns[0].Width = 220;
                    pdfGrid.Columns[1].Width = 50;
                    pdfGrid.Columns[2].Width = 100;
                    pdfGrid.Columns[3].Width = 100;

                    // Dữ liệu sản phẩm
                    foreach (var item in ViewModel.OrderItems)
                    {
                        PdfGridRow row = pdfGrid.Rows.Add();
                        row.Cells[0].Value = item.Product.Name;
                        row.Cells[1].Value = item.Quantity.ToString();
                        row.Cells[2].Value = $"{item.SalePrice:N0} ₫";
                        row.Cells[3].Value = $"{(item.Quantity * item.SalePrice):N0} ₫";
                    }

                    // Định dạng ô trong bảng
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

                    // Áp dụng style cho từng dòng và căn lề
                    foreach (PdfGridRow row in pdfGrid.Rows)
                    {
                        // Style cơ bản cho mọi cell
                        row.ApplyStyle(cellStyle);

                        // Bold cho cột thành tiền
                        row.Cells[3].Style.Font = boldFont;

                        // Căn lề
                        row.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                        row.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                    }

                    // Vẽ bảng
                    PdfGridLayoutResult gridResult = pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(30, currentY));
                    currentY = gridResult.Bounds.Bottom + 30;

                    // ----- TỔNG TIỀN -----
                    // Vẽ border và background cho phần tổng tiền
                    float summaryBoxWidth = 250;
                    float summaryBoxHeight = 100;
                    float summaryBoxX = pageWidth - summaryBoxWidth - 30;

                    // Vẽ hình chữ nhật với màu nền
                    graphics.DrawRectangle(new PdfSolidBrush(lightGreen),
                                          new Syncfusion.Drawing.RectangleF(summaryBoxX, currentY, summaryBoxWidth, summaryBoxHeight));
                    // Vẽ border
                    graphics.DrawRectangle(new PdfPen(borderColor, 1),
                                          new Syncfusion.Drawing.RectangleF(summaryBoxX, currentY, summaryBoxWidth, summaryBoxHeight));

                    float summaryTextX = summaryBoxX + 15;
                    float summaryValueX = summaryBoxX + summaryBoxWidth - 15;
                    float summaryY = currentY + 15;

                    // Tổng sản phẩm
                    graphics.DrawString($"Tổng sản phẩm ({ViewModel.TotalItem}):", normalFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF(summaryTextX, summaryY));

                    PdfStringFormat rightAlign = new PdfStringFormat { Alignment = PdfTextAlignment.Right };
                    graphics.DrawString($"{ViewModel.TotalPrice:N0} ₫", normalFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.RectangleF(summaryTextX, summaryY, summaryBoxWidth - 30, 20), rightAlign);
                    summaryY += 20;

                    // Giảm giá
                    graphics.DrawString($"Giảm giá ({ViewModel.DiscountRate}%):", normalFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF(summaryTextX, summaryY));
                    graphics.DrawString($"{ViewModel.DiscountAmount:N0} ₫", normalFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.RectangleF(summaryTextX, summaryY, summaryBoxWidth - 30, 20), rightAlign);
                    summaryY += 25;

                    // Đường kẻ phân cách
                    graphics.DrawLine(new PdfPen(borderColor, 1), new Syncfusion.Drawing.PointF(summaryTextX, summaryY),
                                      new Syncfusion.Drawing.PointF(summaryValueX, summaryY));
                    summaryY += 10;

                    // Tổng thanh toán
                    graphics.DrawString("Tổng thanh toán:", boldFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF(summaryTextX, summaryY));
                    graphics.DrawString($"{ViewModel.FinalPrice:N0} ₫", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold),
                                        new PdfSolidBrush(brandColor),
                                        new Syncfusion.Drawing.RectangleF(summaryTextX, summaryY, summaryBoxWidth - 30, 20), rightAlign);

                    // ----- FOOTER -----
                    float footerY = pageHeight - 70;

                    // Đường kẻ phân cách
                    graphics.DrawLine(new PdfPen(grayColor, 0.5f), new Syncfusion.Drawing.PointF(30, footerY),
                                      new Syncfusion.Drawing.PointF(pageWidth - 30, footerY));
                    footerY += 15;

                    // Lời cảm ơn
                    string thankYouText = "Cảm ơn quý khách đã mua hàng!";
                    SizeF thankYouSize = headingFont.MeasureString(thankYouText);
                    graphics.DrawString(thankYouText, headingFont, PdfBrushes.Black,
                                        new Syncfusion.Drawing.PointF((pageWidth - thankYouSize.Width) / 2, footerY));
                    footerY += 20;

                    // Thông tin liên hệ
                    string contactText = "Plant POS - Hệ thống quản lý bán hàng chuyên nghiệp";
                    SizeF contactSize = normalFont.MeasureString(contactText);
                    graphics.DrawString(contactText, normalFont, new PdfSolidBrush(grayColor),
                                        new Syncfusion.Drawing.PointF((pageWidth - contactSize.Width) / 2, footerY));

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
                            DefaultButton = ContentDialogButton.Close,
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