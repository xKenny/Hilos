using System;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;

namespace Hilos
{
    class Program
    {
        public static String texto;

        //Este metodo lee el archivo de texto y lo guarda en una variable global
        //Reemplaza la informacion que tenga el archivo resultados y lo prepara para nueva informacion
        public static void leerTexto()
        {
            TextReader leer = new StreamReader("../../../texto.txt");
            texto = leer.ReadToEnd();
            leer.Close();
            System.IO.File.WriteAllText("../../../Resultados.txt", "Resultados :");
        }

        //Este metodo cuenta las palabras terminadas en n
        //Lee la informacion que tenga el archivo Resultados.txt, ya que otro hilo lo puede estar usando
        //Escribe los resultados en el archivo Resultados.txt
        //Se sincronizan los hilos para que esperen en el caso
        //que dos hilos intenten acceder al archivo al mismo tiempo
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Tarea1()
        {
            int contadorPalabras = 0;
            
            string[] palabras = texto.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
            for(int i=0; i<palabras.Length; i++)   
            {
                String temporal = palabras[i];
                if (temporal[temporal.Length-1] == 'n') 
                {
                    contadorPalabras++; 
                }
            }
            TextReader leerResultados1 = new StreamReader("../../../Resultados.txt");
            String textoResultados1 = leerResultados1.ReadToEnd();
            leerResultados1.Close();
            System.IO.File.WriteAllText("../../../Resultados.txt", textoResultados1 + "\nNumero de palabras terminadas en 'n': " + contadorPalabras);
            Console.WriteLine("Numero de palabras terminadas en n: " + contadorPalabras);
        }

        //Este metodo cuenta la cantidad de frases con mas de 15 palabras
        //separa el texto en oraciones y las oraciones en palabras para poder hacer el conteo
        //Lee la informacion que tenga el archivo Resultados.txt, ya que otro hilo lo puede estar usando
        //Escribe los resultados en el archivo Resultados.txt
        //Se sincronizan los hilos para que esperen en el caso
        //que dos hilos intenten acceder al archivo al mismo tiempo
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Tarea2()
        {
            int contadorOraciones = 0;
            string[] oraciones = texto.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            for(int i=0; i<oraciones.Length; i++)
            {
                string[] palabrasOracion = oraciones[i].Split(new char[] { '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (palabrasOracion.Length > 15)
                {
                    contadorOraciones++;
                }
            }
            TextReader leerResultados2 = new StreamReader("../../../Resultados.txt");
            String textoResultados2 = leerResultados2.ReadToEnd();
            leerResultados2.Close();
            System.IO.File.WriteAllText("../../../Resultados.txt", textoResultados2 + "\nNumero de oraciones con mas de 15 palabras: " + contadorOraciones);
            Console.WriteLine("Numero de oraciones con mas de 15 palabras: " + contadorOraciones);
        }

        //Este metodo cuenta la cantidad de parrafos que hay en el texto
        //Aumenta el contador cuando encuentra un salto de linea (\n), valida que haya un punto en la posicion anterior para 
        //asegurar que se termina un parrafo
        //Lee la informacion que tenga el archivo Resultados.txt, ya que otro hilo lo puede estar usando
        //Escribe los resultados en el archivo Resultados.txt
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Tarea3()
        {
            int contadorParrafos = 0;
            char[] arrayCaracteres = texto.ToCharArray();
            for(int i=0; i < arrayCaracteres.Length; i++)
            {
                if(arrayCaracteres[i] == '\n')
                {
                    if(arrayCaracteres[i-1] == '.')
                    {
                        contadorParrafos++;
                    }
                }
            }
            contadorParrafos++; //Se suma 1 ya que el ultimo parrafo no tiene salto de linea
            TextReader leerResultados3 = new StreamReader("../../../Resultados.txt");
            String textoResultados3 = leerResultados3.ReadToEnd();
            leerResultados3.Close();
            System.IO.File.WriteAllText("../../../Resultados.txt", textoResultados3 + "\nCantidad de parrafos: " + contadorParrafos);
            Console.WriteLine("Cantidad de parrafos: " + contadorParrafos);
        }

        //Este metodo cuenta los caracteres alfanumericos diferentes a n o N
        //Divide el texto omitiendo las letras n, N, signos de puntuacion y espacios o saltos de linea
        //Recorre el array resultante y cuenta las letras de cada uno para sumarlas al contador
        //Lee la informacion que tenga el archivo Resultados.txt, ya que otro hilo lo puede estar usando
        //Escribe los resultados en el archivo Resultados.txt
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Tarea4()
        {
            int contadorCaracteresSinN = 0;
            string[] caracteresN = texto.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',','n','N','\n','\r' }, StringSplitOptions.RemoveEmptyEntries);
            for(int i=0; i<caracteresN.Length; i++)
            {
                contadorCaracteresSinN = contadorCaracteresSinN + caracteresN[i].Length;
            }
            TextReader leerResultados4 = new StreamReader("../../../Resultados.txt");
            String textoResultados4 = leerResultados4.ReadToEnd();
            leerResultados4.Close();
            System.IO.File.WriteAllText("../../../Resultados.txt", textoResultados4 + "\nNumero de caracteres alfanumericos distintos a 'n' o 'N' : " + contadorCaracteresSinN);
            Console.WriteLine("Numero de caracteres alfanumericos distintos a 'n' o 'N' : " + contadorCaracteresSinN);
        }

        //Metodo principal que crea e inicia los hilos asignandoles la tarea correspondiente
        //Llama el metodo leertexto para que los metodos puedan acceder a el
        static void Main(string[] args)
        {
            leerTexto();

            Thread tarea1 = new Thread(new ThreadStart(Tarea1));
            Thread tarea2 = new Thread(new ThreadStart(Tarea2));
            Thread tarea3 = new Thread(new ThreadStart(Tarea3));
            Thread tarea4 = new Thread(new ThreadStart(Tarea4));

            tarea1.Start();
            tarea2.Start();
            tarea3.Start();
            tarea4.Start();
        }
    }
}
