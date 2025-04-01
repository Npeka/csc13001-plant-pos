using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csc13001_plant_pos.Utils;
public class RandomUtils
{
    private static readonly Random _random = new();

    public static int RamdomSixDigitOTP()
    {
        return _random.Next(100000, 999999);
    }
}
