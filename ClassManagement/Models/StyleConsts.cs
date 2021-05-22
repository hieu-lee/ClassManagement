using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Models
{
    public class StyleConsts
    {
        public static Dictionary<string, string> ImgSourceLookUp = new()
        {
            ["0"] = Icons.Material.Filled.KeyboardArrowDown,
            ["30px"] = Icons.Material.Filled.KeyboardArrowUp
        };
        public static Dictionary<string, string> ImgSourceLookUp2 = new()
        {
            ["0"] = Icons.Material.Filled.KeyboardArrowDown,
            ["130px"] = Icons.Material.Filled.KeyboardArrowUp
        };
    }
}
