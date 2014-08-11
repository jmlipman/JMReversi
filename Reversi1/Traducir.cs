using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Reversi1
{

    public class Traducir
    {
        //Cantidad aproximada de traducciones, que se saltarán como en una pila de control
        int salto = 50;
        /**
         * Id del idioma:
         * 0 --> español
         * 1 --> ingles
         */ 
        int id_idioma; 


        Dictionary<String, int> identificadores = new Dictionary<String, int>();
        Dictionary<int, String> traduccion = new Dictionary<int, String>();

        /**
         * Constructor que inicializa todas las cadenas de texto, y setea el idioma predefinido (español).
         */ 
        public Traducir(String idioma)
        {

            List<String> id_strings = new List<String>(); //identificadores (cadenas)
            List<String> id_español = new List<String>(); //traducción al español
            List<String> id_ingles = new List<String>(); //traducción al inglés
            id_strings.Add("verde");
            id_español.Add("Verde");
            id_ingles.Add("Green");

            id_strings.Add("azul");
            id_español.Add("Azul");
            id_ingles.Add("Blue");

            id_strings.Add("turnode");
            id_español.Add("Turno de (=*=)");
            id_ingles.Add("(=*=)'s turn");

            id_strings.Add("jugadasrealizadas");
            id_español.Add("Jugadas Realizadas");
            id_ingles.Add("Movements");

            id_strings.Add("movimientolegal");
            id_español.Add("Movimiento legal");
            id_ingles.Add("Legal move");

            id_strings.Add("movimientoilegal");
            id_español.Add("Movimiento ilegal");
            id_ingles.Add("Ilegal move");

            id_strings.Add("juegoterminado");
            id_español.Add("El juego ha terminado.");
            id_ingles.Add("The game is over.");

            id_strings.Add("tablas");
            id_español.Add("Tablas (empate)");
            id_ingles.Add("Draw");

            id_strings.Add("ganador");
            id_español.Add("Ganador");
            id_ingles.Add("Winner");

            id_strings.Add("nosepuedenhacermasjugadas");
            id_español.Add("El jugador actual no puede realizar ninguna jugada.\nDebe pasar turno para que el juego continue.");
            id_ingles.Add("The current player cannot perform any move.\nHe must pass to continue.");

            id_strings.Add("pasarturno");
            id_español.Add("Pasar turno");
            id_ingles.Add("Pass");

            id_strings.Add("reiniciartablero");
            id_español.Add("Reiniciar tablero");
            id_ingles.Add("Restart board");

            id_strings.Add("jugador");
            id_español.Add("Jugador");
            id_ingles.Add("Player");

            id_strings.Add("aleatorio");
            id_español.Add("Aleatorio");
            id_ingles.Add("Random");

            //Añadimos las traducciones a los diccionarios
            int cont = 0;
            foreach (String str in id_strings)
                identificadores.Add(str, cont++);

            cont=0;
            foreach (String str in id_español)
                traduccion.Add(cont++, str);

            cont = 0;
            foreach (String str in id_ingles)
                traduccion.Add(cont++ + salto * 1, str);
            
            //Establecemos el id del idioma
            switch (idioma)
            {
                case "es": id_idioma = 0; break;
                case "en": id_idioma = 1; break;
                default: id_idioma = 1; break;
            }

            
        }
        /**
         * Esta funcion devuelve una cadena (traducción) en base a un identificador.
         * @param cadena identificadora para obtener la traducción.
         * @return cadena traducida.
         */ 
        public String getCadena(String cadena)
        {
            return traduccion[identificadores[cadena]+salto*getIdiomaActual()];
        }

        /**
         * Esta funcion devuelve una cadena (traducción) en base a un identificador.
         * También se encarga de hacer un reemplazamiento.
         * Esto es util para traducciones en las que ciertas cosas cambian de posición.
         * @param cadena identificadora para obtener la traducción.
         * @param cadena2 que va a ser el reemplazo de la cadena traducida.
         * @return cadena traducida.
         */
        public String getCadena(String cadena, String cadena2)
        {
            String tmp = traduccion[identificadores[cadena]+salto*getIdiomaActual()];
            return tmp.Replace("(=*=)",cadena2);
        }

        /**
         * Nos devuelve el id del idioma actual.
         */ 
        public int getIdiomaActual() { return this.id_idioma; }

        /**
         * Establece el idioma actual.
         */ 
        public void setIdioma(int id) { this.id_idioma = id; }
    }
}
