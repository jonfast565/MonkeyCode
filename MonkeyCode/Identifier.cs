using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    class Identifier : IValue
    {
        public string Name { get; set; }
        public bool Temporary { get; set; }
        public string GetValue()
        {
            return Name;
        }
    }
}
