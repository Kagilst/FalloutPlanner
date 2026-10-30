using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Controls
{
    /// <summary>
    /// Hosts content at a fixed design size and scales it uniformly with the available space.
    /// Pair with Window/Page MinWidth/MinHeight so content never shrinks below a readable size.
    /// </summary>
    public class UniformScaleHost : ContentControl
    {
        static UniformScaleHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UniformScaleHost),
                new FrameworkPropertyMetadata(typeof(UniformScaleHost)));
        }

        public static readonly DependencyProperty DesignWidthProperty =
            DependencyProperty.Register(
                nameof(DesignWidth),
                typeof(double),
                typeof(UniformScaleHost),
                new PropertyMetadata(800.0));

        public static readonly DependencyProperty DesignHeightProperty =
            DependencyProperty.Register(
                nameof(DesignHeight),
                typeof(double),
                typeof(UniformScaleHost),
                new PropertyMetadata(450.0));

        public double DesignWidth
        {
            get => (double)GetValue(DesignWidthProperty);
            set => SetValue(DesignWidthProperty, value);
        }

        public double DesignHeight
        {
            get => (double)GetValue(DesignHeightProperty);
            set => SetValue(DesignHeightProperty, value);
        }
    }
}
