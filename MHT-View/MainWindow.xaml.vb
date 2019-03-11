﻿Class MainWindow
    Private _mhtPath As String

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ProcessCommandLineArgs()
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Load the URL passed in
        WebDisplay.Navigate(_mhtPath)

        ' Disable the control so that nobody can click on the webpage
        WebDisplay.IsEnabled = False
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Sub WebDisplay_Navigating(sender As Object, e As NavigatingCancelEventArgs) Handles WebDisplay.Navigating
        ' Reset the size on load
        WebDisplay.Width = 800
        WebDisplay.Height = 600
    End Sub

    Private Sub WebDisplay_LoadCompleted(sender As Object, e As NavigationEventArgs) Handles WebDisplay.LoadCompleted
        ' Size the Window to fit the contents
        WebDisplay.Width = WebDisplay.Document.Body.scrollWidth
        WebDisplay.Height = WebDisplay.Document.Body.scrollHeight

        ' Remove the Scrollbars
        WebDisplay.Document.Body.scroll = "no"

        ' Recenter the window
        UpdateLayout()
        Dim workArea As Rect = SystemParameters.WorkArea
        Left = (workArea.Width - Width) / 2 + workArea.Left
        Top = (workArea.Height - Height) / 2 + workArea.Top
    End Sub

    Private Sub MainWindow_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        ' Capture clicks to make the whole form draggable. 
        If (e.ChangedButton = MouseButton.Left) Then DragMove()
    End Sub

    Private Sub ProcessCommandLineArgs()
        Dim commandLineArgs() As String
        commandLineArgs = Environment.GetCommandLineArgs

        Dim errorMessage As String = Nothing
        ' Argument validation
        If (commandLineArgs Is Nothing OrElse commandLineArgs.LongLength <> 2) Then
            errorMessage = "Invalid number of command line arguments." & Environment.NewLine _
              & "This application only accepts one, mandatory, argument: an *.mht file to display."
        ElseIf Not IO.File.Exists(commandLineArgs(1)) AndAlso
            Not uri.IsWellFormedUriString(commandLineArgs(1), UriKind.RelativeOrAbsolute) Then
            errorMessage = "This is not a valid path: " & Environment.NewLine & commandLineArgs(1)
        ElseIf Not Right(commandLineArgs(1), 4) = ".mht" Then
            errorMessage = "This is not an *.mht file: " & Environment.NewLine & commandLineArgs(1)
        Else
            _mhtPath = commandLineArgs(1)
        End If

        ' If we ran in to some problems we should dump an error message and quit before we even load the form
        If Not String.IsNullOrEmpty(errorMessage) Then
            Console.WriteLine(errorMessage)
            Application.Current.Shutdown()
        End If
    End Sub
End Class
