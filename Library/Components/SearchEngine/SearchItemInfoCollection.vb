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

Imports System
Imports System.Collections

Namespace DotNetNuke.Services.Search
    ''' -----------------------------------------------------------------------------
    ''' Namespace:  DotNetNuke.Services.Search
    ''' Project:    DotNetNuke
    ''' Class:      SearchItemInfoCollection
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a collection of <see cref="SearchItemInfo">SearchItemInfo</see> objects.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''		[cnurse]	11/15/2004	documented
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class SearchItemInfoCollection
        Inherits CollectionBase

#Region "Constructors"

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> class.
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> class containing the elements of the specified source collection.
        ''' </summary>
        ''' <param name="value">A <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> with which to initialize the collection.</param>
        Public Sub New(ByVal value As SearchItemInfoCollection)
            MyBase.New()
            AddRange(value)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> class containing the specified array of <see cref="SearchItemInfo">SearchItemInfo</see> objects.
        ''' </summary>
        ''' <param name="value">An array of <see cref="SearchItemInfo">SearchItemInfo</see> objects with which to initialize the collection. </param>
        Public Sub New(ByVal value As SearchItemInfo())
            MyBase.New()
            AddRange(value)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SearchItemInfoCollection">SearchItemInfoCollectionSearchItemInfoCollection</see> class containing the specified array of <see cref="SearchItemInfo">SearchItemInfo</see> objects.
        ''' </summary>
        ''' <param name="value">An arraylist of <see cref="SearchItemInfo">SearchItemInfo</see> objects with which to initialize the collection. </param>
        Public Sub New(ByVal value As ArrayList)
            MyBase.New()
            AddRange(value)
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Gets the <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> at the specified index in the collection.
        ''' <para>
        ''' In VB.Net, this property is the indexer for the <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> class.
        ''' </para>
        ''' </summary>
        Default Public Property Item(ByVal index As Integer) As SearchItemInfo
            Get
                Return CType(List(index), SearchItemInfo)
            End Get
            Set(ByVal Value As SearchItemInfo)
                List(index) = Value
            End Set
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Add an element of the specified <see cref="SearchItemInfo">SearchItemInfo</see> to the end of the collection.
        ''' </summary>
        ''' <param name="value">An object of type <see cref="SearchItemInfo">SearchItemInfo</see> to add to the collection.</param>
        Public Function Add(ByVal value As SearchItemInfo) As Integer
            Return List.Add(value)
        End Function    'Add

        ''' <summary>
        ''' Gets the index in the collection of the specified <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see>, if it exists in the collection.
        ''' </summary>
        ''' <param name="value">The <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> to locate in the collection.</param>
        ''' <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
        Public Function IndexOf(ByVal value As SearchItemInfo) As Integer
            Return List.IndexOf(value)
        End Function    'IndexOf

        ''' <summary>
        ''' Add an element of the specified <see cref="SearchItemInfo">SearchItemInfo</see> to the collection at the designated index.
        ''' </summary>
        ''' <param name="index">An <see cref="system.int32">Integer</see> to indicate the location to add the object to the collection.</param>
        ''' <param name="value">An object of type <see cref="SearchItemInfo">SearchItemInfo</see> to add to the collection.</param>
        Public Sub Insert(ByVal index As Integer, ByVal value As SearchItemInfo)
            List.Insert(index, value)
        End Sub    'Insert

        ''' <summary>
        ''' Remove the specified object of type <see cref="SearchItemInfo">SearchItemInfo</see> from the collection.
        ''' </summary>
        ''' <param name="value">An object of type <see cref="SearchItemInfo">SearchItemInfo</see> to remove to the collection.</param>
        Public Sub Remove(ByVal value As SearchItemInfo)
            List.Remove(value)
        End Sub    'Remove

        ''' <summary>
        ''' Gets a value indicating whether the collection contains the specified <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see>.
        ''' </summary>
        ''' <param name="value">The <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> to search for in the collection.</param>
        ''' <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
        Public Function Contains(ByVal value As SearchItemInfo) As Boolean
            ' If value is not of type SearchItemInfo, this will return false.
            Return List.Contains(value)
        End Function    'Contains

        ''' <summary>
        ''' Copies the elements of the specified <see cref="SearchItemInfo">SearchItemInfo</see> array to the end of the collection.
        ''' </summary>
        ''' <param name="value">An array of type <see cref="SearchItemInfo">SearchItemInfo</see> containing the objects to add to the collection.</param>
        Public Sub AddRange(ByVal value As SearchItemInfo())
            For i As Integer = 0 To value.Length - 1
                Add(value(i))
            Next i
        End Sub

        ''' <summary>
        ''' Copies the elements of the specified arraylist to the end of the collection.
        ''' </summary>
        ''' <param name="value">An arraylist of type <see cref="SearchItemInfo">SearchItemInfo</see> containing the objects to add to the collection.</param>
        Public Sub AddRange(ByVal value As ArrayList)
            For Each obj As Object In value
                If TypeOf obj Is SearchItemInfo Then
                    Add(CType(obj, SearchItemInfo))
                End If
            Next
        End Sub

        ''' <summary>
        ''' Adds the contents of another <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> to the end of the collection.
        ''' </summary>
        ''' <param name="value">A <see cref="SearchItemInfoCollection">SearchItemInfoCollection</see> containing the objects to add to the collection. </param>
        Public Sub AddRange(ByVal value As SearchItemInfoCollection)
            For i As Integer = 0 To value.Count - 1
                Add(CType(value.List(i), SearchItemInfo))
            Next i
        End Sub

        ''' <summary>
        ''' Copies the collection objects to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
        ''' </summary>
        ''' <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
        ''' <param name="index">The index of the array at which to begin inserting.</param>
        Public Sub CopyTo(ByVal array As SearchItemInfo(), ByVal index As Integer)
            List.CopyTo(array, index)
        End Sub

        ''' <summary>
        ''' Creates a one-dimensional <see cref="T:System.Array">Array</see> instance containing the collection items.
        ''' </summary>
        ''' <returns>Array of type SearchItemInfo</returns>
        Public Function ToArray() As SearchItemInfo()
            Dim arr As SearchItemInfo()
            ReDim Preserve arr(Count - 1)
            CopyTo(arr, 0)

            Return arr
        End Function

        Public Function ModuleItems(ByVal ModuleId As Integer) As SearchItemInfoCollection
            Dim retValue As New SearchItemInfoCollection
            For Each info As SearchItemInfo In Me
                If info.ModuleId = ModuleId Then
                    retValue.Add(info)
                End If
            Next
            Return retValue
        End Function

#End Region

    End Class

End Namespace

