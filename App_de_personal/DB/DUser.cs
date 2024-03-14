using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using App_de_personal.Logica;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;
using System.IO;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;


namespace App_de_personal.DB
{
    public class DUser
    {
        public LUser BuscarUser(string NameUser,string Pass)
        {
			try
			{
				ConexionBD.abrir();
				using (SqlCommand cmd = new SqlCommand("BuscarUser", ConexionBD.conexion))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@nameUser", NameUser);
					cmd.Parameters.AddWithValue ("@Pass", Pass);
					LUser lUser = new LUser();
					SqlDataReader dataReader = cmd.ExecuteReader();
					if (dataReader.HasRows )
					{
						dataReader.Read();
						lUser.Id_Personal = dataReader.GetInt32(0);
						lUser.NameUser = dataReader.GetString(1);
						lUser.Pass = dataReader.GetString(2);
						return lUser;
					}
					else
					{
                        
                        return lUser;
						
					}

				}

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);
				return null;
			}
			finally
			{
				ConexionBD.close();
			}
        }
		public bool InsertarUser(int Id_Personal,string NameUser,string Pass)
		{
			try
			{
				ConexionBD.abrir();
				using (SqlCommand cmd = new SqlCommand("InsertarUser",ConexionBD.conexion))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Id_Personal",Id_Personal);
					cmd.Parameters.AddWithValue("@NameUser", NameUser);
					cmd.Parameters.AddWithValue("@Pass", Pass);
					cmd.ExecuteNonQuery();
                    return true;
				}
			}
			catch (Exception ex)
			{
                
                Lpersonal lpersonal = new Lpersonal();
                lpersonal.Id_personal = Id_Personal;
                ConexionBD.abrir();
                using(SqlCommand cmd = new SqlCommand("DELETE FROM Personal WHERE Id_Personal = @Id_personal", ConexionBD.conexion))
                {
                    cmd.Parameters.AddWithValue("@Id_personal", Id_Personal);
                    cmd.ExecuteNonQuery();
                }
				MessageBox.Show(ex.Message);
                return false;
			}
			finally
			{
                ConexionBD.close();
            }
			
		}
		public Lpersonal BuscarDatosUsuarioPorId(int Id_Personal)
		{
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand($"Select * from Personal where Id_Personal = {Id_Personal}", ConexionBD.conexion))
                {
                    Lpersonal usuario = new Lpersonal();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
						dataReader.Read();
                        usuario.Id_personal = dataReader.GetInt32(0);
                        usuario.Nombre = dataReader.GetString(1);
                        usuario.Identificacion = dataReader.GetString(2);
                        usuario.Pais = dataReader.GetString(3);
                        usuario.Id_cargo = dataReader.GetInt32(4);
                        usuario.SueldoPoHora = (double)dataReader.GetDecimal(5); //esta linea esta mal usuario.SueldoPoHora es un double
                        usuario.Estado = dataReader.GetString(6);
                        usuario.Codigo = dataReader.GetString(7);
						return usuario;
                    }
					else
					{
						return null;
					}
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
				return null;
            }
            finally
            {
                ConexionBD.close();
            }
        }
        public bool MostrarImagenDesdeBD(int Id_personal, MainWindow mainWindow)
        {

            ConexionBD.abrir();
            using (SqlCommand comando = new SqlCommand($"Select imagen from Imagenes where Id_Personal = {Id_personal}", ConexionBD.conexion))
            {
                SqlDataReader dataReader = comando.ExecuteReader();
                try
                {
                    if (dataReader.Read())
                    {
                        //creamos un array sin un tamaño especifico
                        byte[] datosImagen;
                        // Obtener la longitud de los datos de imagen
                        long longitudBytes = dataReader.GetBytes(0, 0, null, 0, 0);                        
                        // Le damos la longitud al array
                        datosImagen = new byte[longitudBytes];
                        // Obtener los datos de la imagen y almacenarlos en el array
                        dataReader.GetBytes(0, 0, datosImagen, 0, (int)longitudBytes);                
                        return MostrarImagenEnControl(datosImagen, mainWindow);
                    }
                    else
                    {
                        
                        return false;
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al recuperar la imagen desde la base de datos: " + ex.Message);
                    return false;
                }
                finally
                {
                    ConexionBD.close();
                }
            }
        }

        
        private bool MostrarImagenEnControl(byte[] datosImagen, MainWindow mainWindow)
        {
            // Este método muestra una imagen en un control de imagen en la ventana principal.
            // Recibe como parámetros los datos de la imagen en forma de un array de bytes y la instancia de la ventana principal.
            // Retorna un valor booleano que indica si la operación fue exitosa.
            try
            {
                // Se crea un nuevo flujo de memoria utilizando los datos de la imagen.
                MemoryStream ms = new MemoryStream(datosImagen);

                // Se crea una nueva instancia de BitmapImage para representar la imagen.
                BitmapImage imagen = new BitmapImage();

                // Se inicia la carga de la imagen.
                imagen.BeginInit();

                // Se especifica el flujo de memoria como origen de la imagen.
                imagen.StreamSource = ms;

                // Se finaliza la carga de la imagen.
                imagen.EndInit();

                // Se asigna la imagen al control de imagen en la ventana principal.
                mainWindow.imgPhoto.Source = imagen;

                // Se indica que la operación fue exitosa.
                return true;
            }
            catch (Exception ex)
            {
                // Se muestra un mensaje de error en caso de que ocurra una excepción.
                MessageBox.Show("Error al mostrar la imagen: " + ex.Message);

                // Se indica que la operación no fue exitosa.
                return false;
            }
        }
        public string CreateHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computar el hash de la entrada y obtener el arreglo de bytes.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convertir el arreglo de bytes a un string en hexadecimal.
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

    }
}
