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
using DecisionTree;

namespace testDecisionTree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            test();
        }

        public void test()
        {
            DecisionNode root = new DecisionNode();
            DecisionNode decision0 = new DecisionNode(int.Parse(Val.Text), x => new DecisionHolder(DecisionType.Boolean, (int)x < int.Parse(Low.Text)), x => Result.Text = Low.Text);
            DecisionNode decision1 = new DecisionNode(int.Parse(Val.Text), x => new DecisionHolder(DecisionType.Boolean, (int)x > int.Parse(Up.Text)), x => Result.Text = Up.Text);
            DecisionNode decision2 = new DecisionNode(int.Parse(Val.Text), x => new DecisionHolder(DecisionType.Boolean, (int)x > int.Parse(Low.Text) && (int)x < int.Parse(Up.Text)), x => Result.Text = Val.Text);
            root.Children.Add(decision0);
            root.Children.Add(decision1);
            root.Children.Add(decision2);
            DecisionNode extraLayer = new DecisionNode(checkB.IsChecked, x => new DecisionHolder(DecisionType.Boolean, (bool)x == true), x => Result.Text = Val.Text);
            decision0.Children.Add(extraLayer);
            decision1.Children.Add(extraLayer);
            decision2.Children.Add(extraLayer);
            root.Decider = x => new DecisionHolder(DecisionType.FirstTrue, root.Children.IndexOf(root.Children.First(y => y.Decider(y.Input).isValid == true)));
            root.Compute(null);
        }
    }
}
