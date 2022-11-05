Imports System.Windows.Forms.Integration
Imports System.Data

Public Class ManageTables

    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    WithEvents G2 As New MyGrid
    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded Then Return
        LoadWFH(WFH, G)
        LoadWFH(WFH2, G2)
        bm.control = New Control() {InvoiceNo, InvoiceNo2, TableId, TableId2, TableSubId, TableSubId2}
        bm.ApplyKeyDown()
        InvoiceNo.Focus()
    End Sub
    Structure GC
        Shared Line As String = "Line"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Qty As String = "Qty"
    End Structure

    Private Sub LoadWFH(ByVal wwwww As WindowsFormsHost, ByVal ggggg As MyGrid)
        wwwww.Child = ggggg
        ggggg.Grid.AllowUserToAddRows = False
        ggggg.Grid.AllowUserToDeleteRows = False
        ggggg.Grid.SelectionMode = Forms.DataGridViewSelectionMode.FullRowSelect

        ggggg.Grid.Columns.Clear()
        ggggg.Grid.ForeColor = System.Drawing.Color.DarkBlue
        ggggg.Grid.Columns.Add(GC.Line, "Line")
        ggggg.Grid.Columns.Add(GC.Id, "كود الصنف")
        ggggg.Grid.Columns.Add(GC.Name, "اسم الصنف")
        ggggg.Grid.Columns.Add(GC.Qty, "الكمية")

        ggggg.Grid.Columns(GC.Line).FillWeight = 110
        ggggg.Grid.Columns(GC.Id).FillWeight = 110
        ggggg.Grid.Columns(GC.Name).FillWeight = 300
        ggggg.Grid.Columns(GC.Qty).FillWeight = 100

        ggggg.Grid.Columns(GC.Line).Visible = False
        ggggg.Grid.Columns(GC.Id).ReadOnly = True
        ggggg.Grid.Columns(GC.Name).ReadOnly = True
        ggggg.Grid.Columns(GC.Qty).ReadOnly = True

    End Sub


    Private Sub TableId_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TableId.KeyUp
        If bm.ShowHelp("الموائد", TableId, TableIdName, e, "select cast(Id as varchar(100)) Id,Name from Tables where StoreId='" & Md.StoreId & "'") Then
            TId_LostFocus(TableId, e)
        End If
    End Sub

    Private Sub TableId2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TableId2.KeyUp
        If bm.ShowHelp("الموائد", TableId2, TableIdName2, e, "select cast(Id as varchar(100)) Id,Name from Tables where StoreId='" & Md.StoreId & "'") Then
            TId_LostFocus2(TableId2, e)
        End If
    End Sub

    Private Sub TId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TableId.LostFocus, TableSubId.LostFocus
        If CType(sender, TextBox).Text.Trim = "" Or CType(sender, TextBox).Text.Trim = "0" Then CType(sender, TextBox).Clear()
        If sender Is TableId Then
            bm.LostFocus(TableId, TableIdName, "select Name from Tables where StoreId='" & Md.StoreId & "' and Id=" & TableId.Text.Trim())
        ElseIf sender Is TableSubId Then
            Dim x As Integer = Val(bm.ExecuteScalar("select MaxSubTable from Statics"))
            If (x < Val(TableSubId.Text)) Then
                bm.ShowMSG("الحد الأقصى للفرعى هو " & x)
                TableSubId.Clear()
            End If
        End If
        If Not e Is Nothing Then
            TestDoublicatinInTables()
        End If
    End Sub

    Private Sub TId_LostFocus2(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TableId2.LostFocus, TableSubId2.LostFocus
        If CType(sender, TextBox).Text.Trim = "" Or CType(sender, TextBox).Text.Trim = "0" Then CType(sender, TextBox).Clear()
        If sender Is TableId2 Then
            bm.LostFocus(TableId2, TableIdName2, "select Name from Tables where StoreId='" & Md.StoreId & "' and Id=" & TableId2.Text.Trim())
        ElseIf sender Is TableSubId2 Then
            Dim x As Integer = Val(bm.ExecuteScalar("select MaxSubTable from Statics"))
            If (x < Val(TableSubId2.Text)) Then
                bm.ShowMSG("الحد الأقصى للفرعى هو " & x)
                TableSubId2.Clear()
            End If
        End If
        If Not e Is Nothing Then
            TestDoublicatinInTables2()
        End If
    End Sub

    Private Sub TestDoublicatinInTables()
        If TableId.Text.Trim = "" Or TableSubId.Text.Trim = "" Then Return
        Dim s As String = bm.ExecuteScalar("select InvoiceNo from SalesMaster where Flag=" & Sales.FlagState.مبيعات_الصالة & " and StoreId=" & Md.StoreId & " and TableId=" & TableId.Text & " and TableSubId=" & TableSubId.Text & " and IsClosed=0")
        InvoiceNo.Text = s
        G.Grid.Rows.Clear()
        If InvoiceNo.Text <> "" Then GetGrid()
    End Sub

    Private Sub TestDoublicatinInTables2()
        If TableId2.Text.Trim = "" Or TableSubId2.Text.Trim = "" Then Return
        Dim s As String = bm.ExecuteScalar("select InvoiceNo from SalesMaster where Flag=" & Sales.FlagState.مبيعات_الصالة & " and StoreId=" & Md.StoreId & " and TableId=" & TableId2.Text & " and TableSubId=" & TableSubId2.Text & " and IsClosed=0")
        InvoiceNo2.Text = s
        G2.Grid.Rows.Clear()
        If InvoiceNo2.Text <> "" Then GetGrid2()
    End Sub

    Sub GetGrid()
        Dim dt As DataTable = bm.ExcuteAdapter("select * from SalesMaster where StoreId=" & Md.StoreId & " and InvoiceNo=" & InvoiceNo.Text & " and Flag=" & Sales.FlagState.مبيعات_الصالة & " and IsClosed=0")
        G.Grid.Rows.Clear()
        TableId.Clear()
        TableIdName.Clear()
        TableSubId.Clear()

        If dt.Rows.Count = 0 Then
            InvoiceNo.Clear()
            Return
        End If
        TableId.Text = dt.Rows(0)("TableId").ToString
        TId_LostFocus(TableId, Nothing)
        TableSubId.Text = dt.Rows(0)("TableSubId").ToString
        dt = bm.ExcuteAdapter("select SD.*,It.Unit,It.UnitSub from SalesDetails SD left join Items It on(SD.ItemId=It.Id) where SD.StoreId=" & Md.StoreId & " and SD.InvoiceNo=" & InvoiceNo.Text & " and Flag=" & Sales.FlagState.مبيعات_الصالة)
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Grid.Rows.Add()
            G.Grid.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G.Grid.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Grid.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            G.Grid.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
        Next
        G.Grid.RefreshEdit()
    End Sub

    Sub GetGrid2()
        Dim dt As DataTable = bm.ExcuteAdapter("select * from SalesMaster where StoreId=" & Md.StoreId & " and InvoiceNo=" & InvoiceNo2.Text & " and Flag=" & Sales.FlagState.مبيعات_الصالة & " and IsClosed=0")
        G2.Grid.Rows.Clear()
        TableId2.Clear()
        TableIdName2.Clear()
        TableSubId2.Clear()

        If dt.Rows.Count = 0 Then
            InvoiceNo2.Clear()
            Return
        End If
        TableId2.Text = dt.Rows(0)("TableId").ToString
        TId_LostFocus2(TableId2, Nothing)
        TableSubId2.Text = dt.Rows(0)("TableSubId").ToString
        dt = bm.ExcuteAdapter("select SD.*,It.Unit,It.UnitSub from SalesDetails SD left join Items It on(SD.ItemId=It.Id) where SD.StoreId=" & Md.StoreId & " and SD.InvoiceNo=" & InvoiceNo2.Text & " and Flag=" & Sales.FlagState.مبيعات_الصالة)
        For i As Integer = 0 To dt.Rows.Count - 1
            G2.Grid.Rows.Add()
            G2.Grid.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G2.Grid.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G2.Grid.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            G2.Grid.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
        Next
        G2.Grid.RefreshEdit()
    End Sub

    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        GetGrid()
    End Sub

    Private Sub txtID_Leave2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo2.LostFocus
        GetGrid2()
    End Sub

    Function DisableControls() As Boolean
        If TableId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد المائدة")
            TableId.Focus()
            Return False
        End If
        If TableSubId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد الفرعى")
            TableSubId.Focus()
            Return False
        End If
        If TableId2.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد المائدة")
            TableId2.Focus()
            Return False
        End If
        If TableSubId2.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد الفرعى")
            TableSubId2.Focus()
            Return False
        End If
        InvoiceNo.IsEnabled = False
        TableId.IsEnabled = False
        TableSubId.IsEnabled = False
        InvoiceNo2.IsEnabled = False
        TableId2.IsEnabled = False
        TableSubId2.IsEnabled = False
        Return True
    End Function

    Sub EnableControls()
        InvoiceNo.IsEnabled = True
        TableId.IsEnabled = True
        TableSubId.IsEnabled = True
        InvoiceNo2.IsEnabled = True
        TableId2.IsEnabled = True
        TableSubId2.IsEnabled = True
    End Sub

    Private Sub btnMoveAll_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnMoveAll.Click
        If Not DisableControls() Then Return
        For i As Integer = G.Grid.Rows.Count - 1 To 0 Step -1
            Try
                G.Grid.CurrentCell = G.Grid.Rows(i).Cells(0)
                Move(G.Grid, G2.Grid, G.Grid.Rows(i).Cells(GC.Qty).Value)
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub btnMove_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnMove.Click
        If Not DisableControls() Then Return
        Try
            Move(G.Grid, G2.Grid, G.Grid.Rows(G.Grid.CurrentRow.Index).Cells(GC.Qty).Value)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBack.Click
        If Not DisableControls() Then Return
        Try
            Move(G2.Grid, G.Grid, G2.Grid.Rows(G2.Grid.CurrentRow.Index).Cells(GC.Qty).Value)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnBackAll_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBackAll.Click
        If Not DisableControls() Then Return
        For i As Integer = G2.Grid.Rows.Count - 1 To 0 Step -1
            Try
                G2.Grid.CurrentCell = G2.Grid.Rows(i).Cells(0)
                Move(G2.Grid, G.Grid, G2.Grid.Rows(i).Cells(GC.Qty).Value)
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub btnBlus_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBlus.Click
        If Not DisableControls() Then Return
        Move(G.Grid, G2.Grid)
    End Sub

    Private Sub btnMinus_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnMinus.Click
        If Not DisableControls() Then Return
        Move(G2.Grid, G.Grid)
    End Sub
    Sub Move(ByVal G_From As Forms.DataGridView, ByVal G_To As Forms.DataGridView, Optional ByVal cnt As Integer = 1)
        Try
            G_From.Rows(0).Visible = True
            If G_From.CurrentRow Is Nothing Then G_From.CurrentCell = G_From.Rows(0).Cells(0)
            Dim r As Forms.DataGridViewRow = G_From.CurrentRow
            If G_From.Rows(r.Index).Cells(GC.Qty).Value = 0 Then
                G_From.Rows(r.Index).Visible = False
                Return
            End If
            Dim Done As Boolean = False
            For i As Integer = 0 To G_To.Rows.Count - 1
                If Math.Abs(Val(G_To.Rows(i).Cells(GC.Line).Value)) = Math.Abs(Val(r.Cells(GC.Line).Value)) Then
                    G_To.Rows(i).Visible = True
                    G_From.Rows(r.Index).Cells(GC.Qty).Value -= cnt
                    G_To.Rows(i).Cells(GC.Qty).Value += cnt
                    Done = True
                    Exit For
                End If
            Next
            If Not Done Then
                Dim x As Integer = G_To.Rows.Add(New String() {G_From.Rows(r.Index).Cells(GC.Line).Value, G_From.Rows(r.Index).Cells(GC.Id).Value, G_From.Rows(r.Index).Cells(GC.Name).Value, G_From.Rows(r.Index).Cells(GC.Qty).Value})
                G_To.Rows(x).Cells(GC.Line).Value *= -1
                G_To.Rows(x).Cells(GC.Qty).Value = cnt
                G_From.Rows(r.Index).Cells(GC.Qty).Value -= cnt
            End If
            If G_From.Rows(0).Cells(GC.Qty).Value = 0 Then G_From.Rows(0).Visible = False
            If G_From.Rows(r.Index).Cells(GC.Qty).Value = 0 Then G_From.Rows(r.Index).Visible = False
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        InvoiceNo.Clear()
        InvoiceNo2.Clear()
        GetGrid()
        GetGrid2()
        EnableControls()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        If InvoiceNo.Text.Trim = "" And InvoiceNo2.Text.Trim = "" Then Return
        If InvoiceNo.Text.Trim = "" Then InvoiceNo.Text = GetNewInvoice(InvoiceNo2.Text, TableId.Text, TableSubId.Text)
        If InvoiceNo2.Text.Trim = "" Then InvoiceNo2.Text = GetNewInvoice(InvoiceNo.Text, TableId2.Text, TableSubId2.Text)
        If InvoiceNo.Text.Trim = "" Or InvoiceNo2.Text.Trim = "" Then Return

        SaveGrid(G.Grid, InvoiceNo.Text)
        SaveGrid(G2.Grid, InvoiceNo2.Text)
        Button2_Click(Nothing, Nothing)
    End Sub

    Function GetNewInvoice(ByVal Inv As String, ByVal Table As String, ByVal TableSub As String) As String
        Return bm.ExecuteScalar("GenerateInvoice", New String() {"StoreId", "Flag", "InvoiceNo", "TableId", "TableSubId"}, New String() {Md.StoreId, Sales.FlagState.مبيعات_الصالة, Inv, Table, TableSub})
    End Function

    Private Sub SaveGrid(ByVal GD As Forms.DataGridView, ByVal Inv As String)
        For i As Integer = 0 To GD.Rows.Count - 1
            bm.ExecuteNonQuery("SaveItemMoved", New String() {"Flag", "StoreId", "InvoiceNo", "ItemId", "Qty", "Line"}, New String() {Sales.FlagState.مبيعات_الصالة, Md.StoreId, Inv, GD.Rows(i).Cells(GC.Id).Value, GD.Rows(i).Cells(GC.Qty).Value, GD.Rows(i).Cells(GC.Line).Value})
        Next
    End Sub

End Class
