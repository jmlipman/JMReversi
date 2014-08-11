using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;




// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Reversi1
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Tablero tablero; //Tablero usado
        Jugador jugadores; //Jugadores
        bool terminado = false; //Determina si el juego ha acabado o no
        Traducir traductor; //Traductor
        private DispatcherTimer performplay;


        /** 
         * Función principal. Se encarga de cargar e inicializar los componentes.
         */
        public MainPage()
        {
            this.InitializeComponent();

            //Componentes invisibles

            //Timer
            performplay = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 100) };

            //Establecemos el idioma
            traductor = new Traducir("es");
            //Inicializamos el tablero con su dimensión
            tablero = new Tablero(8);
            //Inicializamos los jugadores con su nombre
            //jugadores = new Jugador(traductor.getCadena("verde"), traductor.getCadena("azul"));
            jugadores = new Jugador("Player", "Robot");
            //Turno actual
            turnoactual.Text = traductor.getCadena("turnode", jugadores.nombreJugadorActual());
            //Jugadas realizadas
            jugadasrealizadas.Text = traductor.getCadena("jugadasrealizadas");
            //Establecemos el contenido de los botones y las textblocks
            //pasarturnoboton.Content = traductor.getCadena("pasarturno");
            pasarturnoboton.Text = traductor.getCadena("pasarturno");
            jug1.Text = traductor.getCadena("jugador") + " 1: " + jugadores.consultarJugador1();
            jug2.Text = traductor.getCadena("jugador") + " 2: " + jugadores.consultarJugador2();
            randomjug1.Content = traductor.getCadena("aleatorio");
            randomjug2.Content = traductor.getCadena("aleatorio");
            //reiniciartableroboton.Content = traductor.getCadena("reiniciartablero");
            reiniciartableroboton.Text = traductor.getCadena("reiniciartablero");
            //Dibuajos el tablero
            DibujarTableroInicial(tablero);

            performplay.Tick += performplay_Tick;
        }

        void performplay_Tick(object sender, object e)
        {
            performplay.Stop();
            //Obtenemos todas las celdas que vamos a cambiar
            Celda[] outputArray = tablero.getCeldasVolteadas().ToArray();
            foreach (Celda celdaOutput in outputArray)
            {
                //Actualizamos las celdas del tablero (estructura de datos)
                tablero.getCelda(celdaOutput.getY(), celdaOutput.getX()).setFicha(celdaOutput.getFicha());
                //Actualizamos las celdas del tablero (interfaz gráfico)
                ActualizarCasilla(celdaOutput.getY(), celdaOutput.getX(), celdaOutput.getFicha());
            }

            //Después de modificar el tablero, cambiamos el turno
            cambiarTurno();
            if (jugadores.nombreJugadorActual() != "Robot" && jugadores.consultarJugador2() == "Robot")
                progressring.IsActive = false;
            
        }

        /**
         *  Dibuja el tablero la primera vez que se ejecuta la aplicación.
         *  @param tablero (contenido) que se va a dibujar.
         */ 
        public void DibujarTableroInicial(Tablero tablero)
        {

            int tamañoRectangulo = 60; //Tamaño del rectángulo
            int espaciado = 6; //Interespaciado entre los rectángulos
            int margenPropio = espaciado / 2 + tamañoRectangulo; //Margen de cada rectangulo (tamaño + la mitad del espaciado)
            int margenDerechoTotal = (tablero.getTamaño() / 2 - 1)*margenPropio*2+margenPropio; //Margen de la derecha total
            int tmp = tablero.getTamaño() / 2 - 1; //A donde irán los valores iniciales

            for (int a = 0; a < tablero.getTamaño(); a++)
            {
                for (int b = 0; b < tablero.getTamaño(); b++)
                {
                    //Creamos un rectángulo
                    CreateARectangleWithSolidBrush(tamañoRectangulo,
                        0, margenDerechoTotal - (b * (margenPropio * 2)),
                        0, margenDerechoTotal - (a * (margenPropio * 2)), a + "," + b);
                    //Actualizamos el contenido de la casilla dependiendo de la ficha que tenga
                    ActualizarCasilla(a, b, tablero.getCelda(a, b).getFicha());
                }
            }
           
            //Ponemos las banderas
            banderaespaña.Source = new BitmapImage(new Uri("ms-appx:///Assets/spanishflag.png", UriKind.Absolute));
            banderainglaterra.Source = new BitmapImage(new Uri("ms-appx:///Assets/englishflag.png", UriKind.Absolute));
        }

        
        /**
         * Se activa cuando se clickea en una celda. Queremos realizar alguna jugada.
         */ 
        void Celda_Click(object sender, PointerRoutedEventArgs e)
        {

            //Obtenemos las coordenadas del nombre de los rectángulos
            String coordenadas = ((Rectangle)sender).Name;

            //Se realiza una jugada
            realizarJugada(new Celda(coordenadas, jugadores.consultarTurno()));
            
        }

        /**
         * Intentará realizar una jugada, aunque para eso haya que ver si es legal el movimiento y demás.
         * @param celda de la jugada que se quiere realizar.
         */ 
        public void realizarJugada(Celda celda)
        {
            if (!terminado) //Si no está terminado procedemos a jugar, si está terminado no hacemos nada.
            {
                //Creamos una celda con la jugada realizada, poniendo de ficha la del jugador que ha clickeado
                //Celda celda = new Celda(coordenadas, jugadores.consultarTurno());
                string jugadastxt;
                //Comprobamos si la jugada realizada es legal
                if (tablero.esLegal(celda))
                {
                    progressring.IsActive = true;
                    flip.Play();
                    turnoactual.Foreground = new SolidColorBrush(Colors.White);
                    jugadastxt = traductor.getCadena("movimientolegal");

                    performplay.Start();

                    //Se obtiene el movimiento realizado
                    jugadas.Text = jugadastxt + " (" + celda.getX() + "," + celda.getY() + ")";
                }
                else
                {//Se ha producido un movimiento ilegal
                    jugadastxt = traductor.getCadena("movimientoilegal");
                    turnoactual.Foreground = new SolidColorBrush(Colors.Red);
                    turnoactual.Text = traductor.getCadena("movimientoilegal");
                }

            }
        }

        /**
         * Esta función se encarga de cambiar el turno en el juego
         */ 
        public void cambiarTurno()
        {
            //Se cambia el turno en los jugadores (estructura de datos)
            jugadores.cambiarTurno();
            //Se cambia el turno en el interfaz gráfico.
            turnoactual.Text = traductor.getCadena("turnode", jugadores.nombreJugadorActual());

            //Vemos si el tablero está acabado o simplemente el jugador actual no puede tirar.
            if (tablero.estaAcabado(jugadores.consultarTurno()))
            {
                //Ahora comprobamos si el otro jugador no puede jugar. En tal caso, se acabó.
                if (tablero.estaAcabado(Jugador.fichaOponente(jugadores.consultarTurno())))
                {
                    //Sonido de finalización del juego.
                    tada.Play();

                    terminado = true;
                    turnoactual.Foreground = new SolidColorBrush(Colors.Red);
                    turnoactual.Text = traductor.getCadena("juegoterminado") + "\n";
                    //Miramos a ver si hay tablas o si alguien ha ganado
                    if (jugadores.consultarGanador(tablero) == "tttablas")
                        turnoactual.Text += traductor.getCadena("tablas");
                    else
                        turnoactual.Text += traductor.getCadena("ganador") + ": " + jugadores.consultarGanador(tablero);

                }
                else //No se pueden realizar mas jugadas, asi que tendrá que pasar.
                    turnoactual.Text = traductor.getCadena("nosepuedenhacermasjugadas");
            }

            //Si el juego no ha acabado y se quiere jugar contra la máquina, empezamos a usar el algoritmo.
            if (!terminado && jugadores.nombreJugadorActual() == "Robot")
            {
                
                //Jugada del robot
                //El último parámetro dependerá de la dificultad: facil(1), medio(3) y dificil(6)
                AlfaBeta algoritmo = new AlfaBeta(tablero, jugadores.consultarTurno(), 6);
                //Realizamos la jugada que hemos calculado con el algoritmo
                realizarJugada(algoritmo.obtenerJugada());

                //IMPORTANTE: ESTO DE AQUI LO VOY A QUITAR, PERO AL FINALIZAR UNA JUGADA ME HA DADO NULLPOINTEREXCEPTION
                //jugadas.Text = algoritmo.obtenerJugada().toString();
            }
        }

        /**
         * Esta función cambia las casillas (entorno gráfico).
         * Dependiendo del jugador, se pueden establecer unas propiedades u otras.
         * @param y coordenada.
         * @param x coordenada.
         * @param ficha jugada.
         */ 
        public void ActualizarCasilla(int y, int x, char ficha)
        {
            //Obtenemos el nombre de la celda.
            String nameCelda = y + "," + x;
            //Establecemos un color
            /*SolidColorBrush fillColor = new SolidColorBrush();
            //Dependiendo de la ficha, tenemos un color distinto
            switch (ficha)
            {
                case '-': fillColor.Color = Colors.White; break;
                case 'O': fillColor.Color = Colors.Blue; break;
                case 'X': fillColor.Color = Colors.Green; break;
                default: fillColor.Color = Colors.Red; break;
            }
            
            //Obtenemos la celda actual
            var celdaActual = (Rectangle)FindName(nameCelda);
            //Le cambiamos el color por el nuevo
            celdaActual.Fill = fillColor;*/

            String token_name = "";

            switch (ficha)
            {
                //case '-': fillColor.Color = Colors.White; break;
                case 'O': token_name = "blacktoken"; break;
                case 'X': token_name = "whitetoken"; break;
            }

            //Obtenemos la celda actual
            var celdaActual = (Rectangle)FindName(nameCelda);

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/"+token_name+".png", UriKind.RelativeOrAbsolute));
            celdaActual.Fill = imageBrush;

            
        }



        /**
         * Esta función se encarga de reiniciar el tablero.
         * Se ejecuta cuando se clickea en su botón correspondiente.
         */ 
        private void ReiniciarTablero_Click(object sender, RoutedEventArgs e)
        {
            //Establecemos algunos parámetros tal y como estaban al principio
            terminado = false;
            tablero = new Tablero(8);
            turnoactual.Foreground = new SolidColorBrush(Colors.White);
            jugadores = new Jugador(jugadores.consultarJugador1(), jugadores.consultarJugador2());
            jugadas.Text = "";
            turnoactual.Text = traductor.getCadena("turnode", jugadores.nombreJugadorActual());
            //Se pinta de nuevo el tablero
            for (int a = 0; a < tablero.getTamaño(); a++)
                for (int b = 0; b < tablero.getTamaño(); b++)
                    ActualizarCasilla(a, b, tablero.getCelda(a, b).getFicha());
        }

        /**
         * Esta función se encarga de cambiar el turno.
         * Se ejecuta cuando se clickea en su botón correspondiente.
         */ 
        private void CambiarTurno_Click(object sender, RoutedEventArgs e)
        {
            cambiarTurno();
        }

        /**
         * Función auxiliar que se encarga de crear rectágulos, que serán las celdas.
         */ 
        public void CreateARectangleWithSolidBrush(int tamañoRectangulo, int marginLeft,
            int marginRight, int marginTop, int marginBottom, String name)
        {

            // Create a Rectangle
            Rectangle rectangle = new Rectangle();
            rectangle.Height = tamañoRectangulo;
            rectangle.Width = tamañoRectangulo;
            rectangle.Margin = new Thickness(bottom: marginBottom, top: marginTop, right: marginRight, left: marginLeft);
            rectangle.Name = name;
            rectangle.PointerPressed += Celda_Click;
            // Create a blue and a black Brush

            SolidColorBrush fillColor = new SolidColorBrush();
            fillColor.Color = Colors.White;
            SolidColorBrush strokeColor = new SolidColorBrush();
            strokeColor.Color = Colors.Black;

            // Set Rectangle's width and color
            rectangle.StrokeThickness = 1;
            rectangle.Stroke = strokeColor;

            // Fill rectangle with blue color
            rectangle.Fill = fillColor;

            // Add Rectangle to the Grid.
            //LayoutRoot.Children.Add(blueRectangle);
            TableroGrid.Children.Add(rectangle);

        }

        /**
         * Se activa cuando se clickea en una bandera. Se encarga de cambiar el idioma del tablero.
         * Para ello, cambia los valores que se ven actualmente, y se establece el idioma.
         */ 
        private void banderaClick(object sender, PointerRoutedEventArgs e)
        {
            //Obtenemos el nombre de la bandera presionada
            String nombre = ((Image)sender).Name;
            int id_local;

            switch (nombre)
            {
                case "banderaespaña": id_local = 0; break;
                case "banderainglaterra": id_local = 1; break;
                default: id_local = 1; break;
            }
            //comprobar el idioma actual
            if (traductor.getIdiomaActual() != id_local)
            {
                //Se establece el id del nuevo idioma
                traductor.setIdioma(id_local);
                //Se traducen las cadenas de texto que actualmente están siendo vistas en el tablero.
                //reiniciartableroboton.Content = traductor.getCadena("reiniciartablero");
                reiniciartableroboton.Text = traductor.getCadena("reiniciartablero");
                //pasarturnoboton.Content = traductor.getCadena("pasarturno");
                pasarturnoboton.Text = traductor.getCadena("pasarturno");
                jugadasrealizadas.Text = traductor.getCadena("jugadasrealizadas");
                jugadas.Text = "";
                if (jugadores.consultarJugador1() == "Verde" || jugadores.consultarJugador1() == "Green") {
                    jugadores.cambiarJugador1(traductor.getCadena("verde"));
                    jugadores.cambiarJugador2(traductor.getCadena("azul"));
                }
                turnoactual.Text = traductor.getCadena("turnode", jugadores.nombreJugadorActual());
            }

        }

    }
}
