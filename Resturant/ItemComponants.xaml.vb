Imports System.Data
Imports DNBSoft.WPF.RibbonControl
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class ItemComponants

    Public TableName As String = "Items"
    Public SubId As String = "Id"
    Public SubName As String = "Name"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer
    Public Flag As Integer
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "الأقسام الرئيسية", Tp As String = "الأقسام الفرعية", It As String = "الأصناف"


    Public Structure FlagState
        'Don't forget to edit RPTs and Stored Procedures after Editing this structure
        Shared الأصناف_النصف_مصنعة As Integer = 1
        Shared الأصناف_التامة As Integer = 2
        Shared الكومبو As Integer = 3
    End Structure


    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded Then Return
        RdoGrouping_Checked(Nothing, Nothing)
        TabItem1.Header = TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader
        bm.Fields = New String() {SubId, SubName}
        bm.control = New Control() {MainItemId, MainItemName}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        
        LoadGroups()
        LoadAllItems()
        LoadWFH()
        btnNew_Click(sender, e)
        bm.ApplyKeyDown()
    End Sub

    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Unit As String = "Unit"
        Shared Qty As String = "Qty"
    End Structure

    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G


        G.Grid.ForeColor = System.Drawing.Color.DarkBlue
        G.Grid.Columns.Add(GC.Id, "كود الصنف")
        G.Grid.Columns.Add(GC.Name, "اسم الصنف")
        G.Grid.Columns.Add(GC.Unit, "الوحدة")
        G.Grid.Columns.Add(GC.Qty, "الكمية")

        G.Grid.Columns(GC.Id).FillWeight = 100
        G.Grid.Columns(GC.Name).FillWeight = 300
        G.Grid.Columns(GC.Unit).FillWeight = 150
        G.Grid.Columns(GC.Qty).FillWeight = 100

        G.Grid.Columns(GC.Name).ReadOnly = True
        G.Grid.Columns(GC.Unit).ReadOnly = True

        AddHandler G.Grid.CellEndEdit, AddressOf GridCalcRow
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If G.Grid.Columns(e.ColumnIndex).Name = GC.Id Then
            AddItem(G.Grid.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
        End If
        G.Grid.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub

    Sub SetStyle(ByVal x As Button)
        x.Style = Application.Current.FindResource("GlossyCloseButton")
        x.VerticalContentAlignment = Windows.VerticalAlignment.Center
        x.Width = 100
        x.Height = 50
        x.Margin = New Thickness(10, 10, 0, 0)
    End Sub

    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WItems.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
            TabItems.Header = It

            Dim dt As DataTable = bm.ExcuteAdapter("LoadGroups")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WGroups.Children.Add(x)
                AddHandler x.Click, AddressOf LoadTypes
            Next
        Catch
        End Try
    End Sub

    Private Sub LoadTypes(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes.Tag = xx.Tag
            WTypes.Children.Clear()
            WItems.Children.Clear()

            TabTypes.Header = Tp & " - " & xx.Content
            TabItems.Header = It

            Dim dt As DataTable = bm.ExcuteAdapter("LoadTypes", New String() {"GroupId"}, New String() {xx.Tag.ToString})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & xx.Tag.ToString & "_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WTypes.Children.Add(x)
                AddHandler x.Click, AddressOf LoadItems
            Next
        Catch
        End Try
    End Sub

    Sub LoadAllItems()
        Try
            HelpDt = bm.ExcuteAdapter("Select Id,Name,PurchasePrice From Items where " & ItemWhere())
            HelpDt.TableName = "tbl"
            HelpDt.Columns(0).ColumnName = FirstColumn
            HelpDt.Columns(1).ColumnName = SecondColumn
            HelpDt.Columns(2).ColumnName = ThirdColumn

            dv.Table = HelpDt
            HelpGD.ItemsSource = dv
            HelpGD.Columns(0).Width = 75
            HelpGD.Columns(1).Width = 220
            HelpGD.Columns(2).Width = 75

            HelpGD.SelectedIndex = 0
        Catch
        End Try

    End Sub

    Private Sub txtId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.GotFocus
        Try
            dv.Sort = FirstColumn
        Catch
        End Try
    End Sub

    Private Sub txtName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        Try
            dv.Sort = SecondColumn
        Catch
        End Try
    End Sub

    Private Sub txtPrice_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrice.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtName.TextChanged, txtPrice.TextChanged
        Try
            dv.RowFilter = " [" & FirstColumn & "] >" & IIf(txtID.Text.Trim = "", 0, txtID.Text) & " and [" & SecondColumn & "] like '" & txtName.Text & "%' and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
        Catch
        End Try
    End Sub
    Private Sub HelpGD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.PreviewKeyDown, txtName.PreviewKeyDown, txtPrice.PreviewKeyDown
        Try
            If e.Key = Input.Key.Up Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub HelpGD_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles HelpGD.MouseDoubleClick
        Try
            AddItem(HelpGD.Items(HelpGD.SelectedIndex)(0))
        Catch ex As Exception
        End Try
    End Sub

    Function ItemWhere() As String
        Select Case Flag
            Case FlagState.الأصناف_النصف_مصنعة
                Return " ItemType in(0) "
            Case FlagState.الأصناف_التامة
                Return " ItemType in(0,1) "
            Case FlagState.الكومبو
                Return " ItemType in(2) "
            Case Else
                Return " "
        End Select
    End Function

    Function ItemHeader() As String
        Select Case Flag
            Case FlagState.الأصناف_النصف_مصنعة
                Return "الأصناف النصف مصنعة"
            Case FlagState.الأصناف_التامة
                Return "الأصناف التامة"
            Case FlagState.الكومبو
                Return "الكومبو"
            Case Else
                Return " "
        End Select
    End Function

    Function MainItemWhere() As String
        Select Case Flag
            Case FlagState.الأصناف_النصف_مصنعة
                Return " ItemType =1 "
            Case FlagState.الأصناف_التامة
                Return " ItemType =2 "
            Case FlagState.الكومبو
                Return " ItemType =3 "
            Case Else
                Return " "
        End Select
    End Function

    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()

            TabItems.Header = It & " - " & xx.Content

            Dim dt As DataTable = bm.ExcuteAdapter("Select * From Items where " & ItemWhere() & " and GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag.ToString)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WItems.Children.Add(x)
                AddHandler x.Click, AddressOf TabItem
            Next
        Catch
        End Try
    End Sub

    Private Sub TabItem(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        AddItem(x.Tag)
    End Sub

    Sub AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        Try
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.Grid.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                For x As Integer = 0 To G.Grid.Rows.Count - 1
                    If Not G.Grid.Rows(x).Cells(GC.Id).Value Is Nothing AndAlso G.Grid.Rows(x).Cells(GC.Id).Value.ToString = Id.ToString Then
                        i = x
                        Exists = True
                        GoTo Br
                    End If
                Next
                i = G.Grid.Rows.Add()
Br:
            End If

            Dim dt As DataTable = bm.ExcuteAdapter("Select * From Items where Id='" & Id & "' and " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Grid.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Grid.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)
                Return
            End If
            G.Grid.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
            G.Grid.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)
            G.Grid.Rows(i).Cells(GC.Unit).Value = dr(0)(GC.Unit)
            If Val(G.Grid.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Grid.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Grid.Rows(i).Cells(GC.Qty).Value)

            If Move Then
                G.Grid.Focus()
                G.Grid.Rows(i).Selected = True
                G.Grid.FirstDisplayedScrollingRowIndex = i
                G.Grid.CurrentCell = G.Grid.Rows(i).Cells(GC.Qty)
                G.Grid.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.Grid.BeginEdit(True)
            End If
            If Exists Then
                G.Grid.Rows(i).Selected = True
                G.Grid.FirstDisplayedScrollingRowIndex = i
                G.Grid.CurrentCell = G.Grid.Rows(i).Cells(GC.Name)
                G.Grid.CurrentCell = G.Grid.Rows(i).Cells(GC.Qty)
                G.Grid.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.Grid.BeginEdit(True)
            End If
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
        TestDouplication(i)
    End Sub

    Dim lop As Boolean = False

    Sub ClearRow(ByVal i As Integer)
        G.Grid.Rows(i).Cells(GC.Id).Value = Nothing
        G.Grid.Rows(i).Cells(GC.Name).Value = Nothing
        G.Grid.Rows(i).Cells(GC.Unit).Value = Nothing
        G.Grid.Rows(i).Cells(GC.Qty).Value = Nothing
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True
        bm.FillControls()
        Notes.Clear()

        Dim dt As DataTable = bm.ExcuteAdapter("select * from ItemComponants where MainItemId=" & MainItemId.Text & " and Flag=" & Flag)

        G.Grid.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Grid.Rows.Add()
            G.Grid.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Grid.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            G.Grid.Rows(i).Cells(GC.Unit).Value = dt.Rows(i)("Unit").ToString
            G.Grid.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            Notes.Text = dt.Rows(i)("Notes").ToString
        Next
        Notes.Focus()
        G.Grid.RefreshEdit()
        lop = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {MainItemId.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If MainItemId.Text.Trim = "" Then Return

        G.Grid.EndEdit()
        If Not bm.SaveGrid(G.Grid, "ItemComponants", New String() {"Flag", "MainItemId"}, New String() {Flag, MainItemId.Text}, New String() {"ItemId", "ItemName", "Unit", "Qty"}, New String() {GC.Id, GC.Name, GC.Unit, GC.Qty}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.String, VariantType.Integer}, New String() {GC.Id}) Then Return
        bm.ExcuteNonQuery("Update ItemComponants set Notes='" & Notes.Text.Trim.Replace("'", "''") & "' where Flag='" & Flag & "' and MainItemId='" & MainItemId.Text & "'")
        bm.ExecuteNonQuery("UpdatePurchasePrice", New String() {"Id"}, New String() {MainItemId.Text})
        If sender Is btnPrint Then
            'Print()
            MainItemId_Leave(MainItemId, Nothing)
            Return
        End If

        btnNew_Click(sender, e)
        AllowClose = True
    End Sub

    Function TestDouplication(ByVal i As Integer) As Boolean
        Dim s As String = G.Grid.Rows(i).Cells(0).Value
        If s = "" Then Return True
        For j As Integer = 0 To G.Grid.Rows.Count - 1
            If i = j Then Continue For
            If G.Grid.Rows(i).Cells(0).Value = G.Grid.Rows(j).Cells(0).Value Then
                bm.ShowMSG("تم تكرار الصنف بالسطر رقم " & (j + 1).ToString)
                G.Grid.Rows(i).Cells(0).Value = ""
                ClearRow(i)
                G.Grid.CurrentCell = G.Grid.Rows(j).Cells(0)
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        Try
            bm.ClearControls(False)
            MainItemId.Clear()
            Notes.Clear()
            txtFlag.Text = Flag
            bm.AppendWhere = " and " & MainItemWhere()
            G.Grid.Rows.Clear()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من المسح؟") Then
            bm.ExcuteNonQuery("delete from ItemComponants where MainItemId='" & MainItemId.Text.Trim & "'  and Flag=" & Flag)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {MainItemId.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub MainItemId_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainItemId.KeyUp
        If bm.ShowHelp(ItemHeader, MainItemId, MainItemName, e, "select cast(Id as varchar(100)) Id,Name from Items where " & MainItemWhere()) Then
            MainItemId_Leave(Nothing, Nothing)
        End If
    End Sub
    Private Sub MainItemId_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainItemId.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {MainItemId.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, MainItemId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtPrice.KeyDown
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

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.Grid.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("هل أنت متأكد من حذف السطر الحالى؟") Then
                G.Grid.Rows.Remove(G.Grid.CurrentRow)
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub RdoGrouping_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoGrouping.Checked, RdoSearch.Checked
        Try
            If RdoGrouping.IsChecked Then
                txtID.Visibility = Visibility.Hidden
                txtName.Visibility = Visibility.Hidden
                txtPrice.Visibility = Visibility.Hidden
                HelpGD.Visibility = Visibility.Hidden
                PanelGroups.Visibility = Visibility.Visible
                PanelTypes.Visibility = Visibility.Visible
                PanelItems.Visibility = Visibility.Visible
            ElseIf RdoSearch.IsChecked Then
                txtID.Visibility = Visibility.Visible
                txtName.Visibility = Visibility.Visible
                txtPrice.Visibility = Visibility.Visible
                HelpGD.Visibility = Visibility.Visible
                PanelGroups.Visibility = Visibility.Hidden
                PanelTypes.Visibility = Visibility.Hidden
                PanelItems.Visibility = Visibility.Hidden
            End If
        Catch
        End Try
    End Sub


End Class
