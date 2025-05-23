/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.
 
  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/

using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetNuke.Services.Syndication 
{
    /// <summary>
    /// Late-bound RSS HTTP Handler to publish RSS channel
    /// </summary>
    public class GenericRssHttpHandlerBase : RssHttpHandlerBase<GenericRssChannel, GenericRssElement, GenericRssElement> 
    {
    }
}
