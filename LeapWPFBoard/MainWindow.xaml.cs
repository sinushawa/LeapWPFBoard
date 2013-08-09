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
            cntrl = new Controller();
            listener = new LeapListener();
            cntrl.AddListener(listener);
            listener.FrameFired += new EventHandler(onFrameFired);
            listener.ClickedFired += new EventHandler(onClickFired);
        }
        private void onFrameFired(object sender, EventArgs args)
        {
            this.Dispatcher.BeginInvoke(new Action(this.DispatchMove), DispatcherPriority.ApplicationIdle);
        }
        private void DispatchMove()
        {
            LeapX = listener.XDelta*2*(this.Width);
            LeapX=LeapX.Boxed(-((this.Width-50) / 2.0f), ((this.Width-50) / 2.0f));
            LeapY = listener.YDelta*2*(this.Height);
            LeapY = LeapY.Boxed(-((this.Height-50) / 2.0f), ((this.Height-50) / 2.0f));
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
