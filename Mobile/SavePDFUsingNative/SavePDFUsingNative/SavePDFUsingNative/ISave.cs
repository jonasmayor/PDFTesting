using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavePDFUsingNative
{
    public interface ISave
    {
        string Save(MemoryStream fileStream);
    }
}
