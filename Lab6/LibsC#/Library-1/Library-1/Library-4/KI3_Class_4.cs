using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_3;

namespace Library_4
{
    public class KI3_Class_4
    {
        public static double F4(double x, double y)
        {
            KI3_Class_3 kI3_Class_3 = new KI3_Class_3();
            return 4 * kI3_Class_3.F3(x, y) - 3;
        }
    }
}
