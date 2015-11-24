using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Xps;

namespace KinectTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Rectangle> AreaRects;
        double ratio = 0.5;
        double kinect_width = 1920;
        double kinect_height = 1080;

        public MainWindow()
        {
            InitializeComponent();
            AreaRects = new List<Rectangle>();
            AreaRects.Add(AreaRect1);
            AreaRects.Add(AreaRect2);
            AreaRects.Add(AreaRect3);
            AreaRects.Add(AreaRect4);
            AreaRects.Add(AreaRect5);
            AreaRects.Add(AreaRect6);
            border1.Width = kinect_width * ratio;
            border1.Height = kinect_height * ratio;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            visibleRectangle(0);
            

            DispatcherTimer dispTimer = new DispatcherTimer();
            dispTimer.Interval = new TimeSpan(1000000);
            dispTimer.Tick += dispTimer_Tick;
            dispTimer.Start();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        // 単一ページの印刷
        private void PrintSinglePage()
        {
            // 1.各種オブジェクトの生成
            LocalPrintServer lps = new LocalPrintServer();
            PrintQueue queue = lps.DefaultPrintQueue;
            XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(queue);

            // 2. 用紙サイズの設定
            PrintTicket ticket = queue.DefaultPrintTicket;
            ticket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
            ticket.PageOrientation = PageOrientation.Portrait;

            // 3. FixedPage の生成
            FixedPage page = new FixedPage();

            // 4. 印字データの作成
            //RenderTargetBitmap rtb = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Bgr32);
            
            Canvas canvas = new Canvas();
            //Image img = new Image();
            //img.Width = 1024;
            //img.Height = 768;
            //img.Source = bg_img.Source;
            //Rectangle rec = new Rectangle();
            //rec.Fill = new ImageBrush(bg_img.Source);
            //rec.Width = 1024;
            //rec.Height = 768;
            //Canvas.SetTop(rec, 0);
            //Canvas.SetLeft(rec, 0);
            //canvas.Children.Add(rec);
            TextBlock tb = new TextBlock();
            tb.Text = "TEST";
            tb.FontSize = 24;
            Canvas.SetTop(tb, 100);
            Canvas.SetLeft(tb, 100);
            canvas.Children.Add(tb);

            //maingrid.Children.Add(canvas);
            page.Children.Add(canvas);
            
            //// 5. 印刷の実行
            writer.Write(page, ticket);
        }

        void dispTimer_Tick(object sender, EventArgs e)
        {
            Point hitpoint1 = convertRatioPoint(getRandomPoint(), ratio);
            //Point hitpoint2 = convertRatioPoint(getRandomPoint(), ratio);

            canvas1.Children.Clear();
            addCanvasPoint(canvas1, hitpoint1, 10, new SolidColorBrush(Colors.Red));
            //addCanvasPoint(canvas1, hitpoint2, 10, new SolidColorBrush(Colors.Green));
            int hit_num1 = hitArea(hitpoint1);
            //int hit_num2 = hitArea(hitpoint2);
            visibleRectangle(hit_num1);
            //if (hit_num1 == hit_num2)
            //{
            //    visibleRectangle(hit_num1);
            //}
            //else
            //{
            //    visibleRectangle(0);
            //}
        }

        Random r = new Random();
        private Point getRandomPoint()
        {
            Point re = new Point();

            double x = r.NextDouble() * kinect_width;
            double y = r.NextDouble() * kinect_height;
            re.X = x;
            re.Y = y;

            return re;
        }

        private Point convertRatioPoint(Point point, double ratio)
        {
            return new Point(point.X * ratio, point.Y * ratio);
        } 

        private bool isContainsRect(Rectangle rect, Point point)
        {
            bool re = false;

            Point rect_p = rect.TranslatePoint(new Point(0, 0), border1);
            double width = rect.ActualWidth;
            double height = rect.ActualHeight;

            if ((point.X >= rect_p.X && point.X < (rect_p.X + width)) && (point.Y >= rect_p.Y && point.Y < (rect_p.Y + height)))
            {
                re = true;
            }

            return re;
        }

        private void addCanvasPoint(Canvas canvas, Point point, double size, SolidColorBrush scb)
        {
            canvas1.Children.Add(new Ellipse()
            {
                Margin = new Thickness(point.X - size / 2, point.Y - size / 2, 0, 0),
                Fill = scb,
                Width = size,
                Height = size,
            });
        }

        private int hitArea(Point point)
        {
            int re = 0;

            for (int i = 0; i < AreaRects.Count; i++)
            {
                if (isContainsRect(AreaRects[i], point))
                {
                    re = i + 1;
                }
            }
            return re;
        }

        private void visibleRectangle(int rect_num)
        {
            for (int i = 0; i < AreaRects.Count; i++)
            {
                AreaRects[i].Visibility = System.Windows.Visibility.Hidden;
                if (i + 1 == rect_num)
                {
                    AreaRects[i].Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    //PrintDialog pd = new PrintDialog();
                    //pd.PrintTicket.PageMediaSize = new PageMediaSize(maingrid.ActualWidth, maingrid.ActualHeight);
                    ////pd.PrintTicket.PageOrientation = PageOrientation.Landscape;
                    //if (pd.ShowDialog() == true)
                    //{
                    //    //Image img = new Image();
                    //    //img.Stretch = Stretch.Uniform;
                    //    //img.Width = 512;
                    //    //img.Height = 384;
                    //    //img.Source = bg_img.Source;
                    //    //img.Visibility = System.Windows.Visibility.Visible;
                    //    //maingrid.Children.Add(img);
                        
                    //    pd.PrintVisual(maingrid, "bgimg");

                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                //PrintSinglePage();
                //ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\hirano\Desktop\望年会2014\サッカーゲーム\soccer_1024-768.jpg");
                //info.Verb = "print";
                //Process.Start(info);
                //PrintPage();
                printCanvas();
            }
        }
        // FormattedText の生成
        private FormattedText CreateFormatedText(string s, double width, TextAlignment align)
        {
            FormattedText fmtText = new FormattedText(
                s,
                System.Globalization.CultureInfo.CurrentCulture,
                System.Windows.FlowDirection.LeftToRight,
                new Typeface("ＭＳ Ｐゴシック"),
                24.0d, Brushes.Black);
            fmtText.MaxTextWidth = width;
            fmtText.TextAlignment = align;
            return fmtText;
        }

        // ページの描画
        private void DrawPage(DrawingContext dc)
        {
            // Lineの描画
            //for (int i = 0; i < 10; i++)
            //{
            //    Pen pen = new Pen(Brushes.Black, 1.0d + (i * 0.5d));
            //    dc.DrawLine(pen, new Point(100.0d, 100.0d + (i * 10.0d)),
            //                     new Point(200.0d, 100.0d + (i * 10.0d)));
            //}

            //// 矩形の描画
            //dc.DrawRectangle(Brushes.Blue, new Pen(Brushes.Black, 1.0d),
            //                               new Rect(250.0d, 100.0d, 100.0d, 100.0d));

            //// 円の描画
            //dc.DrawEllipse(Brushes.Green, new Pen(Brushes.Black, 1.0d),
            //                              new Point(450.0d, 150.0d), 50.0d, 50.0d);

            //// ジオメトリの描画
            //StreamGeometry sg1 = new StreamGeometry();
            //using (StreamGeometryContext ctx = sg1.Open())
            //{
            //    ctx.BeginFigure(new Point(600.0d, 100.0d), true, true);
            //    ctx.LineTo(new Point(650.0d, 180.0d), true, true);
            //    ctx.LineTo(new Point(550.0d, 180.0d), true, true);
            //}
            //sg1.Freeze();
            //dc.DrawGeometry(Brushes.Red, new Pen(Brushes.Black, 2.0d), sg1);

            //StreamGeometry sg2 = new StreamGeometry();
            //using (StreamGeometryContext ctx = sg2.Open())
            //{
            //    ctx.BeginFigure(new Point(550.0d, 120.0d), true, true);
            //    ctx.LineTo(new Point(650.0d, 120.0d), true, true);
            //    ctx.LineTo(new Point(600.0d, 200.0d), true, true);
            //}
            //sg2.Freeze();
            //dc.DrawGeometry(Brushes.Yellow, new Pen(Brushes.Black, 2.0d), sg2);

            // イメージの描画
            Uri imageUri = new Uri(@"C:\Users\hirano\Desktop\望年会2014\サッカーゲーム\soccer_1024-768.jpg");
            dc.DrawImage(bg_img.Source, new Rect(0.0d, 0.0d, 512.0d, 384.0d));

            // テキストの描画
            //dc.DrawRectangle(null, new Pen(Brushes.Black, 1.0d), new Rect(99.0d, 399.0d, 502.0d, 102.0d));

            //FormattedText fmtTextL = CreateFormatedText("左寄せ文字列", 500.0d, TextAlignment.Left);
            //dc.DrawText(fmtTextL, new Point(100.0d, 400.0d + (30.0d - fmtTextL.Height) / 2.0d));

            //FormattedText fmtTextC = CreateFormatedText("中央寄せ文字列", 500.0d, TextAlignment.Center);
            //dc.DrawText(fmtTextC, new Point(100.0d, 430.0d + (30.0d - fmtTextC.Height) / 2.0d));

            //FormattedText fmtTextR = CreateFormatedText("右寄せ文字列", 500.0d, TextAlignment.Right);
            //dc.DrawText(fmtTextR, new Point(100.0d, 460.0d + (30.0d - fmtTextR.Height) / 2.0d));
        }

        // 印刷処理
        private void PrintPage()
        {
            // 各種オブジェクトの生成と取得
            LocalPrintServer lps = new LocalPrintServer();
            PrintDialog pd = new PrintDialog();
            PrintQueue queue = pd.PrintQueue;
            XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(queue);

            // 用紙サイズの設定
            PrintTicket ticket = pd.PrintTicket;
            ticket.PageMediaSize = new PageMediaSize(PageMediaSizeName.JapanHagakiPostcard);
            

            // VisualsToXpsDocumentの生成
            VisualsToXpsDocument v2xps = writer.CreateVisualsCollator() as VisualsToXpsDocument;
            v2xps.BeginBatchWrite();
            try
            {
                ContainerVisual container = new ContainerVisual();
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    DrawPage(dc);
                }
                container.Children.Add(dv);
                v2xps.Write(container, ticket);
            }
            finally
            {
                v2xps.EndBatchWrite();
            }
        }

        private void  printCanvas() {
            // 印刷ダイアログを作成。
            var dPrt = new PrintDialog();

            // 印刷ダイアログを表示して、プリンタ選択と印刷設定を行う。
            //if (dPrt.ShowDialog() == true)
            //{
                // ここから印刷を実行する。

                // 印刷可能領域を取得する。
                var area = dPrt.PrintQueue.GetPrintCapabilities().PageImageableArea;

                // 上と左の余白を含めた印刷可能領域の大きさのCanvasを作る。
                var canv = new Canvas();
                canv.Width = area.OriginWidth + area.ExtentWidth;
                canv.Height = area.OriginHeight + area.ExtentHeight;

                // ここでCanvasに描画する。
                Image img = new Image();
                img.Source = bg_img.Source;
                img.Width = canv.Width;
                img.Height = canv.Height;
                img.Stretch = Stretch.UniformToFill;

                Canvas.SetLeft(img, 0);
                Canvas.SetTop(img, 0);
                canv.Children.Add(img);

                /* ここで単ページのVisualを直接印刷する場合、PrintVisual()と言うメソッドもある。
                dPrt.PrintVisual(canv, "PrintTest1");
                */

                // FixedPageを作って印刷対象（ここではCanvas）を設定する。
                var page = new FixedPage();
                page.Children.Add(canv);

                // PageContentを作ってFixedPageを設定する。
                var cont = new PageContent();
                cont.Child = page;

                // FixedDocumentを作ってPageContentを設定する。
                var doc = new FixedDocument();
                doc.Pages.Add(cont);

                // 印刷する。
                dPrt.PrintDocument(doc.DocumentPaginator, "Print1");
            //}
        }
    }
}
