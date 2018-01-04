using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSMPP
{
    public static class CommandStatus
    {
        public const uint ESME_ROK = 0x00000000;
        public const uint ESME_RINVMSGLEN = 0x00000001;
        public const uint ESME_RINVCMDLEN = 0x00000002;
        public const uint ESME_RINVCMDID = 0x00000003;
        public const uint ESME_RINVBNDSTS = 0x00000004;
        public const uint ESME_RALYBND = 0x00000005;
        public const uint ESME_RINVPRTFLG = 0x00000006;

        public const uint ESME_RINVREGDLVFLG = 0x00000007;
        public const uint ESME_RSYSERR = 0x00000008;

        public const uint ESME_RINVSRCADR = 0x0000000A;

        public const uint ESME_RINVDSTADR = 0x0000000B;
        public const uint ESME_RINVMSGID = 0x0000000C;
        public const uint ESME_RBINDFAIL = 0x0000000D;
        public const uint ESME_RINVPASWD = 0x0000000E;
        public const uint ESME_RINVSYSID = 0x0000000F;

        public const uint ESME_RCANCELFAIL = 0x00000011;
        public const uint ESME_RREPLACEFAIL = 0x00000013;

        public const uint ESME_RMSGQFUL = 0x00000014;
        public const uint ESME_RINVSERTYP = 0x00000015;
        public const uint ESME_RINVNUMDESTS = 0x00000033;
        public const uint ESME_RINVDLNAME = 0x00000034;
        public const uint ESME_RINVDESTFLAG = 0x00000040;

        public const uint ESME_RINVSUBREP = 0x00000042;
        public const uint ESME_RINVESMCLASS = 0x00000043;
        public const uint ESME_RCNTSUBDL = 0x00000044;
        public const uint ESME_RSUBMITFAIL = 0x00000045;
        public const uint ESME_RINVSRCTON = 0x00000048;
        public const uint ESME_RINVSRCNPI = 0x00000049;

        public const uint ESME_RINVDSTTON = 0x00000050;
        public const uint ESME_RINVDSTNPI = 0x00000051;
        public const uint ESME_RINVSYSTYP = 0x00000053;
        public const uint ESME_RINVREPFLAG = 0x00000054;
        public const uint ESME_RINVNUMMSGS = 0x00000055;

        public const uint ESME_RTHROTTLED = 0x00000058;
        public const uint ESME_RINVSCHED = 0x00000061;
        public const uint ESME_RINVEXPIRY = 0x00000062;

        public const uint ESME_RINVDFTMSGID = 0x00000063;
        public const uint ESME_RX_T_APPN = 0x00000064;
        public const uint ESME_RX_P_APPN = 0x00000065;
        public const uint ESME_RX_R_APPN = 0x00000066;
        public const uint ESME_RQUERYFAIL = 0x00000067;

        public const uint ESME_RINVOPTPARSTREAM = 0x000000C0;
        public const uint ESME_ROPTPARNOTALLWD = 0x000000C1;
        public const uint ESME_RINVPARLEN = 0x000000C2;
        public const uint ESME_RMISSINGOPTPARAM = 0x000000C3;
        public const uint ESME_RINVOPTPARAMVAL = 0x000000C4;


        public const uint ESME_RDELIVERYFAILURE = 0x000000FE;
        public const uint ESME_RUNKNOWNERR = 0x000000FF;
        
    }
}
