﻿#pragma checksum "..\..\MainAdmin.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DB3970F841150D4BED8E348E3377EEF493CA46F740998C2DC5C370F421E9E8D0"
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
    /// MainAdmin
    /// </summary>
    public partial class MainAdmin : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\MainAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblNeededSubmit;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\MainAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblIsEnough;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\MainAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem tabMaintain;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\MainAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnQTTMaintain;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\MainAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnKCMaintain;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\MainAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCTQTTMaintain;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\MainAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnKHMaintain;
        
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
            System.Uri resourceLocater = new System.Uri("/QLQTT;component/mainadmin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainAdmin.xaml"
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
            this.lblNeededSubmit = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.lblIsEnough = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.tabMaintain = ((System.Windows.Controls.TabItem)(target));
            return;
            case 4:
            this.btnQTTMaintain = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\MainAdmin.xaml"
            this.btnQTTMaintain.Click += new System.Windows.RoutedEventHandler(this.btnQTTMaintain_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnKCMaintain = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\MainAdmin.xaml"
            this.btnKCMaintain.Click += new System.Windows.RoutedEventHandler(this.btnKCMaintain_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnCTQTTMaintain = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\MainAdmin.xaml"
            this.btnCTQTTMaintain.Click += new System.Windows.RoutedEventHandler(this.btnCTQTTMaintain_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnKHMaintain = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\MainAdmin.xaml"
            this.btnKHMaintain.Click += new System.Windows.RoutedEventHandler(this.btnKHMaintain_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

