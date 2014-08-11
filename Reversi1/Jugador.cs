using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi1
{
    public class Jugador
    {
        String jug1, jug2;
        char turno = 'X'; //turno por defecto

        /**
         * Establecemos el nombre de los jugadores.
         * @param jug1 nombre del jugador 1.
         * @param jug2 nombre del jugador 2.
         */
        public Jugador(String jug1, String jug2)
        {

            this.jug1 = jug1;
            this.jug2 = jug2;
        }


        /**
         * Esta función nos devuelve el nombre del jugador que más fichas tiene en el tablero, o tablas.
         * @param tablero en el que contaremos las fichas para determinar al ganador.
         * @return nombre del ganador.
         */ 
        public String consultarGanador(Tablero tablero)
        {
            int contador = 0;
            //Vamos contando las fichas del tablero
            for (int a = 0; a < tablero.getTamaño(); a++)
                for (int b = 0; b < tablero.getTamaño(); b++)
                    if (tablero.getCelda(a, b).getFicha() == 'X')
                        contador++;
                    else if (tablero.getCelda(a, b).getFicha() == 'O')
                        contador--;

            //Devolvemos el resultado
            if (contador > 0)
                return jug1;
            else if (contador < 0)
                return jug2;
            return "tttablas";
        }

        /**
         * Esta función nos devuelve la ficha del oponente, en base a la ficha dada.
         * @param ficha contraria de la que buscamos.
         * @return ficha del oponente.
         */ 
        public static char fichaOponente(char ficha)
        {
            if (ficha == 'X')
                return 'O';
            return 'X';
        }

        /**
         * Esta función nos devuelve el nombre del jugador actual, del que le toca jugar.
         * @return nombre del jugador actual.
         */ 
        public String nombreJugadorActual()
        {
            if (consultarTurno() == 'X')
                return jug1;
            return jug2;
        }

        //Estas funciones nos devuelven diversa información
        public String consultarJugador1() { return this.jug1; }
        public String consultarJugador2() { return this.jug2; }
        public void cambiarTurno() { this.turno = fichaOponente(this.turno); }
        public char consultarTurno() { return turno; }
        public void cambiarJugador1(String nuevoNombre) { jug1 = nuevoNombre; }
        public void cambiarJugador2(String nuevoNombre) { jug2 = nuevoNombre; }
    }
}
