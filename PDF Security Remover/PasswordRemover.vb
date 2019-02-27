Imports DevExpress.Pdf

Module PasswordRemover

#Region "Variables"
    Dim Password As String
    Dim PasswordFailed As Boolean = False
#End Region

#Region "Subs"
    Sub Main()
        If My.Application.CommandLineArgs.Count > 0 Then
            For Each File As String In My.Application.CommandLineArgs
                If My.Computer.FileSystem.FileExists(File) AndAlso IO.Path.GetExtension(File) = ".pdf" Then
                    Console.WriteLine("Info: Processing File - {0}", IO.Path.GetFileName(File))
                    Using Doc As New PdfDocumentProcessor
                        AddHandler Doc.PasswordRequested, AddressOf PasswordRequested
                        Try
LoadDocument:
                            Doc.LoadDocument(File)
                            Doc.SaveDocument(File)
                        Catch ex As PdfIncorrectPasswordException
                            If Not PasswordFailed Then
                                Console.WriteLine("Error: Incorrect Password..! Try Again!")
                                PasswordFailed = True
                                Password = ""
                                GoTo LoadDocument
                            Else
                                Console.WriteLine("Error: Process Failed. Press any key to exit...")
                                Console.Read()
                                End
                            End If
                        End Try
                    End Using
                End If
            Next
        End If

        Console.WriteLine("Process Completed. Press any key to exit...")
        Console.Read()
    End Sub
#End Region

#Region "Events"
    Private Sub PasswordRequested(ByVal sender As Object, ByVal e As PdfPasswordRequestedEventArgs)
        If Password = "" Then
            Console.Write("Enter Password:")
            Password = Console.ReadLine
        End If
        e.PasswordString = Password
    End Sub
#End Region

End Module
