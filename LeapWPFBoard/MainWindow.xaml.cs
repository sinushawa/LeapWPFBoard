using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Ink;
using Leap;

namespace LeapWPFBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LeapWPFBoardWin : Window
    {
        public int boardWidth;
        public int boardHeight;
        public Controller cntrl;
        public LeapListener listener;
        DrawingAttributes touchIndicator = new DrawingAttributes();
        

        public static readonly DependencyProperty leapX = DependencyProperty.Register("LeapX", typeof(double), typeof(LeapWPFBoardWin), new FrameworkPropertyMetadata(default(double)));
        public double LeapX
        {
            get { return (double)GetValue(leapX); }
            set { SetValue(leapX, value); }
        }
        public static readonly DependencyProperty leapY = DependencyProperty.Register("LeapY", typeof(double), typeof(LeapWPFBoardWin), new FrameworkPropertyMetadata(default(double)));
        public double LeapY
        {
            get { return (double)GetValue(leapY); }
            set { SetValue(leapY, value); }
        }

        public LeapWPFBoardWin()
        {
            InitializeComponent();

            boardWidth = (int)this.Width;
            boardHeight = (int)this.Height;

            touchIndicator.Width = 20;
            touchIndicator.Height = 20;
            touchIndicator.StylusTip = StylusTip.Ellipse;

            cntrl = new Controller();
            listener = new LeapListener();
            cntrl.AddListener(listener);
            listener.FrameFired += new TouchFired(onFrameFired);
            listener.ClickedFired += new EventHandler(onClickFired);
        }

        private void onFrameFired(PointableTracker _tracker)
        {
            this.Dispatcher.BeginInvoke(new TouchFired(DispatchMove), DispatcherPriority.ApplicationIdle, _tracker);
        }
        private void DispatchMove(PointableTracker _tracker)
        {
            paintCanvas.Strokes.Clear();
            float tx = _tracker.vector.x * boardWidth;
            float ty = boardHeight - _tracker.vector.y * boardHeight;
            StylusPoint touchPoint = new StylusPoint(tx, ty);
            touchIndicator.Color = _tracker.color;
            StylusPointCollection tips = new StylusPointCollection();
            tips.Add(touchPoint);
            Stroke touchStroke = new Stroke(tips, touchIndicator);
            paintCanvas.Strokes.Add(touchStroke);
            
        }
        private void onClickFired(object sender, EventArgs args)
        {
        }
        private void onClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cntrl.RemoveListener(listener);
            cntrl.Dispose();
        }
    }
    public static class extensions
    {
        public static double Boxed(this double num, double lower, double upper)
        {
            double result;
            if (num > lower && num < upper)
            {
                result = num;
            }
            else if (num <= lower)
            {
                result = lower;
            }
            else
            {
                result = upper;
            }
            return result;
        }
    }
}
