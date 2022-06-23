using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class Device
    {
        [Key]
        public int ID { get; set; }
        public int DeviceBrand_FK { get; set; }
        public string Devicename { get; set; }
        public double DeviceCost { get; set; }
        public string GeneralDescription { get; set; }
        public string Specification { get; set; }
        public string Screensize { get; set; }
        public string Ram { get; set; }
        public string Rom { get; set; }        
        public string OS_UI { get; set; }
        public string Sim { get; set; }
        public string Dimension { get; set; }
        public string Batterylife { get; set; }
        public string Netw { get; set; }
        public string Display { get; set; }
        public string Frontcamera { get; set; }
        public string Backcamera { get; set; }
        public string Chipset { get; set; }
        public string Processor { get; set; }
        public string MainPhotoUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateMOdified { get; set; }
        public int IsVisible { get; set; }
    }
}
