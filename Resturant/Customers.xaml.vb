Imports System.Data
Public Class Customers
    Public TableName As String = "Customers"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded Then Return
        bm.Fields = New String() {SubId, SubName, "AccNo", "CountryId", "CityId", "AreaId", "Address", "Floor", "Appartment", "Tel", "Mobile", "Bal0", "DescPerc"}
        bm.control = New Control() {txtID, txtName, AccNo, CountryId, CityId, AreaId, Address, Floor, Appartment, Tel, Mobile, Bal0, DescPerc}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
        bm.ApplyKeyDown()
        bm.ApplyKeyDown(New Control() {AccName, CountryName, CityName, AreaName, btnSave, btnDelete, btnNew, btnFirst, btnLast, btnNext, btnPrevios})
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls()
        AccNo_LostFocus(Nothing, Nothing)
        CountryId_LostFocus(Nothing, Nothing)
        CityId_LostFocus(Nothing, Nothing)
        AreaId_LostFocus(Nothing, Nothing)
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If
        Bal0.Text = Val(Bal0.Text.Trim)
        DescPerc.Text = Val(DescPerc.Text.Trim)
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

        AccName.Clear()
        CountryName.Clear()
        CityName.Clear()
        AreaName.Clear()

        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
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
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        txtName.SelectAll()
        txtName.Focus()
        txtName.SelectAll()
        txtName.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub CountryId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CountryId.KeyUp
        bm.ShowHelp("الدول", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries")
    End Sub

    Private Sub CityId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CityId.KeyUp
        bm.ShowHelp("المحافظات", CityId, CityName, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=" & CountryId.Text.Trim)
    End Sub

    Private Sub AreaId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AreaId.KeyUp
        bm.ShowHelp("المناطق", AreaId, AreaName, e, "select cast(Id as varchar(100)) Id,Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, CountryId.KeyDown, CityId.KeyDown, AreaId.KeyDown, AccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Bal0.KeyDown, DescPerc.KeyDown
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



    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
        CityId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId.LostFocus
        bm.LostFocus(CityId, CityName, "select Name from Cities where CountryId=" & CountryId.Text.Trim & " and Id=" & CityId.Text.Trim())
        AreaId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub AreaId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AreaId.LostFocus
        bm.LostFocus(AreaId, AreaName, "select Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text.Trim & " and Id=" & AreaId.Text.Trim())
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , 1)
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , 1)
    End Sub

End Class
