using System.Windows;
using System.Windows.Media;
namespace MyToDoLibrary
{
 

    /// <summary>
    /// 二次贝塞尔曲线
    /// </summary>
    public class ArrowQuadraticBezier : ArrowBase
    {
        #region Fields

        #region DependencyProperty

        /// <summary>
        /// 控制点1
        /// </summary>
        public static readonly DependencyProperty ControlPointProperty = DependencyProperty.Register(
            "ControlPoint",
            typeof(Point),
            typeof(ArrowQuadraticBezier),
            new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 结束点
        /// </summary>
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint",
            typeof(Point),
            typeof(ArrowQuadraticBezier),
            new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion DependencyProperty

        /// <summary>
        /// 二次贝塞尔曲线
        /// </summary>
        private readonly QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment();

        #endregion Fields

        #region Properties

        /// <summary>
        /// 控制点1
        /// </summary>
        public Point ControlPoint
        {
            get { return (Point)this.GetValue(ControlPointProperty); }
            set { this.SetValue(ControlPointProperty, value); }
        }

        /// <summary>
        /// 结束点
        /// </summary>
        public Point EndPoint
        {
            get { return (Point)this.GetValue(EndPointProperty); }
            set { this.SetValue(EndPointProperty, value); }
        }

        #endregion Properties

        #region Protected Methods

        /// <summary>
        /// 填充Figure
        /// </summary>
        /// <returns>PathSegment集合</returns>
        protected override PathSegmentCollection FillFigure()
        {
            quadraticBezierSegment.Point1 = ControlPoint;
            quadraticBezierSegment.Point2 = EndPoint;

            return new PathSegmentCollection
            {
                quadraticBezierSegment
            };
        }

        /// <summary>
        /// 获取开始箭头处的结束点
        /// </summary>
        /// <returns>开始箭头处的结束点</returns>
        protected override Point GetStartArrowEndPoint()
        {
            return ControlPoint;
        }

        /// <summary>
        /// 获取结束箭头处的开始点
        /// </summary>
        /// <returns>结束箭头处的开始点</returns>
        protected override Point GetEndArrowStartPoint()
        {
            return ControlPoint;
        }

        /// <summary>
        /// 获取结束箭头处的结束点
        /// </summary>
        /// <returns>结束箭头处的结束点</returns>
        protected override Point GetEndArrowEndPoint()
        {
            return EndPoint;
        }

        #endregion  Protected Methods
    }
}
