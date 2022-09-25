using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;

namespace MyToDo.ViewModels
{
    /// <summary>
    /// 承载图片用控件，可拖动平移，滚轮缩放，获取光标位置信息之类
    /// </summary>
    public class uclImageBoxModel : BindableBase
    {
        public uclImageBoxModel()
        {
        }

        public uclImageBoxModel(BitmapSource image)
        {
            _Image = image;
        }

        #region Properties

        private BitmapSource _Image;

        public BitmapSource Image
        {
            get { return _Image; }
            set
            {
                if (value != _Image)
                {
                    using (Mat mat = value.ToMat())
                    {
                        if (mat.Channels() == 3)
                        {
                            mat.GetRectangularArray(out Vec3b[,] vec3Ds);
                            _ImageData3b = vec3Ds;
                            _ImageDatab = null;
                        }
                        else
                        {
                            mat.GetRectangularArray(out byte[,] vecDs);
                            _ImageDatab = vecDs;
                            _ImageData3b = null;
                        }
                    }

                    _Image = value;
                    _Image.Freeze();
                    RaisePropertyChanged();
                }
            }
        }

        private double _X;
        private double _Y;
        private string _StringX;
        private Vec3b[,] _ImageData3b;
        private byte[,] _ImageDatab;

        public string StringX
        {
            get { return _StringX; }
            set { _StringX = value; RaisePropertyChanged(); }
        }

        private string _StringY;

        public string StringY
        {
            get { return _StringY; }
            set { _StringY = value; RaisePropertyChanged(); }
        }

        private string _R;

        public string R
        {
            get { return _R; }
            set { _R = value; RaisePropertyChanged(); }
        }

        private string _G;

        public string G
        {
            get { return _G; }
            set { _G = value; RaisePropertyChanged(); }
        }

        private string _B;

        public string B
        {
            get { return _B; }
            set { _B = value; RaisePropertyChanged(); }
        }

        #endregion Properties



        #region Events

        /// <summary>
        /// 图片按下事件，获取中建按下时刻的位置，为后续拖动提供坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
                _MiddleButtonClickedPosition = e.GetPosition((IInputElement)e.Source);
        }

        private Point _MiddleButtonClickedPosition;//记录中键点击的位置。。。。。中间拖拉移动

        /// <summary>
        /// 图片鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ImageMouseMove(object sender, MouseEventArgs e)
        {
            var cursorPosition = e.GetPosition((IInputElement)e.Source);
            double x = cursorPosition.X;
            double y = cursorPosition.Y;
            //获取控件大小
            Image imageControl = (Image)(IInputElement)e.Source;

            double xRatio = Image.Width / imageControl.ActualWidth;
            double yRatio = Image.Height / imageControl.ActualHeight;

            _X = (float)(x * xRatio);
            _Y = (float)(y * yRatio);

            StringX = _X.ToString("0.00");
            StringY = _Y.ToString("0.00");
            //获取图片像素信息
            if (_ImageData3b != null)
            {
                //准了
                B = _ImageData3b[(int)_Y, (int)_X].Item0.ToString("000");
                G = _ImageData3b[(int)_Y, (int)_X].Item1.ToString("000");
                R = _ImageData3b[(int)_Y, (int)_X].Item2.ToString("000");
            }
            else
            {
                B = _ImageDatab[(int)_Y, (int)_X].ToString("000");
                G = _ImageDatab[(int)_Y, (int)_X].ToString("000");
                R = _ImageDatab[(int)_Y, (int)_X].ToString("000");
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

        /// <summary>
        /// 图片滚轮缩放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ImageMouseWheel(object sender, MouseWheelEventArgs e)
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

        /// <summary>
        /// 图片还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ImageRecover_MenuItem(object sender, RoutedEventArgs e)
        {
            var scr = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent((DependencyObject)e.Source)) as ScrollViewer;

            var im = scr.Content as Image;

            var group = im.RenderTransform as TransformGroup;
            group.Children[0] = new ScaleTransform();
            group.Children[1] = new TranslateTransform();
        }

        #endregion Events
    }
}