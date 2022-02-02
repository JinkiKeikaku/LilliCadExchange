using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadHelper
{
    internal class LcdStreamWriter
    {
        StreamWriter mSw;
        public LcdStreamWriter(StreamWriter sw)
        {
            mSw = sw;
        }

        public void WriteLine(string s)
        {
            mSw.WriteLine(s);
        }

        public void WriteParameterLine(params object[] args)
        {
            int a;
            mSw.Write("\t");
            for (var i=0; i < args.Length-1; i++)
            {
                if(args[i] is Boolean b)
                {
                    mSw.Write($"{BooleanToInt(b)} ");
                }
                else
                {
                    mSw.Write($"{args[i]} ");
                }
            }
            mSw.WriteLine(args[^1]);
        }
        int BooleanToInt(bool flag) => flag ? 1 : 0;
    }
}
