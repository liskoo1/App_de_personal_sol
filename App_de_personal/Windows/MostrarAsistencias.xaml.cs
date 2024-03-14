using App_de_personal.DB;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace App_de_personal.Windows
{
    /// <summary>
    /// Lógica de interacción para MostrarAsistencias.xaml
    /// </summary>
    public partial class MostrarAsistencias : UserControl
    {
        private RegistroEntradaSalida Registro;
        public DataTable dt;
        public MostrarAsistencias(RegistroEntradaSalida registroEntradaSalida)
        {
            Registro = registroEntradaSalida;
            InitializeComponent();

        }

        private void dataListadoAsistencias_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {

        }

        private void btnMostrarTodasAsistencias_Click(object sender, RoutedEventArgs e)
        {
            dt = new DataTable();
            Dpersonal dpersonal = new Dpersonal();
            dpersonal.mostrarTodasAsistencias(ref dt);
            dataListadoAsistencias.ItemsSource = dt.DefaultView;
        }

        private void txtBuscadorAsistencias_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            Registro.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dt = new DataTable();
            Dpersonal dpersonal = new Dpersonal();
            dpersonal.mostrarTodasAsistencias(ref dt);
            dataListadoAsistencias.ItemsSource = dt.DefaultView;
        }

        private void txtBuscadorAsistencias_TextChanged(object sender, TextChangedEventArgs e)
        {
            dt = new DataTable();
            Dpersonal dpersonal = new Dpersonal();
            dpersonal.BuscarPersonalYAsistencia(ref dt, txtBuscadorAsistencias.Text);
            dataListadoAsistencias.ItemsSource = dt.DefaultView;
        }

        private void dataListadoAsistencias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            try 
            {
                DataRowView dataRowView = (DataRowView)dataListadoAsistencias.SelectedItem;
                if (dataRowView["Firma"] != DBNull.Value)
                {
                    byte[] firma = (byte[])dataRowView["Firma"];
                    try
                    {
                        // Se crea un nuevo flujo de memoria utilizando los datos de la imagen.
                        MemoryStream ms = new MemoryStream(firma);

                        // Se crea una nueva instancia de BitmapImage para representar la imagen.
                        BitmapImage imagen = new BitmapImage();

                        // Se inicia la carga de la imagen.
                        imagen.BeginInit();

                        // Se especifica el flujo de memoria como origen de la imagen.
                        imagen.StreamSource = ms;

                        // Se finaliza la carga de la imagen.
                        imagen.EndInit();

                        // Se asigna la imagen al control de imagen en la ventana principal.
                        imgFirma.Source = imagen;

                    }
                    catch (Exception ex)
                    {
                        // Se muestra un mensaje de error en caso de que ocurra una excepción.
                        MessageBox.Show("Error al mostrar la imagen: " + ex.Message);

                    }
                }
                else
                {
                    imgFirma.Source = null;
                }
            }
            catch { }

        }
    }
}
