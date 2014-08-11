using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi1
{
    public class AlfaBeta
    {

        private Celda jugada = null; //celda de la jugada final que hará el robot.
        private char ficha; //ficha del robot.
        private int profundidad; //profundidad del algoritmo.

        /**
         * Permite establecer los parámetros necesarios para el algoritmo.
         * Además, se inicializará el algoritmo llamando a la función correspondiente.
         * @param tablero del que se parte.
         * @param ficha del robot.
         * @param profundidad del algoritmo.
         */
        public AlfaBeta(Tablero tablero, char ficha, int profundidad)
        {
            this.ficha = ficha;
            this.profundidad = profundidad;
            inicioAlgoritmo(tablero);
        }


        /**
         * Esta función inicializa el algoritmo alfabeta.
         * @param tablero del que partimos.
         */ 
        public void inicioAlgoritmo(Tablero tablero)
        {
            int alfa=-999, beta=999;
            int tmp_alfa;
            Celda celda;
            //Buscamos por todo el tablero celdas
            for (int a = 0; a < tablero.getTamaño(); a++)
            {
                for (int b = 0; b < tablero.getTamaño(); b++)
                {
                    celda = new Celda(b, a, ficha);
                    //Encontramos una celda en la que podemos jugar
                    if (tablero.esLegal(celda) && tablero.comprobarSiHayCeldasAfectadas(celda))
                    {
                        //La primera vez, se guarda, ya que siempre iremos como mínimo por la primera rama del árbol de búsqueda.
                        if(jugada==null)
                            jugada = celda;
                        //Debug.WriteLine("llego con y: " + b + " y x: " + a);
                        //Debug.WriteLine("Celda: "+celda.toString());
                       // Debug.WriteLine("Tablero a evaluar:");
                       // tablero.printearTableroDebug();
                        tmp_alfa = recurAlgoritmo(nuevoTablero(tablero, celda), alfa, beta, 1, Jugador.fichaOponente(ficha));
                        
                       //Si la función de evaluación es mayor que el alfa que ya tenemos, se guarda esa jugada como favorita.
                        if (tmp_alfa > alfa)
                        {
                            jugada = celda;
                            alfa = tmp_alfa;
                        }
                    }
                }
            }
        }


        /**
         * Algoritmo recursivo alfa-beta que se encarga de recorrer el árbol de búsqueda y encontrar las mejores jugadas.
         * @param tablero del que se parte.
         * @param alfa valor.
         * @param beta valor.
         * @param profundidadaActual.
         * @param ficha con la que se juega.
         * @return valor alfa o beta heurístico.
         */ 
        public int recurAlgoritmo(Tablero tablero, int alfa, int beta, int profundidadActual, char ficha)
        {
            int tmp_alfa, tmp_beta,devolver=0;
            Celda celda;
            //Cuando la profundidad sea mayor de la que estamos, vamos por este camino
            if (profundidadActual < profundidad)
            {
                //Recorremos el tablero
                for (int a = 0; a < tablero.getTamaño(); a++)
                {
                    for (int b = 0; b < tablero.getTamaño(); b++)
                    {
                        celda = new Celda(a, b, ficha);
                        if (tablero.esLegal(celda) && tablero.comprobarSiHayCeldasAfectadas(celda))
                        {
                            //Corte de nodos.
                            if (alfa < beta)
                            {
                                if (profundidadActual % 2 == 1) //Estoy en MIN
                                {
                                   // Debug.WriteLine("Tablero que voy a comprobar con profundidad "+profundidadActual);
                                   // nuevoTablero(tablero, celda).printearTableroDebug();
                                    tmp_beta = recurAlgoritmo(nuevoTablero(tablero, celda), alfa, beta, profundidadActual+1, Jugador.fichaOponente(ficha));
                                   // Debug.WriteLine("Estoy en MIN. Celda: "+celda.toString()+". tmp_beta=" + tmp_beta + " beta=" + beta);
                                    if (tmp_beta < beta)
                                    {
                                        beta = tmp_beta;
                                        devolver = beta;
                                    }
                                }
                                else //Estoy en MAX
                                {
                                    tmp_alfa = recurAlgoritmo(nuevoTablero(tablero, celda), alfa, beta, profundidadActual+1, Jugador.fichaOponente(ficha));
                                   // Debug.WriteLine("Estoy en MAX. Celda: " + celda.toString() + " tmp_alfa=" + tmp_alfa + " alfa=" + alfa);
                                    if (tmp_alfa > alfa)
                                    {
                                        alfa = tmp_alfa;
                                        devolver = alfa;
                                    }
                                }
                            }

                        }
                    }
                }

            }
            else
            {
                //Devolvemos el valor de la hoja.
                devolver = evaluarTablero(tablero, ficha);
               // Debug.WriteLine("Profundidad más baja. Devuelvo la heuristica del siguiente talbero: " + devolver);
               // tablero.printearTableroDebug();
            }

            return devolver;
        }

        /**
         * Creamos un nuevo tablero a partir de uno ya existente y realizando los cambios de una jugada.
         * @param tablero del que vamos a crear otro.
         * @param celda jugada.
         * @return tablero resultante.
         */
        public Tablero nuevoTablero(Tablero tablero, Celda celda)
        {
            //Creamos un nuevo tablero
            Tablero nuevoTablero = new Tablero(tablero);

            //Volteamos todas las celdas afectadas por la jugada
            Celda[] outputArray = tablero.getCeldasVolteadas().ToArray();
            foreach (Celda celdaOutput in outputArray)
            {
                //Actualizamos las celdas del tablero (estructura de datos)
                nuevoTablero.getCelda(celdaOutput.getY(), celdaOutput.getX()).setFicha(celdaOutput.getFicha());
            }
            return nuevoTablero;
        }

        /**
         * Función heurística que se encarga de evaluar el tablero a partir de una ficha.
         * Dependiendo de la ficha, un tablero puede ser bueno o malo.
         * @param tablero a evaluar.
         * @param ficha usada para evaluar el tablero.
         */ 
        public int evaluarTablero(Tablero tablero, char ficha)
        {
            int evaluacion = 0;
            int multiploFicha=0;

            //Si la profundidad es 1, usamos una función heurística sencillita
            if (profundidad == 1)
            {
                for (int a = 0; a < tablero.getTamaño(); a++)
                {
                    for (int b = 0; b < tablero.getTamaño(); b++)
                    {
                        if (tablero.getCelda(b, a).getFicha() == ficha)
                            evaluacion++;
                        else if (tablero.getCelda(b, a).getFicha() == Jugador.fichaOponente(ficha))
                            evaluacion--;
                    }
                }
            }
            else //Si no, usamos una función heurística más interesante
            {
                for (int a = 0; a < tablero.getTamaño(); a++)
                {
                    for (int b = 0; b < tablero.getTamaño(); b++)
                    {
                        if (tablero.getCelda(b, a).getFicha() == '-')
                            multiploFicha = 0;
                        else if (tablero.getCelda(b, a).getFicha() == ficha)
                            multiploFicha = 1;
                        else if (tablero.getCelda(b, a).getFicha() == ficha)
                            multiploFicha = -1;


                        if (a == 0 || b == 0 || a == (tablero.getTamaño() - 1) || b == (tablero.getTamaño() - 1))
                        {
                            //Esquinas
                            if ((a == 0 && b == 0) || (a == (tablero.getTamaño() - 1) && b == 0) ||
                                (a == 0 && b == (tablero.getTamaño() - 1)) ||
                                (a == (tablero.getTamaño() - 1) && b == (tablero.getTamaño() - 1)))
                                evaluacion += multiploFicha * 6;
                            else //Bordes
                                evaluacion += multiploFicha * 4;
                        } //Anteriores al borde
                        else if (a == 1 || b == 1 || a == (tablero.getTamaño() - 2) || b == (tablero.getTamaño() - 2))
                        {
                            evaluacion += multiploFicha * 2 * -1;
                        }
                        else //Resto
                        {
                            evaluacion += multiploFicha;
                        }
                    }
                }
            }
            
            //Debug.WriteLine("Evaluacion tablero: " + evaluacion);
            return evaluacion;
        }

        //Nos permite obtener la celda de la jugada
        public Celda obtenerJugada() { return this.jugada; }
    }

}
