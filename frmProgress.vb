Imports FastReport.Gauge.Simple
Imports Org.BouncyCastle.Asn1.Cmp

Public Class frmProgress
    Public Property ProgressValue As Integer
        Get
            Return pbProgress.Value
        End Get
        Set(value As Integer)
            pbProgress.Value = value
            lblStatus.Text = $"Обработка... {value}%"
            Me.Refresh()
        End Set
    End Property
End Class
