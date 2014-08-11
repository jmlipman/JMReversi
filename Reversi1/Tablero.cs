using Reversi1;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Tablero
{
    private Celda[,] celdas; //celdas del tablero
    private Celda[] celdasAfectadas = new Celda[8]; //celdas colindantes de la celda casilla jugada
    private List<Celda> outputCeldas; //celdas a las que les vamos a dar la vuelta
    private int tamañoMaximo=10,tamañoMinimo=6; //dimensiones minimas y maximas del tablero
    private int tamaño; //tamaño del tablero

    /**
     * Creamos el tablero y establecemos su tamaño.
     * @param tamaño del tablero.
     */ 
    public Tablero(int tamaño)
	{
        //Ajustamos el tamaño
        if (tamaño % 2 != 0) tamaño += 1;
        if (tamaño < tamañoMinimo || tamaño > tamañoMaximo) tamaño = 8;
        this.tamaño = tamaño;
        celdas = new Celda[tamaño, tamaño];

        int tmp = getTamaño() / 2 - 1; //Temporal para establecer las posiciones iniciales

        //Creamos las celdas
        for (int a = 0; a < getTamaño(); a++)
            for (int b = 0; b < getTamaño(); b++)
                getTablero()[a, b] = new Celda(a, b, '-');
        
        //Establecemos el contenido de las celdas centrales
        getCelda(tmp, tmp).setFicha('X');
        getCelda(tmp, tmp + 1).setFicha('O');
        getCelda(tmp + 1, tmp).setFicha('O');
        getCelda(tmp + 1, tmp + 1).setFicha('X');
	}

    /**
     * Este constructor nos permite crear un tablero idéntico a otro ya existente.
     */ 
    public Tablero(Tablero tablero)
    {
        tamaño = tablero.getTamaño();
        celdas = new Celda[tamaño, tamaño];
        for (int a = 0; a < getTamaño(); a++)
            for (int b = 0; b < getTamaño(); b++)
                getTablero()[a, b] = new Celda(a, b, tablero.getCelda(a,b).getFicha());
    }

    /**
     * Esta función nos devuelve si el movimiento pasado por parámetro es legal.
     * @param celda del movimiento que queremos realizar.
     * @return boolean dependiendo de si es legal o no el movimiento.
     */ 
    public Boolean esLegal(Celda celda)
    {
        //Para que sea un movimiento legal, la celda tiene que estar vacia, y tiene que haber casillas afectadas
        if (celda!=null && getCelda(celda.getY(),celda.getX()).getFicha()=='-' && comprobarSiHayCeldasAfectadas(celda))
            return true;
        return false;
    }

    /**
     * Esta función comprueba si hay celdas afectadas.
     * En ese caso, las pasa a una variable global pública.
     * @param celda de la jugada que vamos a comprobar.
     * @return boolean dependiendo de si hay celdas afectadas o no.
     */ 
    public bool comprobarSiHayCeldasAfectadas(Celda celda)
    {

        //Posibles celdas en donde buscará continuar
        //Son las colindantes con ficha del oponente
        List<Celda> viasTemporales = new List<Celda>(); 
        //Celdas que igual cambiamos o igual no.
        List<Celda> tmpCeldasFinales;
        //Rotaciones que debemos hacer para recorrer una fila, columna o diagonal
        int x_rotar, y_rotar, x_base_rotar, y_base_rotar;
        //Inicializamos outputceldas para que se vacie.
        outputCeldas = new List<Celda>();

        int[,] posiciones = new int[8, 2];
        posiciones[0, 0] = -1; posiciones[0, 1] = -1;
        posiciones[1, 0] = -1; posiciones[1, 1] = 0;
        posiciones[2, 0] = -1; posiciones[2, 1] = 1;
        posiciones[3, 0] = 0; posiciones[3, 1] = -1;
        posiciones[4, 0] = 0; posiciones[4, 1] = 1;
        posiciones[5, 0] = 1; posiciones[5, 1] = -1;
        posiciones[6, 0] = 1; posiciones[6, 1] = 0;
        posiciones[7, 0] = 1; posiciones[7, 1] = 1;

        //Visitamos todas las posibles posiciones en las que puede seguir la fila
        for(int a=0;a<8;a++)
            if (celda.getY() + posiciones[a, 0] >= 0 && celda.getY() + posiciones[a, 0] < tamaño
                && celda.getX() + posiciones[a, 1] >= 0 && celda.getX() + posiciones[a, 1] < tamaño
                && getCelda(celda.getY() + posiciones[a, 0], celda.getX() + posiciones[a, 1]).getFicha() == Jugador.fichaOponente(celda.getFicha()))
                viasTemporales.Add(new Celda(celda.getY() + posiciones[a, 0], celda.getX() + posiciones[a, 1],
                    getCelda(celda.getY() + posiciones[a, 0], celda.getX() + posiciones[a, 1]).getFicha()));

        
        //Hasta este momento hemos obtenido las direcciones de las celdas a las que podemos ir en busca de una ruta a voltear.
        foreach (Celda viatemporal in viasTemporales)
        {
            tmpCeldasFinales = new List<Celda>();
            //Obtenemos una base con la que iremos permutando
            x_base_rotar = viatemporal.getX() - celda.getX();
            y_base_rotar = viatemporal.getY() - celda.getY();
            x_rotar = x_base_rotar;
            y_rotar = y_base_rotar;

            //Por si no entramos en el while, hay que aumentarlas igualmente
            /*if (!(celda.getY() + y_rotar >= 0 && celda.getY() + y_rotar < tamaño && celda.getX() + x_rotar >= 0 && celda.getX() + x_rotar < tamaño &&
                getCelda(celda.getY() + y_rotar, celda.getX() + x_rotar).getFicha() == Jugador.fichaOponente(celda.getFicha())))
            {
                y_rotar += y_base_rotar;
                x_rotar += x_base_rotar;
            }*/

            //En este while vamos buscando rutas
            while (celda.getY() + y_rotar >= 0 && celda.getY() + y_rotar < tamaño && celda.getX() + x_rotar >= 0 && celda.getX() + x_rotar < tamaño && 
                getCelda(celda.getY() + y_rotar, celda.getX() + x_rotar).getFicha() == Jugador.fichaOponente(celda.getFicha()))
            {
                //Vamos añadiendo celdas y vamos rotando para hacernos con esa línea
                tmpCeldasFinales.Add(new Celda(celda.getY() + y_rotar, celda.getX() + x_rotar, celda.getFicha()));
                y_rotar += y_base_rotar;
                x_rotar += x_base_rotar;
            }

            //Una vez llegado al limite, comprobamos si la ficha que hemos encontrado es nuestra
            //En tal caso, volteamos todo
            if (celda.getX() + x_rotar >= 0 && celda.getX() + x_rotar < tamaño && celda.getY() + y_rotar >= 0 && celda.getY() + y_rotar < tamaño 
                && (getCelda(celda.getY() + y_rotar, celda.getX() + x_rotar).getFicha() == celda.getFicha()))
            {
                //Añadimos la celda actual
                outputCeldas.Add(celda);
                //Y las celdas que hemos obtenido (su ruta)
                outputCeldas.AddRange(tmpCeldasFinales);
            }

        }  //foreach
        
        //Dependiendo de si tenemos algo en el output, devolvemos true o false.
        if (outputCeldas.Count != 0)
            return true;
        return false;
    }

    /**
     * Esta función determina si el tablero está acabado dado una ficha.
     * Se comprueba si se puede meter esa ficha en alguna celda vacia.
     * En caso de que no se pueda, se declara como acabado (para ese jugador).
     * El tablero estará completamente acabado cuando ninguno de los jugadores pueda jugar.
     * @param fichaActual de la que sabrá si puede continuar poniendo mas fichas de su color o no.
     * @return boolean dependiendo de si puede continuar o no.
     */ 
    public bool estaAcabado(char fichaActual)
    {
        for(int a=0;a<getTamaño();a++)
            for(int b=0;b<getTamaño();b++)
                if(esLegal(new Celda(a,b,fichaActual)))
                    return false;
        return true;
    }

    /**
     * Función de debugging que permite printear el tablero.
     */ 
    public void printearTableroDebug()
    {
        for (int a = 0; a < 8; a++)
            Debug.WriteLine(getCelda(a, 0).getFicha() + "" + getCelda(a, 1).getFicha() + "" + getCelda(a, 2).getFicha() + "" +
                getCelda(a, 3).getFicha() + "" + getCelda(a, 4).getFicha() + "" + getCelda(a, 5).getFicha() + "" +
                getCelda(a, 6).getFicha() + "" + getCelda(a, 7).getFicha());
    }


    //Funciones que nos devuelven diversos valores
    public int getTamaño() { return this.tamaño; }
    public Celda[,] getTablero() { return this.celdas; }
    public Celda getCelda(int y, int x) { return celdas[y, x]; }
    public List<Celda> getCeldasVolteadas() { return this.outputCeldas; }
    
}
