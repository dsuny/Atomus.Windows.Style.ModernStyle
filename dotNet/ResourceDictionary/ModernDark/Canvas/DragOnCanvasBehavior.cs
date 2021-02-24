using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Atomus.Windows.Style
{
    public class DragOnCanvasBehavior : Behavior<UIElement>
    {
        public UIElement BaseUIElement
        {
            get { return (UIElement)GetValue(BaseUIElementProperty); }
            set { SetValue(BaseUIElementProperty, value); }
        }
        public static readonly DependencyProperty BaseUIElementProperty =
            DependencyProperty.Register("BaseUIElement", typeof(UIElement), typeof(DragOnCanvasBehavior), new PropertyMetadata(null));

        public UIElement Target
        {
            get { return (UIElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(UIElement), typeof(DragOnCanvasBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            this.AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            base.OnDetaching();
        }

        private Point _initMousePosition;
        private Point _initItemPosition;
        private bool _onDrag = false;

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this._onDrag == false)
            {
                UIElement captureBase = (this.BaseUIElement ?? this.AssociatedObject);
                captureBase.MouseMove += OnMouseMove;
                captureBase.MouseLeftButtonUp += OnMouseLeftButtonUp;

                UIElement target = (this.Target ?? this.AssociatedObject);
                this._onDrag = true;

                this._initMousePosition = e.GetPosition(this.BaseUIElement);

                dynamic data = (target as ContentControl).DataContext;
                if (!ReferenceEquals(null, data))
                {
                    this._initItemPosition = new Point(data.X, data.Y);
                }

                captureBase.CaptureMouse();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && this._onDrag == true)
            {
                UIElement captureBase = (this.BaseUIElement ?? this.AssociatedObject);
                UIElement target = (this.Target ?? this.AssociatedObject);
                Vector delta = e.GetPosition(captureBase) - this._initMousePosition;

                dynamic data = (target as ContentControl).DataContext;
                if (!ReferenceEquals(null, data))
                {
                    data.X = (int)(this._initItemPosition.X + delta.X);
                    data.Y = (int)(this._initItemPosition.Y + delta.Y);
                }
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._onDrag = false;
            UIElement captureBase = (this.BaseUIElement ?? this.AssociatedObject);
            captureBase.ReleaseMouseCapture();
            captureBase.MouseMove -= OnMouseMove;
            captureBase.MouseLeftButtonUp -= OnMouseLeftButtonUp;
        }
    }

    //public class DragOnCanvasBehavior : Behavior<UIElement>
    //{
    //    public UIElement BaseUIElement
    //    {
    //        get { return (UIElement)GetValue(BaseUIElementProperty); }
    //        set { SetValue(BaseUIElementProperty, value); }
    //    }
    //    public static readonly DependencyProperty BaseUIElementProperty =
    //        DependencyProperty.Register("BaseUIElement", typeof(UIElement), typeof(DragOnCanvasBehavior), new PropertyMetadata(null));

    //    public UIElement Target
    //    {
    //        get { return (UIElement)GetValue(TargetProperty); }
    //        set { SetValue(TargetProperty, value); }
    //    }
    //    public static readonly DependencyProperty TargetProperty =
    //        DependencyProperty.Register("Target", typeof(UIElement), typeof(DragOnCanvasBehavior), new PropertyMetadata(null));

    //    protected override void OnAttached()
    //    {
    //        this.AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
    //        base.OnAttached();
    //    }

    //    protected override void OnDetaching()
    //    {
    //        this.AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
    //        base.OnDetaching();
    //    }

    //    private Point _initMousePosition;
    //    private Point _initItemPosition;
    //    private bool _onDrag = false;

    //    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    //    {
    //        if (this._onDrag == false)
    //        {
    //            UIElement captureBase = (this.BaseUIElement ?? this.AssociatedObject);
    //            captureBase.MouseMove += OnMouseMove;
    //            captureBase.MouseLeftButtonUp += OnMouseLeftButtonUp;

    //            UIElement target = (this.Target ?? this.AssociatedObject);
    //            this._onDrag = true;
    //            this._initMousePosition = e.GetPosition(captureBase);

    //            double x = Canvas.GetLeft(target);
    //            double y = Canvas.GetTop(target);
    //            x = double.IsNaN(x) ? 0 : x;
    //            y = double.IsNaN(y) ? 0 : y;
    //            this._initItemPosition = new Point(x, y);

    //            captureBase.CaptureMouse();
    //        }
    //    }

    //    private void OnMouseMove(object sender, MouseEventArgs e)
    //    {
    //        if (e.LeftButton == MouseButtonState.Pressed && this._onDrag == true)
    //        {
    //            UIElement captureBase = (this.BaseUIElement ?? this.AssociatedObject);
    //            UIElement target = (this.Target ?? this.AssociatedObject);
    //            Vector delta = e.GetPosition(captureBase) - this._initMousePosition;
    //            Canvas.SetLeft(target, this._initItemPosition.X + delta.X);
    //            Canvas.SetTop(target, this._initItemPosition.Y + delta.Y);
    //        }
    //    }

    //    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    //    {
    //        this._onDrag = false;
    //        UIElement captureBase = (this.BaseUIElement ?? this.AssociatedObject);
    //        captureBase.ReleaseMouseCapture();
    //        captureBase.MouseMove -= OnMouseMove;
    //        captureBase.MouseLeftButtonUp -= OnMouseLeftButtonUp;
    //    }
    //}
}
