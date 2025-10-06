using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace LargeViewRenderError;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private AdornerLayer _adornerLayer = null!;

    private bool _isSelecting;

    private SelectionBoxAdorner? _selectionBoxAdorner;

    private Point _startPoint;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Double.TryParse(tbH.Text, out double power);

        if (power > 100)
        {
            sv.ScrollToHorizontalOffset(power);
        }
        else
        {
            sv.ScrollToHorizontalOffset(Math.Pow(2, power));
        }
    }

    // 鼠标按下事件，开始选区
    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // 只有左键点击时才开始
        if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
        {
            _startPoint = e.GetPosition(canvas);
            _isSelecting = true;

            // 创建 SelectionBoxAdorner 并添加到 AdornerLayer
            _selectionBoxAdorner = new SelectionBoxAdorner(canvas, _startPoint);
            _adornerLayer.Add(_selectionBoxAdorner);
        }
    }

    // 鼠标移动事件，更新选区大小
    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isSelecting && _selectionBoxAdorner != null)
        {
            var currentPoint = e.GetPosition(canvas);
            _selectionBoxAdorner.UpdateEndPoint(currentPoint);
            txtX.Text = $"X: {currentPoint.X}";
            txtY.Text = $"Y: {currentPoint.Y}";
            Debug.WriteLine(">>> " + currentPoint);
        }
    }

    // 鼠标松开事件，结束选区
    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_isSelecting)
        {
            _isSelecting = false;

            if (_selectionBoxAdorner != null)
            {
                _adornerLayer.Remove(_selectionBoxAdorner);
            }
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _adornerLayer = AdornerLayer.GetAdornerLayer(canvas); // 获取 AdornerLayer
    }
}
