using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Molecula.Workflows.Designer.Converter
{
    public class NodeLinkContentConverter : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new NodeLinkContentConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            values = EnsureSize(values);
            var startNodeRectangle = GetRectangle(values[0], values[1], values[4], values[5]);
            var endNodeRectangle = GetRectangle(values[2], values[3], values[6], values[7]);
            var startPoint = CalcStartPoint(startNodeRectangle);
            var endPoint = CalcEndPoint(endNodeRectangle);
            var pathPoints = CalcPathPoints(startPoint, endPoint);
            var figure = new PathFigure(startPoint, new[] { new PolyBezierSegment(pathPoints, true) }, false);
            return new PathFigureCollection(new[] { figure });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        private static object[] EnsureSize(object[] values)
        {
            var buffer = new object[8];
            values.CopyTo(buffer, 0);
            return buffer;
        }

        private static Rect GetRectangle(object left, object top, object width, object height)
            => new Rect(GetValue(left, 0), GetValue(top, 0), GetValue(width, 1), GetValue(height, 1));

        private static double GetValue(object value, double defaultValue)
            => (value as double?) ?? defaultValue;

        private static Point CalcStartPoint(Rect start)
            => new Point(start.Right, start.Height / 2 + start.Top);

        private static Point CalcEndPoint(Rect end)
            => new Point(end.Left, end.Height / 2 + end.Top);

        private static IEnumerable<Point> CalcPathPoints(Point start, Point end)
        {
            var diff = end.X - start.X;
            diff = Math.Min((diff > 0) ? diff * 0.5 : -diff * 0.9, 100);
            return new[]
            {
                new Point(start.X + diff, start.Y),
                new Point(end.X - diff, end.Y),
                end,
            };
        }
    }
}
