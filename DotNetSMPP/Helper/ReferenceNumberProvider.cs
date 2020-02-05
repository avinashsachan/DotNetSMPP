using System;

namespace DotNetSMPP
{
    class ReferenceNumberProvider
    {
        private uint _seq { get; set; }
        private object Obj_Seq = new object();
        public ReferenceNumberProvider()
        {
            _seq = 1;
        }

        public uint GetNumber()
        {
            lock (Obj_Seq)
            {
                try
                {
                    return _seq++;
                }
                catch (Exception)
                {
                    _seq = 0;
                    return _seq++;
                }
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
