using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex2_BoxMuller
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BoxMuller bm = new BoxMuller();

                StringBuilder sb = new StringBuilder("");
                for (int i = 0; i < 100; ++i)
                {
                    sb.Append(bm.Z1);
                    sb.Append('\t');
                    sb.Append(bm.Z2);
                    sb.AppendLine();

                    bm.Next();
                }
                File.WriteAllText("boxmuller.txt", sb.ToString());
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Exception:", ex.ToString());
            }
        }
    }
}
