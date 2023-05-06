using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class Lexico
    {
        public List<Error> listaError; 
        public List<Token> listaToken; 

        private string codigoFuente;
        private int linea;

        private int[,] matrizTransicion =
        {
             //  0       1      2       3       4      5      6       7     8     9       10     11    12     13      14      15       16     17      18     19     
            //  let  || Dig ||  +  ||   -   ||  *  ||  /  ||  ;  ||  .  ||  :  ||  ,  ||  (  ||  )  ||  "  ||  >  ||   <  ||   =   ||  EB  || NL  || TAB ||  oc  ||
       /* 0 */{  1   ,   2   , 103  ,  104  ,  105  ,  5   , 114  , 115  ,  8   , 117  , 118  , 119  , 9    , 10   , 11    ,  12    , 0     ,  0   ,  0   , 501  },
       /* 1 */{  1   ,   1   , 100  ,  100  ,  100  , 100  , 100  , 100  , 100  , 100  , 100  , 100  , 100  , 100  , 100   ,  100   , 100   ,  100 , 100  , 100  },
       /* 2 */{  101 ,   2   , 101  ,  101  ,  101  , 101  , 101  , 3    , 101  , 101  , 101  , 101  , 101  , 101  , 101   ,  101   , 101   ,  101 , 101  , 101  },
       /* 3 */{  500 ,   4   , 500  ,  500  ,  500  , 500  , 500  , 500  , 500  , 500  , 500  , 500  , 500  , 500  , 500   ,  500   , 500   ,  500 , 500  , 500  },
       /* 4 */{  102 ,   4   , 102  ,  102  ,  102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102   ,  102   , 102   ,  102 , 102  , 102  },
       /* 5 */{  106 ,   106 , 106  ,  106  ,  6    , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106   ,  106   , 106   ,  106 , 106  , 106  },
       /* 6 */{  6   ,   6   , 6    ,  6    ,  7    , 6    , 6    , 6    , 6    , 6    , 6    , 6    , 6    , 6    , 6     ,  6     , 6     ,  6   , 6    , 6    },
       /* 7 */{  6   ,   6   , 6    ,  6    ,  6    , 0    , 6    , 6    , 6    , 6    , 6    , 6    , 6    , 6    , 6     ,  6     , 6     ,  6   , 6    , 6    },
       /* 8 */{  116 ,   116 , 116  ,  116  ,  116  , 116  , 116  , 116  , 116  , 116  , 116  , 116  , 116  , 116  , 116   ,  113   , 116   ,  116 , 116  , 116  },
       /* 9 */{  9   ,   9   , 9    ,  9    ,  9    , 9    , 9    , 9    , 9    , 9    , 9    , 9    , 120  , 9    , 9     ,  9     , 9     ,  9   , 9    , 9    },
       /*10 */{  107 ,   107 , 107  ,  107  ,  107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107   ,  108   , 107   ,  107 , 107  , 107  },
       /*11 */{  109 ,   109 , 109  ,  109  ,  109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 112  , 109   ,  110   , 109   ,  109 , 109  , 109  },
       /*12 */{  502 ,   502 , 502  ,  502  ,  502  , 502  , 502  , 502  , 502  , 502  , 502  , 502  , 502  , 502  , 502   ,  111   , 502   ,  502 , 502  , 502  },

       };

        public Lexico(string codigoFuenteInterface)
        {
            codigoFuente = codigoFuenteInterface + " ";
            listaToken = new List<Token>();  // inicializar
            listaError = new List<Error>();  // inicializar
        }

        private int esPalabraReservada(string lexema)
        {
            switch (lexema)
            {
                case "program":
                    return 200;
                case "var":
                    return 201;
                case "string":
                    return 202;
                case "integer":
                    return 203;
                case "real":
                    return 204;
                case "boolean":
                    return 205;
                case "begin":
                    return 206;
                case "end":
                    return 207;
                case "read":
                    return 208;
                case "write":
                    return 209;
                case "if":
                    return 210;
                case "then":
                    return 211;
                case "else":
                    return 212;
                case "while":
                    return 213;
                case "do":
                    return 214;
                case "and":
                    return 215;
                case "or":
                    return 216;
                case "not":
                    return 217;
                case "endif":
                    return 218;
                case "endwhile":
                    return 219;
                case "endelse":
                    return 220;
                default:
                    return 100;
            }
        }

        private char SiguienteCaracter(int i)
        {
            return Convert.ToChar(codigoFuente.Substring(i, 1));
        }

        private int RegresarColumna(char caracter)
        {
            if (char.IsLetter(caracter))
            {
                return 0;
            }
            else if (char.IsDigit(caracter))
            {
                return 1;
            }
            else if (caracter.Equals('+'))
            {
                return 2;
            }
            else if (caracter.Equals('-'))
            {
                return 3;
            }
            else if (caracter.Equals('*'))
            {
                return 4;
            }
            else if (caracter.Equals('/'))
            {
                return 5;
            }
            else if (caracter.Equals(';'))
            {
                return 6;
            }
            else if (caracter.Equals('.'))
            {
                return 7;
            }
            else if (caracter.Equals(':'))
            {
                return 8;
            }
            else if (caracter.Equals(','))
            {
                return 9;
            }
            else if (caracter.Equals('('))
            {
                return 10;
            }
            else if (caracter.Equals(')'))
            {
                return 11;
            }
            else if (caracter.Equals('"'))
            {
                return 12;
            }
            else if (caracter.Equals('>'))
            {
                return 13;
            }
            else if (caracter.Equals('<'))
            {
                return 14;
            }
            else if (caracter.Equals('='))
            {
                return 15;
            }
            else if (caracter.Equals(' '))
            {
                return 16;
            }
            else if (caracter.Equals('\n'))
            {
                return 17;
            }
            else if (caracter.Equals('\t'))
            {
                return 18;
            }
            else  //simbolo desconocido
            {
                return 19;
            }

        }

        private TipoToken esTipo(int estado)
        {
            switch (estado)
            {
                case 100:
                    return TipoToken.Identificador;
                case 101:
                    return TipoToken.Numero_Entero;
                case 102:
                    return TipoToken.Numero_Decimal;
                case 103:
                    return TipoToken.Operador_Aritmetico;
                case 104:
                    return TipoToken.Operador_Aritmetico;
                case 105:
                    return TipoToken.Operador_Aritmetico;
                case 106:
                    return TipoToken.Operador_Aritmetico;
                case 107:
                    return TipoToken.Operador_Relacional;
                case 108:
                    return TipoToken.Operador_Relacional;
                case 109:
                    return TipoToken.Operador_Relacional;
                case 110:
                    return TipoToken.Operador_Relacional;
                case 111:
                    return TipoToken.Operador_Relacional;
                case 112:
                    return TipoToken.Operador_Relacional;
                case 113:
                    return TipoToken.Operador_Asignacion;
                case 114:
                    return TipoToken.Simbolo_Puntuacion;
                case 115:
                    return TipoToken.Simbolo_Puntuacion;
                case 116:
                    return TipoToken.Simbolo_Puntuacion;
                case 117:
                    return TipoToken.Simbolo_Puntuacion;
                case 118:
                    return TipoToken.Simbolo_Agrupacion;
                case 119:
                    return TipoToken.Simbolo_Agrupacion;
                case 120:
                    return TipoToken.Cadena;
                default:
                    return TipoToken.Palabra_Reservada;
            }
        }

        private Error ManejoErrores(int estado)
        {
            string mensajeError;

            switch (estado)
            {
                case 500:
                    mensajeError = "Se esperaba digito";
                    break;
                case 501:
                    mensajeError = "Simbolo no valido";
                    break;
                case 502:
                    mensajeError = "Se esperaba =";
                    break;
                default:
                    mensajeError = "Error inesperado";
                    break;
            }
            return new Error() { Codigo = estado, MensajeError = mensajeError, Tipo = tipoError.Lexico, Linea = linea };
        }

        public List<Error> getListaError
        {
            get
            {
                return listaError;
            }
        }

        public List<Token> EjecutarLexico()
        {
            int estado = 0; //  la fila de la matriz y el estado actual del AFD
            int columna = 0; // presenta la columna de la matriz

            char caracterActual;
            string lexema = string.Empty;
            linea = 1;

            for (int puntero = 0; puntero < codigoFuente.ToCharArray().Length; puntero++)
            {
                caracterActual = SiguienteCaracter(puntero);

                if (caracterActual.Equals('\n'))
                {
                    linea++;
                }

                lexema += caracterActual;

                columna = RegresarColumna(caracterActual);
                estado = matrizTransicion[estado, columna];

                if (estado >= 100 && estado < 500) //detectar estados finales
                {
                    if (lexema.Length > 1)
                    {
                        if (lexema == "==" || lexema == "<>" || lexema == "<="|| lexema == ">="||lexema == ":="||lexema.EndsWith('"'))
                        {

                        }
                        else
                        {
                            lexema = lexema.Remove(lexema.Length - 1);
                            puntero--;
                        }
                    }

                    Token nuevoToken = new Token() { ValorToken = estado, Lexema = lexema, Linea = linea };

                    if (estado == 100)
                        nuevoToken.ValorToken = esPalabraReservada(nuevoToken.Lexema);

                    nuevoToken.TipoToken = esTipo(nuevoToken.ValorToken);

                    listaToken.Add(nuevoToken); //agrego el token a la lista

                    /*inicializo valores*/
                    estado = 0;
                    columna = 0;
                    lexema = string.Empty;
                }
                else if (estado >= 500)
                {
                    listaError.Add(ManejoErrores(estado));

                    estado = 0;
                    columna = 0;
                    lexema = string.Empty;
                }
                else if (estado == 0)
                {
                    columna = 0;
                    lexema = string.Empty;
                }
            }
            return listaToken; // el resultado final del lexico
        } //METODO PRINCIPAL
    }
}
