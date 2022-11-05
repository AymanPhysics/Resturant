Imports System.Data

Public Class HelpAddingItems
    Dim bm As New BasicMethods
    Public FirstColumn As String = "الكــــــــــــود", SecondColumn As String = "الاســــــــــــم"
    Dim dt As New DataTable
    Dim dv As New DataView
    Private Sub Help_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Loaded
        Banner1.StopTimer = True
        Banner1.Header = "الإضافات"
        Try
            dt = bm.ExcuteAdapter("select cast(Id as varchar(100)) Id,Name,cast(0 as bit) 'اختيار' from items where Adding=1")
            dt.TableName = "tbl"
            dt.Columns(0).ColumnName = FirstColumn
            dt.Columns(1).ColumnName = SecondColumn

            dv.Table = dt
            DataGridView1.ItemsSource = dv
            DataGridView1.Columns(0).Width = 120
            DataGridView1.Columns(1).Width = 250
            DataGridView1.Columns(2).Width = 50

            DataGridView1.Columns(0).IsReadOnly = True
            DataGridView1.Columns(1).IsReadOnly = True

            DataGridView1.SelectedIndex = 0
        Catch
        End Try
        txtID.Focus()
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

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtName.TextChanged
        dv.RowFilter = " [" & FirstColumn & "] like '%" & txtID.Text & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%'"
    End Sub

    Public SelectedId As Integer = 0
    Public SelectedName As String = ""

    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = System.Windows.Input.Key.Enter Then
                SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
                SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
                Close()
            ElseIf e.Key = Input.Key.Escape Then
                Close()
            ElseIf e.Key = Input.Key.Up Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub DataGridView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DataGridView1.MouseDoubleClick
        Try
            'SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
            'SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
            'Close()
        Catch ex As Exception
        End Try
    End Sub


    Private Sub DataGridView1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles DataGridView1.SelectionChanged
        'DataGridView1.BeginEdit()
    End Sub
End Class