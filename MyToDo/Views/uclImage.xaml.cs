using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;

namespace MyToDo.Views
{
    /// <summary>
    /// uclImage.xaml 的交互逻辑
    /// </summary>
    public partial class uclImage : UserControl
    {
        public uclImage()
        {
            InitializeComponent();
        }

        private double _X;
        private double _Y;
        private Vec3b[,] _ImageData3b;
        private byte[,] _ImageDatab;
        private Point _MiddleButtonClickedPosition;//记录中键点击的位置。。。。。中间拖拉移动

        private BitmapSource _ImageSource;

        public BitmapSource ImageSource
        {
            get { return (BitmapSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource",
                typeof(BitmapSource), typeof(uclImage),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(OnCurrentReadingChanged)));

        private static void OnCurrentReadingChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            var cursorPosition = e.GetPosition((IInputElement)e.Source);
            double x = cursorPosition.X;
            double y = cursorPosition.Y;
            //获取控件大小
            Image imageControl = (Image)(IInputElement)e.Source;

            double xRatio = ImageSource.Width / imageControl.ActualWidth;
            double yRatio = ImageSource.Height / imageControl.ActualHeight;

            _X = (float)(x * xRatio);
            _Y = (float)(y * yRatio);

            Path_X.Text = _X.ToString("0.00");
            Path_Y.Text = _Y.ToString("0.00");
            //获取图片像素信息
            if (_ImageData3b != null)
            {
                //准了
                Path_B.Text = _ImageData3b[(int)_Y, (int)_X].Item0.ToString("000");
                Path_G.Text = _ImageData3b[(int)_Y, (int)_X].Item1.ToString("000");
                Path_R.Text = _ImageData3b[(int)_Y, (int)_X].Item2.ToString("000");
            }
            else if (_ImageDatab != null)
            {
                Path_B.Text = _ImageDatab[(int)_Y, (int)_X].ToString("000");
                Path_G.Text = _ImageDatab[(int)_Y, (int)_X].ToString("000");
                Path_R.Text = _ImageDatab[(int)_Y, (int)_X].ToString("000");
            }

            //当中键按下，移动图片
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Image im = sender as Image;
                var group = im.RenderTransform as TransformGroup;
                var ttf = group.Children[1] as TranslateTransform;//对应Xaml位置    这样搞，放大缩小有点奇怪

                ttf.X += cursorPosition.X - _MiddleButtonClickedPosition.X;
                ttf.Y += cursorPosition.Y - _MiddleButtonClickedPosition.Y;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (Mat mat = ImageSource.ToMat())
            {
                if (mat.Channels() == 3)
                {
                    mat.GetRectangularArray<Vec3b>(out Vec3b[,] vec3Ds);
                    _ImageData3b = vec3Ds;
                    _ImageDatab = null;
                }
                else
                {
                    mat.GetRectangularArray<byte>(out byte[,] vecDs);
                    _ImageDatab = vecDs;
                    _ImageData3b = null;
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
                _MiddleButtonClickedPosition = e.GetPosition((IInputElement)e.Source);
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Image sf = sender as Image;
            var group = sf.RenderTransform as TransformGroup;
            var sc = group.Children[0] as ScaleTransform;//对应Xaml位置    这样搞，放大缩小有点奇怪
            var cursorPosition = e.GetPosition((IInputElement)e.Source);
            sc.CenterX = cursorPosition.X;
            sc.CenterY = cursorPosition.Y;
            if (e.Delta > 0)
            {
                sc.ScaleX += 0.02;
                sc.ScaleY += 0.02;
            }
            else
            {
                if (sc.ScaleX > 0.55)
                {
                    sc.ScaleX -= 0.02;
                    sc.ScaleY -= 0.02;
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var scr = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent((DependencyObject)e.Source)) as ScrollViewer;

            var im = scr.Content as Image;

            var group = im.RenderTransform as TransformGroup;
            group.Children[0] = new ScaleTransform();
            group.Children[1] = new TranslateTransform();
        }
    }
}