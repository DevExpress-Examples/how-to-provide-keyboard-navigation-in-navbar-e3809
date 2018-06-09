using DevExpress.Data.Extensions;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.NavBar;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace NavBarAttachedProperty {
    public class MyConverter: IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if ((bool)value)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }

    public class NavBarKeyboardHelper : Behavior<NavBarControl> {
        public NavBarControl NavBar { get { return AssociatedObject; } }

        protected override void OnAttached() {
            base.OnAttached();
            NavBar.FocusVisualStyle = null;
            NavBar.Focusable = true;
            NavBar.PreviewKeyDown += OnPreviewKeyDown;
            NavBar.Loaded += OnLoaded;
            ThemeManager.AddThemeChangedHandler(NavBar, OnThemeChanged);
            UpdateKeyboardProperties();
            NavBar.Focus();
            NavBar.PreviewMouseDown += NavBar_PreviewMouseDown;
        }

        void NavBar_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            NavBar.Focus();
        }

       
        

        protected override void OnDetaching() {
            base.OnDetaching();
            NavBar.PreviewKeyDown -= OnPreviewKeyDown;
            NavBar.PreviewKeyDown -= OnPreviewKeyDown;
            NavBar.Loaded -= OnLoaded;
        }

        void OnThemeChanged(DependencyObject sender, ThemeChangedRoutedEventArgs e) {
            UpdateKeyboardProperties();
        }

        void OnLoaded(object sender, RoutedEventArgs e) {
            UpdateKeyboardProperties();
        }
        void UpdateKeyboardProperties() {
            LayoutHelper.ForEachElement(NavBar, fe => SetFocusableToFalse(fe));
        }
        void SetFocusableToFalse(FrameworkElement fe) {
            if (fe is NavBarControl) {
                KeyboardNavigation.SetTabNavigation(fe, KeyboardNavigationMode.Continue);
                return;
            }
            fe.Focusable = false;
            KeyboardNavigation.SetIsTabStop(fe, false);
            KeyboardNavigation.SetTabNavigation(fe, KeyboardNavigationMode.None);
        }

        void OnPreviewKeyDown(object sender, KeyEventArgs e) {
            e.Handled = OnPreviewKeyDownImpl(e);
        }

        private bool OnPreviewKeyDownImpl(KeyEventArgs e) {
            if (e.Key == Key.Down) {
                return NextItem();
            }
            if (e.Key == Key.Up) {
                return PrevItem();
            }
            if (e.Key == Key.Left) {
                NavBarGroup selectedGroup = NavBar.SelectedGroup as NavBarGroup;
                selectedGroup.IsExpanded = false;
                return true;
            }
            if (e.Key == Key.Right) {
                NavBarGroup selectedGroup = NavBar.SelectedGroup as NavBarGroup;
                selectedGroup.IsExpanded = true;
                return true;
            }
            if (e.Key == Key.Tab && !IsShiftPressed(e)) {
                NavBarGroup currentGroup = NavBar.SelectedGroup as NavBarGroup;
                var currentGroupIndex = NavBar.Groups.IndexOf(currentGroup);
                if (currentGroupIndex < NavBar.Groups.Count - 1) {
                    NavBar.SelectedGroup = NavBar.Groups[currentGroupIndex + 1];

                } else {
                    NavBar.SelectedGroup = NavBar.Groups[0];
                }
                currentGroup = NavBar.SelectedGroup as NavBarGroup;
                if (currentGroup.Items.Count > 0)
                    NavBar.SelectedItem = currentGroup.Items[0];
                return true;
            }
            if (e.Key == Key.Tab && IsShiftPressed(e)) {
                NavBarGroup currentGroup = NavBar.SelectedGroup as NavBarGroup;
                var ind = NavBar.Groups.IndexOf(currentGroup);
                if (ind > 0) {
                    NavBar.SelectedGroup = NavBar.Groups[ind - 1];
                } else {
                    NavBar.SelectedGroup = NavBar.Groups[NavBar.Groups.Count - 1];
                }
                currentGroup = NavBar.SelectedGroup as NavBarGroup;
                if (currentGroup.Items.Count>0)
                NavBar.SelectedItem = currentGroup.Items[0];
                return true;
            }
            return false;
        }

        bool PrevGroup() {
            if (NavBar.ActiveGroup == null)
                return false;
            var index = NavBar.Groups.IndexOf(NavBar.ActiveGroup);
            if (index < 1) return false;
            NavBar.SelectedGroup = NavBar.Groups[index - 1];
            return PrevItem();
        }
        bool NextGroup() {
            if (NavBar.ActiveGroup == null)
                return false;
            var index = NavBar.Groups.IndexOf(NavBar.ActiveGroup);
            if (index >= NavBar.Groups.Count - 1) return false;
            NavBar.SelectedGroup = NavBar.Groups[index + 1];
            return NextItem();
        }
        bool PrevItem() {
            if (NavBar.ActiveGroup == null)
                return false;
            var newIndex = NavBar.ActiveGroup.SelectedItemIndex - 1;
            if (newIndex == -2)
                newIndex = NavBar.ActiveGroup.Items.Count - 1;
            if (!NavBar.ActiveGroup.Items.IsValidIndex(newIndex))
                return PrevGroup();
            NavBar.ActiveGroup.SelectedItemIndex = newIndex;
            return true;
        }
        bool NextItem() {
            if (NavBar.ActiveGroup == null)
                return false;
            var newIndex = NavBar.ActiveGroup.SelectedItemIndex + 1;
            if (!NavBar.ActiveGroup.Items.IsValidIndex(newIndex))
                return NextGroup();
            NavBar.ActiveGroup.SelectedItemIndex = newIndex;
            return true;
        }

        bool IsShiftPressed(KeyEventArgs e) {
            return ModifierKeysHelper.IsShiftPressed(ModifierKeysHelper.GetKeyboardModifiers(e));
        }
        bool IsCtrlPressed(KeyEventArgs e) {
            return ModifierKeysHelper.IsShiftPressed(ModifierKeysHelper.GetKeyboardModifiers(e));
        }
    }
}