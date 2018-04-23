Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls
Imports DevExpress.Xpf.NavBar
Imports DevExpress.Xpf.Core

Namespace NavBarAttachedProperty.Example
    Partial Public Class MainWindow
        Inherits Window

        Public Shared ReadOnly ThemesProperty As DependencyProperty = DependencyProperty.Register("Themes", GetType(List(Of Theme)), GetType(MainWindow), New UIPropertyMetadata(New List(Of Theme)()))
        Public Shared ReadOnly SelectedThemeProperty As DependencyProperty = DependencyProperty.Register("SelectedTheme", GetType(Theme), GetType(MainWindow), New UIPropertyMetadata(Nothing))

        Public Property Themes() As List(Of Theme)
            Get
                Return DirectCast(GetValue(ThemesProperty), List(Of Theme))
            End Get
            Set(ByVal value As List(Of Theme))
                SetValue(ThemesProperty, value)
            End Set
        End Property
        Public Property SelectedTheme() As Theme
            Get
                Return DirectCast(GetValue(SelectedThemeProperty), Theme)
            End Get
            Set(ByVal value As Theme)
                SetValue(SelectedThemeProperty, value)
            End Set
        End Property

        Public Sub New()
            Themes.Add(Theme.DeepBlue)
            Themes.Add(Theme.LightGray)
            Themes.Add(Theme.Office2007Black)
            Themes.Add(Theme.Office2007Blue)
            Themes.Add(Theme.Office2007Silver)
            Themes.Add(Theme.Seven)
            DataContext = Me
            InitializeComponent()
        End Sub
        Private Sub ListBox_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
        If DirectCast(sender, ListBox).SelectedItem.ToString().Contains("ExplorerBarView") Then
            navBar.View = New ExplorerBarView()
        End If
        If DirectCast(sender, ListBox).SelectedItem.ToString().Contains("NavigationPaneView") Then
            navBar.View = New NavigationPaneView()
        End If
        If DirectCast(sender, ListBox).SelectedItem.ToString().Contains("SideBarView") Then
            navBar.View = New SideBarView()
        End If
        End Sub
    End Class
End Namespace