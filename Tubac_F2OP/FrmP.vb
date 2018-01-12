Imports System.Windows.Forms
Imports System.IO
Imports System.Data.SqlClient
Public Class FrmP
#Region "Variables"
    Public con As New Conexion
    Public itemcode As String
    Dim valBarCode As String
    Dim oCompany As SAPbobsCOM.Company
    Dim connectionString As String = Conexion.ObtenerConexion.ConnectionString
    Public Ready As Boolean
    Private Const CP_NOCLOSE_BUTTON As Integer = &H200
    Public Shared oInvGenExit As SAPbobsCOM.Documents
    Public Shared SQL_Conexion As SqlConnection = New SqlConnection()
    Public Shared ba As New List(Of String)
    Public Shared quantity As New List(Of Integer)

#End Region
    Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim myCp As CreateParams = MyBase.CreateParams
            myCp.ClassStyle = myCp.ClassStyle Or CP_NOCLOSE_BUTTON
            Return myCp
        End Get
    End Property

    Public Sub New(ByVal user As String)
        MyBase.New()
        InitializeComponent()
        '  Note which form has called this one
        ToolStripStatusLabel1.Text = user
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim SQL_da As SqlDataAdapter = New SqlDataAdapter("SELECT T0.DocNum FROM OWOR T0 where T0.Type = 'P' and T0.Status = 'P' and T0.DocNum LIKE '" + TextBox2.Text + "%' ORDER BY T0.DocNum", con.ObtenerConexion())
        Dim DT_dat As System.Data.DataTable = New System.Data.DataTable()
        SQL_da.Fill(DT_dat)
        DGV.DataSource = DT_dat
        con.ObtenerConexion.Close()
    End Sub

    Private Sub FrmP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.Select()
        cargaORDER()
    End Sub

    Public Function cargaORDER()
        Dim SQL_da As SqlDataAdapter = New SqlDataAdapter("SELECT T0.DocNum FROM OWOR T0 where T0.Type = 'P' and T0.Status = 'P'", con.ObtenerConexion())
        Dim DT_dat As System.Data.DataTable = New System.Data.DataTable()
        SQL_da.Fill(DT_dat)
        DGV.DataSource = DT_dat
        con.ObtenerConexion.Close()
    End Function

    Private Sub DGV_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV.CellContentClick
        txtOrder.Text = DGV(0, DGV.CurrentCell.RowIndex).Value.ToString()
        Panel1.Visible = True
        DGV2.Visible = True
        Dim SQL_da As SqlDataAdapter = New SqlDataAdapter("SELECT T0.ItemCode, T2.ItemName, T0.PlannedQty FROM OWOR T0 inner join OITM T2 on T0.ItemCode = T2.ItemCode where T0.DocNum= '" + txtOrder.Text + "'", con.ObtenerConexion())
        Dim DT_dat As System.Data.DataTable = New System.Data.DataTable()
        SQL_da.Fill(DT_dat)
        Label3.Text = DT_dat.Rows(0).Item("ItemCode").ToString
        Label5.Text = DT_dat.Rows(0).Item("ItemName").ToString
        Label7.Text = DT_dat.Rows(0).Item("PlannedQty").ToString & " TM"
        con.ObtenerConexion.Close()

        Dim turnoAM_I As DateTime = CType("6:00:00 AM", DateTime)
        Dim turnoAM_F As DateTime = CType("6:00:00 PM", DateTime)

        Dim result As Integer = 0
        Dim result2 As Integer = 0
        result = DateTime.Compare(turnoAM_I, TimeOfDay.ToShortTimeString)
        result2 = DateTime.Compare(turnoAM_F, TimeOfDay.ToShortTimeString)
        If result = -1 Then
            If result2 = 1 Then
                Label9.Text = "Turno Dia"
            Else
                Label9.Text = "Turno Noche"
            End If
        End If
    End Sub

    Private Sub txtOrder_TextChanged(sender As Object, e As EventArgs) Handles txtOrder.TextChanged
        Dim SQL_da As SqlDataAdapter = New SqlDataAdapter("SELECT T0.ItemCode, T0.BaseQty, T0.U_ancho, isnull(T0.LineNum,0) FROM WOR1 T0 where T0.[DocEntry] like '" + txtOrder.Text + "%'", con.ObtenerConexion())
        Dim DT_dat As System.Data.DataTable = New System.Data.DataTable()
        SQL_da.Fill(DT_dat)
        DGV2.DataSource = DT_dat
        con.ObtenerConexion.Close()
    End Sub
End Class
