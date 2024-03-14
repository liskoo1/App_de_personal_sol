using App_de_personal.DB;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public partial class CrearDataBase : Window
    {

        public CrearDataBase()
        {
            InitializeComponent();
            
        }

       
        private async Task CrearBaseDeDatos()
        {
            try
            {
                //Abrimos conexión a base de datos desde master
                ConexionBD.abrirMaster(txtDataSource.Text);
                //metemos el path en una variable
                string scriptFilePath = "../../../Resource/script.sql";
                //Leemos el archivo
                string script = File.ReadAllText(scriptFilePath);
                //Cambiamos todas las coincidencias con el nuevo nombre de la base de datos en el script
                string newScript = script.Replace("new", txtNombreDb.Text);
                //Cambiamos el nombre del servidor en el script
                string newScript2 = newScript.Replace(@"DESKTOP-IUJGFQQ\SQLEXPRESS", txtDataSource.Text);
                //metemos el nuevo path en una variable para no cambiar el archivo original y sea reutilizable
                string newScriptFilePath = "../../../Resource/script2.sql";
                //Escribimos el archivo nuevo con todo cambiado para generar nuestra base de datos
                File.WriteAllText(newScriptFilePath, newScript2);
                //Instancimos Process que nos permite abrir la consola
                Process process = new Process();
                //Le indicamos que es un proceso sqlcmd
                process.StartInfo.FileName = "sqlcmd";
                //Pasamos el nombre del servidor donde queremos que cree nuestra base de datos  y le pasamos el scrip
                //que contiene toda la query ya modificada
                process.StartInfo.Arguments = @$"-S {txtDataSource.Text} -i {newScriptFilePath}";
                //Iniciamos proceso
                process.Start();
                string conexion = @$"Data Source={txtDataSource.Text};Initial Catalog={txtNombreDb.Text};Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True";
                File.WriteAllText("../../../Resource/ConexionDB.txt", conexion);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                ConexionBD.closeMaster(txtDataSource.Text);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click();
            // Cerrar la aplicación actual
            //Application.Current.Shutdown();

            // Volver a abrir la aplicación
            //Process.Start(Application.ResourceAssembly.Location);

        }
        private async void Click()
        {
            if(!string.IsNullOrEmpty(txtDataSource.Text) ||
                !string.IsNullOrEmpty(txtNombreDb.Text) ||
                !string.IsNullOrEmpty(txtCif.Text) ||
                !string.IsNullOrEmpty(txtNombreEmpresa.Text) ||
                !string.IsNullOrEmpty(txtDireccionEmpresa.Text) ||
                !string.IsNullOrEmpty(txtTelefono.Text) ||
                !string.IsNullOrEmpty(txtEmail.Text))
            {
                //lismpìamos pantalla
                gridContenedor.Children.Clear();
                //Introducimos el control de usuario en es ventana dentro del grid
                Actualizando actualizando = new Actualizando();
                gridContenedor.Children.Add(actualizando);
                //Esperamos 3 segundo
                await Task.Delay(3000);
                await Task.Yield();
                //Creamos base de datos
                var taeras = new List<Task>
                {
                    CrearBaseDeDatos(),
                    InsertarEmpresa()
                };

                await Task.WhenAll(taeras);

                MessageBox.Show("Base de datos creada");
                this.Close();
                
            }
            else
            {
                MessageBox.Show("Hay algun campo vacio. Todos los campos deben de estar rellenados para crear la base de datos", "Campos vacios", MessageBoxButton.OK,MessageBoxImage.Information) ;
            }

        }
        private async Task InsertarEmpresa()
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("InsertarEmpresa", ConexionBD.conexion))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Cif", txtCif.Text);
                    cmd.Parameters.AddWithValue("@Nombre",txtNombreEmpresa.Text);
                    cmd.Parameters.AddWithValue("@Direccion",txtDireccionEmpresa.Text);
                    cmd.Parameters.AddWithValue("@Telefono",Convert.ToInt32(txtTelefono.Text));
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);   
            }
            finally
            {
                ConexionBD.close();
            }
        }
    }
}
