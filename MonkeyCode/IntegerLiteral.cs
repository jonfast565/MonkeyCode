using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCode
{
    class IntegerLiteral : IValue
    {
        public int Value { get; set; }

        public string GetValue()
        {
            return Value.ToString();
        }
    }
}
