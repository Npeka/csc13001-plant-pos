﻿#pragma checksum "G:\GithubRepo\csc13001-plant-pos\frontend\csc13001-plant-pos\csc13001-plant-pos\View\Authentication\FormLogin.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DF1E06F0AF491B59B465D6B9E4EB328AF1154D6343ADBD4414836038AB2BF73A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace csc13001_plant_pos.View.Authentication
{
    partial class FormLogin : 
        global::Microsoft.UI.Xaml.Controls.UserControl, 
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
            case 2: // View\Authentication\FormLogin.xaml line 15
                {
                    this.Username = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 3: // View\Authentication\FormLogin.xaml line 25
                {
                    this.PasswordBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.PasswordBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.PasswordBox)this.PasswordBox).PasswordChanged += this.Password_PasswordChanged;
                }
                break;
            case 4: // View\Authentication\FormLogin.xaml line 34
                {
                    this.LoginButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.LoginButton).Click += this.LoginButton_Click;
                }
                break;
            case 5: // View\Authentication\FormLogin.xaml line 55
                {
                    global::Microsoft.UI.Xaml.Documents.Hyperlink element5 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Documents.Hyperlink>(target);
                    ((global::Microsoft.UI.Xaml.Documents.Hyperlink)element5).Click += this.NavigateToForgotPassword_Click;
                }
                break;
            case 6: // View\Authentication\FormLogin.xaml line 43
                {
                    this.ButtonProgressRing = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ProgressRing>(target);
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

