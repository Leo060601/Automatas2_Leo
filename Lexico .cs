using System.IO;
using System;
using System.Globalization;


namespace Evalua
{ 

    
    public class Lexico:Token 
    {
        protected int linea;
        const int  E=-2;//negativo por si crece el automata 
        const int  F=-1;
        int[,] TranD =
        {

          // WS,EF,EL, L, D, ., E, +, -, =, :, ;, &, |, !, >, <, *, %, ", ', ?,La, /
            { 0, F, 0, 1, 2,33, 1,23,24, 8,10,12,13,14,15,19,20,27,27,29,30,32,33,34},//Estado 0
            { F, F, F, 1, 1, F, 1, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 1
            { F, F, F, F, 2, 3, 5, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 2
            { E, E, E, E, 4, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E},//Estado 3
            { F, F, F, F, 4, F, 5, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 4
            { E, E, E, E, 7, E, E, 6, 6, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E},//Estado 5
            { E, E, E, E, 7, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E},//Estado 6
            { F, F, F, F, 7, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 7
            { F, F, F, F, F, F, F, F, F, 9, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 8
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 9
            { F, F, F, F, F, F, F, F, F,11, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 10
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 11
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 12
            { F, F, F, F, F, F, F, F, F, F, F, F,16, F, F, F, F, F, F, F, F, F, F, F},//Estado 13
            { F, F, F, F, F, F, F, F, F, F, F, F, F,17, F, F, F, F, F, F, F, F, F, F},//Estado 14
            { F, F, F, F, F, F, F, F, F,18, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 15
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 16
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 17
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 18
            { F, F, F, F, F, F, F, F, F,21, F, F, F, F, F,39, F, F, F, F, F, F, F, F},//Estado 19
            { F, F, F, F, F, F, F, F, F,22, F, F, F, F, F,22,40, F, F, F, F, F, F, F},//Estado 20
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F ,F, F},//Estado 21
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 22
            { F, F, F, F, F, F, F,25, F,25, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 23
            { F, F, F, F, F, F, F, F,26,26, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 24
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 25
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 26
            { F, F, F, F, F, F, F, F, F,28, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 27
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 28
            {29, E,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,31,29,29,29,29},//Estado 29
            {30, E,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,31,30,30,30},//Estado 30
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 31
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 32
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 33
            { F, F, F, F, F, F, F, F, F,35, F, F, F, F, F, F, F,37, F, F, F, F, F,36},//Estado 34
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 35
            {36, 0, 0,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36},//Estado 36
            {37, E,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,38,37,37,37,37,37,37},//Estado 37
            {37, E,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,38,37,37,37,37,37, 0},//Estado 38
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//Estado 39
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F} //Estado 40
          // WS,EF,EL, L, D, ., E, +, -, =, :, ;, &, |, !, >, <, *, %, ", ', ?,La, /
        };
        private StreamReader archivo;
        protected StreamWriter log;
        public Lexico()
        {
            linea=1;
            archivo = new StreamReader("C:\\Archivos\\Prueba.cs");
            log = new StreamWriter("C:\\Archivos\\Prueba.Log");
            log.AutoFlush=true;
            log.WriteLine("Instituo Tecnologico de Queretaro");
            log.WriteLine("Leonardo Zu??iga Almaraz");
            log.WriteLine("\n-----------------------------------\n");
            log.WriteLine("Contenido de Prueba.cs: \n");
           

            //Requerimiento 1
            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("es-MX");
            log.WriteLine("Fecha y Hora: {0}, {1:G}",localDate.ToString(culture), localDate.Kind+"\n");
            log.WriteLine("-----------------------------------");
        }
        
        public int Columna(char Trancision)
        {
            if(Trancision == 10)
            {
                linea++;
                return 2;
            }
            else if(FinAchivo())
            {
                return 1;
            }
            else if(char.IsWhiteSpace(Trancision))
            {
                return 0;
            }
            else if(Trancision == 'E' || Trancision == 'e')
            {
                return 6;
            }
            else if(char.IsLetter(Trancision))
            {
                return 3;
            }
            else if(char.IsDigit(Trancision))
            {
                return 4;
            }
            else if(Trancision == '.')
            {
                return 5;
            }
            else if(Trancision == '+')
            {
                return 7;
            }
            else if(Trancision == '-')
            {
                return 8;
            }
            else if(Trancision == '=')
            {
                return 9;
            }
            else if(Trancision == ':')
            {
                return 10;
            }
            else if(Trancision == ';')
            {
                return 11;
            }
            else if(Trancision == '&')
            {
                return 12;
            }
            else if(Trancision == '|')
            {
                return 13;
            }
            else if(Trancision == '!')
            {
                return 14;
            }
            else if(Trancision == '>')
            {
                return 15;  
            }
            else if(Trancision == '<')
            {
                return 16; 
            }
            else if(Trancision == '*')
            {
                return 17;
            }
            else if(Trancision == '%')
            {
                return 18;
            }
            else if(Trancision == '"')
            {
                return 19;
            }
            else if(Trancision == '\'')
            {
                return 20;
            }
            else if(Trancision == '?')
            {
                return 21;
            }
            else if(Trancision == '/')
            {
                return 23;
            }
            else
            {
                return 22;
            }
        }

        private void Clasifica(int Estado)
        {
            switch(Estado)
            {
                case 1: setClasificacion(Tipos.identificador);
                break;
                case 2: setClasificacion(Tipos.numero);
                break;
                case 8: setClasificacion(Tipos.asignacion);
                break;
                case 9: 
                case 18: 
                case 19:
                case 20: setClasificacion(Tipos.opRelacional);
                break;
                case 10: 
                case 13: 
                case 14: 
                case 33: setClasificacion(Tipos.caracter);
                break;
                case 11: setClasificacion(Tipos.inicializacion);
                break;
                case 12: setClasificacion(Tipos.finSentencia);
                break;
                case 15: 
                case 16: 
                case 17: setClasificacion(Tipos.opLogico);
                break;
                case 23:
                case 24: setClasificacion(Tipos.opTermino);
                break;
                case 25:
                case 26: setClasificacion(Tipos.incTermino);
                break;
                case 27: 
                case 34: setClasificacion(Tipos.opFactor);
                break;
                case 28: 
                case 35: setClasificacion(Tipos.incFactor);
                break;
                case 31: setClasificacion(Tipos.Cadena);
                break;
                case 32: setClasificacion(Tipos.ternario);
                break;
                //Requerimiento 3
                case 39: setClasificacion(Tipos.opFlujoEntrada); //opFlujoEntrada
                break;
                //Requerimiento 4
                case 40: setClasificacion(Tipos.opFlujoSalida); //opFlujoSalida
                break;
            }

        }
        public void NextToken()
        {
            char c;
            string Buffer="";
            int Estado=0;
            while(Estado>=0)
            {
                c=(char)archivo.Peek();
                Estado=TranD[Estado,Columna(c)]; 
                if(Estado>=0)
                {
                    archivo.Read();
                    if(c=='\n')// para contar lineas 
                    { 
                        linea++;
                    }
                    if(Estado>0)
                    { 
                        Clasifica(Estado);
                        Buffer+=c;
                    }
                    else
                    { 
                        Buffer=""; //cadena vacia
                    }
                }
            }
            if(Estado == E)
            {
                if(Buffer [0] == '"' || Buffer [0] == '\'')
                {
                    //levantar excepcion correspondiente
                    
                    throw new Error("ERROR LEXICO: No se cerro la cadena", linea, log);
                }
                else if(char.IsDigit(Buffer [0]))
                {
                    throw new Error("ERROR LEXICO: Se espera un digito",linea, log);
                }
                
            }

            
            
            setContenido(Buffer);
            if(getClasificacion()==Tipos.identificador)
            {
                switch(getContenido())
                { 
                    case "char":
                    case "int": 
                    case "float":
                    setClasificacion(Tipos.tipoDato);
                    break;
                    //zona 
                    case "private":
                    case "public": 
                    case "protected":
                    setClasificacion(Tipos.zona);
                    break;
                    //condicion
                    case "if":
                    case "else": 
                    case "switch":
                    setClasificacion(Tipos.condicion);
                    break;
                    //ciclos
                    case "while":
                    case "for": 
                    case "do":
                    setClasificacion(Tipos.ciclo);
                    break;
                }
            }
            if(!FinAchivo())
            {
                log.WriteLine(getContenido()+" = "+getClasificacion());
            }
        }
        public bool FinAchivo()
        {
            return archivo.EndOfStream;
        }

    }
}