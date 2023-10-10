using System.Collections;
using System.Collections.Generic;

//Namespace differentiates classes/functions with same name from
// different libraries
namespace BehaviorTree{

    public enum NodeState
    {
        RUNNING, SUCCESS, FAILURE
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        //Empty constructor, parent node is null
        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                Attach(child);
            }
        }

        //Create edge between node and its new child
        private void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        //Virtual so every derived node class can implement its own
        // evaluation function, and have unique role in tree
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        //Shared data between nodes
        private Dictionary<string, object> dataContext = new Dictionary<string, object>();

        //Set data by adding key to dict
        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }

        //Check to see if data is defined somewhere in branch,
        //  not just at this node
        //-Recursively work up the branch until:
        //--Find key we're looking for
        // or
        //--Reach root of tree
        public object GetData(string key)
        {
            object value = null;
            if(dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            //If key found in data...
            if(dataContext.ContainsKey(key))
            {
                //Remove key from dict, return
                dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while(node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared){
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }
}

