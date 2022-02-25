using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace PatientRecordApplication
{
     class Staff
     {
          public int PatientID { get; set; }
          public string PatientName { get; set; }
          public double BalanaceOwed { get; set; }
     }
}