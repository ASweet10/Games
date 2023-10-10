using System.Collections.Generic;

namespace BehaviorTree
{
    //Sequence is like an "AND" logic gate
    // This node only succeeds if all children succeed
    public class Sequence : Node
    {
        public Sequence(): base() { }
        public Sequence(List<Node> children) : base(children) { }

        //Iterate through all children
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}