using System.Collections.Generic;
using System.Linq;
using E7Bot.Annotations;

namespace E7Bot
{
    public class Node
    {
        public bool active;
        public int id;
        public Action action;
        public List<Action> actions;
        public Node parent;
        public Node left;
        public Node right;

        public Node()
        {
            active = false;
            actions = new List<Action>();
        }

        public void deleteActionAt(string name)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].name == name)
                {
                    actions.RemoveAt(i);
                }
                
            }
        }
    }

    public class Tree
    {
        public Node root;
        public Node c;
        public List<int> avaiId;
        public int lastId;

        public Tree()
        {
            root = null;
            lastId = 0;
        }

        public Node ReturnRoot()
        {
            return root;
        }

        public Node getNext()
        {
            if (c.left != null)
            {
                if (c.left.active)
                    return c.left;
            }

            if (c.right != null)
            {
                if (c.right.active)
                    return c.right;
            }

            return c;
        }

        public void run()
        {
            if (c.left != null)
            {
                c.left.active = c.left.action.run();
            }

            c = getNext();
        }

        public void deleteNode(int id)
        {
            Node nToDlt = inOrder(root, id);
            Node parent = nToDlt.parent;
            if (nToDlt != null)
            {
                if (nToDlt.left != null)
                {
                    parent.left = nToDlt.left;
                }
                else if (nToDlt.right != null)
                {
                    parent.right = nToDlt.right;
                }
            }
        }

        public Node inOrder([CanBeNull] Node node, int id)
        {
            inOrder(node?.left, id);
            if (id.Equals(node?.id))
            {
                return node;
            }

            inOrder(node?.right, id);
            return null;
        }

        public void Insert(bool left = true)
        {
            Node newNode = new Node();
            if (avaiId.Count == 0)
            {
                root.id = lastId++;
            }
            else
            {
                root.id = avaiId.Last();
            }

            if (root == null)
            {
                root = newNode;
                c = root;
            }
            else
            {
                Node current = root;
                Node parent;
                while (true)
                {
                    parent = current;
                    newNode.parent = parent;
                    if (left)
                    {
                        current = current.left;
                        if (current == null)
                        {
                            parent.left = newNode;
                            return;
                        }
                    }
                    else
                    {
                        current = current.right;
                        if (current == null)
                        {
                            parent.right = newNode;
                            return;
                        }
                    }
                }
            }
        }
    }