﻿#pragma checksum "..\..\ActionsList.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "99D4E177549237BDAD261CCAE84032683B8933E9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using E7Bot;
using NHotkey.Wpf;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace E7Bot {
    
    
    /// <summary>
    /// ActionsList
    /// </summary>
    public partial class ActionsList : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 24 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox nameBox;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label prev;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label nxtNodeLeft;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label nxtNodeRight;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UpdateNode;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBox1;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button listBtn;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView nodeListView;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView rightNodeListView;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\ActionsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addImage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/E7Bot;component/actionslist.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ActionsList.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.nameBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.prev = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.nxtNodeLeft = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.nxtNodeRight = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.UpdateNode = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\ActionsList.xaml"
            this.UpdateNode.Click += new System.Windows.RoutedEventHandler(this.Update_Node_OnClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ComboBox1 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.listBtn = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\ActionsList.xaml"
            this.listBtn.Click += new System.Windows.RoutedEventHandler(this.getImages);
            
            #line default
            #line hidden
            return;
            case 8:
            this.nodeListView = ((System.Windows.Controls.ListView)(target));
            return;
            case 9:
            
            #line 42 "..\..\ActionsList.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.addImageOnClick);
            
            #line default
            #line hidden
            return;
            case 11:
            this.rightNodeListView = ((System.Windows.Controls.ListView)(target));
            return;
            case 12:
            
            #line 58 "..\..\ActionsList.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.addImageOnClick);
            
            #line default
            #line hidden
            return;
            case 14:
            this.addImage = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\ActionsList.xaml"
            this.addImage.Click += new System.Windows.RoutedEventHandler(this.addImageOnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 10:
            
            #line 45 "..\..\ActionsList.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.deleteImage);
            
            #line default
            #line hidden
            break;
            case 13:
            
            #line 61 "..\..\ActionsList.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.deleteImage);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

