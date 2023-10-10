using System.Collections.Generic;

namespace BehaviorTree
{
    //Selector is like an "OR" logic gate
    // This node only succeeds if all children succeed
    public class Selector : Node
    {
        public Selector(): base() { }
        public Selector(List<Node> children) : base(children) { }

        //Iterate through all children
        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}