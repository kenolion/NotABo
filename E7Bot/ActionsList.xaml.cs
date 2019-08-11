using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace E7Bot
{
    public partial class ActionsList : Window
    {
        private Node n;

        public ObservableCollection<Action> actions { get; set; }
        
        public ObservableCollection<Action> rActions { get; set; }
        public ActionsList(Node node)
        {
            n = node;
            actions = new ObservableCollection<Action>();
            rActions = new ObservableCollection<Action>();
            InitializeComponent();
            nodeListView.ItemsSource = actions;
            rightNodeListView.ItemsSource = rActions;
            nameBox.Text = n.name;
            if (n.left != null)
                nxtNodeLeft.Content = n.left.name;
            if (n.right != null)
                nxtNodeRight.Content = n.right.name;
            n.lActions = node.lActions;
            n.rActions = node.rActions;
            if (n.lActions != null)
            {
                foreach (var act in n.lActions)
                {
                    actions.Add(act);
                }
            }
            if (n.rActions != null)
            {
                foreach (var act in n.rActions)
                {
                    rActions.Add(act);
                }
            }
            prev.Content = n.parent?.name ?? "No Parent";
            getImages(null,null);
        }


        private void deleteImage(object sender, RoutedEventArgs e)
        {
            Button btn = (Button) sender;
            string temp = btn.Tag.ToString();
            if (temp == "1")
            {
                n.rActions.Remove((Action) btn.DataContext);
                rActions.Remove((Action) btn.DataContext);
            }
            else
            {
                n.lActions.Remove((Action)btn.DataContext);
                actions.Remove((Action) btn.DataContext);
            }
         
        }
        
        public void getImages(object sender, RoutedEventArgs e)
        {
            Config.getImages();
            ComboBox1.Items.Clear();
            for (int j = 0; j < Config.fileList.Count; j++)
            {
                ComboBox1.Items.Add(Config.fileList[j]);
            }
        }

        private void addImageOnClick(object sender, RoutedEventArgs e)
        {
            Button test = (Button)sender;
           string temp = test.Tag.ToString();
            
            if (n.lActions == null)
            {
                n.lActions = new List<Action>();
            } 
            if (n.rActions == null)
            {
                n.rActions = new List<Action>();
            }

            if (temp == "0" && !String.IsNullOrEmpty(ComboBox1.SelectionBoxItem.ToString()))
            {
                actions.Add(new Action(ComboBox1.SelectionBoxItem.ToString(), true));
                n.lActions?.Add(actions.Last());
            }
            if (temp == "1" && !String.IsNullOrEmpty(ComboBox1.SelectionBoxItem.ToString()))
            {
                rActions.Add(new Action(ComboBox1.SelectionBoxItem.ToString(), true));
                n.rActions?.Add(rActions.Last());
            }
        
        }

        private void Update_Node_OnClick(object sender, RoutedEventArgs e)
        {
            n.name = nameBox.Text;
        }
        
    }
}