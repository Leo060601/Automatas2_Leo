using System;
using System.Collections.Generic;
namespace Evalua
{
    /* 
    Requerimiento 1: Ajustar la salida del printf: Quitar dobles comillas, 
                     ejecutar las setencias de escape(\n, \t) <<check>>
                     y asociar la lista de variables a la salidas % 
    Requerimiento 2: Cuando una variable no este declarada levantar excepcion
    Requerimiento 3: Asignar los valores del scanf a las variables correspondientes
    Requerimiento 4: Agregar el do-while 
    Requerimiento 5: Controlar la ejecucion del if anidado
    */
    public class Lenguaje:Sintaxis
    {
        List<Variable> LV; //declaracion
        Stack<float> SE;
        public Lenguaje()
        {
            LV=new List<Variable>(); //instancia el objeto lista
            SE=new Stack<float>(); //instancia el objeto stack
        }
        // Programa	-> 	Librerias Variables Main
        public void Programa()
        {
            Librerias();
            Variables();
            Main();
            ImprimeLista();
        }
        // Librerias->	#include<identificador(.h)?> Librerias?
        private void Librerias()
        {
            Match("#");
            Match("include");
            Match("<");
            Match(Tipos.identificador);
            if(getContenido()==".")
            {
                Match(".");
                Match("h");
            }
            Match(">");
            if(getContenido()=="#")
            {
                Librerias();
            }
        }
        private Variable.TDatos StrignToEnum(string tipo)
        {
            switch(tipo)
            {
                case "char": return Variable.TDatos.CHAR;
                case "int": return Variable.TDatos.INT;
                case "float": return Variable.TDatos.FLOAT;
                default: return Variable.TDatos.sinTipo;
            }
        }
        //Variables ->  tipoDato ListaIdentificadores; Variables?
        private void Variables()
        {
            Variable.TDatos tipo=Variable.TDatos.CHAR; //inicializamos=que cero
            tipo=StrignToEnum(getContenido());
            Match(Tipos.tipoDato);
            ListaIdentificadores(tipo);
            Match(Tipos.finSentencia);
            if(getClasificacion()==Tipos.tipoDato) //recursividad de Variables
            {
                Variables();
            }
        }
        private void ImprimeLista()
        {
            log.WriteLine("Lista de Variables");
            foreach (Variable L in LV)
            {
                log.WriteLine(L.getNombre() + " "+ L.getTipoDato() + " "+ L.getValor());
            }
        }
        private bool Existe(string nombre)
        {
            foreach (Variable L in LV)
            {
                if(L.getNombre()== nombre)
                {
                    return true;
                }    
            }
            return false; //no existe la variable
        }
        private void Modifica(string nombre, float valor)
        {
            foreach (Variable L in LV)
            {
                if(L.getNombre()== nombre)
                {
                    L.setValor(valor); 
                }    
            }
        }

        private float GetValor(string nombre)
        {
            foreach (Variable L in LV)
            {
                if(L.getNombre()== nombre)
                {
                    return L.getValor(); 
                }    
            }
            return 0;
        }

        //ListaIdentificadores ->  identificador (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TDatos tipo)
        {
            if(tipo != Variable.TDatos.sinTipo)
            {
                if(!Existe(getContenido())) 
                {
                    LV.Add(new Variable(getContenido(), tipo));   
                }
                else 
                {
                    throw new Error("ERROR DE SINTAXIS: Variable duplicada: " + getContenido(),linea,log);
                }    
            }
            Match(Tipos.identificador);
            if(getContenido()==",")//recursividad de lista
            {
                Match(",");
                ListaIdentificadores(tipo);
            }
        }
        // Main  ->	void main() BloqueInstrucciones
        private void Main()
        {
            Match("void");
            Match("main");
            Match("(");
            Match(")");
            BloqueInstrucciones(true);
        }
        // BloqueInstrucciones ->  {ListaInstrucciones}
        private void BloqueInstrucciones(bool ejecuta)
        {
            Match("{");
            ListaInstrucciones(ejecuta);
            Match("}");
        }
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool ejecuta)
        {
            Instruccion(ejecuta);
            if(getContenido() != "}") 
            {
                ListaInstrucciones(ejecuta);
            }
        }
        // Instrccion  -> Printf  | Scanf | If | For | While | Switch | Asignacion 
        private void Instruccion(bool ejecuta)
        {
            if(getContenido() == "printf")
            {
                Printf(ejecuta);
            }
            else if(getContenido() == "scanf")
            {
                Scanf(ejecuta);
            }
            else if(getContenido() == "if")
            {
                If(ejecuta);
            }
            else if(getContenido() == "for")
            {
                For(ejecuta);
            }
            else if(getContenido() == "while")
            {
                While(ejecuta);
            }
            else if(getContenido() == "do")
            {
                DoWhile(ejecuta);
            }
            else if(getContenido() == "switch")
            {
                Switch(ejecuta);
            }
            else
            {
                Asignacion(ejecuta);
            }
        }
        // Printf -> printf(cadena (,ListaIdentificadores)?);
        private void Printf(bool ejecuta)
        {
            Match("printf");
            Match("(");
            string string1 = getContenido();
            string NuevoString = string1.Replace("\"","").Replace("\\n", "\n").Replace("\\t","\t");
            Match(Tipos.Cadena);
            if(getContenido() == ",")
            {
                Match(",");
                ListaIdentificadores(Variable.TDatos.sinTipo);
            }
            Match(")");
            Match(Tipos.finSentencia);
                if(ejecuta)
            {
                Console.Write(NuevoString);
            }
        } 
        // Scanf -> scanf(cadena,ListadeAmpersas);
        private void Scanf(bool ejecuta)
        {
            Match("scanf");
            Match("(");
            Match(Tipos.Cadena);
            Match(",");
            ListadeAmpersas(ejecuta);
            Match(")");
            Match(Tipos.finSentencia);
        } 
        // ListadeAmpersas -> & identificador(,ListadeAmpersas)?
        private void ListadeAmpersas(bool ejecuta)
        {
            Match("&");
            if(!Existe(getContenido())) //buscarlos en Match(Tipos.identificador) y agregar excepcion
            {
                throw new Error("ERROR DE SINTAXIS: Variable no declarada: " + getContenido(),linea,log);
            }
            if(ejecuta)
            {
                float valor=float.Parse(Console.ReadLine());//lo asigna 
                Modifica(getContenido(),valor);
            }
            Match(Tipos.identificador);
            if(getContenido()==",")
            {
                Match(",");
                ListadeAmpersas(ejecuta);
            }
        }
        // If -> if(Condicion) BloqueInstrucciones | Intruccion (else BloqueInstrcciones | Instruccion)?
        private void If(bool ejecuta)
        {
            Match("if");
            Match("(");
            bool evalua = Condicion();
            Console.WriteLine(evalua);
            Match(")");
            if(getContenido()!="{")
            {
                Instruccion(evalua);
            }
            else
            {
                BloqueInstrucciones(!evalua);
            } 
            if(getContenido()=="else")
            {
                Match("else");
                if(getContenido()!="{")
                {
                    Instruccion(!evalua);
                }
                else
                {
                    BloqueInstrucciones(!evalua);
                }
            }
        }
        // ***Condicion -> Expresion oprRelacional Expresion  
        private bool Condicion()
        {
            //considerando la negacion 
            if(getContenido()=="!")
            {
                Match("!");
            }
            Expresion();
            string operador = getContenido();
            Match(Tipos.opRelacional);
            Expresion();
            float Resultado2 = SE.Pop();
            float Resultado1 = SE.Pop();
            switch(operador)
            {
                case "==": return Resultado1==Resultado2;
                case ">=": return Resultado1>=Resultado2;
                case ">":  return Resultado1>Resultado2;
                case "<":  return Resultado1<=Resultado2;
                case "<=": return Resultado1<Resultado2;
                default:   return Resultado1!=Resultado2;
            }
        }

        //Negacion -> ! Condicion 
        private void Negacion()
        {
            Match("!");
            Condicion();   
        }
        
        //***FOR -> for(identificador=Expresion ; condicion ; indetificador incTermino) Instruccion | BloqueInstrucciones
        private void For(bool ejecuta)
        {
            Match("for");
            Match("(");
            if(getContenido()=="int" || getContenido()=="float") //EXTRA
            {
                Match(Tipos.tipoDato);
            }
            string variable= getContenido();
            Match(Tipos.identificador);
            Match("=");
            Expresion();
            float Resultado = SE.Pop();
            Modifica(variable,Resultado);
            Match(Tipos.finSentencia);
            bool evalua = Condicion();
            Match(Tipos.finSentencia);
            string variable2= getContenido();
            Match(Tipos.identificador);
            string operador = getContenido();
            Match(Tipos.incTermino);
            if(operador=="++")
            {
                Modifica(variable2,GetValor(variable2)+1);
            }
            else if(operador=="--")
            {
                Modifica(variable2,GetValor(variable2)-1);
            }
            Modifica(variable,Resultado);
            Match(")");
            if(getContenido()=="{")
            {
                BloqueInstrucciones(evalua);
            }else
            {
                Instruccion(evalua);
            }
        }

        //Do -> do BloqueInstrucciones while(condicion)
        private void DoWhile(bool ejecuta)
        {
            bool evalua = true;
            Match("do");
            BloqueInstrucciones(evalua);
            Match("while");
            Match("(");
            evalua = Condicion();
            Console.WriteLine(evalua);
            Match(")");
        }

        // While -> while(Condicion) BloqueInstrucciones | Instruccion
        private void While(bool ejecuta)
        {
            Match("while");
            Match("(");
            bool evalua = Condicion();
            Match(")");
            if(getContenido()!="{") // bloque de instrucciones y instrucciones
            {
                Instruccion(evalua);
            }
            else
            {
                BloqueInstrucciones(evalua);
            }
        }

        //Switch -> switch ( expresion ) { Case (Case)? (Default)? }
        private void Switch(bool ejecuta)
        {
            Match("switch");
            Match("(");
            Expresion();
            float Resultado = SE.Pop();
            Match(")");
            Match("{");
            Case();
            if(getContenido()=="case")
            {
                Case();
            }
            if(getContenido()=="default")
            {
                Default();
            }
            Match("}");
        }

        //Case  -> case numero : (Case)? BloqueInstrucciones | Instruccion (break;)?
        private void Case()
        {
            Match("case");
            Match(Tipos.numero);
            Match(":");
            if(getContenido()=="case")
            {
                Case();
            }
            else
            {
                if(getContenido()!="{")
                {
                    Instruccion(true);
                }
                else
                {
                    BloqueInstrucciones(true);
                }
                if(getContenido()=="break")
                {
                Match("break");
                Match(Tipos.finSentencia);
                }
            }
        }

        //Default -> default : BloqueInstrucciones | Instruccion (break;)?
        private void Default()
        {
            Match("default");
            Match(":");
            if(getContenido()=="{")
            {
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }
            if(getContenido()=="break")
            {
                Match("break");
                Match(Tipos.finSentencia);
            }
        }

        //Asigancion -> identificador = Expresion;
        private void Asignacion(bool ejecuta)
        {
            string variable = getContenido();
            Match(Tipos.identificador);
            Match(Tipos.asignacion);
            Expresion();
            float Resultado = SE.Pop();
            Modifica(variable,Resultado);
            Match(Tipos.finSentencia);
        }

        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }

        //MasTermino -> (opTermino termino)?
        private void MasTermino()
        {
            if(getClasificacion()==Tipos.opTermino)
            {
                string operador = getContenido();
                Match(Tipos.opTermino);
                Termino(); //segundo termino
                //Console.Write(operador + " ");
                float N1=SE.Pop();
                float N2=SE.Pop();
                switch(operador)
                {
                    case "+": SE.Push(N2+N1); break;
                    case "-": SE.Push(N2-N1); break;
                }
            }
        }

        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }

        //PorFactor -> (opFactor Factor)?
        private void PorFactor()
        {
            if(getClasificacion()==Tipos.opFactor)
            {
                string operador = getContenido();
                Match(Tipos.opFactor);
                Factor(); //segundo factor
                Console.Write(operador + " ");
                float N1=SE.Pop();
                float N2=SE.Pop();
                switch(operador)
                {
                    case "*": SE.Push(N2*N1); break;
                    case "/": SE.Push(N2/N1); break;
                }
            }
        }
        
        //Factor -> Numero | identificador | (Expresion)
        private void Factor()
        {
            if(getClasificacion()==Tipos.numero)
            {
                //Console.Write(getContenido()+ " ");
                SE.Push(float.Parse(getContenido())); //metemos al stack los numeros
                Match(Tipos.numero);
            }
            else if(getClasificacion()==Tipos.identificador)
            {
                //Console.Write(getContenido()+ " ");
                SE.Push(GetValor(getContenido())); //metemos al stack los numeros
                Match(Tipos.identificador);
            }
            else
            {
                Match("(");
                Expresion();
                Match(")");
            }
        }

        
    }
}