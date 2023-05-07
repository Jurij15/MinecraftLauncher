using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Helpers
{
    public class StringHelper
    {
        public static string ReplaceDisallowedChars(string OriginString)
        {
            //its a little stupid, but xaml names are weird af 
            string RetValue = OriginString;
            RetValue = RetValue.Replace(" ", "APPNAMESPACE");
            RetValue = RetValue.Replace("-", "APPNAMEDASH");
            RetValue = RetValue.Replace("+", "APPNAMEPLUS");
            RetValue = RetValue.Replace(".", "APPNAMEDOT");
            RetValue = RetValue.Replace(",", "APPNAMECOMMA");
            //even more stupid
            RetValue = RetValue.Replace("0", "APPNAMEZERO");
            RetValue = RetValue.Replace("1", "APPNAMEONE");
            RetValue = RetValue.Replace("2", "APPNAMETWO");
            RetValue = RetValue.Replace("3", "APPNAMETHREE");
            RetValue = RetValue.Replace("4", "APPNAMEFOUR");
            RetValue = RetValue.Replace("5", "APPNAMEFIVE");
            RetValue = RetValue.Replace("6", "APPNAMESIX");
            RetValue = RetValue.Replace("7", "APPNAMESEVEN");
            RetValue = RetValue.Replace("8", "APPNAMEEIGHT");
            RetValue = RetValue.Replace("9", "APPNAMENINE");

            RetValue = RetValue.Replace("(", "APPNAMEOPENBRACKET");
            RetValue = RetValue.Replace(")", "APPNAMECLOSEBRACKET");

            RetValue = RetValue.Replace("–", "APPNAMEENDASH"); //i didnt know this existed wth

            return RetValue;
        }

        public static string ReturnDisallowedChars(string OriginString)
        {
            string RetValue = OriginString;
            RetValue = RetValue.Replace("APPNAMESPACE", " ");
            RetValue = RetValue.Replace("APPNAMEDASH", "-");
            RetValue = RetValue.Replace("APPNAMEPLUS", "+");
            RetValue = RetValue.Replace("APPNAMEDOT", ".");
            RetValue = RetValue.Replace("APPNAMECOMMA", ",");

            RetValue = RetValue.Replace("APPNAMEZERO", "0");
            RetValue = RetValue.Replace("APPNAMEONE", "1");
            RetValue = RetValue.Replace("APPNAMETWO", "2");
            RetValue = RetValue.Replace("APPNAMETHREE", "3");
            RetValue = RetValue.Replace("APPNAMEFOUR", "4");
            RetValue = RetValue.Replace("APPNAMEFIVE", "5");
            RetValue = RetValue.Replace("APPNAMESIX", "6");
            RetValue = RetValue.Replace("APPNAMESEVEN", "7");
            RetValue = RetValue.Replace("APPNAMEEIGHT", "8");
            RetValue = RetValue.Replace("APPNAMENINE", "9");

            RetValue = RetValue.Replace("APPNAMEOPENBRACKET", "(");
            RetValue = RetValue.Replace("APPNAMECLOSEBRACKET", ")");

            RetValue = RetValue.Replace("APPNAMEENDASH", "–");

            return RetValue;
        }
    }
}
