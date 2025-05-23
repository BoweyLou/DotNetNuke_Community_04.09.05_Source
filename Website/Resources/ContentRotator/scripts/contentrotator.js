﻿/*
  DotNetNuke® - http://www.dotnetnuke.com
  Copyright (c) 2002-2008
  by DotNetNuke Corporation
 
  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
  to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 
  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
  of the Software.
 
  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
  DEALINGS IN THE SOFTWARE.

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' This script enables client-side rotation of content.
	'''
	''' Ensure that ~/Resources/Shared/scripts/init.js is called from the browser before calling this script
	''' This script will fail if the required AJAX libraries loaded by init.js are not present.
	''' </summary>
	''' <remarks>
	'''	Based mostly on GreyWyvern's HTML content Scroller & Marquee script
	'''	Portions Copyright GreyWyvern 2007
	'''	Licenced for free distribution under the BSDL
	''' </remarks>
	''' <history>
	'''     Version 1.0.0: Feb. 28, 2007, Nik Kalyani, nik.kalyani@dotnetnuke.com 
	''' </history>
	''' -----------------------------------------------------------------------------
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// C O N T E N T R O T A T O R                                                                                //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("DotNetNuke.UI.WebControls.ContentRotator");
// END: Namespace management

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: Rotator class                                                                                       //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
DotNetNuke.UI.WebControls.ContentRotator.Rotator = function(instanceVarName, containerId, width, height, direction, interval, elementIdPrefix, theme, resourcesFolderUrl)
{
      DotNetNuke.UI.WebControls.ContentRotator.Rotator.initializeBase(this, [instanceVarName, resourcesFolderUrl, theme, elementIdPrefix]);
      this._control = "ContentRotator";
      this._theme = (typeof(theme) == "undefined" ? "Simple" : theme);     
      this._rssProxyUrl = (typeof(rssProxyUrl) == "undefined" ? "http://pipes.yahoo.com/pipes/Aqn6j8_H2xG4_N_1JhOy0Q/run?_render=json&_callback=[CALLBACK]&feedUrl=" : rssProxyUrl);
      this.addStyleSheet();
        
      this._height = (typeof(height) == "undefined" ? 200 : height);
      this._width = (typeof(width) == "undefined" ? 400 : width);
      this._direction = (typeof(direction) == "undefined" ? "left" : (direction == "left" || direction == "right" || direction == "up" || direction == "down" ? direction : "left"));
      this._interval = (typeof(interval) == "undefined" ? 2500 : interval);
      
      this._container = $get(containerId);
      this._container.className = "Normal " + this.getStylePrefix() + "Container";
      this._container.style.position = "relative";
      this._container.style.width = this._width + "px";
      this._container.style.height = this._height + "px";
      this._container.style.display = "block";
      this._container.innerHTML = this.displayLoader();
      this._container.style.overflow = "hidden";
      this._offset = (this._direction == "up" || this._direction == "down") ? this._height : this._width;

      this._content = [];
      this._contentprev = 0;
      this._contentcurr = 1;
      this._motion = false;
      this._mouse = false;      
      this._pause = false;
      this._archive = null;
}

DotNetNuke.UI.WebControls.ContentRotator.Rotator.prototype = 
{
        isPaused :
        function()
        {
            return(this._pause);
        },
        
        resume :
        function()
        {
            if (this._pause)
            {
                this._pause = false;
                // Make sure container is visible
                this._container.style.display = "block";
            }
        },
        
        pause :
        function(hide)
        {
            if (!this._pause)
            {
                this._pause = true;
                if (hide)
                    this._container.style.display = "none";
            }
        },
        
        getContainer :
        function()
        {
            return(this._container);
        },
        
        addContent :
        function(content)
        {
            var div = document.createElement("div");
            div.style.position = "absolute";
            div.style.width = this._width + "px";
            div.style.height = this._height + "px";
            div.style.overflow = "hidden";
            div.style.left = div.style.top = "0px";
            switch (this._direction) 
            {
                case "up": div.style.top = this._height + "px"; break;
                case "down": div.style.top = -(this._height + 2) + "px"; break;
                case "left": div.style.left = this._width + "px"; break;
                case "right": div.style.left = -(this._width + 2) + "px"; break;
            }
            
            div.innerHTML = content;
            this._container.appendChild(this._content[this._content.length] = div);
        },
        
        addFeedContent :
        function(url, attributeToUse)
        {            
            // Create a new function
            var counter = 0;
            try
            {
                while(eval(this._instanceVarName + counter))
                    counter++;
            }
            catch(e)
            {
            }

            // Dynamically create a callback function and pass to it the instance name and callback data
            eval(this._instanceVarName + counter + " = new Function(\"data\", \"rssRenderingHandler('" + this._instanceVarName + "', data,'" + attributeToUse + "')\")");

            var newScript = document.createElement("script");
            newScript.type = "text/javascript";
            newScript.src = this.getRssProxyUrl(this._instanceVarName + counter) + url.urlEncode();
            document.getElementsByTagName("head")[0].appendChild(newScript);    
        },
        
        getRssProxyUrl :
        function(callback)
        {
            return(this._rssProxyUrl.replace("[CALLBACK]", callback));
        },

        // BEGIN: scrollLoop
        _scrollLoop : 
        function() 
        {
            if (this._content.length == 0)
            {
                var self = this;
                setTimeout
                (
                    self.getInstanceVarName() + "._scrollLoop()",
                        50
                 );
            }
            
            if (this._pause)
                return false;
                
            if (!this._motion && this._mouse) 
                return false;

            if (this._offset == 1) 
            {
                // Content has scrolled to its destination
                this._contentprev = this._contentcurr;
                this._contentcurr = (this._contentcurr + 1 >= this._content.length) ? 0 : this._contentcurr + 1;
                if (this._direction == "up" || this._direction == "down") 
                {
                    this._content[this._contentcurr].style.top = ((this._direction == "down") ? "-" : "") + this._height + "px";
                    this._content[this._contentprev].style.top = "0px";
                    this._offset = this._height;
                } 
                else 
                {
                    this._content[this._contentcurr].style.left = ((this._direction == "right") ? "-" : "") + this._width + "px";
                    this._content[this._contentprev].style.left = "0px";
                    this._offset = this._width;
                } 
                this._motion = false;
            } 
            else 
            {
                if (!this._motion) 
                {
                    this._motion = true; 
                    var x = -1;
                    while (true) 
                    { 
                        if (Math.abs(this._offset) - Math.pow(2, ++x) <= Math.abs(this._offset) / 2) break; 
                    }
                    this._offset = (this._direction == "up" || this._direction == "left") ? Math.pow(2, x) : -Math.pow(2, x);
                } 
                else 
                    this._offset /= 2;
                    
                if (this._direction == "up" || this._direction == "down") 
                {
                    this._content[this._contentcurr].style.top = this._offset + "px";
                    this._content[this._contentprev].style.top = (((this._direction == "down") ? this._height : -(this._height + 2)) + this._offset) + "px";
                } 
                else 
                {
                    this._content[this._contentcurr].style.left = this._offset + "px";
                    this._content[this._contentprev].style.left = (((this._direction == "right") ? this._width : -(this._width + 2)) + this._offset) + "px";
                } 
                var self = this;
                setTimeout
                (
                    self.getInstanceVarName() + "._scrollLoop()",
                    30
                 );
            }
            
        },
        // END: scrollLoop
        
        // BEGIN: scroll
        scroll : 
        function()
        {
            while (this._container.firstChild) 
                    this._container.removeChild(this._container.firstChild);

            var self = this;
            this._container.onmouseover = function() 
            { 
                self._mouse = true;

            }
            
            this._container.onmouseout = function() 
            { 
                self._mouse = false;
            }
                
            setInterval        
            (
                self.getInstanceVarName() + "._scrollLoop()",
                self._interval
            );
        }                
        // END: scroll        
}
DotNetNuke.UI.WebControls.ContentRotator.Rotator.registerClass("DotNetNuke.UI.WebControls.ContentRotator.Rotator", DotNetNuke.UI.WebControls.BaseControl);
// END: Rotator class

function rssRenderingHandler(instanceVarName, result, attributeToUse)
{
    var instance = eval(instanceVarName);

    if (result != null)
    {
        var itemCollection = result.value["items"];
        for(var item in itemCollection)
        {   
            var itemData = [];
            if (typeof(itemCollection[item]) == "object")
            {
                var ivalues = DotNetNuke.UI.WebControls.Utility.recurseElement("", itemCollection[item]);
                for(var iv in ivalues)
                    itemData[iv] = ivalues[iv];
            }
            else
                itemData[item] = itemCollection[item];

            if (itemData[attributeToUse])
                instance.addContent(itemData[attributeToUse])                
        }

    }
    
}
