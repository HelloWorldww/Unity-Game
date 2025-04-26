using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class Termination
    {
        public string commas { get; set; }
        public string value { get; set; }
    }

    public static class Currency
    {
        public static string CurrencyText(this double amount)
        {
            //long length = amount.ToString().Length;
            int length = (int)Math.Ceiling(Math.Log10(amount));

            Termination[] termination = {
            new Termination { commas = ",", value = "K" },
            new Termination { commas = ",,", value = "M" },
            new Termination { commas = ",,,", value = "B" },
            new Termination { commas = ",,,,", value = "T" },
            new Termination { commas = ",,,,,", value = "Qa" },
            new Termination { commas = ",,,,,,", value = "Qi" },
            new Termination { commas = ",,,,,,,", value = "Sx" },
            new Termination { commas = ",,,,,,,,", value = "Sp" },
            new Termination { commas = ",,,,,,,,,", value = "Oc" },
            new Termination { commas = ",,,,,,,,,,", value = "No" },
            new Termination { commas = ",,,,,,,,,,,", value = "De" },
            new Termination { commas = ",,,,,,,,,,,,", value = "Und" },
            new Termination { commas = ",,,,,,,,,,,,,", value = "Dud" },
            new Termination { commas = ",,,,,,,,,,,,,,", value = "Trd" },
            new Termination { commas = ",,,,,,,,,,,,,,,", value = "Qad" },
            new Termination { commas = ",,,,,,,,,,,,,,,,", value = "Qid" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,", value = "Sxd" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,", value = "Spt" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,", value = "Nod" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,", value = "Vig" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,", value = "Cen" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,", value = "A" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,", value = "B" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,", value = "C" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,", value = "D" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,", value = "E" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "F" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "G" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "H" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "I" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "J" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "K" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "L" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "M" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "N" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "O" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "P" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Q" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "R" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "S" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "T" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "U" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "V" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "W" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "X" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Y" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Z" }, // 46
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Aa" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Bb" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Cc" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Dd" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Ee" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Ff" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Gg" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Hh" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Ii" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Jj" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Kk" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Ll" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Mm" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Nn" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Oo" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Pp" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Qq" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Rr" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Ss" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Tt" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Uu" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Vv" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Ww" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Xx" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Yy" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "Zz" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "AAa" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "BBb" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "CCc" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "DDd" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "EEe" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "FFf" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "GGg" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "HHh" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "IIi" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "JJj" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "KKk" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "LLl" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "MMm" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "NNn" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "OOo" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "PPp" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "QQq" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "RRr" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "SSs" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "TTt" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "UUu" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "VVv" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "WWw" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value = "XXx" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value= "YYy" },
            new Termination { commas = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", value= "ZZz" },
        };

            if (length > 297)
            {
                return amount.ToString($"0{termination[98].commas}.##{termination[98].value}");
            }
            else if (length > 294)
            {
                return amount.ToString($"0{termination[97].commas}.##{termination[97].value}");
            }
            else if (length > 291)
            {
                return amount.ToString($"0{termination[96].commas}.##{termination[96].value}");
            }
            else if (length > 288)
            {
                return amount.ToString($"0{termination[95].commas}.##{termination[95].value}");
            }
            else if (length > 285)
            {
                return amount.ToString($"0{termination[94].commas}.##{termination[94].value}");
            }
            else if (length > 282)
            {
                return amount.ToString($"0{termination[93].commas}.##{termination[93].value}");
            }
            else if (length > 279)
            {
                return amount.ToString($"0{termination[92].commas}.##{termination[92].value}");
            }
            else if (length > 276)
            {
                return amount.ToString($"0{termination[91].commas}.##{termination[91].value}");
            }
            else if (length > 273)
            {
                return amount.ToString($"0{termination[90].commas}.##{termination[90].value}");
            }
            else if (length > 270)
            {
                return amount.ToString($"0{termination[89].commas}.##{termination[89].value}");
            }
            else if (length > 267)
            {
                return amount.ToString($"0{termination[88].commas}.##{termination[88].value}");
            }
            else if (length > 264)
            {
                return amount.ToString($"0{termination[87].commas}.##{termination[87].value}");
            }
            else if (length > 261)
            {
                return amount.ToString($"0{termination[86].commas}.##{termination[86].value}");
            }
            else if (length > 258)
            {
                return amount.ToString($"0{termination[85].commas}.##{termination[85].value}");
            }
            else if (length > 255)
            {
                return amount.ToString($"0{termination[84].commas}.##{termination[84].value}");
            }
            else if (length > 252)
            {
                return amount.ToString($"0{termination[83].commas}.##{termination[83].value}");
            }
            else if (length > 249)
            {
                return amount.ToString($"0{termination[82].commas}.##{termination[82].value}");
            }
            else if (length > 246)
            {
                return amount.ToString($"0{termination[81].commas}.##{termination[81].value}");
            }
            else if (length > 243)
            {
                return amount.ToString($"0{termination[80].commas}.##{termination[80].value}");
            }
            else if (length > 240)
            {
                return amount.ToString($"0{termination[79].commas}.##{termination[79].value}");
            }
            else if (length > 237)
            {
                return amount.ToString($"0{termination[78].commas}.##{termination[78].value}");
            }
            else if (length > 234)
            {
                return amount.ToString($"0{termination[77].commas}.##{termination[77].value}");
            }
            else if (length > 231)
            {
                return amount.ToString($"0{termination[76].commas}.##{termination[76].value}");
            }
            else if (length > 228)
            {
                return amount.ToString($"0{termination[75].commas}.##{termination[75].value}");
            }
            else if (length > 225)//13
            {
                return amount.ToString($"0{termination[74].commas}.##{termination[74].value}");
            }
            else if (length > 222)
            {
                return amount.ToString($"0{termination[73].commas}.##{termination[73].value}");
            }
            else if (length > 219)
            {
                return amount.ToString($"0{termination[72].commas}.##{termination[72].value}");
            }
            else if (length > 216)
            {
                return amount.ToString($"0{termination[71].commas}.##{termination[71].value}");
            }
            else if (length > 213)
            {
                return amount.ToString($"0{termination[70].commas}.##{termination[70].value}");
            }
            else if (length > 210)
            {
                return amount.ToString($"0{termination[69].commas}.##{termination[69].value}");
            }
            else if (length > 207)
            {
                return amount.ToString($"0{termination[68].commas}.##{termination[68].value}");
            }
            else if (length > 204)
            {
                return amount.ToString($"0{termination[67].commas}.##{termination[67].value}");
            }
            else if (length > 201)
            {
                return amount.ToString($"0{termination[66].commas}.##{termination[66].value}");
            }
            else if (length > 198)
            {
                return amount.ToString($"0{termination[65].commas}.##{termination[65].value}");
            }
            else if (length > 195)
            {
                return amount.ToString($"0{termination[64].commas}.##{termination[64].value}");
            }
            else if (length > 192)
            {
                return amount.ToString($"0{termination[63].commas}.##{termination[63].value}");
            }
            else if (length > 189)
            {
                return amount.ToString($"0{termination[62].commas}.##{termination[62].value}");
            }
            else if (length > 186)
            {
                return amount.ToString($"0{termination[61].commas}.##{termination[61].value}");
            }
            else if (length > 183)
            {
                return amount.ToString($"0{termination[60].commas}.##{termination[60].value}");
            }
            else if (length > 180)
            {
                return amount.ToString($"0{termination[59].commas}.##{termination[59].value}");
            }
            else if (length > 177)
            {
                return amount.ToString($"0{termination[58].commas}.##{termination[58].value}");
            }
            else if (length > 174)
            {
                return amount.ToString($"0{termination[57].commas}.##{termination[57].value}");
            }
            else if (length > 171)
            {
                return amount.ToString($"0{termination[56].commas}.##{termination[56].value}");
            }
            else if (length > 168)
            {
                return amount.ToString($"0{termination[55].commas}.##{termination[55].value}");
            }
            else if (length > 165)
            {
                return amount.ToString($"0{termination[54].commas}.##{termination[54].value}");
            }
            else if (length > 162)
            {
                return amount.ToString($"0{termination[53].commas}.##{termination[53].value}");
            }
            else if (length > 159)
            {
                return amount.ToString($"0{termination[52].commas}.##{termination[52].value}");
            }
            else if (length > 156)
            {
                return amount.ToString($"0{termination[51].commas}.##{termination[51].value}");
            }
            else if (length > 153)
            {
                return amount.ToString($"0{termination[50].commas}.##{termination[50].value}");
            }
            else if (length > 150)
            {
                return amount.ToString($"0{termination[49].commas}.##{termination[49].value}");
            }
            else if (length > 147)//13
            {
                return amount.ToString($"0{termination[48].commas}.##{termination[48].value}");
            }
            else if (length > 144)
            {
                return amount.ToString($"0{termination[47].commas}.##{termination[47].value}");
            }
            else if (length > 141)
            {
                return amount.ToString($"0{termination[46].commas}.##{termination[46].value}");
            }
            else if (length > 138)
            {
                return amount.ToString($"0{termination[45].commas}.##{termination[45].value}");
            }
            else if (length > 135)
            {
                return amount.ToString($"0{termination[44].commas}.##{termination[44].value}");
            }
            else if (length > 132)
            {
                return amount.ToString($"0{termination[43].commas}.##{termination[43].value}");
            }
            else if (length > 129)
            {
                return amount.ToString($"0{termination[42].commas}.##{termination[42].value}");
            }
            else if (length > 126)
            {
                return amount.ToString($"0{termination[41].commas}.##{termination[41].value}");
            }
            else if (length > 123)
            {
                return amount.ToString($"0{termination[40].commas}.##{termination[40].value}");
            }
            else if (length > 120)
            {
                return amount.ToString($"0{termination[39].commas}.##{termination[39].value}");
            }
            else if (length > 117)
            {
                return amount.ToString($"0{termination[38].commas}.##{termination[38].value}");
            }
            else if (length > 114)
            {
                return amount.ToString($"0{termination[37].commas}.##{termination[37].value}");
            }
            else if (length > 111)
            {
                return amount.ToString($"0{termination[36].commas}.##{termination[36].value}");
            }
            else if (length > 108)
            {
                return amount.ToString($"0{termination[35].commas}.##{termination[35].value}");
            }
            else if (length > 105)
            {
                return amount.ToString($"0{termination[34].commas}.##{termination[34].value}");
            }
            else if (length > 102)//13
            {
                return amount.ToString($"0{termination[33].commas}.##{termination[33].value}");
            }
            else if (length > 99)
            {
                return amount.ToString($"0{termination[32].commas}.##{termination[32].value}");
            }
            else if (length > 96)
            {
                return amount.ToString($"0{termination[31].commas}.##{termination[31].value}");
            }
            else if (length > 93)
            {
                return amount.ToString($"0{termination[30].commas}.##{termination[30].value}");
            }
            else if (length > 90)
            {
                return amount.ToString($"0{termination[29].commas}.##{termination[29].value}");
            }
            else if (length > 87)
            {
                return amount.ToString($"0{termination[28].commas}.##{termination[28].value}");
            }
            else if (length > 84)
            {
                return amount.ToString($"0{termination[27].commas}.##{termination[27].value}");
            }
            else if (length > 81)
            {
                return amount.ToString($"0{termination[26].commas}.##{termination[26].value}");
            }
            else if (length > 78)
            {
                return amount.ToString($"0{termination[25].commas}.##{termination[25].value}");
            }
            else if (length > 75)
            {
                return amount.ToString($"0{termination[24].commas}.##{termination[24].value}");
            }
            else if (length > 72)
            {
                return amount.ToString($"0{termination[23].commas}.##{termination[23].value}");
            }
            else if (length > 69)
            {
                return amount.ToString($"0{termination[22].commas}.##{termination[22].value}");
            }
            else if (length > 66)
            {
                return amount.ToString($"0{termination[21].commas}.##{termination[21].value}");
            }
            else if (length > 63)
            {
                return amount.ToString($"0{termination[20].commas}.##{termination[20].value}");
            }
            else if (length > 60)
            {
                return amount.ToString($"0{termination[19].commas}.##{termination[19].value}");
            }
            else if (length > 57)
            {
                return amount.ToString($"0{termination[18].commas}.##{termination[18].value}");
            }
            else if (length > 54)
            {
                return amount.ToString($"0{termination[17].commas}.##{termination[17].value}");
            }
            else if (length > 51)
            {
                return amount.ToString($"0{termination[16].commas}.##{termination[16].value}");
            }
            else if (length > 48)
            {
                return amount.ToString($"0{termination[15].commas}.##{termination[15].value}");
            }
            else if (length > 45)
            {
                return amount.ToString($"0{termination[14].commas}.##{termination[14].value}");
            }
            else if (length > 42)
            {
                return amount.ToString($"0{termination[13].commas}.##{termination[13].value}");
            }
            else if (length > 39)
            {
                return amount.ToString($"0{termination[12].commas}.##{termination[12].value}");
            }
            else if (length > 36)
            {
                return amount.ToString($"0{termination[11].commas}.##{termination[11].value}");
            }
            else if (length > 33)
            {
                return amount.ToString($"0{termination[10].commas}.##{termination[10].value}");
            }
            else if (length > 30)
            {
                return amount.ToString($"0{termination[9].commas}.##{termination[9].value}");
            }
            else if (length > 27)
            {
                return amount.ToString($"0{termination[8].commas}.##{termination[8].value}");
            }
            else if (length > 24)
            {
                return amount.ToString($"0{termination[7].commas}.##{termination[7].value}");
            }
            else if (length > 21)
            {
                return amount.ToString($"0{termination[6].commas}.##{termination[6].value}");
            }
            else if (length > 18)
            {
                return amount.ToString($"0{termination[5].commas}.##{termination[5].value}");
            }
            else if (length > 15)
            {
                return amount.ToString($"0{termination[4].commas}.##{termination[4].value}");
            }
            else if (length > 12)
            {
                return amount.ToString($"0{termination[3].commas}.##{termination[3].value}");
            }
            else if (length > 9)
            {
                return amount.ToString($"0{termination[2].commas}.##{termination[2].value}");
            }
            else if (length > 6)
            {
                return amount.ToString($"0{termination[1].commas}.##{termination[1].value}");
            }
            else if (length > 3 && amount >= 2000)
            {
                return amount.ToString($"0{termination[0].commas}.##{termination[0].value}");
            }

            return amount.ToString("0.##");
        }
    }
}