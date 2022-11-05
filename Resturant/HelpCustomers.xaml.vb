Imports System.Data

Public Class HelpCustomers
    Dim bm As New BasicMethods
    Public FirstColumn As String = "الكــــــــــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "التليفــــون", FourthColumn As String = "الموبايـــــل", FifthColumn As String = "العنـــــــــــوان"

    Dim dt As New DataTable
    Dim dv As New DataView
    Public Header As String = ""
    Private Sub Help_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Loaded
        Banner1.StopTimer = True
        Banner1.Header = Header
        Try
            dt = bm.ExcuteAdapter("select cast(Id as varchar(100)),Name,Tel,Mobile,Address from Customers")
            dt.TableName = "tbl"
            dt.Columns(0).ColumnName = FirstColumn
            dt.Columns(1).ColumnName = SecondColumn
            dt.Columns(2).ColumnName = ThirdColumn
            dt.Columns(3).ColumnName = FourthColumn
            dt.Columns(4).ColumnName = FifthColumn

            dv.Table = dt
            DataGridView1.ItemsSource = dv
            DataGridView1.Columns(0).Width = 120
            DataGridView1.Columns(1).Width = 270
            DataGridView1.Columns(2).Width = 110
            DataGridView1.Columns(3).Width = 110
            DataGridView1.Columns(4).Width = 210

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

    Private Sub txtTel_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTel.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub txtMob_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMob.GotFocus
        Try
            dv.Sort = FourthColumn
        Catch
        End Try
    End Sub

    Private Sub txtAddress_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddress.GotFocus
        Try
            dv.Sort = FifthColumn
        Catch
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtName.TextChanged, txtTel.TextChanged, txtMob.TextChanged, txtAddress.TextChanged
        Try
            dv.RowFilter = " [" & FirstColumn & "] like '%" & txtID.Text & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%' and ([" & ThirdColumn & "] like '%" & txtTel.Text & "%' or [" & FourthColumn & "] like '%" & txtTel.Text & "%') and ([" & ThirdColumn & "] like '%" & txtMob.Text & "%' or [" & FourthColumn & "] like '%" & txtMob.Text & "%') and [" & FifthColumn & "] like '%" & txtAddress.Text & "%'"
        Catch ex As Exception
        End Try
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
            SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
            SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
            Close()
        Catch ex As Exception
        End Try
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        Try
            If txtName.Text.Trim = "" Then
                bm.ShowMSG("برجاء تحديد اسم العميل")
                txtName.Focus()
                Return
            End If
            SelectedId = bm.ExecuteScalar("AddCustomer", New String() {"Name", "Address", "Tel", "Mobile", "UserName"}, New String() {txtName.Text, txtAddress.Text, txtTel.Text, txtMob.Text, Md.UserName})
            SelectedName = txtName.Text
            Close()
        Catch ex As Exception
        End Try
    End Sub


End Class