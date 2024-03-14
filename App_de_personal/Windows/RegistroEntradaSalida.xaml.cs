using App_de_personal.DB;
using App_de_personal.Logica;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using signotec.STPadLibNet;
using System.Drawing.Imaging;
using System.IO;

namespace App_de_personal.Windows
{
    /// <summary>
    /// Lógica de interacción para RegistroEntradaSalida.xaml
    /// </summary>
    public partial class RegistroEntradaSalida : Window
    {
        public STPadLib stPad;

        public string nombreEmpresa;
        public RegistroEntradaSalida(string nombreEmpresa)
        {
            this.nombreEmpresa = nombreEmpresa;
            
            InitializeComponent();
            lblNombreEmpresa.Content = this.nombreEmpresa;
            timer();
            inicializarTableta();
        }


        private void timer()
        {
            // Crear y configurar el DispatcherTimer
            DispatcherTimer timer = new DispatcherTimer();
            //le decimos que los intervalos son de un segundo
            timer.Interval = TimeSpan.FromSeconds(1);
            //Creamos el evento
            timer.Tick += Timer_Tick;
            // Iniciar el temporizador
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Este método se ejecutará cada vez que transcurra el intervalo del temporizador
            // Obtener la hora actual
            DateTime currentTime = DateTime.Now;
            var currentDate = DateTime.Now.ToShortDateString();
            lblDate.Content = currentDate;
            // Actualizar el contenido del Label con la hora actual
            lblClock.Content = currentTime.ToString("HH:mm:ss");
        }

        private void btnAgregarEntrada_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (capturarFirma())
                {
                    byte[] imagenEnByte = File.ReadAllBytes("../../../Resource/imagen.png");
                    //Buscamos al idpersonal del la identificacion introducida en txtIdentificacion
                    var lpersonal = Dpersonal.BuscarIdPersonal(txtIdentificacion.Text);
                    //cremaos un objeto lAsistencia para guardar los datos que recibamos
                    LAsistencia lAsistencia = new LAsistencia();
                    //adjudicamos los datos a cada propiedad del objeto
                    lblNombre.Content = lpersonal.Nombre;
                    lblEstado.Content = lpersonal.Estado;
                    lAsistencia.Id_Personal = lpersonal.Id_personal;
                    //Primero hay que invocar a Date time No se puede 
                    DateTime currentTime = DateTime.Now;
                    //insertamos la fecha actual de entrada
                    lAsistencia.Fecha_Entrada = currentTime;  
                    lAsistencia.Horas = 0;
                    lAsistencia.Observaciones = txtObservaciones.Text;
                    lAsistencia.Firma = imagenEnByte;                    //llamamos a la funcion e introducimos los datos el parametro que recibe es un objeto LAsistencia
                    Dpersonal funcion = new Dpersonal();
                    if (funcion.Insertar_Asistencia(lAsistencia))
                    {
                        firma.Strokes.Clear();
                        txtObservaciones.Text = null;
                        txtIdentificacion.Text = null;
                        this.Close();
                    }

                }
                else 
                {
                    inicializarTableta();
                    MessageBox.Show("Debes de firmar antes de ticar");
                }
            }
            catch(Exception ex)
            {
                inicializarTableta();
                firma.Strokes.Clear();
                
            }
        }

        private void btnAgregarSalida_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdentificacion.Text))
            {
                var lpersonal = Dpersonal.BuscarIdPersonal(txtIdentificacion.Text);
                Dpersonal funcion = new Dpersonal();
                funcion.UpdateFecha_Hora(lpersonal.Id_personal,txtObservaciones.Text);
                this.Close();
            }
            
        }
        public void inicializarTableta()
        {
            stPad = new STPadLib(); // Asume que STPadLib es la clase correcta; reemplázalo por la clase real del SDK.

            try
            {
                stPad.DeviceOpen(0); // Intenta abrir el primer dispositivo conectado.


                stPad.SignatureDataReceived += STPad_SignatureDataReceived;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el dispositivo: {ex.Message}");
            }
            try
            {

                // Iniciar la captura de la firma.
                stPad.SignatureStart();
                stPad.DisplaySetText(10, 10, signotec.STPadLibNet.TextAlignment.Left, "Firma");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar la captura de la firma: {ex.Message}");
            }
        }
        public bool capturarFirma()
        {
            try
            {

                // Finaliza la captura de la firma.
                // Este paso depende del SDK y puede involucrar el llamado a métodos como SignatureStop() o SignatureConfirm().
                stPad.SignatureConfirm();

                // Obtén un array de bytes de la imagen del pad
                byte[] signatureData = stPad.SignatureGetSignData();
                if (signatureData == null || signatureData.Length == 0)
                {
                    return false;
                }
                else
                {
                    // guardamos la imagen en un archivo que vamos a reescribir cada vez que se haga una firma en la tab
                    stPad.SignatureSaveAsFileEx("../../../Resource/imagen.png", 720, 500, 250, ImageFormat.Png, 5, System.Drawing.Color.Black, SignatureImageFlag.AlignBottom);
                    //path del archivo creado
                    string path = "../../../Resource/imagen.png"; // Si el archivo no tiene extensión en el nombre, asegúrate de agregarla aquí

                    BitmapImage bitmapImage = new BitmapImage();

                    // Inicializar BitmapImage con el archivo 
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Carga la imagen completamente para mantenerla en memoria
                    bitmapImage.EndInit();
                    ;
                    return true;
                }


            }
            catch (Exception ex)
            {
                
                return false;
            }
            finally
            {
                stPad.DeviceClose(0);
            }
        }
        
        private void STPad_SignatureDataReceived(object sender, SignatureDataReceivedEventArgs e)
        {


            Dispatcher.Invoke(() =>
            {
                int maxX = 8000; // Máximo valor en X reportado por la tableta
                int maxY = 4000; // Máximo valor en Y reportado por la tableta
                int x = e.xPos;
                int y = e.yPos;

                // Transformar coordenadas a la resolución de tu InkCanvas
                double transformedX = (x * firma.ActualWidth) / maxX;
                double transformedY = (y * firma.ActualHeight) / maxY;

                // Crear un nuevo Stroke para el punto
                StylusPointCollection points = new StylusPointCollection();
                points.Add(new StylusPoint(transformedX, transformedY));

                Stroke stroke = new Stroke(points);
                firma.Strokes.Add(stroke);
            });

        }



        private void btnMostrarAsistencias_Click_1(object sender, RoutedEventArgs e)
        {
            MostrarAsistencias mostrarAsistencias = new MostrarAsistencias(this);
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(mostrarAsistencias);
            this.Height = 984;
            if (stPad.SignatureState)
            {
                stPad.DeviceClose(0);
            }
            
        }

        private void ventanaAsistencia_Closed(object sender, EventArgs e)
        {
            if (stPad.SignatureState)
            {
                stPad.DeviceClose(0);
            }
        }

    }
}
