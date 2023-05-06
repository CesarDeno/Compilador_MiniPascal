using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    class Sintactico
    {
        List<Token> listaTokens;
        List<Error> listaErrores;

        List<Variable> listaVariables = new List<Variable>();
        List<int> listaPostfijo = new List<int>();
        List<Polish> listaPolish = new List<Polish>();

        Stack pilaVar = new Stack();
        Stack pilaPostfijo = new Stack();
        Stack pilaEvaluarPostfijo = new Stack();
        Stack pilaPolish = new Stack();

        int p = 0;

        int contadorIfA = 1;
        int contadorIfB = 1;
        int contadorWhileC = 1;
        int contadorWhileD = 1;

        String etiqueta = String.Empty;
        int contadorEtiquetaA = 1;
        int contadorEtiquetaB = 1;
        int contadorEtiquetaC = 1;
        int contadorEtiquetaD = 1;
        

        private int[,] matrizAdicion =
        {   
       //     +    || INT || REAL || STRING || BOOL }
       /* INT    */{  203 ,   204 ,    527  ,  527  },
       /* REAL   */{  204 ,   204 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    202  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  527  }
       };

        private int[,] matrizSubstraccion =
        {   
       //     -    || INT || REAL || STRING || BOOL }
       /* INT    */{  203 ,   204 ,    527  ,  527  },
       /* REAL   */{  204 ,   204 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  527  }
       };

        private int[,] matrizMultiplicacion =
        {   
       //     *    || INT || REAL || STRING || BOOL }
       /* INT    */{  203 ,   204 ,    527  ,  527  },
       /* REAL   */{  204 ,   204 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  527  }
       };

        private int[,] matrizDivision =
        {   
       //     /    || INT || REAL || STRING || BOOL }
       /* INT    */{  204 ,   204 ,    527  ,  527  },
       /* REAL   */{  204 ,   204 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  527  }
       };

        private int[,] matrizAsignacion =
        {   
       //     :=   || INT || REAL || STRING || BOOL }
       /* INT    */{  203 ,   204 ,    527  ,  527  },
       /* REAL   */{  204 ,   204 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    202  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  205  }
       };

        private int[,] matrizMayor =
        {   
       //     >   || INT || REAL || STRING || BOOL }
       /* INT    */{  205 ,   205 ,    527  ,  527  },
       /* REAL   */{  205 ,   205 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  527  }
       };

        private int[,] matrizMenor =
        {   
       //     <   || INT || REAL || STRING || BOOL }
       /* INT    */{  205 ,   205 ,    527  ,  527  },
       /* REAL   */{  205 ,   205 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  527  }
       };

        private int[,] matrizMayorIgual =
        {   
       //     >   || INT || REAL || STRING || BOOL }
       /* INT    */{  205 ,   205 ,    527  ,  527  },
       /* REAL   */{  205 ,   205 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  527  }
       };

        private int[,] matrizMenorIgual =
        {   
       //     <=  || INT || REAL || STRING || BOOL }
       /* INT    */{  205 ,   205 ,    527  ,  527  },
       /* REAL   */{  205 ,   205 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  527  }
       };

        private int[,] matrizComparacion =
        {   
       //     ==  || INT || REAL || STRING || BOOL }
       /* INT    */{  205 ,   205 ,    527  ,  527  },
       /* REAL   */{  205 ,   205 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    205  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  205  }
       };

        private int[,] matrizDiferencia =
        {   
       //     <>  || INT || REAL || STRING || BOOL }
       /* INT    */{  205 ,   205 ,    527  ,  527  },
       /* REAL   */{  205 ,   205 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    205  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  205  }
       };

        private int[,] matrizAnd =
        {   
       //    AND  || INT || REAL || STRING || BOOL }
       /* INT    */{  527 ,   527 ,    527  ,  527  },
       /* REAL   */{  527 ,   527 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  205  }
       };

        private int[,] matrizOr =
        {   
       //    AND  || INT || REAL || STRING || BOOL }
       /* INT    */{  527 ,   527 ,    527  ,  527  },
       /* REAL   */{  527 ,   527 ,    527  ,  527  },
       /* STRING */{  527 ,   527 ,    527  ,  527  },
       /* BOOL   */{  527 ,   527 ,    527  ,  205  }
       };

        private int[] matrizNOT = new int[]
       //    NOT  || INT || REAL || STRING || BOOL }
                   {  527 ,   527 ,    527  ,  205 };

        private int[] matrizSignos = new int[] 
       //    +/-   || INT || REAL || STRING || BOOL }
                   {  203 ,   204 ,    527  ,  527 };

        public Sintactico(List<Token> listaTokensInterfaz, List<Error> listaErroresInterfaz)
        {
            listaTokens = listaTokensInterfaz;
            listaErrores = listaErroresInterfaz;
        }

        public void ejecutarSintactico()
        {

            if (listaTokens.ElementAt<Token>(p).ValorToken == 200)// program
            {
                p++;
                if (listaTokens.ElementAt<Token>(p).ValorToken == 100)// identificador
                {
                    p++;
                    if (listaTokens.ElementAt<Token>(p).ValorToken == 114)// ;
                    {
                        p++;
                        block();
                        if (listaTokens.ElementAt<Token>(p).ValorToken == 115)// .
                        {

                        }
                        else
                        {
                            listaErrores.Add(ManejoErrores(506, listaTokens.ElementAt<Token>(p).Linea));
                            throw new Exception("Error");
                        }
                    }
                    else
                    {
                        listaErrores.Add(ManejoErrores(505, listaTokens.ElementAt<Token>(p).Linea));
                        throw new Exception("Error");
                    }
                }
                else
                {
                    listaErrores.Add(ManejoErrores(504, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
            }
            else
            {
                listaErrores.Add(ManejoErrores(503, listaTokens.ElementAt<Token>(p).Linea));
                throw new Exception("Error");
            }
        }

        private void block()
        {
            varDeclaPart();
            statementPart();
        }

        private void varDeclaPart()
        {
            if (listaTokens.ElementAt<Token>(p).ValorToken == 201)// var
            {
                p++;
                variableDeclaration();
                if (listaTokens.ElementAt<Token>(p).ValorToken == 114)// ;
                {
                    while (listaTokens.ElementAt<Token>(p).ValorToken == 114)// ;
                    {
                        try
                        {
                            p++;
                        }
                        catch
                        {
                            listaErrores.Add(ManejoErrores(520, listaTokens.ElementAt<Token>(p).Linea));
                            throw new Exception("Error");
                        }

                        if (listaTokens.ElementAt<Token>(p).ValorToken == 100)// id
                        {
                            variableDeclaration();
                        }
                        else if (listaTokens.ElementAt<Token>(p).ValorToken != 206)
                        {
                            listaErrores.Add(ManejoErrores(524, listaTokens.ElementAt<Token>(p).Linea));
                            throw new Exception("Error");
                        }
                    }
                }
                else
                {
                    listaErrores.Add(ManejoErrores(505, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
            }
            else
            {
                listaErrores.Add(ManejoErrores(507, listaTokens.ElementAt<Token>(p).Linea));
                throw new Exception("Error");
            }
        }

        private void variableDeclaration()
        {
            if (listaTokens.ElementAt<Token>(p).ValorToken == 100)// id
            {
                ValidarVarRepetida(); //VERIFICAR SI YA SE DECLARO LA VAR ######## ERROR SEMANTICO 1
                pilaVar.Push(listaTokens.ElementAt<Token>(p).Lexema);

                p++;
                if (listaTokens.ElementAt<Token>(p).ValorToken == 117)// ,
                {
                    p++;
                    variableDeclaration();
                }
                else if (listaTokens.ElementAt<Token>(p).ValorToken == 116)// :
                {
                    p++;

                    InsertarVarALista();//AGREGAR VARIABLE A LISTA DE VAR #####

                    tipo();

                }
                else
                {
                    listaErrores.Add(ManejoErrores(519, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
            }
            else
            {
                listaErrores.Add(ManejoErrores(504, listaTokens.ElementAt<Token>(p).Linea));
                throw new Exception("Error");
            }
        }

        private void tipo()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 202:// string
                    p++;
                    break;
                case 203:// int
                    p++;
                    break;
                case 204:// real
                    p++;
                    break;
                case 205:// boolean
                    p++;
                    break;
                default:
                    listaErrores.Add(ManejoErrores(508, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                    break;
            }
        }

        private void statementPart()
        {

            if (listaTokens.ElementAt<Token>(p).ValorToken == 206)// begin
            {
                p++;
                statement();
                EvaluarListaPostfijo(listaPostfijo); //VERIFICAR COMPATIBILIDAD DE VARIABLES ######## ERROR SEMANTICO 3
                LimpiarPilaPolish();
                while (!(listaTokens.ElementAt<Token>(p).ValorToken == 207))// end
                {
                    statement();
                    EvaluarListaPostfijo(listaPostfijo); //VERIFICAR COMPATIBILIDAD DE VARIABLES ######## ERROR SEMANTICO 3
                    LimpiarPilaPolish();
                }
                if (listaTokens.ElementAt<Token>(p).ValorToken == 207)//end
                {
                    p++;
                    if (listaTokens.ElementAt<Token>(p).ValorToken != 115)// .
                    {
                        listaErrores.Add(ManejoErrores(506, listaTokens.ElementAt<Token>(p).Linea));
                        throw new Exception("Error");
                    }
                }
                else
                {
                    listaErrores.Add(ManejoErrores(521, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
            }
        }

        private void statement()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 100:
                case 208:
                case 209:// id, read, write
                    simpleStatement();
                    break;
                case 210:
                case 213:// if, while
                    structuredStatement();
                    break;
                default:
                    listaErrores.Add(ManejoErrores(510, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                    break;
            }
            if (listaTokens.ElementAt<Token>(p).ValorToken == 114) //;
            {
                p++;
            }
            else
            {
                listaErrores.Add(ManejoErrores(505, listaTokens.ElementAt<Token>(p).Linea));
                throw new Exception("Error");
            }
        }

        private void simpleStatement()
        {
            if (listaTokens.ElementAt<Token>(p).ValorToken == 100)
                assignmentStatement();
            if (listaTokens.ElementAt<Token>(p).ValorToken == 208)
                readStatement();
            if (listaTokens.ElementAt<Token>(p).ValorToken == 209)
                writeStatement();
        }

        private void assignmentStatement()
        {
            variable();
            if (listaTokens.ElementAt<Token>(p).ValorToken == 113)// :=
            {
                AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                p++;
                expression();
            }
            else
            {
                listaErrores.Add(ManejoErrores(522, listaTokens.ElementAt<Token>(p).Linea));
                throw new Exception("Error");
            }
        }

        private void variable()
        {
            entireVariable();
        }

        private void entireVariable()
        {
            variableIdentifier();
        }

        private void variableIdentifier()
        {

            if (listaTokens.ElementAt<Token>(p).ValorToken == 100)
            {
                AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                if (!ValidarVariableDeclarada())
                {
                    listaErrores.Add(ManejoErrores(526, listaTokens.ElementAt<Token>(p).Linea)); //######## ERROR SEMANTICO 2
                    throw new Exception("Error");
                }
                p++;
            }
            else
            {
                listaErrores.Add(ManejoErrores(504, listaTokens.ElementAt<Token>(p).Linea));
                throw new Exception("Error");
            }
        }

        private void readStatement()
        {
            if (listaTokens.ElementAt<Token>(p).ValorToken == 208)// read
            {
                AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                p++;
                if (listaTokens.ElementAt<Token>(p).ValorToken == 118)// (
                {
                    p++;
                    variable();
                    while (listaTokens.ElementAt<Token>(p).ValorToken == 117)// ,
                    {
                        p++;
                        variable();
                    }
                    if (listaTokens.ElementAt<Token>(p).ValorToken == 119)// )
                    {
                        p++;
                    }
                    else
                    {
                        listaErrores.Add(ManejoErrores(516, listaTokens.ElementAt<Token>(p).Linea));
                        throw new Exception("Error");
                    }
                }
                else
                {
                    listaErrores.Add(ManejoErrores(523, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
                LimpiarPilaPolish();
            }
        }

        private void writeStatement()
        {
            if (listaTokens.ElementAt<Token>(p).ValorToken == 209)// write
            {
                AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                p++;
                if (listaTokens.ElementAt<Token>(p).ValorToken == 118)// (
                {
                    p++;
                    if (listaTokens.ElementAt<Token>(p).ValorToken == 100)
                    {
                        variable();
                    }
                    else if (listaTokens.ElementAt<Token>(p).ValorToken == 120) //cadena
                    {
                        unsignedCostant();
                    }
                    while (listaTokens.ElementAt<Token>(p).ValorToken == 117)// ,
                    {
                        p++;
                        variable();
                    }
                    if (listaTokens.ElementAt<Token>(p).ValorToken == 119)// )
                    {
                        p++;
                    }
                    else
                    {
                        listaErrores.Add(ManejoErrores(516, listaTokens.ElementAt<Token>(p).Linea));
                        throw new Exception("Error");
                    }
                }
                else
                {
                    listaErrores.Add(ManejoErrores(523, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
                LimpiarPilaPolish();
            }
        }

        private void expression()
        {
            simpleExpression();
            if (listaTokens.ElementAt<Token>(p).ValorToken == 107 ||// >
               listaTokens.ElementAt<Token>(p).ValorToken == 108 ||// >=
               listaTokens.ElementAt<Token>(p).ValorToken == 109 ||// <
               listaTokens.ElementAt<Token>(p).ValorToken == 110 ||// <=
               listaTokens.ElementAt<Token>(p).ValorToken == 111 ||// ==
               listaTokens.ElementAt<Token>(p).ValorToken == 112)  // <>
            {
                relationalOperator();
                simpleExpression();
            }
            EvaluarListaPostfijo(listaPostfijo); //VERIFICAR COMPATIBILIDAD DE VARIABLES ######## ERROR SEMANTICO 3
            LimpiarPilaPolish();
        }

        private void relationalOperator()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 107:
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 108:
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 109:
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 110:
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 111:
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 112:
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
            }
        }

        private void simpleExpression()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 100:
                case 101:
                case 102:
                case 120:
                case 118:
                case 217: // id, entero, real, cadena, (, not
                    term();
                    if (listaTokens.ElementAt<Token>(p).ValorToken == 103 ||
                        listaTokens.ElementAt<Token>(p).ValorToken == 104 ||
                        listaTokens.ElementAt<Token>(p).ValorToken == 216)
                    {
                        addingOperator();
                        term();
                    }
                    break;
                case 103:
                case 104:// + -
                    sign();
                    term();
                    if (listaTokens.ElementAt<Token>(p).ValorToken == 103 ||
                        listaTokens.ElementAt<Token>(p).ValorToken == 104 ||
                        listaTokens.ElementAt<Token>(p).ValorToken == 216)
                    {
                        addingOperator();
                        term();
                    }
                    break;
            }

        }

        private void addingOperator()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 103:// +
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 104:// -
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 216:// or
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
            }
        }

        private void sign()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 103:// +
                    AgregarListaPostfijo(250);
                    AgregarAPolish(250, "~");
                    p++;
                    break;
                case 104:// -
                    AgregarListaPostfijo(251);
                    AgregarAPolish(251, "@");
                    p++;
                    break;
            }
        }

        private void term()
        {
            factor();
            if (listaTokens.ElementAt<Token>(p).ValorToken == 105 || // *
                listaTokens.ElementAt<Token>(p).ValorToken == 106 || // /
                listaTokens.ElementAt<Token>(p).ValorToken == 215)   // and
            {
                multiplyingOperator();
                term();
            }
        }

        private void multiplyingOperator()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 105:// *
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 106:// /
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 215:// and
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
            }
        }

        private void factor()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 100: // identificador
                    variable();
                    break;
                case 101:
                case 102:
                case 120: // entero, real y string
                    unsignedCostant();
                    break;
                case 118: // (
                    p++;
                    expression();
                    if (listaTokens.ElementAt<Token>(p).ValorToken == 119) // )
                        p++;
                    break;
                case 217: // not
                    AgregarListaPostfijo(listaTokens.ElementAt<Token>(p).ValorToken);
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    factor();
                    break;
                default:
                    listaErrores.Add(ManejoErrores(513, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                    break;
            }
        }

        private void unsignedCostant()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 101:
                case 102: // entero y real
                    unsignedNumber();
                    break;
                case 120: // string
                    AgregarListaPostfijo(esTipo(listaTokens.ElementAt<Token>(p).ValorToken));
                    AgregarAPolish(esTipo(listaTokens.ElementAt<Token>(p).ValorToken), listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                default:
                    listaErrores.Add(ManejoErrores(512, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                    break;
            }

        }

        private void unsignedNumber()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 101: // entero
                    AgregarListaPostfijo(esTipo(listaTokens.ElementAt<Token>(p).ValorToken));
                    AgregarAPolish(esTipo(listaTokens.ElementAt<Token>(p).ValorToken), listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                case 102: // real
                    AgregarListaPostfijo(esTipo(listaTokens.ElementAt<Token>(p).ValorToken));
                    AgregarAPolish(esTipo(listaTokens.ElementAt<Token>(p).ValorToken), listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    break;
                default:
                    listaErrores.Add(ManejoErrores(504, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                    break;
            }
        }

        private void structuredStatement()
        {
            switch (listaTokens.ElementAt<Token>(p).ValorToken)
            {
                case 210:
                    conditionalStatement();
                    break;
                case 213:
                    repetitiveStatement();
                    break;
            }
        }

        private void repetitiveStatement()
        {
            if (listaTokens.ElementAt<Token>(p).ValorToken == 213)// while
            {
                AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                p++;
                expression();
                if (listaTokens.ElementAt<Token>(p).ValorToken == 214)// do
                {
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    while (listaTokens.ElementAt<Token>(p).ValorToken != 219)// endwhile
                    {
                        statement();
                    }
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;

                }
                else
                {
                    listaErrores.Add(ManejoErrores(518, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
            }
        }

        private void conditionalStatement()
        {
            if (listaTokens.ElementAt<Token>(p).ValorToken == 210)// if
            {
                p++;
                expression();
                if (listaTokens.ElementAt<Token>(p).ValorToken == 211)// then
                {
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    while (listaTokens.ElementAt<Token>(p).ValorToken != 218) //endif 
                    {
                        statement();
                    }
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p += 2;

                }
                else
                {
                    listaErrores.Add(ManejoErrores(517, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
                if (listaTokens.ElementAt<Token>(p).ValorToken == 212)// else
                {
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                    while (listaTokens.ElementAt<Token>(p).ValorToken != 220) //endelse
                    {
                        statement();
                    }
                    AgregarAPolish(listaTokens.ElementAt<Token>(p).ValorToken, listaTokens.ElementAt<Token>(p).Lexema);
                    p++;
                }

            }
        }

        //----------------SEMANTICO----------------//

        private void InsertarVarALista()            //AGREGAR VARIABLE A LISTA DE VAR #####
        {
            Variable nuevaVariable;
            int cantidadPila = pilaVar.Count;
            for (int i = 0; i < cantidadPila; i++)
            {
                nuevaVariable = new Variable() { Lexema = (string)pilaVar.Pop() };                  //AGREGAR LEXEMA A LISTA DE VAR
                nuevaVariable.TipoVariable = listaTokens.ElementAt<Token>(p).ValorToken;            //AGREGAR TIPO A LISTA DE VAR
                listaVariables.Add(nuevaVariable);                                                  //AGREGAR VARIABLE A LISTA DE VAR #####
            }
        }

        private void ValidarVarRepetida()           //######## ERROR SEMANTICO 1
        {
            for (int i = 0; i < listaVariables.Count; i++)
            {
                if (listaVariables.ElementAt<Variable>(i).Lexema == listaTokens.ElementAt<Token>(p).Lexema)
                {
                    listaErrores.Add(ManejoErrores(525, listaTokens.ElementAt<Token>(p).Linea));
                    throw new Exception("Error");
                }
            }
        }

        private bool ValidarVariableDeclarada()     //######## ERROR SEMANTICO 2 
        {
            for (int i = 0; i < listaVariables.Count; i++)
            {
                if (listaVariables.ElementAt<Variable>(i).Lexema == listaTokens.ElementAt<Token>(p).Lexema)
                {
                    AgregarListaPostfijo(listaVariables.ElementAt<Variable>(i).TipoVariable);
                    return true;
                }
            }
            return false;
        }

        private int AsignarPrioridad(int op)
        {
            switch (op)
            {
                case 250:// signos
                case 251:
                    return 7;
                case 105:// * /
                case 106:
                    return 6;
                case 103:// + - 
                case 104:
                    return 5;
                case 107:// < <= > >= == <> :=
                case 108:
                case 109:
                case 110:
                case 111:
                case 112:
                case 113:
                    return 4;
                case 217:// NOT
                    return 3;
                case 215:// AND
                    return 2;
                case 216:// OR
                    return 1;
                case 208: //READ & WRITE
                case 209:
                    return 0;
                default:
                    return 0;
            }
        }

        private void AgregarListaPostfijo(int token) {


            if (token == 202 ||
                token == 203 ||
                token == 204 ||
                token == 205)
            {
                listaPostfijo.Add(token);
            }
            else if (token == 103 ||
                     token == 104 ||
                     token == 105 ||
                     token == 106 ||
                     token == 107 ||
                     token == 108 ||
                     token == 109 ||
                     token == 110 ||
                     token == 111 ||
                     token == 112 ||
                     token == 113 ||
                     token == 215 ||
                     token == 216 ||
                     token == 217 ||
                     token == 250 ||
                     token == 251)
            {
                if (pilaPostfijo.Count == 0)
                {
                    pilaPostfijo.Push(token);
                }
                else
                {
                    int prioridadOp1 = AsignarPrioridad((int)pilaPostfijo.Peek());
                    int prioridadOp2 = AsignarPrioridad(token);

                    if (prioridadOp2 <= prioridadOp1)
                    {
                        listaPostfijo.Add((int)pilaPostfijo.Pop());
                        pilaPostfijo.Push(token);
                    }
                    else
                    {
                        pilaPostfijo.Push(token);
                    }
                }
            }
        }

        private void EvaluarListaPostfijo(List<int> listaPostfijo)  //######## ERROR SEMANTICO 3
        {
            while (pilaPostfijo.Count != 0)
            {
                listaPostfijo.Add((int)pilaPostfijo.Pop());
            }

            pilaEvaluarPostfijo.Clear();
            for (int i = 0; i < listaPostfijo.Count; i++)
            {
                if (listaPostfijo.ElementAt<int>(i) == 202 ||
                    listaPostfijo.ElementAt<int>(i) == 203 ||
                    listaPostfijo.ElementAt<int>(i) == 204 ||
                    listaPostfijo.ElementAt<int>(i) == 205)
                {
                    pilaEvaluarPostfijo.Push(listaPostfijo.ElementAt<int>(i));
                }
                else if (listaPostfijo.ElementAt<int>(i) == 103 ||
                         listaPostfijo.ElementAt<int>(i) == 104 ||
                         listaPostfijo.ElementAt<int>(i) == 105 ||
                         listaPostfijo.ElementAt<int>(i) == 106 ||
                         listaPostfijo.ElementAt<int>(i) == 107 ||
                         listaPostfijo.ElementAt<int>(i) == 108 ||
                         listaPostfijo.ElementAt<int>(i) == 109 ||
                         listaPostfijo.ElementAt<int>(i) == 110 ||
                         listaPostfijo.ElementAt<int>(i) == 111 ||
                         listaPostfijo.ElementAt<int>(i) == 112 ||
                         listaPostfijo.ElementAt<int>(i) == 113 ||
                         listaPostfijo.ElementAt<int>(i) == 215 ||
                         listaPostfijo.ElementAt<int>(i) == 216)
                {
                    pilaEvaluarPostfijo.Push(AsignarMatriz((int)pilaEvaluarPostfijo.Pop(), (int)pilaEvaluarPostfijo.Pop(), listaPostfijo.ElementAt(i)));
                }
                else if (listaPostfijo.ElementAt<int>(i) == 217) //Not
                {
                    pilaEvaluarPostfijo.Push(matrizNOT[PrepararParaMatriz((int)pilaEvaluarPostfijo.Pop())]);
                }
                else if (listaPostfijo.ElementAt<int>(i) == 250 || listaPostfijo.ElementAt<int>(i) == 251) // signo + -
                {
                    pilaEvaluarPostfijo.Push(matrizSignos[PrepararParaMatriz((int)pilaEvaluarPostfijo.Pop())]);
                }

                if ((int)pilaEvaluarPostfijo.Peek() == 527) // Errror
                {
                    listaErrores.Add(ManejoErrores(527, listaTokens.ElementAt<Token>(p).Linea));   //ERROR DE COMPATIBILIDAD DE TIPOS
                    throw new Exception("Error");
                }
            }
            listaPostfijo.Clear();
        }

        private int PrepararParaMatriz(int valor)
        {
            switch (valor)
            {
                case 203:
                    return 0;
                case 204:
                    return 1;
                case 202:
                    return 2;
                case 205:
                    return 3;
                default:
                    return 0;
            }
        }

        private int AsignarMatriz(int operando2, int operando1, int operador)
        {
            switch (operador)
            {
                case 103://*
                    return matrizAdicion[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 104://-
                    return matrizSubstraccion[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 105://*
                    return matrizMultiplicacion[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 106:// /
                    return matrizDivision[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 107://>
                    return matrizMayor[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 108://>=
                    return matrizMayorIgual[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 109://<
                    return matrizMenor[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 110://<=
                    return matrizMenorIgual[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 111://==
                    return matrizComparacion[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 112://<>
                    return matrizDiferencia[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 113://:=
                    return matrizAsignacion[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 215://AND
                    return matrizAnd[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                case 216://OR
                    return matrizOr[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)];
                default:
                    return matrizAdicion[PrepararParaMatriz(operando1), PrepararParaMatriz(operando2)]; ;

            }

        }

        private int esTipo(int estado)          //VERIFICAR TIPO DE VARIABLE
        {
            switch (estado)
            {
                case 120:
                    return 202;
                case 101:
                    return 203;
                case 102:
                    return 204;
                default:
                    return 202;
            }
        }

        //----------------CODIGO A POLISH----------------//

        private void AgregarAPolish(int token, String lexema)
        {
            Polish nuevo = new Polish() { Lexema = lexema, Etiqueta = etiqueta };

            if (token == 100 || //OPERANDOS
                token == 202 ||
                token == 203 ||
                token == 204 ||
                token == 120)
            {
                listaPolish.Add(nuevo);
                etiqueta = String.Empty;
            }
            else if (token == 103 || //OPERADORES
                     token == 104 ||
                     token == 105 ||
                     token == 106 ||
                     token == 107 ||
                     token == 108 ||
                     token == 109 ||
                     token == 110 ||
                     token == 111 ||
                     token == 112 ||
                     token == 113 ||
                     token == 208 ||
                     token == 209 ||
                     token == 215 ||
                     token == 216 ||
                     token == 217 ||
                     token == 250 ||
                     token == 251)
            {
                if (pilaPolish.Count == 0)
                {
                    pilaPolish.Push(lexema);
                }
                else
                {
                    int prioridadOp1 = AsignarPrioridadPolish((String)pilaPolish.Peek());
                    int prioridadOp2 = AsignarPrioridadPolish(lexema);

                    if (prioridadOp2 <= prioridadOp1)
                    {
                        nuevo = new Polish() { Lexema = (String)pilaPolish.Pop() };
                        listaPolish.Add(nuevo);
                        etiqueta = String.Empty;
                        pilaPolish.Push(lexema);
                    }
                    else
                    {
                        pilaPolish.Push(lexema);
                    }
                }
            }

            EstructuraIf(token);
            EstructuraWhile(token);
        }

        private void EstructuraIf(int token)
        {
            if (token == 211) //then
            {
                listaPolish.Add(new Polish() { Lexema = "BRF", Apuntador = String.Format("A{0}", contadorIfA) });//SALTO FALSO             
                contadorIfB = contadorIfA;
                contadorIfA++;
            }
            else if (token == 218)//endif
            {
                etiqueta = String.Format("A{0}", contadorEtiquetaA);//ETIQUETA A (SI ES SOLAMENTE IF)
            }
            else if (token == 212)//else
            {
                listaPolish.Add(new Polish() { Lexema = "BRI", Apuntador = String.Format("B{0}", contadorIfB) });//SALTO INCONDICIONAL
                contadorIfB--;
                etiqueta = String.Format("A{0}", contadorEtiquetaA);//ETIQUETA A
                contadorEtiquetaA--;
            }
            else if (token == 220)//endelse
            {
                etiqueta = String.Format("B{0}", contadorEtiquetaB);//ETIQUETA B
                contadorEtiquetaA = contadorEtiquetaB;
                contadorEtiquetaB++;
            }
        }

        private void EstructuraWhile(int token)
        {
            if (token == 213)//while  
            {
                etiqueta = String.Format("D{0}", contadorEtiquetaD);//ETIQUETA D
                contadorEtiquetaC = contadorEtiquetaD;
                contadorEtiquetaD++;
            }
            else if (token == 214)//do
            {
                listaPolish.Add(new Polish() { Lexema = "BRF", Apuntador = String.Format("C{0}", contadorWhileC) }); //SALTO FALSO
                contadorWhileD = contadorWhileC;
                contadorWhileC++;
            }
            else if (token == 219)//endwhile
            {
                listaPolish.Add(new Polish() { Lexema = "BRI", Apuntador = String.Format("D{0}", contadorWhileD) }); //SALTO INCONDICIONAL
                contadorWhileD--;
                etiqueta = String.Format("C{0}", contadorEtiquetaC);//ETIQUETA C
                contadorEtiquetaC--;
            }
        }

        private int AsignarPrioridadPolish(String lexema)
        {
            switch (lexema)
            {
                case "~":// signos
                case "@":
                    return 7;
                case "*":// * /
                case "/":
                    return 6;
                case "+":// + - 
                case "-":
                    return 5;
                case "<":// < <= > >= == <> :=
                case "<=":
                case ">":
                case ">=":
                case "==":
                case "<>":
                case ":=":
                    return 4;
                case "NOT":// NOT
                    return 3;
                case "AND":// AND
                    return 2;
                case "OR":// OR
                    return 1;
                case "read": //READ & WRITE
                case "write":
                    return 0;
                default:
                    return 0;
            }
        }

        private void LimpiarPilaPolish()
        {
            while (pilaPolish.Count != 0)
            {
                String lexemaPop = (String)pilaPolish.Pop();
                Polish nuevo = new Polish() { Lexema = lexemaPop };
                listaPolish.Add(nuevo);

            }
        }

        public List<Polish> getListaPolish
        {
            get
            {
                return listaPolish;
            }
        }

        //----------------MANEJO DE ERRORES----------------//

        public List<Error> getListaError
        {
            get
            {
                return listaErrores;
            }
        }

        private Error ManejoErrores(int estado, int linea)
        {
            string mensajeError;
            tipoError tipo =  tipoError.Sintactico;

            switch (estado)
            {
                case 503:
                    mensajeError = "Se esperaba program";
                    break;
                case 504:
                    mensajeError = "Se esperaba identificador";
                    break;
                case 505:
                    mensajeError = "Se esperaba ;";
                    break;
                case 506:
                    mensajeError = "Se esperaba .";
                    break;
                case 507:
                    mensajeError = "Se esperaba var";
                    break;
                case 508:
                    mensajeError = "Se esperaba un tipo de variable valido";
                    break;
                case 509:
                    mensajeError = "Se esperaba que se terminaran de declarar las variables de manera correcta";
                    break;
                case 510:
                    mensajeError = "Se esperaba un statement";
                    break;
                case 511:
                    mensajeError = "Se esperaba un entero o real";
                    break;
                case 512:
                    mensajeError = "Se esperaba el valor para asignarle a la variable";
                    break;
                case 513:
                    mensajeError = "Se esperaba un factor valido";
                    break;
                case 514:
                    mensajeError = "Se esperaba un operador de multiplicacion";
                    break;
                case 515:
                    mensajeError = "Se esperaba un operador de relacional";
                    break;
                case 516:
                    mensajeError = "Se esperaba )";
                    break;
                case 517:
                    mensajeError = "Se esperaba then";
                    break;
                case 518:
                    mensajeError = "Se esperaba do";
                    break;
                case 519:
                    mensajeError = "Se esperaba mas identificadores o que se definiera el tipo de variable";
                    break;
                case 520:
                    mensajeError = "Se esperaba begin";
                    break;
                case 521:
                    mensajeError = "Se esperaba end";
                    break;
                case 522:
                    mensajeError = "Se esperaba :=";
                    break;
                case 523:
                    mensajeError = "Se esperaba (";
                    break;
                case 524:
                    mensajeError = "Se esperaba Begin u otra variable";
                    break;
                case 525:
                    mensajeError = "Variable ya declarada";
                    tipo = tipoError.Semantico;
                    break;
                case 526:
                    mensajeError = "Variable no declarada";
                    tipo = tipoError.Semantico;
                    break;
                case 527:
                    mensajeError = "Incompatibilidad de Tipos";
                    tipo = tipoError.Semantico;
                    break;
                default:
                    mensajeError = "Error inesperado";
                    tipo = tipoError.Semantico;
                    break;
            }
            return new Error() { Codigo = estado, MensajeError = mensajeError, Tipo = tipo, Linea = linea }; 
        }
    }
}
