﻿#pragma checksum "..\..\SelectSizeForChangeAction.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "306E9036D8898A1A4D95F523B050553F5098E99FF659D5F2C2C12055E7818FB7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using QLQTT;
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


namespace QLQTT {
    
    
    /// <summary>
    /// SelectSizeForChangeAction
    /// </summary>
    public partial class SelectSizeForChangeAction : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\SelectSizeForChangeAction.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvwDK;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\SelectSizeForChangeAction.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbbKC;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\SelectSizeForChangeAction.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblOldID;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\SelectSizeForChangeAction.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmit;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\SelectSizeForChangeAction.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Uri resourceLocater = new System.Uri("/QLQTT;component/selectsizeforchangeaction.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SelectSizeForChangeAction.xaml"
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
            
            #line 10 "..\..\SelectSizeForChangeAction.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.start_Load);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lvwDK = ((System.Windows.Controls.ListView)(target));
            
            #line 18 "..\..\SelectSizeForChangeAction.xaml"
            this.lvwDK.Loaded += new System.Windows.RoutedEventHandler(this.lvwDK_Loaded);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cbbKC = ((System.Windows.Controls.ComboBox)(target));
            
            #line 48 "..\..\SelectSizeForChangeAction.xaml"
            this.cbbKC.Loaded += new System.Windows.RoutedEventHandler(this.cbbKC_Loaded);
            
            #line default
            #line hidden
            return;
            case 4:
            this.lblOldID = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.btnSubmit = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\SelectSizeForChangeAction.xaml"
            this.btnSubmit.Click += new System.Windows.RoutedEventHandler(this.btnSubmit_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 69 "..\..\SelectSizeForChangeAction.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

