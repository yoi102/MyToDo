namespace MyToDoLibrary
{
    public enum MyShapeType
    {
        Rectangle,
        Circle,
        Ellipse,

    }


    public class Myshape
    {

        private MyShapeType _ShapeType;

        public MyShapeType ShapeType
        {
            get { return _ShapeType; }
            set { _ShapeType = value; }
        }




    }
}