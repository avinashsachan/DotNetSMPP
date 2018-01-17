using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSMPP
{
    public class SequenceProvider
    {
        private int _seq { get; set; }
        private object Obj_Seq = new object();
        public SequenceProvider()
        {
            _seq = 1;
        }

        public int GetNumber()
        {
            lock (Obj_Seq)
            {
                return _seq++;
            }
        }

        internal void Reset()
        {
            lock (Obj_Seq)
            {
                _seq = 1;
            }
        }
    }

}
