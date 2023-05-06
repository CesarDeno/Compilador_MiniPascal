using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class Variable
    {
        private int tipoVar;
        private String lexema;

        public String Lexema
        {
            get
            {
                return lexema;
            }

            set
            {
                lexema = value;
            }
        }
        public int TipoVariable
        {
            get
            {
                return tipoVar;
            }

            set
            {
                tipoVar = value;
            }
        }

        
    }

}
