Imports DevExpress.Data.Extensions
Imports DevExpress.Xpf.Core
Imports DevExpress.Xpf.Core.Native
Imports DevExpress.Xpf.Editors.Helpers
Imports DevExpress.Xpf.NavBar
Imports System
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data
Imports System.Windows.Input
Imports System.Windows.Interactivity

Namespace NavBarAttachedProperty
    Public Class MyConverter
        Implements IValueConverter

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
            If DirectCast(value, Boolean) Then
                Return Visibility.Visible
            End If
            Return Visibility.Collapsed
        End Function
        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return Nothing
        End Function
    End Class

    Public Class NavBarKeyboardHelper
        Inherits Behavior(Of NavBarControl)

        Public ReadOnly Property NavBar() As NavBarControl
            Get
                Return AssociatedObject
            End Get
        End Property

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            NavBar.FocusVisualStyle = Nothing
            NavBar.Focusable = True
            AddHandler NavBar.PreviewKeyDown, AddressOf OnPreviewKeyDown
            AddHandler NavBar.Loaded, AddressOf OnLoaded
            ThemeManager.AddThemeChangedHandler(NavBar, AddressOf OnThemeChanged)
            UpdateKeyboardProperties()
            NavBar.Focus()
            AddHandler NavBar.PreviewMouseDown, AddressOf NavBar_PreviewMouseDown
        End Sub

        Private Sub NavBar_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            NavBar.Focus()
        End Sub




        Protected Overrides Sub OnDetaching()
            MyBase.OnDetaching()
            RemoveHandler NavBar.PreviewKeyDown, AddressOf OnPreviewKeyDown
            RemoveHandler NavBar.PreviewKeyDown, AddressOf OnPreviewKeyDown
            RemoveHandler NavBar.Loaded, AddressOf OnLoaded
        End Sub

        Private Sub OnThemeChanged(ByVal sender As DependencyObject, ByVal e As ThemeChangedRoutedEventArgs)
            UpdateKeyboardProperties()
        End Sub

        Private Sub OnLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            UpdateKeyboardProperties()
        End Sub
        Private Sub UpdateKeyboardProperties()
            LayoutHelper.ForEachElement(NavBar, Sub(fe) SetFocusableToFalse(fe))
        End Sub
        Private Sub SetFocusableToFalse(ByVal fe As FrameworkElement)
            If TypeOf fe Is NavBarControl Then
                KeyboardNavigation.SetTabNavigation(fe, KeyboardNavigationMode.Continue)
                Return
            End If
            fe.Focusable = False
            KeyboardNavigation.SetIsTabStop(fe, False)
            KeyboardNavigation.SetTabNavigation(fe, KeyboardNavigationMode.None)
        End Sub

        Private Sub OnPreviewKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            e.Handled = OnPreviewKeyDownImpl(e)
        End Sub

        Private Function OnPreviewKeyDownImpl(ByVal e As KeyEventArgs) As Boolean
            If e.Key = Key.Down Then
                Return NextItem()
            End If
            If e.Key = Key.Up Then
                Return PrevItem()
            End If
            If e.Key = Key.Left Then
                Dim selectedGroup As NavBarGroup = TryCast(NavBar.SelectedGroup, NavBarGroup)
                selectedGroup.IsExpanded = False
                Return True
            End If
            If e.Key = Key.Right Then
                Dim selectedGroup As NavBarGroup = TryCast(NavBar.SelectedGroup, NavBarGroup)
                selectedGroup.IsExpanded = True
                Return True
            End If
            If e.Key = Key.Tab AndAlso Not IsShiftPressed(e) Then
                Dim currentGroup As NavBarGroup = TryCast(NavBar.SelectedGroup, NavBarGroup)
                Dim currentGroupIndex = NavBar.Groups.IndexOf(currentGroup)
                If currentGroupIndex < NavBar.Groups.Count - 1 Then
                    NavBar.SelectedGroup = NavBar.Groups(currentGroupIndex + 1)

                Else
                    NavBar.SelectedGroup = NavBar.Groups(0)
                End If
                currentGroup = TryCast(NavBar.SelectedGroup, NavBarGroup)
                If currentGroup.Items.Count > 0 Then
                    NavBar.SelectedItem = currentGroup.Items(0)
                End If
                Return True
            End If
            If e.Key = Key.Tab AndAlso IsShiftPressed(e) Then
                Dim currentGroup As NavBarGroup = TryCast(NavBar.SelectedGroup, NavBarGroup)
                Dim ind = NavBar.Groups.IndexOf(currentGroup)
                If ind > 0 Then
                    NavBar.SelectedGroup = NavBar.Groups(ind - 1)
                Else
                    NavBar.SelectedGroup = NavBar.Groups(NavBar.Groups.Count - 1)
                End If
                currentGroup = TryCast(NavBar.SelectedGroup, NavBarGroup)
                If currentGroup.Items.Count > 0 Then
                NavBar.SelectedItem = currentGroup.Items(0)
                End If
                Return True
            End If
            Return False
        End Function

        Private Function PrevGroup() As Boolean
            If NavBar.ActiveGroup Is Nothing Then
                Return False
            End If
            Dim index = NavBar.Groups.IndexOf(NavBar.ActiveGroup)
            If index < 1 Then
                Return False
            End If
            NavBar.SelectedGroup = NavBar.Groups(index - 1)
            Return PrevItem()
        End Function
        Private Function NextGroup() As Boolean
            If NavBar.ActiveGroup Is Nothing Then
                Return False
            End If
            Dim index = NavBar.Groups.IndexOf(NavBar.ActiveGroup)
            If index >= NavBar.Groups.Count - 1 Then
                Return False
            End If
            NavBar.SelectedGroup = NavBar.Groups(index + 1)
            Return NextItem()
        End Function
        Private Function PrevItem() As Boolean
            If NavBar.ActiveGroup Is Nothing Then
                Return False
            End If
            Dim newIndex = NavBar.ActiveGroup.SelectedItemIndex - 1
            If newIndex = -2 Then
                newIndex = NavBar.ActiveGroup.Items.Count - 1
            End If
            If Not NavBar.ActiveGroup.Items.IsValidIndex(newIndex) Then
                Return PrevGroup()
            End If
            NavBar.ActiveGroup.SelectedItemIndex = newIndex
            Return True
        End Function
        Private Function NextItem() As Boolean
            If NavBar.ActiveGroup Is Nothing Then
                Return False
            End If
            Dim newIndex = NavBar.ActiveGroup.SelectedItemIndex + 1
            If Not NavBar.ActiveGroup.Items.IsValidIndex(newIndex) Then
                Return NextGroup()
            End If
            NavBar.ActiveGroup.SelectedItemIndex = newIndex
            Return True
        End Function

        Private Function IsShiftPressed(ByVal e As KeyEventArgs) As Boolean
            Return ModifierKeysHelper.IsShiftPressed(ModifierKeysHelper.GetKeyboardModifiers(e))
        End Function
        Private Function IsCtrlPressed(ByVal e As KeyEventArgs) As Boolean
            Return ModifierKeysHelper.IsShiftPressed(ModifierKeysHelper.GetKeyboardModifiers(e))
        End Function
    End Class
End Namespace