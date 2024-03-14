using App_de_personal.Logica;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_de_personal.DB
{
    class DEmpresa
    {
        public LEmpresa BuscarEmpresa()
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand($"Select top 1 * from Empresa;", ConexionBD.conexion))
                {
                    LEmpresa empresa = new LEmpresa();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        empresa.Id_Empresa = dataReader.GetInt32(0);
                        empresa.Cif = dataReader.GetString(1);
                        empresa.Nombre = dataReader.GetString(2);
                        empresa.Direccion = dataReader.GetString(3);
                        empresa.Telefono = dataReader.GetInt32(4);
                        empresa.Email = dataReader.GetString(5);

                        return empresa;
                    }
                    else
                    {
                        return empresa;
                    }
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                ConexionBD.close();
            }
        }
    }
}
