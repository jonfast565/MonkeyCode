using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    class Symbol
    {
        public string Name { get; set; }
        public SymbolLocation Location { get; set; }
        public int Offset { get; set; }
    }
}
