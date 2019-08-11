using System;
using System.Collections.Generic;
using System.Linq;
using E7Bot.Annotations;

namespace E7Bot
{
    [Serializable]
    public class Node
    {
        public bool active;
        public int id { get; set; }

        public bool isLeftChild{ get; set; }
        
        public string name { get; set; }
        public List<Action> lActions;
        public List<Action> rActions;
        public Node parent;
        public Node left;
        public Node right;

        public Node(List<Action> lactions, List<Action> ractions, string name)
        {
            active = false;
            this.name = name;
            this.lActions = lactions?.ToList();
            isLeftChild = false;
            this.rActions = ractions?.ToList();
        }

        public void deleteActionAt(string name)
        {
            for (int i = 0; i < lActions.Count; i++)
            {
                if (lActions[i].name == name)
                {
                    lActions.RemoveAt(i);
                }
            }
        }
    }

    [Serializable]
    public class Tree
    {
        public Node root;
        public Node c;
        public Node tempFind;
        public HashSet<int> avaiId;
        public int lastId;

        public Tree()
        {
            root = null;
            lastId = 0;
            avaiId = new HashSet<int>();
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
            Console.WriteLine(c.name);
            bool goNxt = true;
            for (int i = 0; i < c.lActions.Count; i++)
            {
                goNxt = c.lActions[i].run();
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

            if (c.right != null)
            {
                goNxt = true;
                for (int i = 0; i < c.rActions.Count; i++)
                {
                    goNxt = c.rActions[i].run();
                    if (!goNxt)
                    {
                        break;
                    }
                }


                c.right.active = goNxt;
            }

            c = getNext();
        }

        public void deleteNode(int id)
        {
            inOrder(root, id);
            Node nToDlt = tempFind;
            Node parent = nToDlt.parent;
            if (nToDlt != null)
            {
                if (nToDlt.left != null)
                {
                    if (nToDlt.left != root )
                    {
                        parent.left = nToDlt.left;
                    }

                    if (nToDlt.isLeftChild)
                    {
                        parent.left = null;
                        if (nToDlt.left != null)
                        {
                            parent.left = nToDlt.left;
                        }
                    }
                    /*else
                    {
                        parent.right = null;
                        if (nToDlt.right != null)
                        {
                           
                        }
                    }*/
                }

                nToDlt.right = null;
                if (nToDlt.id == root.id)
                {
                    root = null;
                    nToDlt = null;
                }

                avaiId.Add(id);
            }
        }

        public Node getNodeById(int id)
        {
            inOrder(root, id);
            return tempFind;
        }

        public Node getNodeByName(string name)
        {
            inOrder(root, name);
            return tempFind;
        }

        private void inOrder([CanBeNull] Node node, int id)
        {
            if (node != null)
            {
                if (node.left != root)
                {
                    inOrder(node?.left, id);
                }

                if (id.Equals(node?.id))
                {
                    tempFind = node;
                }

                inOrder(node?.right, id);
            }
        }

        private void inOrder([CanBeNull] Node node, string name)
        {
            if (node != null)
            {
                if (node.left != root)
                {
                    inOrder(node?.left, name);
                }

                if (name.Equals(node?.name))
                {
                    tempFind = node;
                }

                inOrder(node?.right, name);
            }
        }

        public void Insert(List<Action> lActions, List<Action> rActions, string name, bool left = true)
        {
            Node newNode = new Node(lActions, rActions, name);
            if (avaiId.Count == 0)
            {
                newNode.id = lastId++;
            }
            else
            {
                int lastId =avaiId.Last();
                newNode.id = lastId;
                avaiId.Remove(lastId);
            }

            if (root == null)
            {
                root = newNode;
                c = root;
                root.left = root;
            }
            else
            {
                Node current = root;
                Node parent;
                while (true)
                {
                    parent = current;
                    //newNode.parent = parent;
                    //newNode.left = root;
                    if (left)
                    {
                        newNode.isLeftChild = true;

    
                            if (current.left == root)
                            {
                                current.left = newNode;
                                newNode.parent = parent;
                                newNode.left = root;
                                return;
                            }

                            current = current.left;
                    }
                    else
                    {
                        if (current.right == root)
                        {
                            current.right = newNode;
                            newNode.parent = parent;
                            newNode.right = root;
                            return;
                        }
                        current = current.right;
          
                    }
                }
            }
        }
    }
}