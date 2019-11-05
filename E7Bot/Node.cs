using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Timers;
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
        public bool click { get; set; }

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
            click = true;
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

        public List<Node> listOfNodes;

        private E7Timer timer;

        public Tree()
        {
            root = null;
            lastId = 0;
            avaiId = new HashSet<int>();
            listOfNodes = new List<Node>();
        }

        public Node ReturnRoot()
        {
            return root;
        }

        public void saveChk()
        {
            timer = null;
        }


        public Node getNext()
        {
            if (c.left != null)
            {
                if (c.left.active)
                {
                    Config.shutDowntime.Start();
                    c.active = false;
                    return c.left;
                }
            }

            if (c.right != null)
            {
                if (c.right.active)
                {
                    Config.shutDowntime.Start();
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
            bool firstFail = false;
            if (timer == null)
            {
                timer = new E7Timer(1000);
                timer.SetFunction(nxtTimer);
            }

            if (!timer.isStart)
            {
                for (int i = 0; i < c.lActions.Count; i++)
                {
                    goNxt = c.lActions[i].run();

                    if (!goNxt && !firstFail)
                    {
                        firstFail = true;
                    }
                }

                if (c.left != null)
                {
                    c.left.active = !firstFail;

                    if (c.right != null && !c.left.active)
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
                }
            }

            if (!goNxt && c.click)
            {
                VirtualMouse.LeftClick();
            }


            if (goNxt)
            {
                if (!timer.isStart)
                    timer.Start();
            }
        }

        public void nxtTimer(Object source, ElapsedEventArgs e)
        {
            c = getNext();
            timer.Stop();
        }

        public void resetToRoot()
        {
            c = root;
        }

        public Node deleteNode(int id)
        {
            Node nToDlt = getNodeById(id);
            Node parent = nToDlt.parent;
            if (listOfNodes == null)
            {
                listOfNodes = new List<Node>();
            }

            listOfNodes?.Remove(nToDlt);

            if (nToDlt == root)
            {
                //root = null;
                lastId = 0;
                avaiId.Clear();
                listOfNodes.Clear();
                return nToDlt;
            }

            if (nToDlt != null)
            {
                if (nToDlt.left != null)
                {
                    if (nToDlt.left != root)
                    {
                        if (nToDlt.isLeftChild)
                        {
                            parent.left = nToDlt.left;
                            nToDlt.left.parent = parent;
                        }
                    }

                    if (!nToDlt.isLeftChild && parent != root)
                    {
                        parent.right = root;
                    }
            
                    if (nToDlt.right != root)
                    {
                        nToDlt.right.parent = parent;
                    }
                }


                findRelatedNodeandDelete(nToDlt);
                // delete right node if not null
                if (!isNull(nToDlt.right))
                {
                    findRelatedNodeandDelete(nToDlt.right);
                }
                
                if (nToDlt.id == root.id)
                {
                    // root = null;
                    lastId = 0;
                    listOfNodes.Clear();
                    avaiId.Clear();
                }

                avaiId.Add(id);
              
            }

            return nToDlt;
        }

        private Boolean isNull(Node che)
        {
            if (che == null || che == root)
            {
                return true;
            }

            return false;
        }
        
        public Boolean findRelatedNodeandDelete(Node nodeToDlt)
        {
            bool exs = false;
            foreach (var node in listOfNodes)
            {
                if (nodeToDlt.parent.id != node.id)
                {
                    if (node.left.id == nodeToDlt.id)
                    {
                        node.left = root;
                        exs = true;
                    }

                    if (node.right.id == nodeToDlt.id)
                    {
                        node.right = root;
                        exs = true;
                    }
                }
            }

            return exs;
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
                        find = inOrder(node?.left, id, find);
                    }

                    if (id.Equals(node?.id))
                    {
                        return node;
                    }

                    if (node.right != root)
                    {
                        find = inOrder(node?.right, id, find);
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
                lastId += 1;
                lastAsgId = lastId;
                return lastId;
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
            if (listOfNodes == null)
            {
                listOfNodes = new List<Node>();
            }

            listOfNodes.Add(newNode);
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

                newNode.isLeftChild = true;

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
            if (listOfNodes == null)
            {
                listOfNodes = new List<Node>();
            }

            listOfNodes.Add(newNode);
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