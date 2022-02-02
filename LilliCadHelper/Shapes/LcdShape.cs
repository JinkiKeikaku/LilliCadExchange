
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace LilliCadHelper.Shapes
{
    public abstract class LcdShape
    {
        internal abstract void Read(LcdStreamReader sr);
        internal abstract void Write(LcdStreamWriter sw);
    }
}
