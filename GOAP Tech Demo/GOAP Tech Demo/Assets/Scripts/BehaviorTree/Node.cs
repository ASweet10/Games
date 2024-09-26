using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree{
    public enum NodeState { RUNNING, SUCCESS, FAILURE }
    public class Node 
    {
        protected NodeState state;
        public Node parent;

        //Data shared between nodes; object can store any type
        private Dictionary<string, object> dataContext = new Dictionary<string, object>();
        protected List<Node> children = new List<Node>();

        public Node() { //Default constructor; Parent is null
            parent = null; 
        } 

        public Node(List<Node> children){
            foreach (Node child in children){
                Attach(child);
            }
        }

        public void Attach(Node node){  //Create edge between this node and new child
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value){
            dataContext[key] = value;
        }

        public object GetData(string key){
            object val = null;

            //If object found...
            if(dataContext.TryGetValue(key, out val)){
                return val;
            }

            Node node = parent;
            if(node != null){ //Recursively call GetData until out of nodes
                val = node.GetData(key);
            }

            return val;
        }

        public bool ClearData(string key){
            bool cleared = false;

            //If object found...
            if(dataContext.ContainsKey(key)){
                dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            if(node != null){ //Recursively call ClearData until out of nodes
                cleared = node.ClearData(key);
            }
            return cleared;
        }
    }
}

