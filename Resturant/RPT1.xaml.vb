Imports System.Data

Public Class RPT1
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Detail As Integer = 0
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "@CashierId", "@IsClosedOnly", "@ItemId", "@CountryId", "@GroupId", "@TypeId", "@SaveId", "@ToId", "ItemName", "CountryName", "GroupName", "TypeName", "StoreName"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, Shift.SelectedValue.ToString, 0, Val(StoreId.Text), Val(FromInvoice.Text), Val(ToInvoice.Text), 0, Flag, ComboBox1.SelectedValue.ToString, 0, 0, IIf(IsClosedOnly.IsChecked, 1, 0), 0, 0, 0, 0, 0, 0, "", "", "", "", StoreName.Text}
        rpt.Header = Md.MyProject.ToString
        Select Case Detail
            Case 0
                rpt.RptPath = "Sales2.rpt"
            Case 1
                rpt.RptPath = "Sales.rpt"
            Case 2
                rpt.RptPath = "DeletedSales.rpt"
            Case 3
                rpt.RptPath = "ItemsSales5.rpt"
            Case 4
                rpt.RptPath = "ItemsSales8.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded Then Return
        bm.FillCombo("Shifts", Shift, "")
        LoadCbo()
        Shift.SelectedValue = 0
        FromDate.SelectedDate = New DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0)
        StoreId.Text = ""
        StoreId_LostFocus(Nothing, Nothing)
        bm.ApplyKeyDown(New Control() {StoreId, Shift, FromDate, ToDate})
        If Md.Cashier = 1 AndAlso Not Md.Manager = 1 AndAlso Not Md.GeneralManager = 1 Then
            IsClosedOnly.IsChecked = True
            StoreId.Text = Md.StoreId
            StoreId_LostFocus(Nothing, Nothing)
            FromDate.SelectedDate = Md.CurrentDate
            ToDate.SelectedDate = Md.CurrentDate
            StoreId.IsEnabled = False
            'FromDate.IsEnabled = False
            'ToDate.IsEnabled = False
        End If

    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("المخازن", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Stores") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub


    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Stores where Id=" & StoreId.Text.Trim())
    End Sub

    Private Sub LoadCbo()
        Dim dt As New DataTable("tbl")
        dt.Columns.Add("Id")
        dt.Columns.Add("Name")
        dt.Rows.Add(New String() {0, "الكل"})
        Select Case Flag
            Case 1
                dt.Rows.Add(New String() {1, "أرصدة افتتاحية"})
                dt.Rows.Add(New String() {2, "إضافة"})
                dt.Rows.Add(New String() {3, "تسوية إضافة"})
                dt.Rows.Add(New String() {4, "صرف"})
                dt.Rows.Add(New String() {5, "تسوية صرف"})
                dt.Rows.Add(New String() {6, "هدايا"})
                dt.Rows.Add(New String() {7, "هالك"})
                dt.Rows.Add(New String() {8, "تحويل إلى مخزن"})
            Case 2
                dt.Rows.Add(New String() {9, "مشتريات"})
                dt.Rows.Add(New String() {10, "مردودات مشتريات"})
            Case 3
                dt.Rows.Add(New String() {11, "مبيعات الصالة"})
                dt.Rows.Add(New String() {12, "مردودات مبيعات الصالة"})
                dt.Rows.Add(New String() {13, "مبيعات التيك أواى"})
                dt.Rows.Add(New String() {14, "مردودات مبيعات التيك أواى"})
                dt.Rows.Add(New String() {15, "مبيعات التوصيل"})
                dt.Rows.Add(New String() {16, "مردودات مبيعات التوصيل"})
                IsClosedOnly.Visibility = Visibility.Visible
        End Select


        Dim dv As New DataView
        dv.Table = dt
        ComboBox1.ItemsSource = dv
        ComboBox1.SelectedValuePath = "Id"
        ComboBox1.DisplayMemberPath = "Name"
        ComboBox1.SelectedIndex = 0
    End Sub

End Class
