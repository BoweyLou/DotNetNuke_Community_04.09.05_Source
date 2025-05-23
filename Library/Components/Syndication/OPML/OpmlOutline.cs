/*
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
*/

using System;
using System.Collections.Generic;
using System.Xml;

namespace DotNetNuke.Services.Syndication
{
    /// <summary>
    /// Class for managing an OPML feed outline
    /// <history>
    ///     Created     Nik Kalyani     3/1/2007
    /// </history>
    /// </summary>

    public class OpmlOutline
    {
        private string _text = string.Empty;
        private string _type = "rss";
        private bool _isComment = false;
        private bool _isBreakpoint = false;
        private DateTime _created = DateTime.MinValue;
        private string _category = string.Empty;
        private Uri _xmlUrl = null;
        private Uri _htmlUrl = null;
        private string _title = string.Empty;
        private Uri _url = null;
        private string _language = string.Empty;
        private string _description = string.Empty;
        private OpmlOutlines _outlines = null;

        public string Description
        {
            get 
            { 
                return _description; 
            }
            set 
            { 
                _description = value; 
            }
        }

        public string Title
        {
            get 
            { 
                return _title; 
            }
            set 
            { 
                _title = value; 
            }
        }

        public string Type
        {
            get 
            { 
                return _type; 
            }
            set 
            { 
                _type = value; 
            }
        }

        public string Text
        {
            get 
            { 
                return _text; 
            }
            set 
            { 
                _text = value; 
            }
        }

        public Uri HtmlUrl
        {
            get 
            { 
                return _htmlUrl; 
            }
            set 
            { 
                _htmlUrl = value; 
            }
        }

        public Uri XmlUrl
        {
            get 
            { 
                return _xmlUrl; 
            }
            set 
            {
                _xmlUrl = value; 
            }
        }

        public Uri Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                _created = value;
            }
        }

        public bool IsComment
        {
            get
            {
                return _isComment;
            }
            set
            {
                _isComment = value;
            }
        }

        public bool IsBreakpoint
        {
            get
            {
                return _isBreakpoint;
            }
            set
            {
                _isBreakpoint = value;
            }
        }

        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }

        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        public string Version
        {
            get
            {
                return "2.0";
            }
        }


        public OpmlOutlines Outlines
        {
            get 
            {
                return _outlines; 
            }
            set 
            { 
                _outlines = value; 
            }
        }

        public OpmlOutline() : base()
        {
            _outlines = new OpmlOutlines();
        }

        public XmlElement ToXml
        {
            get
            {
                XmlDocument opmlDoc = new XmlDocument();
                XmlElement outlineNode = opmlDoc.CreateElement("outline");

                if (!String.IsNullOrEmpty(Title))
                    outlineNode.SetAttribute("title", Title);

                if (!String.IsNullOrEmpty(Description))
                    outlineNode.SetAttribute("description", Description);

                if (!String.IsNullOrEmpty(Text))
                    outlineNode.SetAttribute("text", Text);

                if (!String.IsNullOrEmpty(Type))
                    outlineNode.SetAttribute("type", Type);

                if (!String.IsNullOrEmpty(Language))
                    outlineNode.SetAttribute("language", Language);

                if (!String.IsNullOrEmpty(Category))
                    outlineNode.SetAttribute("category", Category);

                if (Created > DateTime.MinValue)
                    outlineNode.SetAttribute("created", Created.ToString("r", null));

                if (HtmlUrl != null)
                    outlineNode.SetAttribute("htmlUrl", HtmlUrl.ToString());

                if (XmlUrl != null)
                    outlineNode.SetAttribute("xmlUrl", XmlUrl.ToString());

                if (Url != null)
                    outlineNode.SetAttribute("url", Url.ToString());

                outlineNode.SetAttribute("isComment", (IsComment ? "true" : "false"));
                outlineNode.SetAttribute("isBreakpoint", (IsBreakpoint ? "true" : "false"));

                foreach (OpmlOutline childOutline in Outlines)
                {
                    outlineNode.AppendChild(childOutline.ToXml);
                }

                return outlineNode;
            }
        }
    }

    public class OpmlOutlines : List<OpmlOutline> { }

}