Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class Sales

    Public MainTableName As String = "Stores"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "SalesMaster"
    Public TableDetailsName As String = "SalesDetails"

    Public MainId As String = "StoreId"
    Public SubId As String = "InvoiceNo"

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
        Shared أرصدة_افتتاحية As Integer = 1
        Shared إضافة As Integer = 2
        Shared تسوية_إضافة As Integer = 3
        Shared صرف As Integer = 4
        Shared تسوية_صرف As Integer = 5
        Shared هدايا As Integer = 6
        Shared هالك As Integer = 7
        Shared تحويل_إلى_مخزن As Integer = 8
        Shared مشتريات As Integer = 9
        Shared مردودات_مشتريات As Integer = 10
        Shared مبيعات_الصالة As Integer = 11
        Shared مردودات_مبيعات_الصالة As Integer = 12
        Shared مبيعات_التيك_أواى As Integer = 13
        Shared مردودات_مبيعات_التيك_أواى As Integer = 14
        Shared مبيعات_التوصيل As Integer = 15
        Shared مردودات_مبيعات_التوصيل As Integer = 16
    End Structure



    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = False
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded Then Return
        RdoGrouping_Checked(Nothing, Nothing)
        bm.FillCombo("Shifts", Shift, "")

        WithService.Visibility = Windows.Visibility.Hidden
        ServiceValue.Visibility = Windows.Visibility.Hidden
        lblTotalAfterDiscount.Visibility = Windows.Visibility.Hidden
        TotalAfterDiscount.Visibility = Windows.Visibility.Hidden

        TabItem1.Header = TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader


        lblPayed.Visibility = Windows.Visibility.Hidden
        lblRemaining.Visibility = Windows.Visibility.Hidden
        Payed.Visibility = Windows.Visibility.Hidden
        Remaining.Visibility = Windows.Visibility.Hidden


        If Flag = FlagState.أرصدة_افتتاحية Then
            lblDayDate.Visibility = Visibility.Hidden
            DayDate.Visibility = Visibility.Hidden
            lblShift.Visibility = Visibility.Hidden
            Shift.Visibility = Visibility.Hidden
        ElseIf Flag = FlagState.تحويل_إلى_مخزن Then
            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المخزن المحول إليه"
        ElseIf Flag = FlagState.مشتريات Or Flag = FlagState.مردودات_مشتريات Then
            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المورد"

            lblDiscount.Visibility = Visibility.Visible
            lblPerc.Visibility = Visibility.Visible
            lblLE.Visibility = Visibility.Visible
            DiscountPerc.Visibility = Visibility.Visible
            DiscountValue.Visibility = Visibility.Visible

            lblTotalAfterDiscount.Visibility = Visibility.Visible
            TotalAfterDiscount.Visibility = Visibility.Visible
        ElseIf Md.MyProject = Project.Restuarant AndAlso (Flag = FlagState.مبيعات_الصالة Or Flag = FlagState.مردودات_مبيعات_الصالة) Then
            If Flag = FlagState.مبيعات_الصالة Then
                btnCloseTable.Visibility = Visibility.Visible
                TabItemTables.Visibility = Visibility.Visible
            End If

            lblTableId.Visibility = Visibility.Visible
            TableId.Visibility = Visibility.Visible
            TableIdName.Visibility = Visibility.Visible
            lblTableSubId.Visibility = Visibility.Visible
            TableSubId.Visibility = Visibility.Visible
            lblNoOfPersons.Visibility = Visibility.Visible
            NoOfPersons.Visibility = Visibility.Visible

            WithTax.Visibility = Visibility.Visible
            Taxvalue.Visibility = Visibility.Visible
            WithService.Visibility = Visibility.Visible
            ServiceValue.Visibility = Visibility.Visible
            CancelMinPerPerson.Visibility = Visibility.Visible
            lblMinPerPerson.Visibility = Visibility.Visible
            MinPerPerson.Visibility = Visibility.Visible
            GroupBoxPaymentType.Visibility = Visibility.Visible

            lblWaiter.Visibility = Visibility.Visible
            WaiterId.Visibility = Visibility.Visible
            WaiterName.Visibility = Visibility.Visible

            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "العميل"

            lblDiscount.Visibility = Visibility.Visible
            lblPerc.Visibility = Visibility.Visible
            lblLE.Visibility = Visibility.Visible
            DiscountPerc.Visibility = Visibility.Visible
            DiscountValue.Visibility = Visibility.Visible

            lblTotalAfterDiscount.Visibility = Visibility.Visible
            TotalAfterDiscount.Visibility = Visibility.Visible

            lblPayed.Visibility = Windows.Visibility.Visible
            lblRemaining.Visibility = Windows.Visibility.Visible
            Payed.Visibility = Windows.Visibility.Visible
            Remaining.Visibility = Windows.Visibility.Visible

        ElseIf Md.MyProject = Project.Restuarant AndAlso (Flag = FlagState.مبيعات_التيك_أواى Or Flag = FlagState.مردودات_مبيعات_التيك_أواى) Then

            WithTax.Visibility = Visibility.Visible
            Taxvalue.Visibility = Visibility.Visible

            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible

            GroupBoxPaymentType.Visibility = Visibility.Visible

            lblDiscount.Visibility = Visibility.Visible
            lblPerc.Visibility = Visibility.Visible
            lblLE.Visibility = Visibility.Visible
            DiscountPerc.Visibility = Visibility.Visible
            DiscountValue.Visibility = Visibility.Visible

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "العميل"

            lblTotalAfterDiscount.Visibility = Visibility.Visible
            TotalAfterDiscount.Visibility = Visibility.Visible

            btnSave.Visibility = Visibility.Hidden
            If Md.Cashier = 1 AndAlso Md.Manager = 0 Then btnDelete.Visibility = Visibility.Hidden

            lblPayed.Visibility = Windows.Visibility.Visible
            lblRemaining.Visibility = Windows.Visibility.Visible
            Payed.Visibility = Windows.Visibility.Visible
            Remaining.Visibility = Windows.Visibility.Visible

        ElseIf Md.MyProject = Project.Restuarant AndAlso (Flag = FlagState.مبيعات_التوصيل Or Flag = FlagState.مردودات_مبيعات_التوصيل) Then
            If Flag = FlagState.مبيعات_التوصيل Then
                btnCloseTable.Content = "تم التحصيل"
                btnCloseTable.Visibility = Visibility.Visible
                TabItemDelivery.Visibility = Visibility.Visible
                TabControl1.Items.Remove(TabItemTables)
            End If

            WithTax.Visibility = Visibility.Visible
            Taxvalue.Visibility = Visibility.Visible

            WithService.Visibility = Visibility.Visible
            ServiceValue.Visibility = Visibility.Visible
            WithService.Content = "توصيل"

            lblDeliveryman.Visibility = Visibility.Visible
            DeliverymanId.Visibility = Visibility.Visible
            DeliverymanName.Visibility = Visibility.Visible

            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible

            GroupBoxPaymentType.Visibility = Visibility.Visible

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "العميل"

            lblDiscount.Visibility = Visibility.Visible
            lblPerc.Visibility = Visibility.Visible
            lblLE.Visibility = Visibility.Visible
            DiscountPerc.Visibility = Visibility.Visible
            DiscountValue.Visibility = Visibility.Visible

            lblTotalAfterDiscount.Visibility = Visibility.Visible
            TotalAfterDiscount.Visibility = Visibility.Visible

            btnSave.Visibility = Visibility.Hidden
            If Md.Cashier = 1 AndAlso Md.Manager = 0 Then btnDelete.Visibility = Visibility.Hidden

            lblPayed.Visibility = Windows.Visibility.Visible
            lblRemaining.Visibility = Windows.Visibility.Visible
            Payed.Visibility = Windows.Visibility.Visible
            Remaining.Visibility = Windows.Visibility.Visible

        End If

        bm.Fields = New String() {MainId, SubId, "DayDate", "Shift", "Flag", "ToId", "WaiterId", "TableId", "TableSubId", "NoOfPersons", "WithTax", "Taxvalue", "WithService", "ServiceValue", "CancelMinPerPerson", "MinPerPerson", "PaymentType", "CashValue", "DiscountPerc", "DiscountValue", "Notes", "IsClosed", "IsCashierPrinted", "Cashier", "DeliverymanId", "Total", "Payed", "Remaining"}
        bm.control = New Control() {StoreId, InvoiceNo, DayDate, Shift, txtFlag, ToId, WaiterId, TableId, TableSubId, NoOfPersons, WithTax, Taxvalue, WithService, ServiceValue, CancelMinPerPerson, MinPerPerson, PaymentType, CashValue, DiscountPerc, DiscountValue, Notes, IsClosed, IsCashierPrinted, CashierId, DeliverymanId, Total, Payed, Remaining}
        bm.KeyFields = New String() {MainId, SubId}

        bm.Table_Name = TableName

        If Md.Cashier = 1 AndAlso Md.Manager = 0 Then
            StoreId.IsEnabled = False
            DayDate.IsEnabled = False
            Shift.IsEnabled = False
            CashierId.IsEnabled = False

            btnDelete.Visibility = Visibility.Hidden
            btnFirst.Visibility = Visibility.Hidden
            btnNext.Visibility = Visibility.Hidden
            btnPrevios.Visibility = Visibility.Hidden
            btnLast.Visibility = Visibility.Hidden

        End If
        LoadGroups()
        LoadAllItems()
        RdoCash_Checked(Nothing, Nothing)
        LoadWFH()
        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = Md.CurrentDate
        Shift.SelectedValue = Md.CurrentShiftId
        StoreId.Text = Md.StoreId
        StoreId_LostFocus(Nothing, Nothing)
        btnNew_Click(sender, e)
        bm.ApplyKeyDown()
    End Sub


    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Unit As String = "Unit"
        Shared Qty As String = "Qty"
        Shared Price As String = "Price"
        Shared UnitSub As String = "UnitSub"
        Shared QtySub As String = "QtySub"
        Shared PriceSub As String = "PriceSub"
        Shared Value As String = "Value"
        Shared IsPrinted As String = "IsPrinted"
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
        G.Grid.Columns.Add(GC.Price, "السعر")
        G.Grid.Columns.Add(GC.UnitSub, "الوحدة (فرعى)")
        G.Grid.Columns.Add(GC.QtySub, "الكمية (فرعى)")
        G.Grid.Columns.Add(GC.PriceSub, "السعر (فرعى)")
        G.Grid.Columns.Add(GC.Value, "القيمة")
        G.Grid.Columns.Add(GC.IsPrinted, "طباعة للمطبخ")

        G.Grid.Columns(GC.Id).FillWeight = 110
        G.Grid.Columns(GC.Name).FillWeight = 300
        G.Grid.Columns(GC.Unit).FillWeight = 150
        G.Grid.Columns(GC.Qty).FillWeight = 100
        G.Grid.Columns(GC.Price).FillWeight = 100
        G.Grid.Columns(GC.UnitSub).FillWeight = 100
        G.Grid.Columns(GC.QtySub).FillWeight = 100
        G.Grid.Columns(GC.PriceSub).FillWeight = 100
        G.Grid.Columns(GC.Value).FillWeight = 100
        G.Grid.Columns(GC.IsPrinted).FillWeight = 100

        G.Grid.Columns(GC.Name).ReadOnly = True
        G.Grid.Columns(GC.Unit).ReadOnly = True
        G.Grid.Columns(GC.Price).ReadOnly = ReadOnlyState()
        G.Grid.Columns(GC.UnitSub).ReadOnly = True
        G.Grid.Columns(GC.PriceSub).ReadOnly = ReadOnlyState()
        G.Grid.Columns(GC.Value).ReadOnly = True

        G.Grid.Columns(GC.UnitSub).Visible = Md.SaleQtySub
        G.Grid.Columns(GC.QtySub).Visible = Md.SaleQtySub
        G.Grid.Columns(GC.PriceSub).Visible = Md.SaleQtySub
        G.Grid.Columns(GC.IsPrinted).Visible = False

        AddHandler G.Grid.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.Grid.KeyDown, AddressOf GridKeyDown
    End Sub

    Function ReadOnlyState() As Boolean
        If TestSalesAndReturn() And Not Md.AllowCashierToEditPrice Then
            Return True
        Else
            Return False
        End If
    End Function

    Function Fm() As Integer
        Select Case Flag
            Case FlagState.مبيعات_الصالة, FlagState.مردودات_مبيعات_الصالة
                Return 1
            Case FlagState.مبيعات_التيك_أواى, FlagState.مردودات_مبيعات_التيك_أواى
                Return 2
            Case FlagState.مبيعات_التوصيل, FlagState.مردودات_مبيعات_التوصيل
                Return 3
            Case Else
                Return 0
        End Select
    End Function

    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WItems.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
            TabItems.Header = It

            Dim dt As DataTable = bm.ExcuteAdapter("LoadGroups2", New String() {"Form"}, New String() {Fm()})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = New TextBlock With {.Text = dt.Rows(i)("Name").ToString, .TextWrapping = TextWrapping.Wrap}
                'x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WGroups.Children.Add(x)
                AddHandler x.Click, AddressOf LoadTypes
            Next
        Catch
        End Try
    End Sub

    Sub LoadTables()
        Try
            WTables.Children.Clear()
            WSubTables.Children.Clear()
            Dim dt As DataTable = bm.ExcuteAdapter("LoadTables", New String() {"StoreId"}, New String() {StoreId.Text})
            Dim dtInv As DataTable = bm.ExcuteAdapter("select InvoiceNo,TableId,TableSubId,dbo.ToStrTime(OpennedDate) OpennedTime,NoOfPersons,IsCashierPrinted from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and IsClosed=0")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Name = "Table_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Width = 100
                x.Height = 100
                x.Cursor = Input.Cursors.Pen
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = x.Content
                WTables.Children.Add(x)

                If dtInv.Select("TableId=" & x.Tag & " and TableSubId>1").Length > 0 Then
                    x.Background = System.Windows.Media.Brushes.MediumSpringGreen
                    x.Content &= vbCrLf & "مائدة مقسمة"
                ElseIf dtInv.Select("TableId=" & x.Tag).Length > 0 Then
                    If dtInv.Select("TableId=" & x.Tag)(0)("IsCashierPrinted") = 1 Then
                        x.Background = System.Windows.Media.Brushes.Magenta
                    Else
                        x.Background = System.Windows.Media.Brushes.Red
                    End If
                    x.Content &= vbCrLf & dtInv.Select("TableId=" & x.Tag)(0).Item("OpennedTime").ToString & vbCrLf & "العدد: " & dtInv.Select("TableId=" & x.Tag)(0).Item("NoOfPersons").ToString
                Else
                    x.Background = System.Windows.Media.Brushes.LimeGreen
                End If

                AddHandler x.Click, AddressOf btnTableClick
            Next
        Catch
        End Try
    End Sub

    Sub LoadUnPaiedInvoices()
        Try
            WDelivery.Children.Clear()
            Dim dt As DataTable = bm.ExcuteAdapter("select InvoiceNo,dbo.ToStrTime(OpennedDate) OpennedTime from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and IsClosed=0")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Name = "Delivery_" & dt.Rows(i)("InvoiceNo").ToString
                x.Tag = dt.Rows(i)("InvoiceNo").ToString
                x.Width = 100
                x.Height = 100
                x.Cursor = Input.Cursors.Pen
                x.Content = dt.Rows(i)("InvoiceNo").ToString & vbCrLf & vbCrLf & dt.Rows(i)("OpennedTime").ToString
                x.ToolTip = x.Content
                WDelivery.Children.Add(x)
                x.Background = System.Windows.Media.Brushes.Red
                AddHandler x.Click, AddressOf btnDeliveryClick
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

            TabTypes.Header = Tp & " - " & xx.Content.ToString
            TabItems.Header = It

            Dim dt As DataTable = bm.ExcuteAdapter("LoadTypes2", New String() {"GroupId", "Form"}, New String() {xx.Tag.ToString, Fm()})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & xx.Tag.ToString & "_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = New TextBlock With {.Text = dt.Rows(i)("Name").ToString, .TextWrapping = TextWrapping.Wrap}
                'x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WTypes.Children.Add(x)
                AddHandler x.Click, AddressOf LoadItems
            Next
        Catch
        End Try
    End Sub


    Sub LoadAllItems()
        Try
            HelpDt = bm.ExcuteAdapter("Select Id,Name," & PriceFieldName(GC.Price) & " Price From Items where " & ItemWhere())
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
        Dim st As String = ""
        If Md.MyProject = Project.Restuarant And TestSalesAndReturn() Then
            st = " ItemType in(2,3) " 'تام و كومبو
            If Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.مردودات_مبيعات_الصالة Then
                st &= " and IsTables=1 "
            ElseIf Flag = FlagState.مبيعات_التيك_أواى OrElse Flag = FlagState.مردودات_مبيعات_التيك_أواى Then
                st &= " and IsTakeAway=1 "
            ElseIf Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل Then
                st &= " and IsDelivary=1 "
            End If
        Else
            st = " ItemType in(0,1,2) " 'خام ونصف مصنع وتام
        End If
        Return st
    End Function
    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()

            TabItems.Header = It & " - " & xx.Content.ToString

            Dim dt As DataTable = bm.ExcuteAdapter("Select * From Items where " & ItemWhere() & " and GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag.ToString)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = New TextBlock With {.Text = dt.Rows(i)("Name").ToString, .TextWrapping = TextWrapping.Wrap}
                'x.Content = dt.Rows(i)("Name").ToString
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
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.Grid.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                For x As Integer = 0 To G.Grid.Rows.Count - 1
                    If Not G.Grid.Rows(x).Cells(GC.Id).Value Is Nothing AndAlso G.Grid.Rows(x).Cells(GC.Id).Value.ToString = Id.ToString AndAlso Not G.Grid.Rows(x).ReadOnly AndAlso Not G.Grid.Rows(x).Cells(GC.IsPrinted).Value = 1 Then
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
                CalcTotal()
                Return
            End If
            G.Grid.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
            G.Grid.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)
            G.Grid.Rows(i).Cells(GC.Unit).Value = dr(0)(GC.Unit)
            If Val(G.Grid.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Grid.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Grid.Rows(i).Cells(GC.Qty).Value)
            G.Grid.Rows(i).Cells(GC.Price).Value = dr(0)(PriceFieldName(GC.Price))
            G.Grid.Rows(i).Cells(GC.UnitSub).Value = dr(0)(GC.UnitSub)
            G.Grid.Rows(i).Cells(GC.QtySub).Value = 0
            G.Grid.Rows(i).Cells(GC.PriceSub).Value = dr(0)(PriceFieldName(GC.PriceSub))
            If G.Grid.Rows(i).Cells(GC.IsPrinted).Value <> 1 Then G.Grid.Rows(i).Cells(GC.IsPrinted).Value = 0

            CalcRow(i)
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
                G.Grid.CurrentCell = G.Grid.Rows(i).Cells(GC.Price)
                G.Grid.CurrentCell = G.Grid.Rows(i).Cells(GC.Qty)
                G.Grid.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.Grid.BeginEdit(True)
            End If
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
    End Sub

    Dim lop As Boolean = False
    Sub CalcRow(ByVal i As Integer)
        Try
            If G.Grid.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Grid.Rows(i).Cells(GC.Id).Value.ToString = "" Then
                ClearRow(i)
                CalcTotal()
                Return
            End If
            G.Grid.Rows(i).Cells(GC.Qty).Value = Val(G.Grid.Rows(i).Cells(GC.Qty).Value)
            G.Grid.Rows(i).Cells(GC.Price).Value = Val(G.Grid.Rows(i).Cells(GC.Price).Value)
            G.Grid.Rows(i).Cells(GC.QtySub).Value = Val(G.Grid.Rows(i).Cells(GC.QtySub).Value)
            G.Grid.Rows(i).Cells(GC.PriceSub).Value = Val(G.Grid.Rows(i).Cells(GC.PriceSub).Value)

            G.Grid.Rows(i).Cells(GC.Value).Value = Math.Round(Val(G.Grid.Rows(i).Cells(GC.Qty).Value) * Val(G.Grid.Rows(i).Cells(GC.Price).Value) + Val(G.Grid.Rows(i).Cells(GC.QtySub).Value) * Val(G.Grid.Rows(i).Cells(GC.PriceSub).Value), 2)

        Catch ex As Exception
            ClearRow(i)
        End Try
        CalcTotal()
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Grid.Rows(i).Cells(GC.Id).Value = Nothing
        G.Grid.Rows(i).Cells(GC.Name).Value = Nothing
        G.Grid.Rows(i).Cells(GC.Unit).Value = Nothing
        G.Grid.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Grid.Rows(i).Cells(GC.Price).Value = Nothing
        G.Grid.Rows(i).Cells(GC.UnitSub).Value = Nothing
        G.Grid.Rows(i).Cells(GC.QtySub).Value = Nothing
        G.Grid.Rows(i).Cells(GC.PriceSub).Value = Nothing
        G.Grid.Rows(i).Cells(GC.Value).Value = Nothing
        G.Grid.Rows(i).Cells(GC.IsPrinted).Value = Nothing
    End Sub
    Private Sub RdoCash_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoCash.Checked, RdoVisa.Checked, RdoCashVisa.Checked, RdoFuture.Checked, RdoManagers.Checked, RdoEmployees.Checked
        If txtID Is Nothing Then Return
        Try
            GroupBoxPaymentType.Header = "طريقة الدفع : " & CType(sender, RadioButton).Content
            PaymentType.Text = 0
            If RdoCash.IsChecked Then
                PaymentType.Text = 1
            ElseIf RdoVisa.IsChecked Then
                PaymentType.Text = 2
            ElseIf RdoCashVisa.IsChecked Then
                PaymentType.Text = 3
            ElseIf RdoFuture.IsChecked Then
                PaymentType.Text = 4
            ElseIf RdoManagers.IsChecked Then
                PaymentType.Text = 5
            ElseIf RdoEmployees.IsChecked Then
                PaymentType.Text = 6
            End If
        Catch ex As Exception
        End Try

        Try
            If RdoCashVisa.IsChecked Then
                CashValue.Visibility = Windows.Visibility.Visible
                lblCashValue.Visibility = Windows.Visibility.Visible
            Else
                CashValue.Visibility = Windows.Visibility.Hidden
                lblCashValue.Visibility = Windows.Visibility.Hidden
                CashValue.Text = 0
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If G.Grid.Columns(e.ColumnIndex).Name = GC.Id Then
            AddItem(G.Grid.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
        End If
        G.Grid.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        CalcRow(e.RowIndex)
    End Sub


    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("المخازن", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Stores") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub


    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Stores where Id=" & StoreId.Text.Trim())
        ClearControls()
    End Sub

    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ToId.KeyUp
        Dim Title, tbl As String
        If Flag = FlagState.تحويل_إلى_مخزن Then
            tbl = "Stores"
            Title = "المخازن"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf Flag = FlagState.مشتريات Or Flag = FlagState.مردودات_مشتريات Then
            tbl = "Suppliers"
            Title = "الموردين"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf TestSalesAndReturn() Then
            tbl = "Customers"
            Title = "العملاء"
            If bm.ShowHelpCustomers(ToId, ToName, e) Then
                ToId_LostFocus(Nothing, Nothing)
            End If
        End If
    End Sub

    Function TestSalesAndReturn() As Boolean
        Return (Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.مردودات_مبيعات_الصالة OrElse Flag = FlagState.مبيعات_التيك_أواى OrElse Flag = FlagState.مردودات_مبيعات_التيك_أواى OrElse Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل)
    End Function

    Function TestSalesAndOnly() As Boolean
        'Return (Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.مبيعات_التيك_أواى OrElse Flag = FlagState.مبيعات_التوصيل)
        Return (Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.مبيعات_التوصيل)
    End Function

    Function TestDelivary() As Boolean
        Return (Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل)
    End Function

    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        Dim tbl As String
        If Flag = FlagState.تحويل_إلى_مخزن Then
            tbl = "Stores"
        ElseIf Flag = FlagState.مشتريات Or Flag = FlagState.مردودات_مشتريات Then
            tbl = "Suppliers"
        ElseIf TestSalesAndReturn() Then
            tbl = "Customers"
        Else
            Return
        End If
        bm.LostFocus(ToId, ToName, "select Name from " & tbl & " where Id=" & ToId.Text.Trim())
        Dim s As String = ""
        If TestSalesAndReturn() Then
            Dim dt As DataTable = bm.ExcuteAdapter("GetCustomerData", New String() {"Id"}, New String() {Val(ToId.Text)})
            If dt.Rows.Count > 0 Then
                If Not lop Then DiscountPerc.Text = Val(dt.Rows(0)("DescPerc").ToString)
                For i As Integer = 0 To dt.Columns.Count - 2
                    s &= dt.Rows(0)(i).ToString & IIf(i = dt.Columns.Count - 1, "", vbCrLf)
                Next
            End If
        End If
        ToId.ToolTip = Nothing
        ToName.ToolTip = Nothing
        If Val(ToId.Text) > 0 Then
            Dim t As New ToolTip With {.Content = s}
            ToId.ToolTip = t
            ToName.ToolTip = t
            ToolTipService.SetShowDuration(ToId, 30000)
            ToolTipService.SetShowDuration(ToName, 30000)
        End If
    End Sub

    Private Sub WaiterId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles WaiterId.KeyUp
        bm.ShowHelp("الويترز", WaiterId, WaiterName, e, "select cast(Id as varchar(100)) Id,EnName Name from Employees where Waiter=1 and Stopped=0")
    End Sub

    Private Sub WaiterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles WaiterId.LostFocus
        bm.LostFocus(WaiterId, WaiterName, "select EnName Name from Employees where Waiter=1 and Id=" & WaiterId.Text.Trim() & " and Stopped=0")
    End Sub

    Private Sub DeliverymanId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles DeliverymanId.KeyUp
        bm.ShowHelp("الطيارين", DeliverymanId, DeliverymanName, e, "select cast(Id as varchar(100)) Id,EnName Name from Employees where Deliveryman=1 and Stopped=0")
    End Sub

    Private Sub DeliverymanId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DeliverymanId.LostFocus
        bm.LostFocus(DeliverymanId, DeliverymanName, "select EnName Name from Employees where Deliveryman=1 and Id=" & DeliverymanId.Text.Trim() & " and Stopped=0")
    End Sub


    Private Sub CashierId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CashierId.KeyUp
        bm.ShowHelp("الكاشير", CashierId, CashierName, e, "select cast(Id as varchar(100)) Id,EnName Name from Employees where Cashier=1 and Stopped=0")
    End Sub

    Private Sub CashierId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CashierId.LostFocus
        bm.LostFocus(CashierId, CashierName, "select EnName Name from Employees where Cashier=1 and Id=" & CashierId.Text.Trim() & " and Stopped=0")
    End Sub


    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        bm.FillControls()

        PaymentType_TextChanged(Nothing, Nothing)
        ToId_LostFocus(Nothing, Nothing)
        CashierId_LostFocus(Nothing, Nothing)
        WaiterId_LostFocus(Nothing, Nothing)
        DeliverymanId_LostFocus(Nothing, Nothing)
        TId_LostFocus(TableId, Nothing)
        TId_LostFocus(TableSubId, Nothing)
        TId_LostFocus(NoOfPersons, Nothing)
        Dim dt As DataTable = bm.ExcuteAdapter("select SD.*,It.Unit,It.UnitSub from SalesDetails SD left join Items It on(SD.ItemId=It.Id) where SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text & bm.AppendWhere)

        G.Grid.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Grid.Rows.Add()
            G.Grid.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Grid.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            G.Grid.Rows(i).Cells(GC.Unit).Value = dt.Rows(i)("Unit").ToString
            G.Grid.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Grid.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            G.Grid.Rows(i).Cells(GC.UnitSub).Value = dt.Rows(i)("UnitSub").ToString
            G.Grid.Rows(i).Cells(GC.QtySub).Value = dt.Rows(i)("QtySub").ToString
            G.Grid.Rows(i).Cells(GC.PriceSub).Value = dt.Rows(i)("PriceSub").ToString
            G.Grid.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Grid.Rows(i).Cells(GC.IsPrinted).Value = dt.Rows(i)("IsPrinted").ToString
            CalcRow(i)
            If Md.Cashier = 1 AndAlso Md.Manager = 0 AndAlso TestSalesAndReturn() Then
                G.Grid.Rows(i).ReadOnly = True
                G.Grid.Rows(i).DefaultCellStyle.BackColor = System.Drawing.Color.PeachPuff
                btnDelete.IsEnabled = False
            End If
        Next
        CalcTotal()
        Notes.Focus()
        G.Grid.RefreshEdit()
        lop = False
        CalcTotalEnd()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If StoreId.Text.Trim = "" Then Return
        If Not CType(sender, Button).IsEnabled Then Return


        If ToId.Visibility = Windows.Visibility.Visible AndAlso ToId.Text.Trim = "" AndAlso Not TestSalesAndReturn() Then
            bm.ShowMSG("برجاء تحديد " & lblToId.Content)
            ToId.Focus()
            Return
        End If
        If TableId.Visibility = Windows.Visibility.Visible AndAlso TableId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد رقم المائدة")
            TableId.Focus()
            Return
        End If
        If TableSubId.Visibility = Windows.Visibility.Visible AndAlso TableSubId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد رقم الفرعى من المائدة")
            TableSubId.Focus()
            Return
        End If
        If NoOfPersons.Visibility = Windows.Visibility.Visible AndAlso NoOfPersons.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد عدد الأفراد")
            NoOfPersons.Focus()
            Return
        End If
        If CashierId.Visibility = Windows.Visibility.Visible AndAlso CashierId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد الكاشير")
            CashierId.Focus()
            Return
        End If
        If WaiterId.Visibility = Windows.Visibility.Visible AndAlso WaiterId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد الويتر")
            WaiterId.Focus()
            Return
        End If
        If DeliverymanId.Visibility = Windows.Visibility.Visible AndAlso DeliverymanId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد الطيار")
            DeliverymanId.Focus()
            Return
        End If
        If Flag = FlagState.تحويل_إلى_مخزن AndAlso ToId.Text.Trim = StoreId.Text Then
            bm.ShowMSG("لا يمكن التحويل لنفس المخزن")
            ToId.Focus()
            Return
        End If

        G.Grid.EndEdit()
        Try
            CalcRow(G.Grid.CurrentRow.Index)
        Catch ex As Exception
        End Try

        For i As Integer = 0 To G.Grid.Rows.Count - 1
            Try
                If Val(G.Grid.Rows(i).Cells(GC.Id).Value) > 0 AndAlso Val(G.Grid.Rows(i).Cells(GC.Qty).Value) <= 0 Then
                    bm.ShowMSG("برجاء تحديد الكمية بالسطر " & (i + 1).ToString)
                    G.Grid.CurrentCell = G.Grid.Rows(i).Cells(GC.Qty)
                    G.Grid.Focus()
                    G.Grid.BeginEdit(True)
                    Return
                End If
            Catch ex As Exception
            End Try
        Next

        TableId.Text = Val(TableId.Text)
        TableSubId.Text = Val(TableSubId.Text)
        NoOfPersons.Text = Val(NoOfPersons.Text)
        MinPerPerson.Text = Val(MinPerPerson.Text)
        ServiceValue.Text = Val(ServiceValue.Text)
        Taxvalue.Text = Val(Taxvalue.Text)
        PaymentType.Text = Val(PaymentType.Text)
        CashValue.Text = Val(CashValue.Text)

        lop2 = True
        DiscountPerc.Text = Val(DiscountPerc.Text)
        DiscountValue.Text = Val(DiscountValue.Text)
        lop2 = False

        ToId.Text = Val(ToId.Text)
        WaiterId.Text = Val(WaiterId.Text)


        If Md.Cashier = 1 Then
            DayDate.SelectedDate = Md.CurrentDate
            Shift.SelectedValue = Md.CurrentShiftId
        End If


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & bm.AppendWhere)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            lblLastEntry.Foreground = System.Windows.Media.Brushes.Red
            'System.Threading.Thread.Sleep(5000)
            lblLastEntry.Foreground = System.Windows.Media.Brushes.Blue
            State = BasicMethods.SaveState.Insert
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text.Trim}) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G.Grid, "SalesDetails", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {txtFlag.Text, StoreId.Text, InvoiceNo.Text}, New String() {"ItemId", "ItemName", "Qty", "Price", "QtySub", "PriceSub", "Value", "IsPrinted"}, New String() {GC.Id, GC.Name, GC.Qty, GC.Price, GC.QtySub, GC.PriceSub, GC.Value, GC.IsPrinted}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer}, New String() {GC.Id}) Then Return


        bm.ExecuteNonQuery("UpdateSalesDetailsComponants", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text})


        Select Case CType(sender, Button).Name
            Case btnPrint.Name
                State = BasicMethods.SaveState.Print
            Case btnCloseTable.Name
                State = BasicMethods.SaveState.Close
        End Select

        TraceInvoice(State.ToString)

        If TestSalesAndOnly() Then PrintPone(1)
        If sender Is btnPrint Or (sender Is btnCloseTable And btnPrint.IsEnabled) Then
            PrintPone(2)
            PrintPone(0)
            'txtID_Leave(Nothing, Nothing)
            'AllowClose = True
            'Return
        End If

        If Not DontClear Then btnNew_Click(sender, e)
        AllowClose = True
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
        TableId.Focus()
    End Sub

    Sub ClearControls()
        Try
            NewId()
            Dim d As DateTime = Nothing
            Try
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try
            Dim s As String = Shift.SelectedValue.ToString
            Dim st As String = StoreId.Text

            bm.ClearControls(False)
            ToId_LostFocus(Nothing, Nothing)
            CashierId_LostFocus(Nothing, Nothing)
            WaiterId_LostFocus(Nothing, Nothing)
            DeliverymanId_LostFocus(Nothing, Nothing)
            TId_LostFocus(TableId, Nothing)
            TId_LostFocus(TableSubId, Nothing)
            TId_LostFocus(NoOfPersons, Nothing)
            PaymentType.Text = 1

            Payed.Clear()
            Remaining.Clear()

            If Md.Cashier = 1 Then
                DayDate.SelectedDate = Md.CurrentDate
                Shift.SelectedValue = Md.CurrentShiftId
                CashierId.Text = Md.UserName
                CashierId_LostFocus(Nothing, Nothing)
            Else
                DayDate.SelectedDate = d
                Shift.SelectedValue = s
            End If

            StoreId.Text = st

            txtFlag.Text = Flag
            bm.AppendWhere = " and Flag=" & Flag
            G.Grid.Rows.Clear()
            CalcTotal()
            'InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & bm.AppendWhere)
            'If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"

            If TableSubId.Visibility = Visibility.Visible Then TableSubId.Text = 1
            If NoOfPersons.Visibility = Visibility.Visible Then NoOfPersons.Text = 1

            WithService.IsChecked = (WithService.Visibility = Visibility.Visible)
            WithTax.IsChecked = (WithTax.Visibility = Visibility.Visible)
        Catch
        End Try
        If Flag = FlagState.مبيعات_الصالة Then TabControl1.SelectedItem = TabItemTables
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من المسح؟") Then
            TraceInvoice("Deleted")
            bm.ExcuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & bm.AppendWhere)

            bm.ExcuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & bm.AppendWhere)

            btnNew_Click(sender, e)
        End If
    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteSales", New String() {"Flag", "StoreId", "InvoiceNo", "UserDelete", "State"}, New String() {txtFlag.Text, StoreId.Text, InvoiceNo.Text, Md.UserName, State})
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyDown, InvoiceNo.KeyDown, ToId.KeyDown, WaiterId.KeyDown, TableId.KeyDown, TableSubId.KeyDown, NoOfPersons.KeyDown, txtID.KeyDown, CashierId.KeyDown, DeliverymanId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Taxvalue.KeyDown, ServiceValue.KeyDown, MinPerPerson.KeyDown, CashValue.KeyDown, DiscountPerc.KeyDown, DiscountValue.KeyDown, txtPrice.KeyDown
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

    Private Sub PaymentType_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles PaymentType.TextChanged
        Try
            If PaymentType.Text = 1 Then
                RdoCash.IsChecked = True
            ElseIf PaymentType.Text = 2 Then
                RdoVisa.IsChecked = True
            ElseIf PaymentType.Text = 3 Then
                RdoCashVisa.IsChecked = True
            ElseIf PaymentType.Text = 4 Then
                RdoFuture.IsChecked = True
            ElseIf PaymentType.Text = 5 Then
                RdoManagers.IsChecked = True
            ElseIf PaymentType.Text = 6 Then
                RdoEmployees.IsChecked = True
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TableId_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TableId.KeyUp
        If bm.ShowHelp("الموائد", TableId, TableIdName, e, "select cast(Id as varchar(100)) Id,Name from Tables where StoreId='" & StoreId.Text & "'") Then
            TId_LostFocus(TableId, Nothing)
        End If
    End Sub



    Private Sub TId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TableId.LostFocus, TableSubId.LostFocus, NoOfPersons.LostFocus
        If CType(sender, TextBox).Text.Trim = "" Or CType(sender, TextBox).Text.Trim = "0" Then CType(sender, TextBox).Clear()

        If sender Is TableId Then
            bm.LostFocus(TableId, TableIdName, "select Name from Tables where StoreId='" & StoreId.Text & "' and Id=" & TableId.Text.Trim())
            TestDoublicatinInTables(False)
        ElseIf sender Is TableSubId Then
            Dim x As Integer = Val(bm.ExecuteScalar("select MaxSubTable from Statics"))
            If (x < Val(TableSubId.Text)) Then
                bm.ShowMSG("الحد الأقصى للفرعى هو " & x)
                TableSubId.Clear()
            End If
            TestDoublicatinInTables(True)
        End If
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.Grid.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("هل أنت متأكد من حذف السطر الحالى؟") Then
                G.Grid.Rows.Remove(G.Grid.CurrentRow)
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Function PriceFieldName(ByVal str As String) As String
        If TestSalesAndReturn() Then
            Return "Sales" & str
        Else
            Return "Purchase" & str
        End If
    End Function

    Private Sub PrintPone(ByVal NewItemsOnly As Integer)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "Payed", "Remaining"}
        rpt.Header = Md.MyProject.ToString

        If NewItemsOnly = 0 Then
            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, 0, Val(Payed.Text), Val(Remaining.Text)}
            rpt.RptPath = "SalesPone.rpt"
            Select Case Flag
                Case FlagState.مبيعات_التوصيل, FlagState.مردودات_مبيعات_التوصيل
                    rpt.Print(, , 2)
                Case Else
                    rpt.Print(, , 1)
            End Select
            'rpt.ShowDialog()
        ElseIf NewItemsOnly = 2 Then
            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, 0, Val(Payed.Text), Val(Remaining.Text)}
            rpt.RptPath = "SalesPoneLabel.rpt"
            Select Case Flag
                Case FlagState.مبيعات_التيك_أواى, FlagState.مردودات_مبيعات_التيك_أواى
                    rpt.Print(, , 1)
            End Select
            'rpt.ShowDialog()
        ElseIf Not (Flag = FlagState.مبيعات_التيك_أواى Or Flag = FlagState.مردودات_مبيعات_التيك_أواى) Then
            rpt.RptPath = "SalesPoneKitchen.rpt"
            For i As Integer = 0 To G.Grid.Rows.Count - 1
                Try
                    If G.Grid.Rows(i).Cells(GC.IsPrinted).Value.ToString = 0 Then
                        Dim dt As DataTable = bm.ExcuteAdapter("GetPrinters", New String() {"Shift", "Flag", "StoreId", "InvoiceNo"}, New String() {Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text})
                        For x As Integer = 0 To dt.Rows.Count - 1
                            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, dt.Rows(x)("PrintingGroupId"), Val(Payed.Text), Val(Remaining.Text)}
                            rpt.Print(dt.Rows(x)("ServerName"), dt.Rows(x)("PrinterName"))
                            'rpt.ShowDialog()
                        Next
                        Exit For
                    End If
                Catch
                End Try
            Next
        End If

    End Sub


    Private Sub RdoGrouping_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoGrouping.Checked, RdoSearch.Checked
        If txtID Is Nothing Then Return
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


    Dim LopCalc As Boolean = False
    Private Sub CalcTotal() Handles Total.TextChanged, DiscountValue.TextChanged, Taxvalue.TextChanged, ServiceValue.TextChanged, MinPerPerson.TextChanged, NoOfPersons.TextChanged, WithTax.Checked, WithTax.Unchecked, WithService.Checked, WithService.Unchecked, CancelMinPerPerson.Checked, CancelMinPerPerson.Unchecked, ToId.LostFocus, DiscountValue.TextChanged
        If LopCalc Or lop Then Return
        Try
            LopCalc = True
            MinPerPerson.Text = Math.Round(0, 2)
            'DiscountValue.Text = Math.Round(0, 2)
            Total.Text = Math.Round(0, 2)
            Taxvalue.Text = Math.Round(0, 2)
            'ServiceValue.Text = Math.Round(0, 2)

            If CancelMinPerPerson.IsChecked Then
                MinPerPerson.Text = Math.Round(0, 2)
            Else
                MinPerPerson.Text = bm.ExecuteScalar("select dbo.GetMinValuePerPerson(" & StoreId.Text & ")")
            End If
            For i As Integer = 0 To G.Grid.Rows.Count - 1
                Total.Text += Val(G.Grid.Rows(i).Cells(GC.Value).Value)
            Next

            If Not lop Or Not IsClosed.IsChecked Then

                If Val(Total.Text) < Val(MinPerPerson.Text) * Val(NoOfPersons.Text) Then
                    Total.Text = Math.Round(Val(MinPerPerson.Text) * Val(NoOfPersons.Text), 2)
                End If

                If WithTax.IsChecked Then
                    Taxvalue.Text = 0.01 * (Val(Total.Text) - Val(DiscountValue.Text)) * Val(bm.ExecuteScalar("select dbo.GetTaxPerc(" & StoreId.Text & ")"))
                Else
                    Taxvalue.Text = Math.Round(0, 2)
                End If
                If WithService.IsChecked Then
                    If TestDelivary() Then
                        'ServiceValue.Text = Val(bm.ExecuteScalar("select dbo.GetDelivaryCost(" & Val(StoreId.Text) & "," & Val(ToId.Text) & ")"))
                    Else
                        'ServiceValue.Text = Math.Round((Val(Total.Text) - Val(DiscountValue.Text)) * Val(bm.ExecuteScalar("select dbo.GetServicePerc(" & StoreId.Text & ")")) / 100, 2)
                    End If
                Else
                    ServiceValue.Text = Math.Round(0, 2)
                End If

            End If

            LopCalc = False
            CalcTotalEnd()
        Catch ex As Exception
        End Try
    End Sub

    Sub CalcTotalEnd()
        TotalAfterDiscount.Text = Math.Round(Val(Total.Text) - Val(DiscountValue.Text) + Val(Taxvalue.Text) + Val(ServiceValue.Text), 2)
    End Sub

    Dim DontClear As Boolean = False
    Private Sub btnCloseTable_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCloseTable.Click
        If btnPrint.IsEnabled Then
            AllowClose = False
            DontClear = True
            btnSave_Click(btnCloseTable, e)
            DontClear = False
            If Not AllowClose Then Return
        End If
        If Not bm.ExcuteNonQuery("update SalesMaster set IsClosed=1,ClosedDate=getdate(),DayDate='" & bm.ToStrDate(Md.CurrentDate) & "',Shift=" & Md.CurrentShiftId & " where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and InvoiceNo=" & InvoiceNo.Text) Then Return
        btnNew_Click(sender, e)
    End Sub

    Private Sub IsClosed_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles IsClosed.Checked, IsClosed.Unchecked
        btnCloseTable.IsEnabled = Not IsClosed.IsChecked
    End Sub


    Private Sub IsCashierPrinted_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles IsCashierPrinted.Checked, IsCashierPrinted.Unchecked
        btnSave.IsEnabled = Not (IsCashierPrinted.IsChecked AndAlso Md.Cashier = 1 AndAlso Md.Manager = 0)
        btnPrint.IsEnabled = Not (IsCashierPrinted.IsChecked AndAlso Md.Cashier = 1 AndAlso Md.Manager = 0)
        btnDelete.IsEnabled = Not (IsCashierPrinted.IsChecked AndAlso Md.Cashier = 1 AndAlso Md.Manager = 0)
    End Sub

    Private Sub btnTableClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        If ChkSplite.IsChecked Then
            LoadSubTables(x.Tag)
        Else
            GetTable(x.Tag, 1)
        End If
    End Sub

    Sub GetTable(ByVal MainTable As Integer, ByVal SubTable As Integer)
        InvoiceNo.Text = bm.ExecuteScalar("select InvoiceNo from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and TableId=" & MainTable & " and TableSubId=" & SubTable & " and IsClosed=0")
        txtID_Leave(Nothing, Nothing)
        TableId.Text = MainTable
        TableSubId.Text = SubTable
        TId_LostFocus(TableId, Nothing)
        TabControl1.SelectedItem = TabItem1
    End Sub


    Private Sub TabControl1_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles TabControl1.SelectionChanged
        If e.AddedItems(0) Is TabItemTables Then
            LoadTables()
        ElseIf e.AddedItems(0) Is TabItemDelivery Then
            LoadUnPaiedInvoices()
        End If
    End Sub

    Private Sub btnDeliveryClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        InvoiceNo.Text = x.Tag
        txtID_Leave(Nothing, Nothing)
        TId_LostFocus(TableId, Nothing)
        TabControl1.SelectedItem = TabItem1
    End Sub

    Private Sub TestDoublicatinInTables(ByVal msg As Boolean)
        If TableId.Text.Trim = "" Or IsClosed.IsChecked Then Return
        Dim s As String = bm.ExecuteScalar("select InvoiceNo from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and TableId=" & TableId.Text & " and TableSubId=" & TableSubId.Text & " and IsClosed=0")
        If s <> "" AndAlso s <> InvoiceNo.Text Then
            If msg Then bm.ShowMSG("هذه المائدة مفتوحة بمسلسل " & s)
            TableSubId.Clear()
        End If
    End Sub

    Private Sub ChkSplite_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ChkSplite.Checked
        SpliteScrollViewer.Visibility = Visibility.Visible
        WSubTables.Children.Clear()
    End Sub
    Private Sub ChkSplite_UnChecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ChkSplite.Unchecked
        SpliteScrollViewer.Visibility = Visibility.Hidden
        WSubTables.Children.Clear()
    End Sub

    Private Sub LoadSubTables(ByVal MyTag As Integer)
        WSubTables.Children.Clear()
        Dim z As Integer = Val(bm.ExecuteScalar("select top 1 MaxSubTable from Statics"))
        Dim dtInv As DataTable = bm.ExcuteAdapter("select InvoiceNo,TableId,TableSubId,dbo.ToStrTime(OpennedDate) OpennedTime,NoOfPersons,IsCashierPrinted from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and IsClosed=0")
        For i As Integer = 1 To z
            Try
                Dim x As New Button
                x.Name = "SubTable_" & i
                x.Tag = MyTag
                x.Width = 50
                x.Height = 50
                x.Cursor = Input.Cursors.Pen
                x.Content = i
                WSubTables.Children.Add(x)

                If dtInv.Select("TableId=" & x.Tag & " and TableSubId=" & i).Length > 0 Then
                    If dtInv.Select("TableId=" & x.Tag & " and TableSubId=" & i)(0)("IsCashierPrinted") = 1 Then
                        x.Background = System.Windows.Media.Brushes.Magenta
                    Else
                        x.Background = System.Windows.Media.Brushes.Red
                    End If
                Else
                    x.Background = System.Windows.Media.Brushes.LimeGreen
                End If

                AddHandler x.Click, AddressOf btnSubTableClick
            Catch
            End Try
        Next

    End Sub

    Private Sub btnSubTableClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = CType(sender, Button)
        GetTable(x.Tag, x.Name.Replace("SubTable_", ""))
    End Sub

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        Try
            bm.ShowHelpAddingItems(Me, sender, e, G.Grid.CurrentRow.Index + 1)
        Catch ex As Exception
        End Try
    End Sub

    Sub SetStyle(ByVal x As Button)
        x.Style = Application.Current.FindResource("GlossyCloseButton")
        x.VerticalContentAlignment = Windows.VerticalAlignment.Center
        x.Width = 100
        x.Height = 50
        x.Margin = New Thickness(10, 10, 0, 0)
    End Sub

    Dim lop2 As Boolean = False
    Private Sub DiscountPerc_TextChanged(sender As Object, e As TextChangedEventArgs) Handles DiscountPerc.TextChanged
        If lop OrElse lop2 Then Return
        DiscountValue.Text = Math.Round(Val(Total.Text) * Val(DiscountPerc.Text) / 100, 2)
        CalcTotal()
    End Sub

    Private Sub Payed_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Payed.TextChanged, TotalAfterDiscount.TextChanged
        Remaining.Clear()
        If Val(Payed.Text) = 0 Then Return
        Remaining.Text = Val(Payed.Text) - IIf(Val(CashValue.Text) > 0, Val(CashValue.Text), Val(TotalAfterDiscount.Text))
    End Sub

End Class
