using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bustop_app.ViewModel
{
    public class businfor
    {
        public int Bus_idx { get; set; }
        public string Bus_num { get; set; }
        public string Bus_cnt { get; set; }
        public string Bus_gap { get; set; }
        public string Bus_NowIn { get; set; }
    }
    public class BusTable
    {
        public int BusIdx { get; set; }

        public string BusNum { get; set; } = null!;

        public int BusCnt { get; set; }

        public int BusGap { get; set; }

        public int BusNowIn { get; set; }
    }

}
