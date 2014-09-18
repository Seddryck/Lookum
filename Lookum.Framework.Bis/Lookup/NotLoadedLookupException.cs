using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lookum.Framework.Lookup
{
    class NotLoadedLookupException : InvalidOperationException
    {
        public NotLoadedLookupException()
            : base()
        {

        }
    }
}
