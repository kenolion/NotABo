using System.Collections.Generic;
using System.Linq;
using E7Bot.Annotations;

namespace E7Bot
{
    public class Node
    {
        public bool active;
        public int id;
        public string name;
        public Action action;
        public List<Action> actions;
        public Node parent;
        public Node left;
        public Node right;

        public Node(List<Action> actions, string name)
        {
            active = false;
            this.name = name;
            this.actions = actions.ToList();
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
            avaiId = new List<int>();
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
            bool goNxt = true;
            for (int i = 0; i < c.actions.Count; i++)
            {
                goNxt = c.actions[i].run();
                if (!goNxt)
                {
                    break;
                }
            }

            if (c.left != null)
            {
                c.left.active = goNxt;
                if (!c.left.active)
                {
                    if (c.right != null)
                        c.right.active = true;
                }
            }

            c = getNext();
        }

        public void deleteNode(int id)
        {
            Node nToDlt = inOrder(root, id);
            Node parent = nToDlt.parent;
            if (nToDlt != null)
            {
                if (nToDlt.left != root)
                {
                    if (nToDlt.left != null)
                    {
                        parent.left = nToDlt.left;
                    }
                }

                nToDlt.right = null;

                avaiId.Add(id);
            }
        }

        public Node getNodeById(int id)
        {
            return inOrder(root, id);
        }
        
        public Node getNodeByName(string name)
        {
            return inOrder(root, name);
        }

        private Node inOrder([CanBeNull] Node node, int id)
        {
            inOrder(node?.left, id);
            if (id.Equals(node?.id))
            {
                return node;
            }

            inOrder(node?.right, id);
            return null;
        }
        
        private Node inOrder([CanBeNull] Node node, string name)
        {
            inOrder(node?.left, name);
            if (name.Equals(node?.name))
            {
                return node;
            }

            inOrder(node?.right, name);
            return null;
        }

        public void Insert(List<Action> actions, string name, bool left = true)
        {
            Node newNode = new Node(actions, name);
            if (avaiId.Count == 0)
            {
                newNode.id = lastId++;
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
                            newNode.left = root;
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
}