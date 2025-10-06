using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace LargeViewRenderError;

public class SelectionBoxAdorner : Adorner
{
    private readonly Pen _border;

    private Point _end;

    private Point _start;

    public Brush BorderBrush { get; set; } = Brushes.DodgerBlue;

    public Brush Fill { get; set; }

    public SelectionBoxAdorner(UIElement adornedElement, Point start) : base(adornedElement)
    {
        BorderBrush = Brushes.DodgerBlue;
        BorderBrush.Freeze();
        Fill = new SolidColorBrush(Color.FromArgb(50, 0, 120, 215));
        Fill.Freeze();
        _border = new(BorderBrush, 1);

        _start = start;
        _end = start;
        IsHitTestVisible = false;

        // 测试
        UseLayoutRounding = false;
        SnapsToDevicePixels = true;
    }

    public Rect GetSelectionRect()
    {
        return new Rect(_start, _end);
    }

    public void UpdateEndPoint(Point end)
    {
        _end = end;
        _end.X = Math.Max(_end.X, 0);
        _end.Y = Math.Max(_end.Y, 0);
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext dc)
    {
        var rect = GetSelectionRect();
        dc.DrawRectangle(Fill, _border, rect);
    }
}
