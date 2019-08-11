using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using E7Bot.Annotations;

namespace E7Bot
{
    [Serializable]
    public class Node
    {
        public bool active;
        public int id { get; set; }

        public bool isLeftChild { get; set; }

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

        public Node(string name)
        {
            active = false;
            this.name = name;
            isLeftChild = false;
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
        public int lastAsgId;

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
                {
                    c.active = false;
                    return c.left;
                }
            }

            if (c.right != null)
            {
                if (c.right.active)
                {
                    c.active = false;
                    return c.right;
                }
                 
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

        public void resetToRoot()
        {
            c = root;
        }

        public bool deleteNode(int id)
        {
            getNodeById(id);
            Node nToDlt = tempFind;
            Node parent = nToDlt.parent;
            if (nToDlt == root)
            {
                root = null;
                lastId = 0;
                avaiId.Clear();
                return true;
            }

            if (nToDlt != null)
            {
                if (nToDlt.left != null)
                {
                    if (nToDlt.left != root)
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

            return false;
        }

        public Node getNodeById(int id)
        {
            tempFind = inOrder(root, id);
            return tempFind;
        }

        public Node getNodeByName(string name)
        {
            tempFind = inOrder(root, name);
            return tempFind;
        }

        private Node inOrder([CanBeNull] Node node, int id, Node find = null)
        {
            if (find == null)
            {
                if (node != null)
                {
                    if (node.left != root)
                    {
                        find = inOrder(node?.left, id);
                    }

                    if (id.Equals(node?.id))
                    {
                        return node;
                    }

                    if (node.right != root)
                    {
                        find = inOrder(node?.right, id);
                    }
                }
            }

            return find;
        }

        private Node inOrder([CanBeNull] Node node, string name, Node find = null)
        {
            if (find == null)
            {
                if (node != null)
                {
                    if (node.left != root)
                    {
                        find = inOrder(node?.left, name);
                    }

                    if (name.Equals(node?.name))
                    {
                        return node;
                    }

                    if (node.right != root && find != null)
                    {
                        find = inOrder(node?.right, name, find);
                    }
                }
            }

            return find;
        }

        public int getAvaiId()
        {
            if (avaiId.Count == 0)
            {
                lastAsgId = lastId;
                return lastId++;
            }

            lastId = avaiId.Last();
            lastAsgId = lastId;
            avaiId.Remove(lastId);
            return lastId;
        }

        public void Insert(int idAt, string name, bool right = false)
        {
            Node newNode = new Node(name);
            Node insertedNode = getNodeById(idAt);
            newNode.id = getAvaiId();

            newNode.parent = insertedNode;
            newNode.left = root;
            newNode.right = root;
            if (!right)
            {
                if (insertedNode.left != root)
                {
                    newNode.left = insertedNode.left;
                    insertedNode.left.parent = newNode;
                }

                insertedNode.left = newNode;
            }
            else
            {
                if (insertedNode.right != root)
                {
                    if (insertedNode.right != null)
                    {
                        newNode.right = insertedNode.right;
                        insertedNode.right.parent = newNode;
                    }
                }

                insertedNode.right = newNode;
            }
        }

        public void Insert(string name, bool right = false)
        {
            Node newNode = new Node(name);

            newNode.id = getAvaiId();

            if (root == null)
            {
                root = newNode;
                c = root;
                root.left = root;
                root.right = root;
            }
            else
            {
                Node current = root;
                Node parent;
                while (true)
                {
                    parent = current;
                    if (!right)
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