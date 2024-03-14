using App_de_personal.Logica;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace App_de_personal.DB
{
    public class Dcargos
    {
        public bool Insertar_Cargos(Lcargos parametros)
        {
            try
            {   //abrimos conexion
                ConexionBD.abrir();
                //creamos el comando
                SqlCommand cmd = new SqlCommand("Insertar_Cargo", ConexionBD.conexion);
                //insertamos datos
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cargo", parametros.Cargo);
                cmd.Parameters.AddWithValue("@SueldoPorHora", parametros.SueldoPorHora);
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
        public bool Update_Cargo(Lcargos parametros)
        {
            try
            {   //Abrimos conexion
                ConexionBD.abrir();
                //Creamos el comando
                SqlCommand cmd = new SqlCommand("Update_Cargo", ConexionBD.conexion);
                //Indicamos que el comandoa utilizar va a ser un procedure
                cmd.CommandType = CommandType.StoredProcedure;
                //Insertamos datos
                cmd.Parameters.AddWithValue("@Id_cargo", parametros.Id_cargo);
                cmd.Parameters.AddWithValue("@Cargo", parametros.Cargo);
                cmd.Parameters.AddWithValue("@SueldoPorHora", parametros.SueldoPorHora);

                //Ejecutamos Query
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cargo Actualizado Correctamente","Correcto",MessageBoxButton.OK,MessageBoxImage.Information);
                return true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { ConexionBD.close(); }
        }
        public void Buscar_Cargo(ref DataTable dt, string buscador)
        {
            try
            {   //abrimos conexion
                ConexionBD.abrir();
                //creamos DataAdapter que es para el mostardo de la tabla
                SqlDataAdapter da = new SqlDataAdapter("Buscar_Cargos", ConexionBD.conexion);
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
        public void Update_Cargo_personal(int Id_Personal, int Id_Cargo, double SueldoPorHora)
        {
            try 
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("ActualizarCargo", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_cargo", Id_Cargo);
                    cmd.Parameters.AddWithValue("@Id_Personal", Id_Personal);
                    cmd.Parameters.AddWithValue("@SueldoPorHora", SueldoPorHora);
                    cmd.ExecuteNonQuery();
                } 
            }
            catch 
            {
                MessageBox.Show("Cargo actualizado");
            }
            finally
            {
                ConexionBD.close() ;
            }
        }
        public void ExportarCargos(DataTable dt)
        {
            try
            {
                ConexionBD.abrir() ;
                using(SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM  Cargos",ConexionBD.conexion))
                {
                    dataAdapter.SelectCommand.CommandType = CommandType.Text;
                    dataAdapter.Fill(dt);
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
            finally 
            {
                ConexionBD.close() ;
            } 
            

        }
    }
}
