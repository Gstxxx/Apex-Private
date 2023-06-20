using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Private_Apex.Translator
{
    public readonly struct LanguagePackage
    {
        public LanguagePackage(string identification, string value)
        {
            ID = identification;
            Value = value;
        }

        public string ID { get; init; }
        public string Value { get; init; }
    }
}
