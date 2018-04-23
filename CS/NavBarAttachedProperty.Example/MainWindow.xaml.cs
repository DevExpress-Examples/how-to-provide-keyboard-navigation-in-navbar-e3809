using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.NavBar;
using DevExpress.Xpf.Core;

namespace NavBarAttachedProperty.Example {
    public partial class MainWindow: Window {
        public static readonly DependencyProperty ThemesProperty = DependencyProperty.Register("Themes", typeof(List<Theme>), typeof(MainWindow), new UIPropertyMetadata(new List<Theme>()));
        public static readonly DependencyProperty SelectedThemeProperty = DependencyProperty.Register("SelectedTheme", typeof(Theme), typeof(MainWindow), new UIPropertyMetadata(null));

        public List<Theme> Themes {
            get { return (List<Theme>)GetValue(ThemesProperty); }
            set { SetValue(ThemesProperty, value); }
        }
        public Theme SelectedTheme {
            get { return (Theme)GetValue(SelectedThemeProperty); }
            set { SetValue(SelectedThemeProperty, value); }
        }

        public MainWindow() {
            Themes.Add(Theme.DeepBlue);
            Themes.Add(Theme.LightGray);
            Themes.Add(Theme.Office2007Black);
            Themes.Add(Theme.Office2007Blue);
            Themes.Add(Theme.Office2007Silver);
            Themes.Add(Theme.Seven);
            DataContext = this;
            InitializeComponent();
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (((ListBox)sender).SelectedItem.ToString().Contains("ExplorerBarView"))
            navBar.View=new ExplorerBarView();
        if (((ListBox)sender).SelectedItem.ToString().Contains("NavigationPaneView"))
            navBar.View = new NavigationPaneView();
        if (((ListBox)sender).SelectedItem.ToString().Contains("SideBarView"))
            navBar.View = new SideBarView();
        }
    }
}