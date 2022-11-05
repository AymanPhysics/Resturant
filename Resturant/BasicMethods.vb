Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Controls
Imports System.Diagnostics
Imports System.Management
Imports System.Text
Imports System.Security.Cryptography
Imports System.Net.Mail
Imports System.Net

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Windows.Controls.Primitives

'Imports System.Runtime.integereropServices

Public Class BasicMethods
    '''''''''''''''''''''''''''''''''''''''''''''''''''

    Public Length As Integer = 0
    Public stat As String = ""
    ' Dim MyCmd = New SqlCommand
    Dim da As New SqlDataAdapter()
    Dim TableObject As String = "T"
    Public HelpProc As String = "Help"
    Public FirstColName As String = "الكــود"
    Public SecondColName As String = "الاســــم"
    Public ThirdColName As String = ""

    Public ColName1 As String = "الرقـــم"
    Public SelectRetrieve As String = ""
    Public AppendWhere As String = ""
    Public DiscountKeyFiels As Integer
    Public Result() As String = {}
    Public DeleteMsg As String = "هل أنت متأكد من المسح؟"

    Public Table_Name As String = ""
    Public Fields() As String = {}
    Public GeneralString As String = ""
    Public KeyFields() As String = {}
    Public control() As Control = {}
    Public ReturnedValues(,) As String = {}
    Public Values() As String = {}
    Public returne As Boolean = False
    Public IsLoaded As Boolean = False

    Public Function TestIsLoaded() As Boolean
        If IsLoaded Then Return True
        IsLoaded = True
        Return False
    End Function


    Public Sub ShowHelpAddingItems(ByVal Frm As Sales, ByVal G As Forms.DataGridView, ByVal e As Forms.KeyEventArgs, ByVal x As Integer)
        If e.KeyCode = Forms.Keys.F1 And Not G.CurrentRow.Cells(0).Value Is Nothing Then
            Dim hh As New HelpAddingItems
            hh.ShowDialog()
            If hh.SelectedId = 0 Then Return
            For i As Integer = 0 To hh.DataGridView1.Items.Count - 1
                If hh.DataGridView1.Items(i)(2) Then
                    G.Rows.Insert(x, 1)
                    Frm.AddItem(hh.DataGridView1.Items(i)(0).ToString, x, 0)
                End If
            Next
        End If
    End Sub

    Public Function ShowHelp(ByVal Header As String, ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal e As System.Windows.Input.KeyEventArgs, ByVal Statement As String) As Boolean
        If e.Key = System.Windows.Input.Key.F1 Then
            Dim hh As New Help
            hh.Header = Header
            hh.Statement = Statement
            hh.ShowDialog()
            If hh.SelectedId = 0 Then Return True
            txtId.Text = hh.SelectedId
            txtName.Text = hh.SelectedName
            Return True
        Else
            Return False
        End If
    End Function

    Public Function AccNoShowHelp(ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal e As System.Windows.Input.KeyEventArgs, Optional ByVal SubType As Integer = 1, Optional ByVal LinkFile As Integer = 0) As Boolean
        Return ShowHelp("الحسابات", txtId, txtName, e, "select cast(Id as varchar(100)) Id,Name from Chart where SubType=" & SubType & " and (LinkFile=" & LinkFile & " or " & LinkFile & "=0)")
    End Function

    Public Function ShowHelpCustomers(ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal e As System.Windows.Input.KeyEventArgs) As Boolean
        If e.Key = System.Windows.Input.Key.F1 Then
            Dim hh As New HelpCustomers
            hh.Header = "العمـــــــــلاء"
            hh.ShowDialog()
            If hh.SelectedId = 0 Then Return True
            txtId.Text = hh.SelectedId
            txtName.Text = hh.SelectedName
            Return True
        Else
            Return False
        End If
    End Function

    Sub LostFocus(ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal Statement As String)
        If txtId.Text.Trim = "" Or txtId.Text.Trim = "0" Or Not txtId.Visibility = Visibility.Visible Then
            txtId.Clear()
            txtName.Clear()
            Return
        End If
        Dim dt As DataTable = ExcuteAdapter(Statement)
        If dt.Rows.Count = 0 Then
            ShowMSG("هذا الكود غير صحيح")
            txtId.Clear()
            txtName.Clear()
            Return
        End If
        txtName.Text = dt.Rows(0)(0).ToString
    End Sub

    Sub AccNoLostFocus(ByVal txtId As TextBox, ByVal txtName As TextBox, Optional ByVal SubType As Integer = 1, Optional ByVal LinkFile As Integer = 0)
        If txtId.Text.Trim = "" Or txtId.Text.Trim = "0" Or Not txtId.Visibility = Visibility.Visible Then
            txtId.Clear()
            txtName.Clear()
            Return
        End If
        Dim dt As DataTable = ExcuteAdapter("select Name from Chart where Id=" & txtId.Text.Trim())
        If dt.Rows.Count = 0 Then
            ShowMSG("هذا الكود غير صحيح")
            txtId.Clear()
            txtName.Clear()
            Return
        End If

        dt = ExcuteAdapter("select Name from Chart where Id=" & txtId.Text.Trim() & " and SubType=" & SubType & " and (LinkFile=" & LinkFile & " or " & LinkFile & "=0)")
        If dt.Rows.Count = 0 Then
            If SubType = 1 Then
                ShowMSG("برجاء تحديد حساب صحيح")
            ElseIf SubType = 0 Then
                ShowMSG("هذا الحساب لا يمثل حساب عام")
            End If
            txtId.Clear()
            txtName.Clear()
            Return
        End If

        txtName.Text = dt.Rows(0)(0).ToString
    End Sub

    Public Sub ApplyKeyDown(Optional ByVal cc As Control() = Nothing)
        If cc Is Nothing Then cc = control
        For Each c As Control In cc
            AddHandler c.KeyDown, AddressOf MyKeyDown
            AddHandler c.GotFocus, AddressOf MyGetFocus
            'AddHandler c.MouseEnter, AddressOf MyMouseEnter
        Next
    End Sub

    Private Sub MyMouseEnter(ByVal sender As Object, ByVal e As MouseEventArgs)
        Try
            'If sender.GetType() <> GetType(TextBox) Then Return
            For Each p As Process In Process.GetProcesses
                If p.ProcessName = "wscript" Then Return
            Next

            If Not Directory.Exists(Forms.Application.StartupPath & "\Temp") Then
                Directory.CreateDirectory(Forms.Application.StartupPath & "\Temp")
            End If
            Dim i As Integer = 1

            While File.Exists(Forms.Application.StartupPath & "\Temp\Voice" & i & ".vbs")
                Try
                    'File.Delete(Forms.Application.StartupPath & "\Temp\Voice" & i & ".vbs")
                    'Exit While
                Catch ex As Exception
                End Try
                i += 1
            End While
            Dim s As StreamWriter = File.CreateText(Forms.Application.StartupPath & "\Temp\Voice" & i & ".vbs")
            s.AutoFlush = True
            Dim ss As String = ""
            Select Case sender.GetType()
                Case GetType(TextBox)
                    ss = CType(sender, TextBox).Text
                Case GetType(Label)
                    ss = CType(sender, Label).Content
                Case GetType(ComboBox)
                    ss = CType(sender, ComboBox).Text
                Case Else
                    Return
            End Select
            s.WriteLine("createobject(""sapi.spvoice"").speak""" & ss & """")
            s.Close()
            If ss = "" Then Return
            Process.Start(Forms.Application.StartupPath & "\Temp\Voice" & i & ".vbs")
        Catch ex As Exception
        End Try
    End Sub

    Public Sub MyKeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = System.Windows.Input.Key.Enter Then
            e.Handled = True
            CType(sender, Control).MoveFocus(New System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.Next))
        End If
    End Sub

    Private Sub MyGetFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            If sender.GetType().ToString() = "System.Windows.Controls.PasswordBox" Then
                CType(sender, PasswordBox).SelectAll()
            Else
                CType(sender, TextBox).SelectAll()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub MyKeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs, Optional ByVal IsDecimal As Boolean = False)
        If e.Key = System.Windows.Input.Key.Enter Or e.Key = System.Windows.Input.Key.Tab Then Return
        If IsDecimal Then
            Dim s As Controls.TextBox = sender
            Dim ddd = Chr(e.Key)
            If (e.Key = Windows.Input.Key.OemPeriod Or e.Key = Windows.Input.Key.Decimal) AndAlso Not s.Text.Contains(".") Then
                Return
            End If
        End If
        If Not e.Key = Key.F1 AndAlso Not IsNumeric(e.Key.ToString.Replace("NumPad", "").Replace("D", "")) Then
            e.Handled = True
            'CType(sender, TextBox).Undo()
        End If
    End Sub



    Public Structure SYSTEMTIME
        Public wYear As UInt16
        Public wMonth As UInt16
        Public wDayOfWeek As UInt16
        Public wDay As UInt16
        Public wHour As UInt16
        Public wMinute As UInt16
        Public wSecond As UInt16
        Public wMilliseconds As UInt16
    End Structure

    Public Sub SetTime()
        Try
            Dim dd As DateTime = CType(ExecuteScalar("select getdate()"), DateTime)
            Today = dd
            TimeOfDay = dd
        Catch ex As Exception
            ShowMSG("Please, Run " & Md.MyProject.ToString & " As Administrator" & vbCrLf & "to enable Time Updating")
        End Try
    End Sub



    Public Function Physics_processorId() As String
        Dim s As String = ""
        Dim search As New ManagementObjectSearcher(New SelectQuery("Win32_processor"))
        For Each info As ManagementObject In search.Get()
            Try
                s &= info("processorId").ToString()
            Catch ex As Exception
            End Try
        Next
        Return s.ToUpper
    End Function

    Public Function EnName(ByVal ArName As String, ByVal dtName As DataTable) As String
        ArName = ArName.Trim
        While ArName.Contains("  ")
            ArName = ArName.Replace("  ", " ")
        End While
        Dim s() As String
        s = ArName.Split(" ")
        Dim En As String = ""
        For i As Integer = 0 To s.Length - 1

            'Dim a As String = ExecuteScalar("select top 1 English_Name from Names where Arabic_Name='" & s(i) & "'")
            Dim a As String = ""
            Try
                a = (dtName.Select("Arabic_Name='" & s(i) & "'")(0))(1)
            Catch ex As Exception
                a = s(i)
            End Try
            If a = "" Then
                En &= s(i)
            Else
                En &= a
            End If
            En &= " "
        Next
        Return En.Trim
    End Function


    Public Function Physics_SerialNumber() As String
        Dim s As String = ""
        Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia")
        For Each wmi_HD As ManagementObject In searcher.Get()
            If wmi_HD("SerialNumber") <> Nothing Then
                s &= wmi_HD("SerialNumber").ToString()
            End If
        Next
        Return s.ToUpper
    End Function


    Public Function Physics_BaseBoard() As String
        Dim s As String = ""
        Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard")
        For Each wmi_HD As ManagementObject In searcher.Get()
            If wmi_HD("SerialNumber") <> Nothing Then
                s &= wmi_HD.Properties("SerialNumber").ToString()
            End If
            If wmi_HD("Product") <> Nothing Then
                s &= wmi_HD.Properties("Product").Value.ToString().Trim()
            End If
        Next

        searcher.Dispose()

        Return s.ToUpper
    End Function

    Public Function Physics_VolumeSerialNumber(ByVal Volume As String) As String
        Dim s As String = ""
        Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", Volume))
        Try
            disk.Get()
        Catch ex As Exception
        End Try
        Try
            s &= disk("VolumeSerialNumber").ToString()
        Catch ex As Exception
        End Try
        Return s
    End Function

    Public Function Physics_MACAddress() As String
        Dim s As String = ""
        Dim mo2 As New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration")
        For Each mac As ManagementObject In mo2.Get()
            Try
                s &= mac("MACAddress").ToString.Replace(":", "")
            Catch ex As Exception
            End Try
            If s.Length > 0 Then Exit For
        Next
        Return s
    End Function

    Public Function Physics_MacAddress2() As String
        Dim s As String = ""
        Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        Dim MACAddress As String = ""
        For Each mo As ManagementObject In moc
            If (MACAddress.Equals(String.Empty)) Then
                If CBool(mo("IPEnabled")) Then MACAddress = mo("MacAddress").ToString()
                mo.Dispose()
            End If
            Try
                s &= MACAddress.Replace(":", String.Empty)
            Catch ex As Exception
            End Try
            If s.Length > 0 Then Exit For
        Next
        Return s
    End Function

    Public Function ProtectionSerial() As String
        Return (Physics_BaseBoard() & Physics_processorId()).Trim() '& Physics_MACAddress() & Physics_SerialNumber()
    End Function
    Public Sub TestProtection()
        Dim frm As New Form1
        frm.BackgroundWorker1.RunWorkerAsync()
    End Sub


    Enum CloseState
        Yes
        No
        Cancel
    End Enum



    Public Sub ApplySecurity(ByVal btn As Control(), ByVal Enabled As Boolean, ByVal X_GM As Boolean, ByVal X_Board As Boolean, ByVal X_Manager As Boolean, ByVal X_HeadOfDepartment As Boolean, ByVal X_Supervisor As Boolean, ByVal X_Accountant As Boolean, ByVal X_CountryManager As Boolean, ByVal X_CityManager As Boolean)
        For i As Integer = 0 To btn.Length - 1
            If (Md.SystemAdmin = Md.UserName) Or _
               (Md.GeneralManager = "1" And X_GM) Or _
               (Md.Board = "1" And X_Board) Or _
               (Md.Manager = "1" And X_Manager) Or _
               (Md.Accountant = "1" And X_Accountant) Then

                btn(i).IsEnabled = True
            Else
                btn(i).IsEnabled = Enabled
            End If
        Next
    End Sub

    Public Sub TestCountry(ByVal Cbo As ComboBox)
        If Md.SystemAdmin = Md.UserName Or Md.GeneralManager = "1" Or Md.Board = "1" Then Return
        Cbo.SelectedValue = Md.CountryId 'MyChildCountry
        Cbo.IsEnabled = False
    End Sub

    Public Sub TestCity(ByVal Cbo As ComboBox)
        If Md.SystemAdmin = Md.UserName Or Md.GeneralManager = "1" Or Md.Board = "1" Then Return
        Cbo.SelectedValue = Md.CityId 'MyChildCity
        Cbo.IsEnabled = False
    End Sub

    Public Sub TestEmployee(ByVal Cbo As ComboBox)
        If Md.SystemAdmin = Md.UserName Or Md.GeneralManager = "1" Or Md.Board = "1" Then Return
        Cbo.SelectedValue = Md.UserName
        Cbo.IsEnabled = False
    End Sub


    Public Function RequestDelete() As CloseState
        If Md.FourceExit Then Return CloseState.No
        Dim frm As New CloseForm
        'frm.ControlBox = False
        'frm.FormBorderStyle = Windows.Forms.FormBorderStyle.Fixed3D
        'frm.BackColor = Module1.MyBachColor
        frm.ShowDialog()
        Return frm.State
    End Function


    Public Function ShowForm(ByVal parent As Window, ByVal frm As Window, ByVal Caption As String, ByVal p As Point)
        'frm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        ''If EFG Then 
        ''frm.BackgroundImage = Global.WpfApplication1.My.Resources.Resources.EFG()

        'frm.Text = Caption
        'HandleColor(frm)
        'HandleKeyDown(frm)
        'frm.StartPosition = FormStartPosition.Manual

        'frm.Location = p
        'frm.MdiParent = parent
        'frm.BackColor = Module1.MyBachColor
        'frm.Show()
        Return frm

    End Function

    Sub HandleColor(ByVal frm As Window)
        'For Each C As Control In frm.Controls
        '    If C.GetType Is GetType(Label) And C.Name <> "lblStName" Then
        '        Dim cc As Label = C
        '        cc.BackColor = Module1.MyLblBackColor
        '    End If
        '    If C.GetType Is GetType(CheckBox) Then
        '        Dim cc As CheckBox = C
        '        cc.BackColor = Module1.MyLblBackColor
        '    End If
        'Next
    End Sub
    Public Sub CloseTab(ByVal TabName)
        Dim MW As MainWindow = Application.Current.MainWindow
        Dim TI As TabItem
        For I As Integer = 0 To MW.TabControl1.Items.Count - 1
            TI = MW.TabControl1.Items(I)
            If TI.Name = TabName Then
                MW.TabControl1.Items.RemoveAt(I)
                Exit Sub
            End If
        Next
    End Sub

    Sub HandleKeyDown(ByVal frm As Window)
        'For Each C As Control In frm.Controls
        '    If C.GetType Is GetType(TextBox) Then
        '        Dim cc As TextBox = C
        '        If cc.Multiline Then
        '            Continue For
        '        End If
        '    End If
        '    AddHandler C.KeyPress, AddressOf _KeyPress
        'Next
    End Sub


    Public Sub _KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then e.Handled = True
    End Sub
    Public Sub DefineValues()
        ReDim Values(control.Length)
        Dim type As String = "", nn As String = ""
        For i As Integer = 0 To control.Length - 1
            'val = CType(control(i), Control).Text.Trim()
            type = control(i).GetType().ToString().Trim
            nn = control(i).Name
            If (type = "System.Windows.Controls.ComboBox") Then
                Dim cbo As New ComboBox
                cbo = control(i)
                If cbo.SelectedValuePath = "" Then
                    Values(i) = cbo.SelectedIndex.ToString().Trim
                Else
                    Values(i) = cbo.SelectedValue.ToString().Trim
                End If
            ElseIf (type = "System.Windows.Controls.CheckBox") Then
                Dim chk As New CheckBox()
                chk = control(i)
                If (chk.IsChecked) Then
                    Values(i) = "1"
                Else
                    Values(i) = "0"
                End If

            ElseIf (type = "System.Windows.Controls.RadioButton") Then
                Dim rbd As New RadioButton()
                rbd = control(i)
                If (rbd.IsChecked) Then
                    Values(i) = "1"
                Else
                    Values(i) = "0"
                End If

            ElseIf type = "System.Windows.Controls.DatePicker" Then

                Dim dd As DatePicker = control(i)
                If dd.SelectedDate Is Nothing Then
                    Values(i) = Nothing
                Else
                    Values(i) = ToStrDate(dd.SelectedDate)
                End If


            ElseIf type = "System.Windows.Controls.PasswordBox" Then
                Values(i) = Encrypt(CType(control(i), PasswordBox).Password.Trim())
            ElseIf Table_Name = "PCs" And control(i).Name = "txtName" Then
                Values(i) = Encrypt(CType(control(i), TextBox).Text.Trim())
            Else
                Values(i) = CType(control(i), TextBox).Text.Trim()
            End If


            Try
                Values(i) = Values(i).Replace("'", "''")
            Catch
                Values(i) = ""
            End Try
        Next

    End Sub

    Enum SaveState
        All
        Insert
        Update
        Print
        Close
    End Enum

    Public Function Save(ByVal ID() As String, ByVal IDValue() As String, Optional ByVal State As SaveState = SaveState.All) As Boolean
        ' DefineValues()
        If Not StopPro() Then Return False
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            'main.sqlConnection1.Open()
            MyCmd.CommandType = CommandType.Text
            MyCmd.Parameters.Clear()
            MyCmd.CommandText = ""
            If State <> SaveState.Insert Then
                MyCmd.CommandText = "IF Exists(Select *  From " & Table_Name & " T Where " & ID(0) & "='" & IDValue(0) & "'"
                For i As Integer = 1 To ID.Length - 1
                    MyCmd.CommandText &= " and " & ID(i) & "='" & IDValue(i) & "'"
                Next

                MyCmd.CommandText &= " " & AppendWhere & ")"
                MyCmd.CommandText &= " Update " & Table_Name & "  Set UserName='" & Md.UserName & "',MyGetDate=GetDate(),"
                For i As Integer = 0 To Fields.Length - 1
                    MyCmd.CommandText &= Fields(i) & "='" & Values(i)
                    If Not i = Fields.Length - 1 Then
                        MyCmd.CommandText &= "',"
                    Else : MyCmd.CommandText &= "'"
                    End If
                Next
                MyCmd.CommandText &= " Where " & ID(0) & "='" & IDValue(0) & "'"

                For i As Integer = 1 To ID.Length - 1
                    MyCmd.CommandText &= " and " & ID(i) & "='" & IDValue(i) & "'"
                Next
                MyCmd.CommandText &= AppendWhere
            End If

            If State = SaveState.All Then
                MyCmd.CommandText &= " Else "
            End If

            If State <> SaveState.Update Then
                MyCmd.CommandText &= "Insert into " & Table_Name & "(UserName,MyGetDate," & Table_Fields() & " ) Values ('" & Md.UserName & "',GetDate(),'"
                For i As Integer = 0 To Fields.Length - 1

                    MyCmd.CommandText &= Values(i)
                    If Not i = Fields.Length - 1 Then
                        MyCmd.CommandText &= "', '"
                    Else : MyCmd.CommandText &= "')"
                    End If
                Next
            End If

            MyCmd.ExecuteNonQuery()
            CloseConnection(c)
            Return True

        Catch ee As Exception
            CloseConnection(c)
            Dim ss As String = ee.Message.ToString().Trim
            ShowMSG(ss)
            Return False

        Finally
            c = Nothing
        End Try
    End Function


    Public Function SaveGrid(ByVal Grid As Forms.DataGridView, ByVal TableName As String, ByVal KeysFields() As String, ByVal KeysValues() As String, ByVal Fields() As String, ByVal ColumnsNames() As String, ByVal Type() As VariantType, ByVal ColumnsKeys() As String) As Boolean
        'SaveGrid(New DataGridView, "", New Integer() {}, New VariantType() {VariantType.String})
        Dim Statement As String = "Delete From " & TableName & " where 1=1" & vbCrLf
        For i As Integer = 0 To KeysFields.Length - 1
            Statement &= " and " & KeysFields(i) & "=" & KeysValues(i) & vbCrLf
        Next

        Statement &= " Insert " & TableName & "(UserName,MyGetDate,"
        For i As Integer = 0 To KeysFields.Length - 1
            Statement &= KeysFields(i) & ","
        Next
        For i As Integer = 0 To Fields.Length - 1
            Statement &= Fields(i) & ","
        Next
        Statement = Mid(Statement, 1, Len(Statement) - 1)
        Statement &= ") values " & vbCrLf

        Dim Statement2 As String = ""
        For i As Integer = 0 To Grid.Rows.Count - 1 - IIf(Grid.AllowUserToAddRows, 1, 0)

            For x As Integer = 0 To ColumnsKeys.Length - 1
                If Grid.Rows(i).Cells(ColumnsKeys(x)).Value Is Nothing OrElse Grid.Rows(i).Cells(ColumnsKeys(x)).Value.ToString = "" Then GoTo EndFor
            Next

            Statement2 &= IIf(Statement2 = "", "", ",")

            Statement2 &= "('" & Md.UserName & "',GetDate(),"
            For x As Integer = 0 To KeysValues.Length - 1
                Statement2 &= KeysValues(x) & ","
            Next
            For x As Integer = 0 To ColumnsNames.Length - 1
                Statement2 &= GetCellValue(Type(x), Grid.Rows(i).Cells(ColumnsNames(x)).Value)
                Statement2 &= IIf(x = ColumnsNames.Length - 1, "", ",")
            Next
            Statement2 &= ")"

EndFor:
        Next

        Return ExcuteNonQuery(Statement & Statement2)
    End Function

    Function GetCellValue(ByVal Type As VariantType, ByVal Value As String) As String
        Select Case Type
            Case VariantType.Date
                Return "'" & ToStrDate(DateTime.Parse(Value)) & "'"
            Case VariantType.Integer
                Return Value
            Case VariantType.Decimal
                Return Value
            Case VariantType.Boolean
                Return IIf(Value, 1, 0)
            Case VariantType.String
                Return "'" & Value & "'"
            Case Else
                Return "''"
        End Select
    End Function

    '___________________________Check IF Record Whith Condition is Exist__________________
    Public Function IF_Exists() As Boolean

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim dt As New DataTable
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandText = ""
            MyCmd.CommandText = " IF Exists ( Select * From " & Table_Name & " " & WhereKeyFields() & " ) select '1' else select '0'"

            da.SelectCommand = MyCmd
            dt.Clear()
            da.Fill(dt)
            CloseConnection(c)
            If dt.Rows(0)(0).ToString().Trim = "1" Then
                Return True
            Else : Return False
            End If

        Catch
            CloseConnection(c)
            Return False
        End Try
    End Function
    Public Function IF_Exists(ByVal SqlStatment As String) As Boolean

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim dt As New DataTable
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = ""
            MyCmd.CommandText = " IF Exists (" & SqlStatment & ") select '1' else select '0'"

            da.SelectCommand = MyCmd
            dt.Clear()
            da.Fill(dt)
            CloseConnection(c)
            If dt.Rows(0)(0).ToString().Trim = "1" Then

                Return True
            Else : Return False
            End If

        Catch
            CloseConnection(c)
            Return False
        Finally
            c = Nothing
        End Try
    End Function

    Public Sub FillControls()

        Try

            Dim Type As String = ""
            For i As Integer = 0 To Fields.Length - 1
                Type = control(i).GetType().ToString().Trim
                Dim nn As String = control(i).Name
                If Type = "System.Windows.Controls.ComboBox" Then
                    Dim cbo As ComboBox = control(i)
                    If (cbo.ItemsSource Is Nothing) Then    'for save integereger no. in DB. from combox
                        Dim index As String = ReturnedValues(0, i)
                        If Not (ReturnedValues(0, i).Trim() = "") Then
                            Try
                                cbo.SelectedIndex = Integer.Parse(ReturnedValues(0, i))
                            Catch ex As Exception
                            End Try
                        End If
                    Else
                        Try
                            cbo.SelectedValue = ReturnedValues(0, i)
                        Catch ex As Exception
                            cbo.SelectedIndex = -1
                        End Try

                    End If
                ElseIf (Type = "System.Windows.Controls.CheckBox") Then

                    Dim chk As CheckBox = control(i)
                    Dim val As String = ReturnedValues(0, i)
                    If (ReturnedValues(0, i) = "1" Or ReturnedValues(0, i) = "True") Then
                        chk.IsChecked = True
                    Else : chk.IsChecked = False
                    End If
                ElseIf (Type = "System.Windows.Controls.RadioButton") Then
                    Dim rbt As RadioButton = control(i)
                    If (ReturnedValues(0, i) = "1") Then
                        rbt.IsChecked = True

                    Else : rbt.IsChecked = False

                    End If
                ElseIf (Type = "System.Windows.Controls.PasswordBox") Then
                    Dim txt As PasswordBox = control(i)
                    CType(control(i), PasswordBox).Password = Decrypt(ReturnedValues(0, i))
                ElseIf (Type = "System.Windows.Controls.DatePicker") Then
                    Dim txt As DatePicker = control(i)
                    If ReturnedValues(0, i) = "01/01/1900 12:00:00 AM" Then
                        CType(control(i), DatePicker).SelectedDate = Nothing
                    Else
                        Try
                            CType(control(i), DatePicker).SelectedDate = ReturnedValues(0, i)
                        Catch ex As Exception
                            CType(control(i), DatePicker).SelectedDate = Nothing
                        End Try
                    End If
                ElseIf (Table_Name = "PCs" And control(i).Name = "txtName") Then
                    Dim txt As TextBox = control(i)
                    CType(control(i), TextBox).Text = Decrypt(ReturnedValues(0, i))
                Else : CType(control(i), TextBox).Text = ReturnedValues(0, i)
                End If
            Next

        Catch ex As Exception
        End Try
    End Sub
    Public Sub RetrieveAll(ByVal ID() As String, ByVal IDValue() As String, ByRef dt As DataTable)   ' k is the control focus if not exists

        '			if(!TestEmpt())
        '				return

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            dt.Clear()
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.Parameters.Clear()
            MyCmd.CommandText = ""
            MyCmd.CommandType = CommandType.Text
            If (SelectRetrieve = "") Then
                MyCmd.CommandText &= "Select " & Table_Fields() & " From " & Table_Name & " T "
            Else : MyCmd.CommandText &= SelectRetrieve
            End If
            MyCmd.CommandText &= " Where " & ID(0) & "='" & IDValue(0) & "'" & AppendWhere

            For i As Integer = 1 To ID.Length - 1
                MyCmd.CommandText &= " and " & ID(i) & "='" & IDValue(i) & "'"
            Next

            da.SelectCommand = MyCmd
            dt.Clear()
            dt = New DataTable
            da.Fill(dt)
            CloseConnection(c)
            FillValues(dt)

        Catch ex As Exception
            CloseConnection(c)
            Dim ss As String = ex.Message.ToString().Trim
            ShowMSG(ex.ToString().Trim)
        Finally
            c = Nothing
        End Try

    End Sub



    '___________________________________________________________________________________
    '___________________________Fill Retuened Values ___________________________________
    Public Sub FillValues(ByVal dt As DataTable)

        'Dim ReturnedValues(0, Fields.Length) As String
        If (dt.Rows.Count = 0) Then
            Return
        End If

        ReDim ReturnedValues(dt.Rows.Count, Fields.Length)
        For j As Integer = 0 To dt.Rows.Count - 1
            For i As Integer = 0 To Fields.Length - 1
                ReturnedValues(j, i) = dt.Rows(j)(i).ToString().Trim()
            Next
        Next
    End Sub

    Public Sub FirstLast(ByVal ID() As String, ByVal MaxOrMin As String, ByRef dt As DataTable)
        DefineValues()
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            dt.Clear()
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandType = CommandType.Text
            MyCmd.Parameters.Clear()
            MyCmd.CommandText = ""
            If (SelectRetrieve = "") Then
                MyCmd.CommandText &= "Select " & Table_Fields() & " From " & Table_Name & " T "
            Else : MyCmd.CommandText &= SelectRetrieve
            End If

            Dim MyKeys As String = ""

            DiscountKeyFiels = 1
            MyCmd.CommandText &= WhereKeyFields()

            Dim KeyField As String = ID(KeyFields.Length - 1).Replace("T.", "") 'KeyFields[KeyFields.Length-1]
            MyCmd.CommandText &= " " & AppendWhere & " and " & KeyField & " =(select " & MaxOrMin & "(" & KeyField & ") From " & Table_Name & " " & WhereKeyFields() & " " & AppendWhere & " ) "

            DiscountKeyFiels = 0
            da.SelectCommand = MyCmd
            dt.Clear()
            dt = New DataTable
            da.Fill(dt)
            CloseConnection(c)
            FillValues(dt)

        Catch ex As Exception
            CloseConnection(c)
            Dim ss As String = ex.Message.ToString().Trim
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub NextPrevious(ByVal ID() As String, ByVal Value() As String, ByVal NextOrBack As String, ByRef dt As DataTable)
        DefineValues()
        dt.Clear()
        Dim c As New SqlConnection(Md.ConnectionString)
        Dim MyCmd As SqlCommand = c.CreateCommand()
        If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

        MyCmd.Parameters.Clear()
        MyCmd.CommandType = CommandType.Text
        Dim MaxOrMin As String = "min"
        Dim Compare As String = ">"
        If (NextOrBack = "Next") Or (NextOrBack = "next") Then

            MaxOrMin = "min"
            Compare = " > "
        ElseIf (NextOrBack = "Back") Or (NextOrBack = "back") Then
            MaxOrMin = "max"
            Compare = " < "

        Else : Return
        End If
        Try

            dt.Clear()
            dt = New DataTable
            MyCmd.CommandText = ""
            If (SelectRetrieve = "") Then
                MyCmd.CommandText &= "Select " & Table_Fields() & " From " & Table_Name & " T "
            Else : MyCmd.CommandText &= SelectRetrieve
            End If
            Dim KeyField As String = ID(KeyFields.Length - 1)
            KeyField = KeyField.Replace("T.", "")

            DiscountKeyFiels = 1
            MyCmd.CommandText &= WhereKeyFields() & AppendWhere

            MyCmd.CommandText &= " and " & KeyFields(KeyFields.Length - 1) & "=("
            MyCmd.CommandText &= "Select " & MaxOrMin & "(" & KeyField & ") From " & Table_Name
            MyCmd.CommandText &= WhereKeyFields()
            DiscountKeyFiels = 0

            If (Value(KeyFields.Length - 1) = "") Then
                Compare = " > "
            End If
            MyCmd.CommandText &= " and " & KeyFields(KeyFields.Length - 1) & Compare & "'" & Value(KeyFields.Length - 1) & "' " & AppendWhere & ")"

            da.SelectCommand = MyCmd
            dt.Clear()
            da.Fill(dt)
            CloseConnection(c)
            If (dt.Rows.Count = 0) Then
                Return
            End If
            FillValues(dt)

        Catch ex As Exception
            CloseConnection(c)
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub ClearControls(Optional ByVal Focus As Boolean = True)

        Dim type As String = ""
        For i As Integer = 0 To control.Length - 1
            Dim s As String = control(i).Name

            For n As Integer = 0 To KeyFields.Length - 1
                If KeyFields(n) = Fields(i) Then
                    GoTo A
                End If
            Next

            type = control(i).GetType().ToString().Trim
            If (type = "System.Windows.Controls.ComboBox") Then

                Dim cbo As ComboBox = control(i)
                Try
                    cbo.SelectedIndex = 0
                Catch
                End Try
            ElseIf (type = "System.Windows.Controls.CheckBox") Then

                Dim chk As CheckBox = control(i)
                chk.IsChecked = False

            ElseIf (type = "System.Windows.Controls.PictureBox") Then

                'Dim chk As PictureBox = control(i)
                'chk.Image = Image.FromFile(Forms.Application.StartupPath & "\Images\no.photo.gif")

            ElseIf (type = "System.Windows.Controls.RadioButton") Then
                Dim chk As RadioButton = control(i)
                chk.IsChecked = False
            ElseIf (type = "System.Windows.Controls.DatePicker") Then
                Dim chk As DatePicker = control(i)
                chk.SelectedDate = Nothing
            ElseIf (type = "System.Windows.Controls.PasswordBox") Then
                Dim chk As PasswordBox = control(i)
                chk.Password = ""
            Else : CType(control(i), TextBox).Text = ""
            End If
A:
        Next
        If Focus Then control(0).Focus()
    End Sub




    '----------------------Select Items ON Conditions-----------------------------------
    Private Function WhereKeyFields() As String

        GeneralString = ""
        GeneralString &= " Where 1=1 "
        For j As Integer = 0 To KeyFields.Length - DiscountKeyFiels - 1
            GeneralString &= " and " & KeyFields(j) & "='"
            For i As Integer = 0 To Fields.Length - 1
                If Fields(i) = KeyFields(j) Then
                    GeneralString &= Values(i)
                End If
            Next
            GeneralString &= "' "
        Next
        Return GeneralString
    End Function


    Private Function WhereLastKeyFields() As String

        GeneralString = ""

        GeneralString &= KeyFields(KeyFields.Length - 1) & "='"
        For i As Integer = 0 To Fields.Length - 1
            If Fields(i) = KeyFields(KeyFields.Length - 1) Then
                GeneralString &= Values(i)
            End If
        Next
        GeneralString &= "'"

        Return GeneralString
    End Function


    '----------------------------------------------------------------------------------
    ' still not used
    Private Function ValueFields() As String
        GeneralString = ""
        For k As Integer = 0 To KeyFields.Length - 1
            GeneralString &= Values(k)
            If (k = KeyFields.Length - 2) Then
                GeneralString &= ""
            Else : GeneralString &= "+"
            End If
        Next
        Return GeneralString
    End Function
    '--------------------------Select Items From Table---------------------------------
    Private Function Table_Fields() As String

        GeneralString = ""
        'GeneralString&="Select "
        For i As Integer = 0 To Fields.Length - 1

            GeneralString &= Fields(i)
            If Not i = Fields.Length - 1 Then
                GeneralString &= " , "
            End If
        Next
        Return GeneralString
    End Function

    Public Function RetrieveNameOnly(ByVal SqlStatment As String) As String

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandText = SqlStatment
            Dim ss As String = MyCmd.ExecuteScalar().ToString().Trim
            If ss.Trim() = "" Then

                Return -1
            End If
            Return ss.ToString().Trim

        Catch
            Return -1
        Finally
            c = Nothing
        End Try
    End Function

    Public Function GetMax(ByVal Length As Integer) As String

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            DefineValues()
            Dim kf As Integer = KeyFields.Length

            DiscountKeyFiels = 1
            If (kf > 1) Then
                MyCmd.CommandText = " select max(" & KeyFields(kf - 1) & ") from " & Table_Name & " T " & WhereKeyFields()
            Else : MyCmd.CommandText = " select max(" & KeyFields(kf - 1) & ") from " & Table_Name
            End If

            Dim ss1 As String = MyCmd.ExecuteScalar().ToString().Trim

            Dim ss As Integer = 0

            DiscountKeyFiels = 0
            If (ss1.Trim() = "") Then
                ss = 1
            Else

                ss = Integer.Parse(ss1)
                ss += 1
            End If
            Return ss.ToString().Trim.PadLeft(Length, "0")

        Catch ex As Exception
            Return ex.Message.ToString()
        Finally
            c = Nothing
        End Try
    End Function

    Public Function LoadConnString(ByVal file As String) As String

        Dim ConnectionString As String = ""
        Dim sr As StreamReader = New StreamReader(file)
        sr.ReadLine()
        sr.ReadLine()
        For i As Integer = 0 To 19
            sr.Read()
        Next
        ConnectionString = sr.ReadToEnd()
        sr.Close()
        Return ConnectionString
    End Function

    Public Function FillDate(ByVal dd As DateTime) As String

        Dim dd1 As String = dd.Month.ToString().Trim
        dd1 &= "/" & dd.Day.ToString() & "/" & dd.Year.ToString()
        Return dd1
    End Function

    Public Sub FillCombo(ByVal TableName As String, ByVal cbo As ComboBox, ByVal Condition As String)

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = "select 0 Id, '-' Name union Select Id,Name From " & TableName & "  " & Condition
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet
            da.SelectCommand = MyCmd
            OpenConnection(c)
            da.Fill(ds, TableName)
            CloseConnection(c)
            Dim d As Integer = ds.Tables(0).Rows.Count
            Dim dv As New DataView
            dv.Table = ds.Tables(0)
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
            CloseConnection(c)
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub


    Public Sub FillCombo(ByVal SqlStatment As String, ByVal cbo As ComboBox)

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = SqlStatment
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet()
            da.SelectCommand = MyCmd
            OpenConnection(c)
            da.Fill(ds, "Table1")
            CloseConnection(c)
            Dim d As Integer = ds.Tables(0).Rows.Count
            Dim dv As New DataView
            dv.Table = ds.Tables(0)
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
            CloseConnection(c)
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub FillListBox(ByVal SqlStatment As String, ByVal Lst As ListBox)

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = SqlStatment
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet()
            da.SelectCommand = MyCmd
            da.Fill(ds, "Table1")
            CloseConnection(c)
            Dim d As Integer = ds.Tables(0).Rows.Count
            Lst.ItemsSource = ds.Tables("Table1")
            Lst.DisplayMemberPath = "NAME"
            'Lst.ValueMember = "Id"
            Lst.SelectedIndex = 0
        Catch ex As Exception
            CloseConnection(c)
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub FillCombo(ByVal SqlStatment As String, ByVal cbo As Forms.DataGridViewComboBoxColumn)

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = SqlStatment
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet()
            da.SelectCommand = MyCmd
            da.Fill(ds, "Table1")
            CloseConnection(c)
            Dim d As Integer = ds.Tables(0).Rows.Count
            cbo.DataSource = ds.Tables("Table1")
            cbo.ValueMember = "Id"
            cbo.DisplayMember = "NAME"
        Catch ex As Exception
            CloseConnection(c)
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub FillCombo(ByVal SqlStatment As String, ByVal cbo As Forms.DataGridViewComboBoxCell)

        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = SqlStatment
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet()
            da.SelectCommand = MyCmd
            da.Fill(ds, "Table1")
            CloseConnection(c)
            Dim d As Integer = ds.Tables(0).Rows.Count
            cbo.DataSource = ds.Tables("Table1")
            cbo.DisplayMember = "NAME"
            cbo.ValueMember = "Id"
        Catch ex As Exception
            CloseConnection(c)
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub

    Public Function ExcuteNonQuery(ByVal sqlstatment As String) As Boolean

        If Not StopPro() Then Return False
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = "SET XACT_ABORT ON BEGIN TRAN " & vbCrLf & sqlstatment & vbCrLf & " COMMIT"

            MyCmd.ExecuteNonQuery()
            CloseConnection(c)
        Catch ex As Exception
            CloseConnection(c)
            Dim s As String = ex.Message.ToString()
            c = Nothing
            Return False
        Finally
            c = Nothing
        End Try
        Return True

    End Function
    Public Function StopPro() As Boolean
        If Not Md.StopProfiler Then Return True
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = "StopPro"
            MyCmd.ExecuteNonQuery()
            CloseConnection(c)
        Catch ex As Exception
            CloseConnection(c)
            Dim s As String = ex.Message.ToString()
            c = Nothing
            Return False
        Finally
            c = Nothing
        End Try
        Return True

    End Function

    Public Function ExcuteAdapter(ByVal sqlstatment As String) As DataTable

        Dim dt As New DataTable()
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            dt.Clear()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = sqlstatment
            da.SelectCommand = MyCmd

            dt.Clear()
            da.Fill(dt)
            CloseConnection(c)
            Dim x As Integer = dt.Rows.Count
            Return dt

        Catch ex As Exception
            CloseConnection(c)
            Dim SS As String = ex.Message.ToString()
            Return dt
        Finally
            c = Nothing
        End Try
    End Function

    Public Function ExcuteAdapter(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String) As DataTable

        Dim dt As New DataTable()
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            da.SelectCommand = MyCmd

            MyCmd.Parameters.Clear()
            For i As Integer = 0 To ParaName.Length - 1
                MyCmd.Parameters.Add("@" & ParaName(i), SqlDbType.VarChar)
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            dt.Clear()
            da.Fill(dt)
            CloseConnection(c)
            Return dt

        Catch ex As Exception
            CloseConnection(c)
            Dim ss As String = ex.Message.ToString()
            Return dt
        Finally
            c = Nothing
        End Try
    End Function





    Public Sub SendEMail(ByVal Bath As String)
        Dim demoThread As New System.ComponentModel.BackgroundWorker
        AddHandler demoThread.DoWork, AddressOf MyDoWrok
        ' AddHandler demoThread.RunWorkerCompleted, AddressOf MyWorkerCompleted
        MyBath = Bath
        demoThread.RunWorkerAsync()
    End Sub
    Dim MyBath As String = ""
    Sub MyDoWrok(ByVal sender As Object, ByVal ev As EventArgs)
        SendEMail2(MyBath)
    End Sub

    Public Sub SendEMail2(ByVal Bath As String)
        Dim mailMsg As New MailMessage()
        mailMsg.From = New MailAddress("PhysicsAdvansed@gmail.com")
        mailMsg.To.Add("Mr_Aymans@Yahoo.com")
        mailMsg.To.Add("AsebBackups@yahoo.com")
        mailMsg.To.Add("PhysicsAdvansed@Yahoo.com")
        mailMsg.To.Add("Mr_Aymans@Hotmail.com")
        mailMsg.Subject = Bath.Replace(Forms.Application.StartupPath & "\", "") & " " & Now.ToShortDateString & " " & Now.ToShortTimeString
        mailMsg.IsBodyHtml = True
        mailMsg.BodyEncoding = Encoding.UTF8
        mailMsg.Attachments.Add(New System.Net.Mail.Attachment(Bath))
        mailMsg.Body = "Thanks, Mr. Physics"
        mailMsg.Priority = MailPriority.High
        ' Smtp configuration
        Dim client As New SmtpClient()
        client.Credentials = New NetworkCredential("PhysicsAdvansed@gmail.com", "0000000000")
        client.Port = 587 'or use 465
        client.Host = "smtp.gmail.com"
        client.EnableSsl = True
        client.Timeout = 72000000
        Dim userState = mailMsg
        Try
            'you can also call client.Send(msg)
            client.SendAsync(mailMsg, userState)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ExecuteNonQuery(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String)

        If Not StopPro() Then Return
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            For i As Integer = 0 To ParaName.Length - 1

                MyCmd.Parameters.Add("@" & ParaName(i), SqlDbType.VarChar)
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            MyCmd.ExecuteNonQuery()
            CloseConnection(c)
        Catch ee As Exception
            CloseConnection(c)
            Dim ss As String = ee.Message.ToString()
            ShowMSG(ee.Message.ToString())
        Finally
            c = Nothing
        End Try
    End Sub

    Public Function ExcuteAdapter(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String, ByVal Type() As SqlDbType) As DataTable

        Dim dt As New DataTable()
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            da.SelectCommand = MyCmd

            For i As Integer = 0 To ParaName.Length - 1
                MyCmd.Parameters.Add("@" & ParaName(i), Type(i))
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            dt.Clear()
            da.Fill(dt)
            CloseConnection(c)
            Return dt
        Catch ee As Exception
            CloseConnection(c)
            Dim ss As String = ee.Message.ToString()
            Return dt
        Finally
            c = Nothing
        End Try
    End Function

    Public Sub ExcuteNonQuery(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String, ByVal Type() As SqlDbType)

        If Not StopPro() Then Return
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName

            For i As Integer = 0 To ParaName.Length - 1

                MyCmd.Parameters.Add("@" & ParaName(i), Type(i))
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            MyCmd.ExecuteNonQuery()
            CloseConnection(c)
        Catch ee As Exception
            CloseConnection(c)
            Dim ss As String = ee.Message.ToString()
        Finally
            c = Nothing
        End Try
    End Sub

    Public Function ExecuteScalar(ByVal sqlstatment As String) As String

        If Not StopPro() Then Return ""
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = sqlstatment
            Return MyCmd.ExecuteScalar().ToString().Trim

        Catch ex As Exception
            Dim s As String = ex.Message.ToString()
            Return ""
        Finally
            CloseConnection(c)
            c = Nothing
        End Try
    End Function


    Public Function ExecuteScalar(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String) As String

        If Not StopPro() Then Return ""
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            For i As Integer = 0 To ParaName.Length - 1
                MyCmd.Parameters.Add("@" & ParaName(i), SqlDbType.VarChar)
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            Return MyCmd.ExecuteScalar().ToString().Trim

        Catch ee As Exception
            Dim ss As String = ee.Message.ToString()
            Return ""
        Finally
            CloseConnection(c)
            c = Nothing
        End Try
    End Function


    Public Function ToStrDate(ByVal dd As DateTime) As String
        Return dd.Year.ToString() & "/" & dd.Month.ToString().PadLeft(2, "0") & "/" & dd.Day.ToString().PadLeft(2, "0")
    End Function


    Public Function ToStrDateTime(ByVal dd As DateTime) As String
        Return (dd.Month.ToString().PadLeft(2, "0") & "/" & dd.Day.ToString().PadLeft(2, "0") & "/" & dd.ToString.Substring(6)).Replace("ص", "am").Replace("م", "pm")
    End Function


    Public Sub FillCombo(ByVal cbo As ComboBox, ByVal tbl As String, ByVal SubId As String, ByVal SubName As String)
        Try
            Dim dt As DataTable = ExcuteAdapter("select " & SubId & " 'Id'," & SubName & " 'Name From " & tbl & " union select 0 Id,'-' Name")

            Dim dv As New DataView
            dv.Table = dt
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub

    Public Sub FillCombo(ByVal cbo As ComboBox, ByVal tbl As String, ByVal SubId As String, ByVal SubName As String, ByVal where As String)
        Try
            Dim dt As DataTable = ExcuteAdapter("select " & SubId & " 'Id'," & SubName & " 'Name' From " & tbl & " " & where & " union select 0 Id,'-' Name")
            Dim dv As New DataView
            dv.Table = dt
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub
    Public Sub FillCombo(ByVal cbo As ComboBox, ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String)

        Dim dt As New DataTable()
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            da.SelectCommand = MyCmd

            MyCmd.Parameters.Clear()
            For i As Integer = 0 To ParaName.Length - 1
                MyCmd.Parameters.Add("@" & ParaName(i), SqlDbType.VarChar)
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            dt.Clear()
            da.Fill(dt)
            CloseConnection(c)
            Dim dv As New DataView
            dv.Table = dt
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
            CloseConnection(c)
        Finally
            c = Nothing
        End Try
    End Sub

    Public Function GetDelayPeriod() As Integer
        Dim s As String = ExecuteScalar("select dbo.GetDelayPeriod()")
        If s = "" Then s = "0"
        Return Val(s)
    End Function

    Public Sub SaveImage(ByVal tbl As String, ByVal ID() As String, ByVal IDValue() As String, ByVal Field As String, ByVal PicPox As Controls.Image)
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim stream As New MemoryStream()
            Dim encoder As New BmpBitmapEncoder()
            Dim myTextBlock As New TextBlock()
            myTextBlock.Text = "Codec Author is: " + encoder.CodecInfo.Author.ToString()
            encoder.Frames.Add(BitmapFrame.Create(PicPox.Source))
            encoder.Save(stream)
            Dim ImageBytes As Byte()
            ImageBytes = stream.ToArray

            Dim statement As String = "update " & tbl & " set " & Field & "=@MyImage Where " & ID(0) & "='" & IDValue(0) & "'"
            For i As Integer = 1 To ID.Length - 1
                statement &= " and " & ID(i) & "='" & IDValue(i) & "'"
            Next

            Dim com As New SqlClient.SqlCommand(statement, c)
            com.Parameters.Add("@MyImage", SqlDbType.Image)
            com.Parameters("@MyImage").Value = ImageBytes

            OpenConnection(c)
            com.ExecuteNonQuery()
            CloseConnection(c)
        Catch ex As Exception
            CloseConnection(c)
        End Try
    End Sub

    Sub OpenConnection(ByVal cc As SqlConnection)
        Try
            If cc.State = ConnectionState.Closed Then cc.Open()
        Catch
        End Try
    End Sub

    Sub CloseConnection(ByVal cc As SqlConnection)
        Try
            cc.Close()
        Catch
        End Try
    End Sub

    Public Sub GetImage(ByVal tbl As String, ByVal ID() As String, ByVal IDValue() As String, ByVal Field As String, ByVal PicPox As Controls.Image)
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim myCommand As SqlClient.SqlCommand
            Dim statement As String = "select " & Field & " from " & tbl & " Where " & ID(0) & "='" & IDValue(0) & "'"
            For i As Integer = 1 To ID.Length - 1
                statement &= " and " & ID(i) & "='" & IDValue(i) & "'"
            Next
            myCommand = New SqlClient.SqlCommand(statement, c)
            OpenConnection(c)
            Dim imagedata() As Byte = CType(myCommand.ExecuteScalar(), Byte())
            CloseConnection(c)
            Dim stmBLOBData As IO.MemoryStream = New IO.MemoryStream(imagedata)

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.StreamSource = stmBLOBData
            bi3.EndInit()
            PicPox.Source = bi3
        Catch ex As Exception
            SetNoImage(PicPox)
            CloseConnection(c)
        End Try

    End Sub


    Public Sub SaveFile(ByVal tbl As String, ByVal KeyId As String, ByVal KeyValue As String, ByVal KeyId2 As String, ByVal KeyValue2 As String, ByVal Field As String, ByVal Path As String)
        Dim c As New SqlConnection(Md.ConnectionString)
        Try
            Dim ImageBytes As Byte() = File.ReadAllBytes(Path)
            Dim com As New SqlClient.SqlCommand("if exists (select * from " & tbl & " where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "') update " & tbl & " set " & Field & "=@MyImage,LastUpdate=getdate() where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "' else insert into " & tbl & "( " & KeyId & "," & KeyId2 & "," & Field & ",LastUpdate) select '" & KeyValue & "','" & KeyValue2 & "',@MyImage,getdate()", c)
            com.Parameters.Add("@MyImage", SqlDbType.Image)
            com.Parameters("@MyImage").Value = ImageBytes
            OpenConnection(c)
            com.ExecuteNonQuery()
            CloseConnection(c)
        Catch ex As Exception
            ShowMSG(ex.Message)
            CloseConnection(c)
        End Try
    End Sub

    Public Sub AllowDorp(ByVal picBox As Controls.Image)
        picBox.AllowDrop = True
        AddHandler picBox.DragLeave, AddressOf pictureBox_DragDrop
        AddHandler picBox.DragEnter, AddressOf pictureBox_DragEnter
    End Sub
    Private Sub pictureBox_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs)
        e.Effects = e.AllowedEffects
    End Sub

    Private Sub pictureBox_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs)
        Dim ss() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim a As Windows.Media.ImageSource
        Dim dp As DependencyProperty
        a.SetCurrentValue(dp, ss(0))
        CType(sender, Controls.Image).Source = a
    End Sub
    Public Sub ShowMSG(ByVal MSG As String)
        Dim mm As New MSG
        mm.MSG = MSG
        mm.DelMsg = False
        mm.ShowDialog()
    End Sub
    Public Function ShowDeleteMSG(ByVal MSG As String) As Boolean
        Dim mm As New MSG
        mm.MSG = MSG
        mm.DelMsg = True
        mm.ShowDialog()
        Return mm.Ok
    End Function

    Public Function MistakeOrUpdate() As Boolean
        Dim mm As New MSG
        mm.MistakeOrUpdate = True
        mm.MSG = "Please, Select Type of Editting ..."
        mm.DelMsg = True
        mm.ShowDialog()
        Return mm.Ok
    End Function

    Public Sub TestMistakeOrUpdate(ByVal OldUser As String, ByVal Tbl As String)
        If MistakeOrUpdate() Then
            ExcuteNonQuery("insert into Mistakes(OldUser,NewUser,Tbl,DayDate) select '" & OldUser & "','" & Md.UserName & "','" & Tbl & "',GETDATE()")
        End If
    End Sub

    Public Function Encrypt(ByVal plainText As String) As String

        Dim passPhrase As String
        Dim saltValue As String
        Dim hashAlgorithm As String
        Dim passwordIterations As Integer
        Dim initVector As String
        Dim keySize As Integer
        passPhrase = "KoKopr@se"        ' can be any string
        saltValue = "N!Les@t"        ' can be any string
        hashAlgorithm = "SHA1"             ' can be "MD5"
        passwordIterations = 5                  ' can be any number
        initVector = "@1B2h3D$e%F^g7H8" ' must be 16 bytes
        keySize = 256                ' can be 192 or 128

        Dim initVectorBytes As Byte()
        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

        Dim saltValueBytes As Byte()
        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)
        Dim plainTextBytes As Byte()
        plainTextBytes = Encoding.UTF8.GetBytes(plainText)
        Dim password As PasswordDeriveBytes
        password = New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)

        Dim keyBytes As Byte()
        keyBytes = password.GetBytes(keySize / 8)

        Dim symmetricKey As RijndaelManaged
        symmetricKey = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim encryptor As ICryptoTransform
        encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)
        Dim memoryStream As MemoryStream
        memoryStream = New MemoryStream()
        Dim cryptoStream As CryptoStream
        cryptoStream = New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)

        cryptoStream.FlushFinalBlock()

        Dim cipherTextBytes As Byte()
        cipherTextBytes = memoryStream.ToArray()

        memoryStream.Close()
        cryptoStream.Close()
        Return Convert.ToBase64String(cipherTextBytes)

    End Function
    Public Function Decrypt(ByVal cipherText As String) As String

        Dim passPhrase As String
        Dim saltValue As String
        Dim hashAlgorithm As String
        Dim passwordIterations As Integer
        Dim initVector As String
        Dim keySize As Integer

        passPhrase = "KoKopr@se"        ' can be any string
        saltValue = "N!Les@t"        ' can be any string
        hashAlgorithm = "SHA1"             ' can be "MD5"
        passwordIterations = 5                  ' can be any number
        initVector = "@1B2h3D$e%F^g7H8" ' must be 16 bytes
        keySize = 256                ' can be 192 or 128



        Dim initVectorBytes As Byte()
        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

        Dim saltValueBytes As Byte()
        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

        Dim cipherTextBytes As Byte()
        cipherTextBytes = Convert.FromBase64String(cipherText)
        Dim password As PasswordDeriveBytes
        password = New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
        Dim keyBytes As Byte()
        keyBytes = password.GetBytes(keySize / 8)
        Dim symmetricKey As RijndaelManaged
        symmetricKey = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim decryptor As ICryptoTransform
        decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)
        Dim memoryStream As MemoryStream
        memoryStream = New MemoryStream(cipherTextBytes)
        Dim cryptoStream As CryptoStream
        cryptoStream = New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)

        Dim plainTextBytes As Byte()
        ReDim plainTextBytes(cipherTextBytes.Length)
        Dim decryptedByteCount As Integer
        decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)

        memoryStream.Close()
        cryptoStream.Close()
        Return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)

    End Function

    Sub SetNoImage(ByVal image As Controls.Image, Optional ByVal IsPerson As Boolean = False, Optional ByVal Ask As Boolean = False)
        If Not Ask OrElse ShowDeleteMSG("هل أنت متأكد من إلغاء الصورة؟") Then
            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri(IIf(IsPerson, "Images\no.photo.gif", "Images\cancel.png"), UriKind.RelativeOrAbsolute)
            bi3.EndInit()
            image.Source = bi3
        End If
    End Sub

    Sub SetNoImage(ByVal image As DNBSoft.WPF.RibbonControl.RibbonPreviewBox)
        Dim bi3 As New BitmapImage
        bi3.BeginInit()
        bi3.UriSource = New Uri("Images\cancel.png", UriKind.RelativeOrAbsolute)
        bi3.EndInit()
        image.Image = bi3
    End Sub

    Sub SetImage(ByVal image As Controls.Image)
        Try
            Dim OFD As New System.Windows.Forms.OpenFileDialog
            OFD.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"
            If OFD.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim bi3 As New BitmapImage
                bi3.BeginInit()
                bi3.UriSource = New Uri(OFD.FileName, UriKind.RelativeOrAbsolute)
                bi3.EndInit()
                image.Source = bi3
            End If
        Catch
        End Try
    End Sub

    Sub SetImage(ByVal image As DNBSoft.WPF.RibbonControl.RibbonPreviewBox, ByVal imagedata As Byte())
        Try
            Dim stmBLOBData As IO.MemoryStream = New IO.MemoryStream(imagedata)
            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.StreamSource = stmBLOBData
            bi3.EndInit()
            image.Image = bi3
        Catch ex As Exception
            SetNoImage(image)
        End Try
    End Sub

    Public Sub GetCurrent()
        Dim dt As DataTable = ExcuteAdapter("GetCurrent")
        If dt.Rows.Count > 0 Then
            Md.CurrentDate = dt.Rows(0)("CurrentDate").ToString
            Md.CurrentShiftId = Val(dt.Rows(0)("CurrentShift").ToString)
            Md.CurrentShiftName = dt.Rows(0)("CurrentShiftName").ToString
            Md.CompanyName = dt.Rows(0)("CompanyName").ToString
        End If
    End Sub



End Class
