using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;

namespace Wpf.Resources.MahAppsExtensions
{
    public class LoadingFlyout : Flyout
    {

        public new static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(LoadingFlyout), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsOpenedChanged));

        private bool _showStatus;
        static LoadingFlyout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingFlyout), new FrameworkPropertyMetadata(typeof(LoadingFlyout)));
        }

        public new bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var showStoryboard = (Storyboard)GetTemplateChild("sbShow");

            if (showStoryboard == null) return;
            ApplyAnimation(Position);
        }

        internal void ApplyAnimation(Position position)
        {
            var root = (Grid)GetTemplateChild("root");

            if (root == null)
            {
                return;
            }

            var hideFrame = (EasingDoubleKeyFrame)GetTemplateChild("hideFrame");
            var hideFrameY = (EasingDoubleKeyFrame)GetTemplateChild("hideFrameY");
            var showFrame = (EasingDoubleKeyFrame)GetTemplateChild("showFrame");
            var showFrameY = (EasingDoubleKeyFrame)GetTemplateChild("showFrameY");

            if (hideFrame == null || showFrame == null || hideFrameY == null || showFrameY == null)
            {
                return;
            }

            if (Position == Position.Left || Position == Position.Right)
            {
                showFrame.Value = 0;
            }

            root.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            switch (position)
            {
                default:
                    HorizontalAlignment = HorizontalAlignment.Left;
                    VerticalAlignment = VerticalAlignment.Bottom;
                    hideFrame.Value = -root.DesiredSize.Width;
                    root.RenderTransform = new TranslateTransform(-root.DesiredSize.Width, 0);
                    break;
                case Position.Right:
                    HorizontalAlignment = HorizontalAlignment.Right;
                    VerticalAlignment = VerticalAlignment.Bottom;
                    hideFrame.Value = root.DesiredSize.Width;
                    root.RenderTransform = new TranslateTransform(root.DesiredSize.Width, 0);
                    break;
            }
        }


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (!sizeInfo.WidthChanged && !sizeInfo.HeightChanged) return;

            if (!IsOpen)
            {
                ApplyAnimation(Position);
                return;
            }

            var root = (Grid)GetTemplateChild("root");
            if (root == null) return;

            var hideFrame = (EasingDoubleKeyFrame)GetTemplateChild("hideFrame");
            var hideFrameY = (EasingDoubleKeyFrame)GetTemplateChild("hideFrameY");
            var showFrame = (EasingDoubleKeyFrame)GetTemplateChild("showFrame");
            var showFrameY = (EasingDoubleKeyFrame)GetTemplateChild("showFrameY");

            if (hideFrame == null || showFrame == null || hideFrameY == null || showFrameY == null)
            {
                return;
            }

            if (Position == Position.Left || Position == Position.Right)
            {
                showFrame.Value = 0;
            }

            switch (Position)
            {
                default:
                    hideFrame.Value = -root.DesiredSize.Width;
                    break;
                case Position.Right:
                    hideFrame.Value = root.DesiredSize.Width;
                    break;
            }
        }

        private static void IsOpenedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var loadingFlyout = (LoadingFlyout)dependencyObject;
            loadingFlyout.IsOpenedChanged(e.NewValue);
        }

        private void IsOpenedChanged(object newValue)
        {
            _showStatus = (bool)newValue;
            VisualStateManager.GoToState(this, _showStatus == false ? "Hide" : "Show", true);
        }


    }
}