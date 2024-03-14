using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows;
using Microsoft.Data.SqlClient;
using App_de_personal.Logica;

namespace App_de_personal.DB
{
    public class Dpersonal
    {
        public bool InsertarPersonal(Lpersonal parametros,int Id_empresa)
        {
            try
            {   //abrimos conexion
                ConexionBD.abrir();
                //creamos el comando
                SqlCommand cmd = new SqlCommand("InsertarPersonal", ConexionBD.conexion);
                //insertamos datos
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", parametros.Nombre);
                cmd.Parameters.AddWithValue("@Identificacion", parametros.Identificacion);
                cmd.Parameters.AddWithValue("@Pais", parametros.Pais);
                cmd.Parameters.AddWithValue("@Id_cargo", parametros.Id_cargo);
                cmd.Parameters.AddWithValue("@SueldoPorHora", parametros.SueldoPoHora);
                cmd.Parameters.AddWithValue("@Id_Empresa", Id_empresa);
                //ejecutamos Query
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { ConexionBD.close(); }
        }
        public bool UpdatePersonal(Lpersonal parametros)
        {
            try
            {   //Abrimos conexion
                ConexionBD.abrir();
                //Creamos el comando
                SqlCommand cmd = new SqlCommand("UpdatePersonal", ConexionBD.conexion);
                //Indicamos que el comandoa utilizar va a ser un procedure
                cmd.CommandType = CommandType.StoredProcedure;
                //Insertamos datos
                cmd.Parameters.AddWithValue("@Id_Personal", parametros.Id_personal);
                cmd.Parameters.AddWithValue("@Nombre", parametros.Nombre);
                cmd.Parameters.AddWithValue("@Identificacion", parametros.Identificacion);
                cmd.Parameters.AddWithValue("@Pais", parametros.Pais);
                cmd.Parameters.AddWithValue("@Id_cargo", parametros.Id_cargo);
                cmd.Parameters.AddWithValue("@SueldoPorHora", parametros.SueldoPoHora);
                cmd.Parameters.AddWithValue("@Estado", parametros.Estado);
                //Ejecutamos Query
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { ConexionBD.close(); }
        }
        public bool EliminarPersonal(Lpersonal parametros)
        {
            try
            {   //abrimos conexion
                ConexionBD.abrir();
                //creamos el comando
                SqlCommand cmd = new SqlCommand("EliminarPersonal", ConexionBD.conexion);
                //insertamos datos
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Personal", parametros.Id_personal);

                //ejecutamos Query
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { ConexionBD.close(); }
        }
        public void MostrarPersonal(ref DataTable dt, int desde, int hasta)
        {
            try
            {   //abrimos conexion
                ConexionBD.abrir();
                //creamos DataAdapter que es para el mostardo de la tabla
                SqlDataAdapter da = new SqlDataAdapter("MostrarPersonal", ConexionBD.conexion);
                //Indicamos que es un procedure
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //Pasamos datos
                da.SelectCommand.Parameters.AddWithValue("@Desde", desde);
                da.SelectCommand.Parameters.AddWithValue("@Hasta", hasta);
                //Con los datos que traemos de la query rellenamos el DataTable "dt"
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally { ConexionBD.close(); }
        }
        public void BuscarPersonal(ref DataTable dt, string buscador)
        {
            try
            {   //abrimos conexion
                ConexionBD.abrir();
                //creamos DataAdapter que es para el mostardo de la tabla
                SqlDataAdapter da = new SqlDataAdapter("BuscarPersonal", ConexionBD.conexion);
                //Indicamos que es un procedure
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //Pasamos datos
                da.SelectCommand.Parameters.AddWithValue("@Buscador", buscador);
                //Con los datos que traemos de la query rellenamos el DataTable "dt"
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally { ConexionBD.close(); }
        }
        public bool Insertar_Asistencia(LAsistencia lAsistencia)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("InsertarAsistencia", ConexionBD.conexion))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Personal", lAsistencia.Id_Personal);
                    cmd.Parameters.AddWithValue("@Fecha_Entrada", lAsistencia.Fecha_Entrada);
                    cmd.Parameters.AddWithValue("@Horas", lAsistencia.Horas);
                    cmd.Parameters.AddWithValue("@ObservacionesEntrada", lAsistencia.Observaciones);
                    cmd.Parameters.Add("@Firma", SqlDbType.VarBinary).Value = lAsistencia.Firma;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Entrada registrada", "Entrada", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
                
            }
            finally
            {
                ConexionBD.close();
            }
        }
        public static Lpersonal BuscarIdPersonal(string identificacion)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("BuscarIdPersonal", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Identificacion",identificacion);
                    Lpersonal lpersonal = new Lpersonal();
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        //primero hay que añadirlos a una variable
                        int Id_personal = sqlDataReader.GetInt32(0);
                        string Nombre = sqlDataReader.GetString(1);
                        string Estado = sqlDataReader.GetString(2);
                        lpersonal.Id_personal = Id_personal;
                        lpersonal.Nombre = Nombre;
                        lpersonal.Estado = Estado;
                        return lpersonal;
                    }
                    else
                    {
                        MessageBox.Show("No hay resultados");
                        return lpersonal;
                    }
             
                }
            }
            catch (Exception e)
            {
                
                MessageBox.Show(e.Message);
                return null;
            }
            finally {   ConexionBD.close();  }

        }
        public void UpdateFecha_Hora(int Id_Personal, string observacionesSalida)
        {
            try
            {

                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("UpdateFecha_Hora", ConexionBD.conexion))

                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Personal", Id_Personal);
                    cmd.Parameters.AddWithValue("@ObservacionesSalida", observacionesSalida);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Salida registrada", "Salida", MessageBoxButton.OK, MessageBoxImage.Information);
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
        public void InsertarImagenEnBD(byte[] datosImagen, int Id_Personal)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand comando = new SqlCommand($"INSERT INTO Imagenes  VALUES (@Id_Personal,@DatosImagen)", ConexionBD.conexion))
                {
                    comando.Parameters.AddWithValue("@Id_Personal", Id_Personal);
                    comando.Parameters.Add("@DatosImagen", SqlDbType.VarBinary).Value = datosImagen;

                    comando.ExecuteNonQuery();
                    MessageBox.Show("Imagen subida correctamente a la base de datos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al subir la imagen");
            }
            finally
            {
                ConexionBD.close();
            }
        }
        public void UpdateImagen(byte[] datosImagen, int Id_Personal)
        {
            try
            {
                //abrimos conexión
                ConexionBD.abrir();
                //Creamos el comando
                using (SqlCommand comando = new SqlCommand($"UPDATE Imagenes SET imagen = @DatosImagen WHERE Id_Personal = @Id_Personal ", ConexionBD.conexion))
                {
                    //Añadimos los datos a la query
                    comando.Parameters.AddWithValue("@Id_Personal", Id_Personal);
                    comando.Parameters.AddWithValue("@DatosImagen", SqlDbType.VarBinary).Value = datosImagen;
                    //Ejecutamos comando
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Imagen actualizada correctamente a la base de datos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la imagen: " + ex.Message);
            }
            finally
            {
                ConexionBD.close();
            }
        }
        public void mostrarTodasAsistencias(ref DataTable dt)
        {
            try
            {
                ConexionBD.abrir();
                using(SqlDataAdapter comando = new SqlDataAdapter("mostrarTodasAsistencias",ConexionBD.conexion))
                {
                    comando.SelectCommand.CommandType = CommandType.StoredProcedure;
                    comando.Fill(dt);
                    
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
        public void BuscarPersonalYAsistencia(ref DataTable dt, string buscador)
        {
            try
            {   //abrimos conexion
                ConexionBD.abrir();
                //creamos DataAdapter que es para el mostardo de la tabla
                SqlDataAdapter da = new SqlDataAdapter("BuscarPersonalYAsistencia", ConexionBD.conexion);
                //Indicamos que es un procedure
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //Pasamos datos
                da.SelectCommand.Parameters.AddWithValue("@Buscador", buscador);
                //Con los datos que traemos de la query rellenamos el DataTable "dt"
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally { ConexionBD.close(); }
        }
    }
}

