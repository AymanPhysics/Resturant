Imports System.Data
Imports System.Windows.Threading

Public Class MainMenuItem

    Public NLevel As Boolean = False
    Dim m As MainWindow = Application.Current.MainWindow
    Dim bm As New BasicMethods

    WithEvents t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 1)}

    Sub GetCurrent(ByVal sender As Object, ByVal e As EventArgs) Handles t.Tick
        bm.GetCurrent()
    End Sub


    Public Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem2.Click
        m.TabControl1.Items.Clear()
        m.AddTAB(sender, New Login, False)
    End Sub

    Private Sub MenuItem3_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem3.Click
        Dim frm As New BasicForm With {.TableName = "Groups", .WithImage = True}
        frm.CheckBox1.Visibility = True
        frm.CheckBox1.Content = "قسم بيع"
        frm.Flag = 2
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem4_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem4.Click
        Dim frm As New BasicForm2
        frm.WithImage = True

        frm.MainTableName = "Groups"
        frm.MainSubId = "Id"
        frm.MainSubName = "Name"
        frm.lblMain.Content = "القسم الرئيسى"

        frm.TableName = "Types"
        frm.MainId = "GroupId"
        frm.SubId = "Id"
        frm.SubName = "Name"
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem5_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem5.Click
        Dim frm As New BasicForm1_2 With {.TableName = "PrintingGroups", .SubName2 = "ServerName", .SubName3 = "PrinterName"}
        frm.lblName2.Content = "Server"
        frm.lblName3.Content = "Printer"
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem6_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem6.Click
        m.AddTab(sender, New Items)
    End Sub

    Private Sub MenuItem9_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem9.Click
        m.AddTab(sender, New Stores)
    End Sub

    Private Sub MenuItem10_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem10.Click
        Dim frm As New BasicForm2

        frm.MainTableName = "Stores"
        frm.MainSubId = "Id"
        frm.MainSubName = "Name"
        frm.lblMain.Content = "المخزن"

        frm.TableName = "Tables"
        frm.MainId = "StoreId"
        frm.SubId = "Id"
        frm.SubName = "Name"
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem11_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem11.Click
        m.AddTab(sender, New Suppliers)
    End Sub

    Private Sub MenuItem12_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem12.Click
        m.AddTab(sender, New Customers)
    End Sub

    Private Sub MenuItem13_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem13.Click
        Dim frm As New BasicForm With {.TableName = "Countries"}
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem14_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem14.Click
        Dim frm As New BasicForm2

        frm.MainTableName = "Countries"
        frm.MainSubId = "Id"
        frm.MainSubName = "Name"
        frm.lblMain.Content = "الدولة"

        frm.TableName = "Cities"
        frm.MainId = "CountryId"
        frm.SubId = "Id"
        frm.SubName = "Name"

        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem23_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem23.Click
        Dim frm As New BasicForm3

        frm.MainTableName = "Countries"
        frm.MainSubId = "Id"
        frm.MainSubName = "Name"
        frm.lblMain.Content = "Country"

        frm.Main2TableName = "Cities"
        frm.Main2MainId = "CountryId"
        frm.Main2SubId = "Id"
        frm.Main2SubName = "Name"
        frm.lblMain2.Content = "City"

        frm.TableName = "Areas"
        frm.MainId = "CountryId"
        frm.MainId2 = "CityId"
        frm.SubId = "Id"
        frm.SubName = "Name"

        frm.Flag = 1
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem19_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem19.Click
        m.AddTab(sender, New Employees)
    End Sub

    Private Sub MenuItem21_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem21.Click
        Dim frm As New BasicForm With {.TableName = "Religions"}
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem22_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem22.Click
        Dim frm As New BasicForm With {.TableName = "Jobs"}
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem35_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem35.Click
        Dim frm As New BasicForm With {.TableName = "Shifts"}
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem18_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem18.Click

        If bm.ExecuteScalar("select isnull((select Max(Id) from Shifts),0)") = Md.CurrentShiftId AndAlso bm.IF_Exists("select * from SalesMaster where Flag=" & Sales.FlagState.مبيعات_الصالة & " and IsClosed=0") Then
            bm.ShowMSG("لا يمكن إغلاق الوردية الأخيرة قبل إغلاق الموائد المفتوحة" & vbCrLf & vbCrLf & "برجاء إغلاق الموائد المفتوحة")
            Return
        End If
        If bm.ShowDeleteMSG("إغلاق الوردية لا يمكنك من إعادة فتحها مرة أخرى" & vbCrLf & vbCrLf & "هل أنت متأكد من إغلاق الوردية؟") Then

            Dim frm As New RPT2
            frm.Flag = 3
            frm.Detail = 1
            Dim rpt As New ReportViewer
            rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "@Flag", "@StoreId", "@ItemId", "@GroupId", "@TypeId"}
            rpt.paravalue = New String() {Md.CurrentDate, Md.CurrentDate, Md.CurrentShiftId.ToString, 0, 0, 0, 0, 0}
            rpt.Header = Md.MyProject.ToString
            rpt.RptPath = "ItemsSales2.rpt"
            rpt.ShowDialog()
            bm.ExcuteNonQuery("exec CloseShift")
            bm.ShowMSG("تم إغلاق الوردية")
        End If
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded Then Return
        LoadMenuitem()

        MenuItem7.Visibility = Windows.Visibility.Collapsed
        'MenuItem37.Visibility = Windows.Visibility.Collapsed
        MenuItem38.Visibility = Windows.Visibility.Collapsed
        MenuItem87.Visibility = Windows.Visibility.Collapsed
        'MenuItem45.Visibility = Windows.Visibility.Collapsed
        MenuItem46.Visibility = Windows.Visibility.Collapsed
        MenuItem88.Visibility = Windows.Visibility.Collapsed
        MenuItem91.Visibility = Windows.Visibility.Collapsed
        MenuItem97.Visibility = Windows.Visibility.Collapsed
        MenuItem100.Visibility = Windows.Visibility.Collapsed




        MenuItem53.Visibility = Windows.Visibility.Collapsed
        MenuItem47.Visibility = Windows.Visibility.Collapsed
        MenuItem54.Visibility = Windows.Visibility.Collapsed


    End Sub
    Sub LoadMenuitem()
        Dim dt As DataTable = bm.ExcuteAdapter("Select * From NLevels Where Id='" & Md.LevelId & "'")
        If dt.Rows.Count = 0 Then
            Application.Current.Shutdown()
            Exit Sub
        End If

        For i As Integer = 0 To Menu1.Items.Count - 1
            Try
                Dim item As MenuItem
                item = Menu1.Items(i)
                item.Visibility = Visibility.Visible
                If Not item.Tag Is Nothing And Not item.Tag = "" Then Continue For
                item.Visibility = IIf(dt.Rows(0)(item.Name), Visibility.Visible, Visibility.Collapsed)
                LoadSub(item, dt)
            Catch
            End Try
            Try
                Dim item As Separator
                item = Menu1.Items(i)
                item.Visibility = Visibility.Visible
                If Not item.Tag Is Nothing And Not item.Tag = "" Then Continue For
                item.Visibility = IIf(dt.Rows(0)(item.Name), Visibility.Visible, Visibility.Collapsed)
            Catch
            End Try
        Next

    End Sub

    Sub LoadSub(ByVal item2 As MenuItem, ByVal dt As DataTable)
        For i As Integer = 0 To item2.Items.Count - 1
            Try
                Dim item As MenuItem
                item = item2.Items(i)
                item.Visibility = Visibility.Visible
                If Not item.Tag Is Nothing And Not item.Tag = "" Then Continue For
                item.Visibility = IIf(dt.Rows(0)(item.Name), Visibility.Visible, Visibility.Collapsed)
                LoadSub(item, dt)
            Catch
            End Try
            Try
                Dim item As Separator
                item = item2.Items(i)
                item.Visibility = Visibility.Visible
                If Not item.Tag Is Nothing And Not item.Tag = "" Then Continue For
                item.Visibility = IIf(dt.Rows(0)(item.Name), Visibility.Visible, Visibility.Collapsed)
            Catch
            End Try
        Next
    End Sub

    Private Sub MenuItem17_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem17.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.مبيعات_الصالة
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem24_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem24.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.مردودات_مبيعات_الصالة
        m.AddTab(sender, frm)
    End Sub
    Private Sub MenuItem49_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem49.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.مبيعات_التيك_أواى
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem50_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem50.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.مردودات_مبيعات_التيك_أواى
        m.AddTab(sender, frm)
    End Sub
    Private Sub MenuItem51_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem51.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.مبيعات_التوصيل
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem52_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem52.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.مردودات_مبيعات_التوصيل
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem25_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem25.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.مشتريات
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem26_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem26.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.مردودات_مشتريات
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem27_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem27.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.أرصدة_افتتاحية
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem28_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem28.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.إضافة
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem29_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem29.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.تسوية_إضافة
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem30_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem30.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.صرف
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem31_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem31.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.تسوية_صرف
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem32_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem32.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.هدايا
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem33_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem33.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.هالك
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem34_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem34.Click
        Dim frm As New Sales
        frm.Flag = Sales.FlagState.تحويل_إلى_مخزن
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem55_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem55.Click
        Dim frm As New RPT1
        frm.Flag = 1
        frm.Detail = 2
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem39_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem39.Click
        Dim frm As New RPT1
        frm.Flag = 1
        frm.Detail = 1
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem40_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem40.Click
        Dim frm As New RPT1
        frm.Flag = 1
        frm.Detail = 0
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem56_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem56.Click
        Dim frm As New RPT1
        frm.Flag = 2
        frm.Detail = 2
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem41_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem41.Click
        Dim frm As New RPT1
        frm.Flag = 2
        frm.Detail = 1
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem42_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem42.Click
        Dim frm As New RPT1
        frm.Flag = 2
        frm.Detail = 0
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem57_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem57.Click
        Dim frm As New RPT1
        frm.Flag = 3
        frm.Detail = 2
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem155_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem155.Click
        Dim frm As New RPT1
        frm.Flag = 3
        frm.Detail = 3
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem156_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem156.Click
        Dim frm As New RPT1
        frm.Flag = 3
        frm.Detail = 4
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem43_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem43.Click
        Dim frm As New RPT1
        frm.Flag = 3
        frm.Detail = 1
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem44_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem44.Click
        Dim frm As New RPT1
        frm.Flag = 3
        frm.Detail = 0
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem53_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem53.Click
        Dim frm As New ItemComponants
        frm.Flag = ItemComponants.FlagState.الأصناف_النصف_مصنعة
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem47_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem47.Click
        Dim frm As New ItemComponants
        frm.Flag = ItemComponants.FlagState.الأصناف_التامة
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem48_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem48.Click
        Dim frm As New ItemComponants
        frm.Flag = ItemComponants.FlagState.الكومبو
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem54_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem54.Click
        If bm.ShowDeleteMSG("تحديث أسعار الأصناف على أساس المكونات" & vbCrLf & vbCrLf & "استمرار؟") Then
            bm.ExecuteNonQuery("UpdatePurchasePrice", New String() {"Id"}, New String() {0})
            bm.ShowMSG("تمت عملية التحديث بنجاح")
        End If
    End Sub

    Private Sub MenuItem58_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem58.Click
        Dim frm As New RPT2
        frm.Flag = 3
        frm.Detail = 0
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem59_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem59.Click
        Dim frm As New RPT2
        frm.Flag = 3
        frm.Detail = 1
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem120_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem120.Click
        Dim frm As New RPT2
        frm.Flag = 3
        frm.Detail = 2
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem60_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem60.Click
        Dim frm As New RPT3
        frm.Flag = 3
        frm.Detail = 0
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem61_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem61.Click
        Dim frm As New Levels
        m.AddTab(sender, frm)
        'Dim frm As New NLevels
        'frm.Show()
    End Sub

    Private Sub MenuItem108_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem108.Click
        Dim frm As New ManageTables
        m.AddTab(sender, frm)
    End Sub

    Private Sub MenuItem112_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem112.Click
        Dim frm As New CreditsDebits With {.TableName = "Debits", .LinkFile = 3}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem113_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem113.Click
        Dim frm As New CreditsDebits With {.TableName = "Credits", .LinkFile = 4}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem115_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem115.Click
        Dim frm As New CreditsDebits With {.TableName = "Saves", .LinkFile = 5}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem116_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem116.Click
        Dim frm As New CreditsDebits With {.TableName = "Banks", .LinkFile = 6}
        m.AddTAB(sender, frm)
    End Sub
    Private Sub MenuItem119_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem119.Click
        Dim frm As New Chart
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem110_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem110.Click
        Dim frm As New Income
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem111_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem111.Click
        Dim frm As New Income With {.TableName = "Outcome"}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem104_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem104.Click
        Dim frm As New RPT4 With {.Flag = 1, .Hdr = CType(sender, MenuItem).Header}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem105_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem105.Click
        Dim frm As New RPT4 With {.Flag = 2, .Hdr = CType(sender, MenuItem).Header}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem106_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem106.Click
        Dim frm As New RPT4 With {.Flag = 3, .Hdr = CType(sender, MenuItem).Header}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem107_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem107.Click
        Dim frm As New RPT4 With {.Flag = 4, .Hdr = CType(sender, MenuItem).Header}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem37_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem37.Click
        Dim frm As New RPT12 With {.Flag = 1, .Hdr = CType(sender, MenuItem).Header}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem45_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem45.Click
        Dim frm As New RPT12 With {.Flag = 2, .Hdr = CType(sender, MenuItem).Header}
        m.AddTAB(sender, frm)
    End Sub

    Private Sub MenuItem121_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem121.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"Header"}
        rpt.paravalue = New String() {"العملاء"}
        'rpt.Header = Md.MyProject.ToString
        rpt.RptPath = "Customers.rpt"
        rpt.ShowDialog()
    End Sub

End Class
