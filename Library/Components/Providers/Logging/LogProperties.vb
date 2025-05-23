'
' DotNetNukeŽ - http://www.dotnetnuke.com
' Copyright (c) 2002-2009
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports System.IO
Imports System.Text
Imports System.Xml

Namespace DotNetNuke.Services.Log.EventLog

    Public Class LogProperties
        Inherits ArrayList

#Region "Public Properties"

        Public ReadOnly Property Summary() As String
            Get
                Return Left(Me.ToString, 75)
            End Get
        End Property

#End Region

#Region "Public Methods"

        Public Sub Deserialize(ByVal content As String)
            Using reader As XmlReader = XmlReader.Create(New StringReader(content))
                reader.ReadStartElement("LogProperties")
                If reader.ReadState <> ReadState.EndOfFile And reader.NodeType <> XmlNodeType.None And reader.LocalName <> "" Then
                    ReadXml(reader)
                End If
                reader.Close()
            End Using
        End Sub

        Public Sub ReadXml(ByVal reader As XmlReader)
            Do
                reader.ReadStartElement("LogProperty")

                'Create new LogDetailInfo object
                Dim logDetail As New LogDetailInfo

                'Load it from the Xml
                logDetail.ReadXml(reader)

                'Add to the collection
                Me.Add(logDetail)

            Loop While reader.ReadToNextSibling("LogProperty")
        End Sub

        Public Function Serialize() As String
            Dim settings As New XmlWriterSettings()
            settings.ConformanceLevel = ConformanceLevel.Fragment
            settings.OmitXmlDeclaration = True

            Dim sb As New StringBuilder()

            Dim writer As XmlWriter = XmlWriter.Create(sb, settings)
            WriteXml(writer)
            writer.Close()
            Return sb.ToString()
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder
            For Each logDetail As LogDetailInfo In Me
                sb.Append(logDetail.ToString())
            Next
            Return sb.ToString()
        End Function

        Public Sub WriteXml(ByVal writer As XmlWriter)
            writer.WriteStartElement("LogProperties")
            For Each logDetail As LogDetailInfo In Me
                logDetail.WriteXml(writer)
            Next
            writer.WriteEndElement()
        End Sub

#End Region

    End Class

End Namespace




