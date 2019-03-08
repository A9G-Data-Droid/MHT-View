Class MainWindow
    Private _commandLineArgs() As String

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ProcessCommandLineArgs()

    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If (_commandLineArgs IsNot Nothing AndAlso _commandLineArgs.Count = 1) Then
            WebDisplay.Navigate(_commandLineArgs(0))
        Else
            ErrorArgs()
            Close()
        End If
    End Sub

    Private Sub ProcessCommandLineArgs()
        _commandLineArgs = Environment.GetCommandLineArgs

        If _commandLineArgs.Count <> 1 Then
            ErrorArgs()
            Application.Current.Shutdown()
        End If
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Shared Sub ErrorArgs()
        Console.WriteLine("Invalid number of command line arguments." & Environment.NewLine _
                          & "This application only accepts one argument: an .MHT file to display.")
    End Sub
End Class
