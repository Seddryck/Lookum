using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mrdb.Etl.CompanyContract
{
    class NotLoadedLookupException : InvalidOperationException
    {
        public NotLoadedLookupException()
            : base()
        {

        }
    }
}
