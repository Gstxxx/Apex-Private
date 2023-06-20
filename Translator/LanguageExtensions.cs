using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Private_Apex.Translator
{
    public static class LanguageExtensions
    {
        public static string At(this List<LanguagePackage> list, string id) => 
            list.FirstOrDefault(item => item.ID == id).Value;
    }
}
