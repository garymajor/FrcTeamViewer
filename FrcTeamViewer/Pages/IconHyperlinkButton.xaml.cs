using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FrcTeamViewer.Pages
{
    public sealed partial class IconHyperlinkButton : UserControl
    {
        /// <summary>
        /// The property for the Icon Text (e.g., "\uE8D4"). The IconHyperlinkButton will display the Segoe MDL2 Asset for that code.
        /// </summary>
        public string IconContent
        {
            get
            {
                return (string)GetValue(IconContentProperty);
            }
            set
            {
                SetValue(IconContentProperty, value);
            }
        }

        /// <summary>
        /// The property for the Button Text.
        /// </summary>
        public string ButtonContent
        {
            get
            {
                return (string)GetValue(ButtonContentProperty);
            }
            set
            {
                SetValue(ButtonContentProperty, value);
            }
        }

        /// <summary>
        /// The property for the Command.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// The property for the CommandParameter.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return (object)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Identified the IconContent dependency property
        /// </summary>
        public static readonly DependencyProperty IconContentProperty =
            DependencyProperty.Register("IconContent", typeof(string), typeof(IconHyperlinkButton), new PropertyMetadata(""));

        /// <summary>
        /// Identified the ButtonContent dependency property
        /// </summary>
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(string), typeof(IconHyperlinkButton), new PropertyMetadata(""));

        /// <summary>
        /// Identified the Command dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(IconHyperlinkButton), new PropertyMetadata(""));

        /// <summary>
        /// Identified the CommandParameter dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(IconHyperlinkButton), new PropertyMetadata(""));

        /// <summary>
        /// Constructor
        /// </summary>
        public IconHyperlinkButton()
        {
            this.InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
