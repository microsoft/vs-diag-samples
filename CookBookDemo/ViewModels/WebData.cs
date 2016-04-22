using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.ViewModels
{
    public class WebData
    {
        public static void GetData()
        {
            //fake web call on UI thread or long computation--Demo
            for (int i = 0; i < Int32.MaxValue / 2; i++) ;
        }
    }
}
