using System;

public class Tablero
{
    private char[,] tablero;
    private int tamañoMaximo=10,tamañoMinimo=6;
    private int tamaño;

    public Tablero(int tamaño)
	{
        if (tamaño % 2 != 0) tamaño += 1;
        if (tamaño < tamañoMinimo || tamaño > tamañoMaximo) tamaño = 8;
        this.tamaño = tamaño;
        tablero = new char[tamaño, tamaño];
	}
}
