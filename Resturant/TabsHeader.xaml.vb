Public Class TabsHeader
    Public MyTabHeader As String
    Public MyTabName As String
    Public WithClose As Visibility
    Dim bm As New BasicMethods
    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        bm.CloseTab(MyTabName)
    End Sub

    Private Sub TabsHeader_GotFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.GotFocus
        TextBlock1.Foreground = New SolidColorBrush(Colors.White)
    End Sub

    Private Sub UserControl_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        TextBlock1.Text = MyTabHeader
        Button1.Visibility = WithClose
        Button1.IsTabStop = False
        If Button1.Visibility = Windows.Visibility.Hidden Then
            TextBlock1.Margin = New Thickness(0)
        End If
    End Sub

    Private Sub TabsHeader_LostFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.LostFocus
        TextBlock1.Foreground = New SolidColorBrush(Colors.Black)
    End Sub
End Class
