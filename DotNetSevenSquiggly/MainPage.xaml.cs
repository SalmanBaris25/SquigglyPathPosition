using Microsoft.Maui.Controls.Shapes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace DotNetSevenSquiggly
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            AddPath();
        }

        void AddPath()
        {
            Path squigglyPath = CreateSquigglyPath(Colors.Green, 1, new Rect(0, 0, 924.445556640625, 22.079999923706055));
            MainLayout.SetLayoutBounds(squigglyPath, new Rect(0, 0, 924.445556640625, 22.079999923706055));
            MainLayout.Add(squigglyPath);
        }


        Path CreateSquigglyPath(Color color, double opacity, Rect bounds)
        {
            Path squigglyPath = GetSquigglyGeometry(
                new Point(bounds.X, bounds.Y + bounds.Height),
                new Point(bounds.X + bounds.Width, bounds.Y + bounds.Height),
                bounds.Height);
            squigglyPath.Stroke = color.MultiplyAlpha((float)opacity);
#if !WINDOWS && !MACCATALYST
            squigglyPath.StrokeThickness = 0.5;
#endif
            return squigglyPath;
        }

        private Path GetSquigglyGeometry(Point startPoint, Point endPoint, double height)
        {
            double dx = startPoint.X - endPoint.X;
            double dy = startPoint.Y - endPoint.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);
            double x = startPoint.X;
            double y = startPoint.Y;
            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure();
            LineSegment lineSegment;
            pathFigure.StartPoint = startPoint;
            pathFigure.IsClosed = false;
            bool showUnderlineAtStart = false;
            double spacing = height * 0.13;
            Path squigglyPath = new Path();
            List<Point> squigglyPointsCollection = new List<Point>();
            for (double distance = 0; distance + spacing < length; distance += spacing)
            {
                if (showUnderlineAtStart)
                {
                    lineSegment = new LineSegment();
                    lineSegment.Point = new Point(x + distance + spacing, y);
                }
                else
                {
                    lineSegment = new LineSegment();
                    lineSegment.Point = new Point(x + distance + spacing, y - spacing);
                }
                squigglyPointsCollection.Add(new Point(lineSegment.Point.X, lineSegment.Point.Y));
                showUnderlineAtStart = !showUnderlineAtStart;
                pathFigure.Segments.Add(lineSegment);
            }
            pathGeometry.Figures.Add(pathFigure);
            squigglyPath.Data = pathGeometry;
            return squigglyPath;
        }
    }
}