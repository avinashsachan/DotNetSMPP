using System;

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
