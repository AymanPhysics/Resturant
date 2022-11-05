Imports System.Data

Public Class Items
    Public TableName As String = "Items"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded Then Return
        If Not Md.MyProject = Project.Restuarant Then
            lblItemType.Visibility = Visibility.Hidden
            ItemType.Visibility = Visibility.Hidden

            lblStoreId.Visibility = Visibility.Hidden
            StoreId.Visibility = Visibility.Hidden
            StoreName.Visibility = Visibility.Hidden

            lblPrintingGroupId.Visibility = Visibility.Hidden
            PrintingGroupId.Visibility = Visibility.Hidden
            PrintingGroupName.Visibility = Visibility.Hidden
        End If

        If Not Md.SaleQtySub Then
            lblUnitSub.Visibility = Visibility.Hidden
            UnitSub.Visibility = Visibility.Hidden
            lblPurchasePriceSub.Visibility = Visibility.Hidden
            PurchasePriceSub.Visibility = Visibility.Hidden
            lblSalesPriceSub.Visibility = Visibility.Hidden
            SalesPriceSub.Visibility = Visibility.Hidden
            lblUnitCount.Visibility = Visibility.Hidden
            UnitCount.Visibility = Visibility.Hidden
        End If

        bm.Fields = New String() {SubId, SubName, "GroupId", "TypeId", "PrintingGroupId", "StoreId", "PurchasePrice", "PurchasePriceSub", "SalesPrice", "SalesPriceSub", "ItemType", "Unit", "UnitSub", "UnitCount", "Adding", "IsTables", "IsTakeAway", "IsDelivary"}
        bm.control = New Control() {txtID, txtName, GroupId, TypeId, PrintingGroupId, StoreId, PurchasePrice, PurchasePriceSub, SalesPrice, SalesPriceSub, ItemType, Unit, UnitSub, UnitCount, Adding, IsTables, IsTakeAway, IsDelivary}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
        ItemType.SelectedIndex = 0
        bm.ApplyKeyDown()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls()
        bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        GroupId_LostFocus(Nothing, Nothing)
        TypeId_LostFocus(Nothing, Nothing)
        PrintingGroupId_LostFocus(Nothing, Nothing)
        StoreId_LostFocus(Nothing, Nothing)
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
        If UnitCount.Text.Trim = "" Then UnitCount.Text = "0"
        If PurchasePrice.Text.Trim = "" Then PurchasePrice.Text = "0"
        If PurchasePriceSub.Text.Trim = "" Then PurchasePriceSub.Text = "0"
        If SalesPrice.Text.Trim = "" Then SalesPrice.Text = "0"
        If SalesPriceSub.Text.Trim = "" Then SalesPriceSub.Text = "0"

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        btnNew_Click(sender, e)
        AllowClose = True
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        bm.SetNoImage(Image1)

        GroupName.Clear()
        TypeName.Clear()
        PrintingGroupName.Clear()
        StoreName.Clear()

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

    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp("الأصناف", txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from Items") Then
            txtID_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        bm.ShowHelp("المخازن", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Stores")
    End Sub

    Private Sub TypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TypeId.KeyUp
        bm.ShowHelp("الأقسام الفرعية", TypeId, TypeName, e, "select cast(Id as varchar(100)) Id,Name from Types where GroupId=" & GroupId.Text.Trim)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, GroupId.KeyDown, TypeId.KeyDown, PrintingGroupId.KeyDown, StoreId.KeyDown, ItemType.KeyDown, UnitCount.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub PrintingGroupId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles PrintingGroupId.KeyUp
        bm.ShowHelp("مجموعات الطباعة", PrintingGroupId, PrintingGroupName, e, "select cast(Id as varchar(100)) Id,Name from PrintingGroups")
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles PurchasePrice.KeyDown, SalesPrice.KeyDown, SalesPriceSub.KeyDown
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



    Private Sub GroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles GroupId.LostFocus
        bm.LostFocus(GroupId, GroupName, "select Name from Groups where Id=" & GroupId.Text.Trim())
        TypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub PrintingGroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles PrintingGroupId.LostFocus
        bm.LostFocus(PrintingGroupId, PrintingGroupName, "select Name from PrintingGroups where Id=" & PrintingGroupId.Text.Trim())
    End Sub

    Private Sub TypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TypeId.LostFocus
        bm.LostFocus(TypeId, TypeName, "select Name from Types where GroupId=" & GroupId.Text.Trim & " and Id=" & TypeId.Text.Trim())
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Stores where Id=" & StoreId.Text.Trim())
    End Sub

    Private Sub GroupId_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles GroupId.KeyUp
        If bm.ShowHelp("الأقسام الرئيسية", GroupId, GroupName, e, "select cast(Id as varchar(100)) Id,Name from Groups") Then
            GroupId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        bm.SetImage(Image1)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, False, True)
    End Sub

    Private Sub SalesPrice_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount.LostFocus, SalesPrice.LostFocus
        Try
            If Val(UnitCount.Text) = 0 Then
                SalesPriceSub.Text = 0
            Else
                SalesPriceSub.Text = Val(SalesPrice.Text) / Val(UnitCount.Text)
            End If
        Catch ex As Exception
            SalesPriceSub.Text = 0
        End Try
    End Sub

    Private Sub PurchasePrice_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount.LostFocus, PurchasePrice.LostFocus
        Try
            If Val(UnitCount.Text) = 0 Then
                PurchasePriceSub.Text = 0
            Else
                PurchasePriceSub.Text = Val(PurchasePrice.Text) / Val(UnitCount.Text)
            End If
        Catch ex As Exception
            PurchasePriceSub.Text = 0
        End Try
    End Sub

    Private Sub ItemType_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ItemType.SelectionChanged
        Select Case ItemType.SelectedIndex
            Case 0, 1
                IsDelivary.IsChecked = False
                IsTables.IsChecked = False
                IsTakeAway.IsChecked = False
            Case 2, 3
                IsDelivary.IsChecked = True
                IsTables.IsChecked = True
                IsTakeAway.IsChecked = True
        End Select
    End Sub
End Class
