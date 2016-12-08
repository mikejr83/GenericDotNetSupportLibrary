using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Helpers
{
    public partial class StringHelper
    {
        public static Guid ParseGuid(string value)
        {
            Guid g = Guid.Empty;

            Guid outG = Guid.Empty;
            if (Guid.TryParse(value, out outG))
                g = outG;

            return g;
        }
    }
}
