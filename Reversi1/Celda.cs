using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi1
{
    public class Celda
    {
        private int x;
        private int y;
        private char ficha;

        /**
         * Creamos una celda en base a las coordenadas y la ficha.
         * @param y coordenada.
         * @param x coordenada.
         * @param ficha de la celda.
         */ 
        public Celda(int y, int x, char ficha)
        {
            this.y = y;
            this.x = x;
            this.ficha = ficha;
        }

        /**
         * Creamos una celda en base a las coordenadas y la ficha.
         * Las coordenadas vienen dadas en un string, separado por coma.
         * @param celdaString cadena de texto de la que extraeremos las coordenadas de la celda. Es del tipo Y,X.
         * @param ficha de la celda.
         */ 
        public Celda(String celdaString, char ficha)
        {
            this.x = Convert.ToInt16(celdaString.Substring(celdaString.IndexOf(',') + 1));
            this.y = Convert.ToInt16(celdaString.Substring(0, celdaString.IndexOf(',')));
            this.ficha = ficha;
        }

        //Estas funciones nos devuelven diversa información
        public int getX() { return this.x; }
        public int getY() { return this.y; }
        public char getFicha() { return this.ficha; }
        public void setFicha(char ficha) { this.ficha = ficha; }
        public String toString() { return "("+getY()+","+getX() + "): "+ficha; }
    }
}
