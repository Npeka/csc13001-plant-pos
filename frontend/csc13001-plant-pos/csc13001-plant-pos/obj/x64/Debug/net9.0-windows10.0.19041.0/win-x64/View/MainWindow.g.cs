﻿#pragma checksum "D:\Users\Admin\source\repos\csc13001-plant-pos\csc13001-plant-pos\View\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "450AE1201030E38AA90197A3C85D0C1DA25A7EEA7B4C12486BD097D899EAE9FF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace csc13001_plant_pos
{
    partial class MainWindow : 
        global::Microsoft.UI.Xaml.Window, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2503")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // View\MainWindow.xaml line 2
                {
                    global::Microsoft.UI.Xaml.Window element1 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Window>(target);
                    ((global::Microsoft.UI.Xaml.Window)element1).Activated += this.Window_Activated;
                }
                break;
            case 2: // View\MainWindow.xaml line 12
                {
                    this.mainFrame = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Frame>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }


        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2503")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

