﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Drawing

Module Md
    Public LastVersion As Integer = 2
    Public MyProjectType As ProjectType = ProjectType.Full
    Public MyProject As Project = Project.Restuarant
    Public SaleQtySub As Boolean = False
    Public StopProfiler As Boolean = False
    Public AllowCashierToEditPrice As Boolean = False

    Public ConnectionString As String = ""
    Public s As New SqlClient.SqlConnectionStringBuilder
    Public FourceExit As Boolean = False
    Public HasLeft As Boolean = False

    Public GeneralManager, Board, Accountant, SystemAdmin, UserName, ArName, JobId, Manager, LevelId, Password, StoreId, CountryId, CityId, Cashier, CompanyName As String
    Public EnName As String = "Please, Login"
    Public CurrentDate As DateTime
    Public CurrentShiftId As Integer = 0
    Public CurrentShiftName As String = ""

    Enum Project
        Restuarant
    End Enum

    Enum ProjectType
        Full
        PCs
    End Enum

       

End Module
