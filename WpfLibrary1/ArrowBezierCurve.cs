using System.Windows;
using System.Windows.Media;
namespace MyToDoLibrary
{
 

    /// <summary>
    /// ����ͷ�ı���������
    /// </summary>
    public class ArrowBezierCurve : ArrowBase
    {
        #region Fields

        #region DependencyProperty

        /// <summary>
        /// ���Ƶ�1
        /// </summary>
        public static readonly DependencyProperty ControlPoint1Property = DependencyProperty.Register(
            "ControlPoint1",
            typeof(Point),
            typeof(ArrowBezierCurve),
            new FrameworkPropertyMetadata(
                new Point(),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// ���Ƶ�2
        /// </summary>
        public static readonly DependencyProperty ControlPoint2Property = DependencyProperty.Register(
            "ControlPoint2", typeof(Point), typeof(ArrowBezierCurve), new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// ������
        /// </summary>
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint", typeof(Point), typeof(ArrowBezierCurve), new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion DependencyProperty

        /// <summary>
        /// ����������
        /// </summary>
        private readonly BezierSegment bezierSegment = new BezierSegment();

        #endregion Fields

        #region Properties

        /// <summary>
        /// ���Ƶ�1
        /// </summary>
        public Point ControlPoint1
        {
            get { return (Point)this.GetValue(ControlPoint1Property); }
            set { this.SetValue(ControlPoint1Property, value); }
        }

        /// <summary>
        /// ���Ƶ�2
        /// </summary>
        public Point ControlPoint2
        {
            get { return (Point)this.GetValue(ControlPoint2Property); }
            set { this.SetValue(ControlPoint2Property, value); }
        }

        /// <summary>
        /// ������
        /// </summary>
        public Point EndPoint
        {
            get { return (Point)this.GetValue(EndPointProperty); }
            set { this.SetValue(EndPointProperty, value); }
        }

        #endregion Properties

        #region Protected Methods

        /// <summary>
        /// ���Figure
        /// </summary>
        /// <returns>PathSegment����</returns>
        protected override PathSegmentCollection FillFigure()
        {
            bezierSegment.Point1 = ControlPoint1;
            bezierSegment.Point2 = ControlPoint2;
            bezierSegment.Point3 = EndPoint;

            return new PathSegmentCollection
            {
                bezierSegment
            };
        }

        /// <summary>
        /// ��ȡ��ʼ��ͷ���Ľ�����
        /// </summary>
        /// <returns>��ʼ��ͷ���Ľ�����</returns>
        protected override Point GetStartArrowEndPoint()
        {
            return ControlPoint1;
        }

        /// <summary>
        /// ��ȡ������ͷ���Ŀ�ʼ��
        /// </summary>
        /// <returns>������ͷ���Ŀ�ʼ��</returns>
        protected override Point GetEndArrowStartPoint()
        {
            return ControlPoint2;
        }

        /// <summary>
        /// ��ȡ������ͷ���Ľ�����
        /// </summary>
        /// <returns>������ͷ���Ľ�����</returns>
        protected override Point GetEndArrowEndPoint()
        {
            return EndPoint;
        }

        #endregion  Protected Methods
    }
}
