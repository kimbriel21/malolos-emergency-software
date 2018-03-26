using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalolosEmergenctNetworkProject
{
    class RequestReport
    {
        public String request_id { get; set; }
        public String location_longhitude { get; set; }
        public String location_latitude { get; set; }
        public String office_branch { get; set; }
        public String emergency_type { get; set; }
        public String emergency_category { get; set; }
        public String date_requested { get; set; }
        public String status { get; set; }
        public String first_name { get; set; }
        public String last_name { get; set; }
        public String contact_number { get; set; }
        public String address { get; set; }
        public String email { get; set; }
    }
}
