using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using App_de_personal.Logica;
namespace App_de_personal.DB
{
    internal class DRegalos
    {
        public int CodigoArticulo;
        public void MostrarRegalo(DataTable dt,string descripcion)
        {
            try
            {
                ConexionBD.abrir();
                if (int.TryParse(descripcion, out CodigoArticulo))
                {
                    using (SqlDataAdapter cmd = new SqlDataAdapter("MostrarArticuloCodigo", ConexionBD.conexion))
                    {
                        cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                        cmd.SelectCommand.Parameters.AddWithValue("@CodigoArticulo", CodigoArticulo);
                        cmd.Fill(dt);
                    }
                }
                else
                {
                    using (SqlDataAdapter cmd = new SqlDataAdapter("MostrarArticulo", ConexionBD.conexion))
                    {
                        cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                        cmd.SelectCommand.Parameters.AddWithValue("@Descripcion",descripcion);
                        cmd.Fill(dt);
                    }
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
        public void MostrarTodosRegalos(DataTable dt)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlDataAdapter cmd = new SqlDataAdapter("MostrarTodosArticulos", ConexionBD.conexion))
                {
                    cmd.Fill(dt);
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
        public void RegaloMasUno(string descripcion)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("ArticuloMasUno",ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.ExecuteNonQuery();
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
        public void RegaloMenosUno(string descripcion)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("ArticuloMenosUno", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.ExecuteNonQuery();
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
        public void InsertarRegalo(string descripcion, int codigo, int Cantidad)
        {
            try
            {
                ConexionBD.abrir();
                using(SqlCommand cmd = new SqlCommand("InsertarArticulo",ConexionBD.conexion))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Descripcion",descripcion);
                    cmd.Parameters.AddWithValue("@CodigoArticulo", codigo);
                    cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message );
            }
            finally
            {
                ConexionBD.close();
            }
        }
        public void ModificarRegalo(int Id_Regalo,string descripcion, int codigo, int Cantidad)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("ModificarArticulo", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Regalo", Id_Regalo);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@CodigoArticulo", codigo);
                    cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
                    cmd.ExecuteNonQuery();
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
        public void EliminarRegalo(int codigo)
        {

            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("EliminarArticulo", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoArticulo", codigo);
                    cmd.ExecuteNonQuery();
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
        public string BuscarRegaloCodigo(int codigo)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("BuscarRegaloCodigo", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoArticulo",codigo);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {   
                        reader.Read();
                        
                        return reader.GetString(1);
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return "";
            }
            finally
            {
                ConexionBD.close();
            }
        }
    }
}
