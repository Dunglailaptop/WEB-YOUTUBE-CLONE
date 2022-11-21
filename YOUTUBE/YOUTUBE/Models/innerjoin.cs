using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace YOUTUBE.Models
{
    public class innerjoin
    {
       
        public video videoo { get; set; }
        public account accountt { get; set; }
        public User user { get; set; }
        public comment commentt { get; set; }
        public channel channels { get; set; }
        public dangkykenh dk { get; set; }
        public lastvideo lastvideos { get; set; }
        public thongbao thongbaos { get; set; }
        public dangkykenh dangkykenhs { get; set; }
    }
}