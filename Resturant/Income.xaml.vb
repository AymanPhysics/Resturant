Imports System.Data

Public Class Income
    Public TableName As String = "Income"
    Public SubId As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded Then Return
        bm.Fields = New String() {SubId, "DayDate", "Value", "MainAccNo", "SubAccNo", "Notes", "Canceled", "Cash", "SheekNo", "SheekPerson", "SheekDate"}
        bm.control = New Control() {txtID, DayDate, Value, MainAccNo, SubAccNo, Notes, Canceled, Cash, SheekNo, SheekPerson, SheekDate}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        RdoCash_Checked(Nothing, Nothing)
        btnNew_Click(sender, e)
        bm.ApplyKeyDown()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls()
        MainAccNo_LostFocus(Nothing, Nothing)
        SubAccNo_LostFocus(Nothing, Nothing)
        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(MainAccNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الحساب العام")
            MainAccNo.Focus()
            Return
        End If
        If Val(SubAccNo.Text) = 0 And SubAccNo.IsEnabled Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo.Focus()
            Return
        End If
        Value.Text = Val(Value.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        btnNew_Click(sender, e)
        AllowClose = True
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        MainAccName.Clear()
        SubAccName.Clear()
        RdoCash.IsChecked = True
        DayDate.SelectedDate = Date.Today
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        DayDate.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من المسح؟") Then
            bm.ExcuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, MainAccNo.KeyDown, SubAccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    Dim AllowClose As Boolean = False
    'Private Sub MyBase_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    If Not btnSave.Enabled Then Exit Sub
    '    Select Case bm.RequestDelete
    '        Case BasicMethods.CloseState.Yes
    '            AllowClose = False
    '            btnSave_Click(Nothing, Nothing)
    '            If Not AllowClose Then e.Cancel = True
    '        Case BasicMethods.CloseState.No

    '        Case BasicMethods.CloseState.Cancel
    '            e.Cancel = True
    '    End Select
    'End Sub

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

    Private Sub RdoCash_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoCash.Checked, RdoCheck.Checked
        Try
            Cash.Text = 0
            If RdoCash.IsChecked Then
                Cash.Text = 1
            ElseIf RdoCheck.IsChecked Then
                Cash.Text = 2
            End If
        Catch ex As Exception
        End Try

        Try
            If RdoCheck.IsChecked Then
                SheekNo.Visibility = Windows.Visibility.Visible
                lblSheekNo.Visibility = Windows.Visibility.Visible

                SheekPerson.Visibility = Windows.Visibility.Visible
                lblSheekPerson.Visibility = Windows.Visibility.Visible

                SheekDate.Visibility = Windows.Visibility.Visible
                lblSheekDate.Visibility = Windows.Visibility.Visible
            Else
                SheekNo.Visibility = Windows.Visibility.Hidden
                lblSheekNo.Visibility = Windows.Visibility.Hidden

                SheekPerson.Visibility = Windows.Visibility.Hidden
                lblSheekPerson.Visibility = Windows.Visibility.Hidden

                SheekDate.Visibility = Windows.Visibility.Hidden
                lblSheekDate.Visibility = Windows.Visibility.Hidden
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Cash_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Cash.TextChanged
        Try
            If Cash.Text = 1 Then
                RdoCash.IsChecked = True
            ElseIf Cash.Text = 2 Then
                RdoCheck.IsChecked = True
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Value.Text = ""
        Value.IsEnabled = False
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked
        Value.IsEnabled = True
    End Sub
End Class
