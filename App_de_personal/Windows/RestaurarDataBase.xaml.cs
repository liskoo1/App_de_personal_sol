using App_de_personal.DB;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Azure.Core.HttpHeader;

namespace App_de_personal.Windows
{
    /// <summary>
    /// Lógica de interacción para RestaurarDataBase.xaml
    /// </summary>
    public partial class RestaurarDataBase : Window
    {
        public RestaurarDataBase()
        {
            InitializeComponent();
        }
        private string conexion;
        private void btnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            importarDb();
        }
        private void importarDb()
        {
            OpenFileDialog saveFileDialog = new OpenFileDialog();
            saveFileDialog.Filter = "Archivos de copia de seguridad (*.bak)|*.bak";
            saveFileDialog.Title = "Exportar base de datos";
            saveFileDialog.ShowDialog();
            string path = saveFileDialog.FileName;
            string nameDb = txtNombreDb.Text;
            string servidor = txtNombreServidor.Text;
            if(!string.IsNullOrEmpty(nameDb) || !string.IsNullOrEmpty(servidor))
            {
                try
                {

                    ConexionBD.abrirMaster(txtNombreServidor.Text);
                    contenedor.Children.Clear();
                    Actualizando actualizando = new Actualizando();
                    contenedor.Children.Add(actualizando);
                    using (SqlCommand cmd = new SqlCommand($"RESTORE DATABASE {nameDb} FROM DISK = '{path}';", ConexionBD.conexion))
                    {
                        cmd.ExecuteNonQuery();
                        System.Windows.MessageBox.Show("Base de Datos exportada.");
                        conexion = @$"Data Source={txtNombreServidor.Text};Initial Catalog={txtNombreDb.Text};Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True";
                        ConexionBaseDeDatos.GenerarArchivoPath(conexion);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    ConexionBD.closeMaster(txtNombreServidor.Text);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Falta algun campo por rellenar");
            }
            
        }
    }
}
