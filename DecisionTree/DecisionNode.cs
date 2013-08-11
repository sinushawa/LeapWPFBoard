using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public enum DecisionType
    {
        None,
        Boolean,
        FirstTrue,
        AnyTrue
    }

    public struct DecisionHolder
    {
        public DecisionType decisionType;
        public bool isValid;
        public int choice;
        public List<int> choicesList;

        public DecisionHolder(DecisionType _decisionType, bool _isValid) : this(_decisionType, _isValid, -1, new List<int>())
        {
        }
        public DecisionHolder(DecisionType _decisionType, int _choice) : this(_decisionType, false, _choice, new List<int>())
        {
        }
        public DecisionHolder(DecisionType _decisionType, List<int> _choicesList)
            : this(_decisionType, false, -1, _choicesList)
        {
        }
        public DecisionHolder(DecisionType _decisionType, bool _isValid, int _choice, List<int> _choicesList)
        {
            decisionType = _decisionType;
            isValid = _isValid;
            choice = _choice;
            choicesList = _choicesList;
        }
    }

    public delegate DecisionHolder DeciderDelegate(object args);
    public delegate void ActionerDelegate(object args);

    public class DecisionNode
    {
        private DecisionNode parent;
        public DecisionNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private List<DecisionNode> children = new List<DecisionNode>();
        public List<DecisionNode> Children
        {
            get { return children; }
            set { children = value; }
        }

        private Object input;
        public Object Input
        {
            get { return input; }
            set { input = value; }
        }

        private Object actionArgs;
        public Object ActionArgs
        {
            get { return actionArgs; }
            set { actionArgs = value; }
        }

        private List<int> link;
        public List<int> Link
        {
            get { return link; }
            set { link = value; }
        }

        private DeciderDelegate decider;
        public DeciderDelegate Decider
        {
            get { return decider; }
            set { decider = value; }
        }

        private ActionerDelegate actioner;
        public ActionerDelegate Actioner
        {
            get { return actioner; }
            set { actioner = value; }
        }

        public DecisionNode()
        {
        }
        public DecisionNode(object _input, DeciderDelegate _decider, ActionerDelegate _actioner)
        {
            Input = _input;
            Decider = _decider;
            Actioner = _actioner;
        }

        public void Compute(object args)
        {
            if (args!=null)
            {
                ActionArgs = args;
            }
            if (children != null)
            {
                foreach (DecisionNode node in children)
                {
                    if (node.Decider.Invoke(node.Input).isValid)
                    {
                        node.Compute(args);
                    }
                }
            }
            if (actioner != null && children.Count == 0)
            {
                actioner.Invoke(actionArgs);
            }
        }
    }
}
