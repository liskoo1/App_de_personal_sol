using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using App_de_personal.Windows;
using System.IO;
using System.Windows;


namespace App_de_personal.DB
{
    public class ConexionBD 
    {
        //@"Data Source=name\SQLEXPRESS;Initial Catalog=SpaceJamp;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True";


        public static SqlConnection conexion;
        public static bool abrir()
        {
            try 
            {
                conexion = new SqlConnection(LeerConexion());
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                    
                }
                return true;
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
                return false;
            }

        }
        public static bool close()
        {
            try
            {
                conexion = new SqlConnection(LeerConexion());
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }
        public static void abrirMaster(string datasource)
        {
            try
            {
                conexion = new SqlConnection(@$"Data Source={datasource};Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True");
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public static void closeMaster(string datasource)
        {
            try
            {
               conexion = new SqlConnection(@$"Data Source={datasource};Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True");
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public static string LeerConexion()
        {
            try
            {
                //Pasamos la direccion donde se encuentra el archivo que contiene el path del servidor
                string path = "../../../Resource/ConexionDB.txt";
                //Leemos el achivo
                string contenido = File.ReadAllText(path);
                return contenido;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }

}