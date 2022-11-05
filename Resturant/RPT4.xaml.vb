Imports System.Data

Public Class RPT4
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Detail As Integer = 0
    Public Hdr As String = ""
    Dim dt As New DataTable

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@MainAccNo", "@SubAccNo", "@FromInvoiceNo", "@ToInvoiceNo", "Header", "Flag"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, Val(MainAccNo.Text), Val(SubAccNo.Text), Val(FromInvoice.Text), Val(ToInvoice.Text), Hdr, IIf(Flag = 2 Or Flag = 4, 2, 1)}
        rpt.Header = Md.MyProject.ToString
        Select Case Flag
            Case 1, 2
                rpt.RptPath = "Income.rpt"
            Case 3, 4
                rpt.RptPath = "Outcome.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded Then Return
        FromDate.SelectedDate = New DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0)
        bm.ApplyKeyDown(New Control() {FromDate, ToDate, MainAccNo, SubAccNo, FromInvoice, ToInvoice})

        If Md.Cashier = 1 AndAlso Not Md.Manager = 1 AndAlso Not Md.GeneralManager = 1 Then
            FromDate.SelectedDate = Md.CurrentDate
            ToDate.SelectedDate = Md.CurrentDate
            'FromDate.IsEnabled = False
            'ToDate.IsEnabled = False
        End If

    End Sub
    Private Sub SubAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo.LostFocus
        If Val(MainAccNo.Text) = 0 Or Not SubAccNo.IsEnabled Then
            SubAccNo.Clear()
            SubAccName.Clear()
            Return
        End If

        dt = bm.ExcuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id=" & MainAccNo.Text & ")")
        bm.LostFocus(SubAccNo, SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim() & " and AccNo=" & Val(MainAccNo.Text))
    End Sub
    Private Sub SubAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubAccNo.KeyUp
        dt = bm.ExcuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id=" & MainAccNo.Text & ")")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("Name"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo=" & Val(MainAccNo.Text)) Then
            SubAccNo_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, , )

        SubAccNo.IsEnabled = bm.ExcuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id=" & MainAccNo.Text & ")").Rows.Count > 0
        SubAccNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e, , )
    End Sub


End Class
