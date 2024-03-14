using App_de_personal.DB;
using App_de_personal.Logica;
using signotec.STPadLibNet;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
using signotec.STPadLibNet;
using System.Drawing.Imaging;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.IO;

namespace App_de_personal.Windows
{
    /// <summary>
    /// Lógica de interacción para AgregarTicket.xaml
    /// </summary>
    public partial class AgregarTicket : Window
    {
        LEmpresa lEmpresa;
        int Id_currenUser;
        byte[] Firma;
        public STPadLib stPad;
        LTickets tickets;
        bool modify;
        public AgregarTicket(LEmpresa _lEmpresa, int idCurrenUser, LTickets ticket = null, bool modificar = false )
        {
            lEmpresa = _lEmpresa;
            Id_currenUser = idCurrenUser;
            tickets = ticket;
            modify = modificar;
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            DTickets dTickets = new DTickets();
            if (modify == false)
            {

                Firma = leerImagenFirma();
                if(Firma != null && Firma.Length > 5)
                {
                    try
                    {

                        dTickets.InsertarTicket(lEmpresa.Id_Empresa, Id_currenUser, Convert.ToInt32(txtNumeroOperacion.Text), txtNombreCompleto.Text, txtIdentificacion.Text, txtMatricula.Text,
                        Convert.ToInt32(txtTelefono.Text), txtDireccion.Text, txtxEmail.Text, Convert.ToDouble(txtCuantia.Text), comboBoxEstado.Text, Firma, txtNotas.Text);
                        this.Close();
                    }
                    catch
                    {
                    
                        MessageBox.Show("Algun dato introducido no es válido en su campo");
                        inicializarTableta();
                        inkFirma.Strokes.Clear();

                    }
                }
                else
                {
                    MessageBox.Show("Falta la firma");
                    inicializarTableta();
                    inkFirma.Strokes.Clear();
                }

            }
            else
            {
                if (comboBoxEstado.Text == "PAGADO")
                {

                    MessageBoxResult result = MessageBox.Show($"Estas seguro que quieres cambiar el estado de este ticket de NO PAGADO a PAGADO ? ", "Modificar ticket", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            dTickets.ModificarTicketAPagado(tickets.Id_Ticket, Convert.ToInt32(txtNumeroOperacion.Text), txtNombreCompleto.Text, txtIdentificacion.Text, txtMatricula.Text,
                            Convert.ToInt32(txtTelefono.Text), txtDireccion.Text, txtxEmail.Text, Convert.ToDouble(txtCuantia.Text), comboBoxEstado.Text, txtNotas.Text);
                            this.Close();
                        }
                        catch
                        { 
                            MessageBox.Show("Algun dato introducido no es válido en su campo");
                        }

                    }
                }
                else
                {
                    try
                    {

                        dTickets.ModificarTicket(tickets.Id_Ticket, Convert.ToInt32(txtNumeroOperacion.Text), txtNombreCompleto.Text, txtIdentificacion.Text, txtMatricula.Text,
                        Convert.ToInt32(txtTelefono.Text), txtDireccion.Text, txtxEmail.Text, Convert.ToDouble(txtCuantia.Text), txtNotas.Text);
                        this.Close();
                    }
                    catch
                    {

                        MessageBox.Show("Algun dato introducido no es válido en su campo");
                    }
                }  
            }    
        }
        private void formAgregarTicket_Loaded(object sender, RoutedEventArgs e)
        {
            inicializarTableta();

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
                Firma = stPad.SignatureGetSignData();
                
                
                if (Firma == null || Firma.Length == 0)
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
                    //cerramos conexion con la tableta 
                    
                    return true;
                }


            }
            catch (Exception ex)
            {

                return false;
            }
            finally { stPad.DeviceClose(0); }
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
                double transformedX = (x * inkFirma.ActualWidth) / maxX;
                double transformedY = (y * inkFirma.ActualHeight) / maxY;

                // Crear un nuevo Stroke para el punto
                StylusPointCollection points = new StylusPointCollection();
                points.Add(new StylusPoint(transformedX, transformedY));

                Stroke stroke = new Stroke(points);
                inkFirma.Strokes.Add(stroke);
            });

        }

        private void formAgregarTicket_Closed(object sender, EventArgs e)
        {
            if (stPad.SignatureState == true)
            {
                stPad.DeviceClose(0);
            }
        }

        private byte[] leerImagenFirma()
        {
            byte[] imagenEnByte;
            try
            {
                
                if (capturarFirma())
                {
                    imagenEnByte = File.ReadAllBytes("../../../Resource/imagen.png");
                    //Buscamos al idpersonal del la identificacion introducida en txtIdentificacion
                    return imagenEnByte;
                }
                else
                {
                    MessageBox.Show("Debes de firmar antes de ticar");
                    return imagenEnByte = [];
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return imagenEnByte = [];
            }
        }
    }
}
