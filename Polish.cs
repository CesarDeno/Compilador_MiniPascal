using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class Polish
    {
        private String lexema;
        private String etiqueta;
        private String apuntador;

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
        public String Etiqueta
        {
            get
            {
                return etiqueta;
            }

            set
            {
                etiqueta = value;
            }
        }

        public String Apuntador
        {
            get
            {
                return apuntador;
            }

            set
            {
                apuntador = value;
            }
        }
    }
}
