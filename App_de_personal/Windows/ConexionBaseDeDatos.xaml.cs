using App_de_personal.DB;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace App_de_personal.Windows
{

    public partial class ConexionBaseDeDatos : Window
    {
        public static string pathConexion { get; set; }
        public ConexionBaseDeDatos()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, RoutedEventArgs e)
        {
            pathConexion = txtPath.Text;
            GenerarArchivoPath(pathConexion);
            try
            {
                ConexionBD.abrir();
                
                using(SqlCommand cmd = new SqlCommand("SELECT * FROM Personal",ConexionBD.conexion))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("La base de datos introducida no cumple con el formato necesario para " +
                            "la ejecución correcta del programa introduzca otra base de datos que cumpla con" +
                            " el standar de este programa");
                    }
                    
                }
                ConexionBD.close();
                
            }
            catch (Exception)
            {
              txtInfo.Text = "La ruta de conexión del servidor no es valida";
            }
            finally
            {
                ConexionBD.close();
            }

            
        }
        public static void GenerarArchivoPath(string pathConexion)
        {
            //Indicamos donde queremos que se cree el archivo con el path indicado
            string path = "../../../Resource/ConexionDB.txt";
            //Generamos el archivo 
            File.WriteAllText(path, pathConexion);
            MessageBox.Show("Archivo creado exitosamente.");
        }
        

        private void btnCrearDb_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
            CrearDataBase crearDataBase = new CrearDataBase();
            crearDataBase.ShowDialog();
        }
    }
}
