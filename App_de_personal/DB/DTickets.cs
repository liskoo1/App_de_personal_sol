using App_de_personal.Logica;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace App_de_personal.DB
{
    public class DTickets
    {
        public void MostrarTodosTickets(DataTable dt)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlDataAdapter cmd = new SqlDataAdapter("MostrarTodosTickets", ConexionBD.conexion))
                {
                    cmd.Fill(dt);
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.close();
            }

        }
        public void MostrarTodosTicketsPendientes(DataTable dt)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlDataAdapter cmd = new SqlDataAdapter("MostrarTodosTicketsPendientes", ConexionBD.conexion))
                {
                    cmd.Fill(dt);
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.close();
            }

        }
        public void MostrarTodosTicketsPagados(DataTable dt)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlDataAdapter cmd = new SqlDataAdapter("MostrarTodosTicketsPagados", ConexionBD.conexion))
                {
                    cmd.Fill(dt);
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.close();
            }

        }
        public void BuscarTicketPorNombre(DataTable dt, string NombreDeudor)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlDataAdapter cmd = new SqlDataAdapter("BuscarTicketPorNombre", ConexionBD.conexion))
                {
                    cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                    cmd.SelectCommand.Parameters.AddWithValue("@NombreDeudor", NombreDeudor);

                    cmd.Fill(dt);
                }
            }
            catch (Exception ex)
            {

                
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.close();
            }

        }
        public LTickets BuscarTicketPorId(int idTicket)

        {
            LTickets lTickets = new LTickets();
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("BuscarTicketId", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Ticket", idTicket);
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    lTickets.Id_Ticket = reader.GetInt32(0);
                    lTickets.Id_Empresa = reader.GetInt32(1);
                    lTickets.Id_Personal = reader.GetInt32(2);
                    lTickets.NumeroOperacion = reader.GetInt32(3);
                    lTickets.NombreDeudor = reader.GetString(4);
                    lTickets.IdentificacionDeudor = reader.GetString(5);
                    lTickets.Matricula = reader.GetString(6);
                    lTickets.Telefono = reader.GetInt32(7);
                    lTickets.DireccionDeudor = reader.GetString(8);
                    lTickets.EmailDeudor = reader.GetString(9);
                    lTickets.CuantiaDeuda = Convert.ToDouble(reader.GetDecimal(10));
                    lTickets.FechaDeDeuda = reader.GetDateTime(11);
                    lTickets.FechaDePago = reader.GetDateTime(12);
                    lTickets.EstadoDeuda = reader.GetString(13);



                    // Obtener la longitud de los datos de imagen 
                    long longitudBytes = reader.GetBytes(14, 0, null, 0, 0);
                    // Le damos la longitud al array
                    lTickets.Firma = new byte[longitudBytes];
                    // Obtener los datos de la imagen y almacenarlos en el array
                    reader.GetBytes(14, 0, lTickets.Firma, 0, (int)longitudBytes);
                    lTickets.Observaciones = reader.GetString(15);
                    return lTickets;
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
                return lTickets;
            }
            finally
            {
                ConexionBD.close();
            }

        }
        public void InsertarTicket(int Id_Empresa, int Id_Personal, int NumeroOperacion, string Nombre, string Identificador, string Matricula, int Telefono, string Direccion, string Email,
            double cuantia, string Estado, byte[] firma, string Observaciones)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("InsertarTicket", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Empresa", Id_Empresa);
                    cmd.Parameters.AddWithValue("@Id_Personal", Id_Personal);
                    cmd.Parameters.AddWithValue("@NumeroOperacion", NumeroOperacion);
                    cmd.Parameters.AddWithValue("@NombreDeudor", Nombre);
                    cmd.Parameters.AddWithValue("@IdentificacionDeudor", Identificador);
                    cmd.Parameters.AddWithValue("@Matricula", Matricula);
                    cmd.Parameters.AddWithValue("@Telefono", Telefono);
                    cmd.Parameters.AddWithValue("@DireccionDeudor", Direccion);
                    cmd.Parameters.AddWithValue("@EmailDeudor", Email);
                    cmd.Parameters.AddWithValue("@CuantiaDeuda", cuantia);
                    cmd.Parameters.AddWithValue("@FechaDeDeuda", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EstadoDeuda", Estado);
                    cmd.Parameters.AddWithValue("@Firma", SqlDbType.VarBinary).Value = firma;
                    cmd.Parameters.AddWithValue("@Observaciones", Observaciones);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);

            }
            finally
            {
                ConexionBD.close();
            }
        }
        public void ModificarTicket(int Id_Ticket, int NumeroOperacion, string Nombre, string Identificador, string Matricula, int Telefono, string Direccion, string Email,
            double cuantia, string Observaciones)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("ModificarTicket", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Ticket", Id_Ticket);
                    cmd.Parameters.AddWithValue("@NumeroOperacion", NumeroOperacion);
                    cmd.Parameters.AddWithValue("@NombreDeudor", Nombre);
                    cmd.Parameters.AddWithValue("@IdentificacionDeudor", Identificador);
                    cmd.Parameters.AddWithValue("@Matricula", Matricula);
                    cmd.Parameters.AddWithValue("@Telefono", Telefono);
                    cmd.Parameters.AddWithValue("@DireccionDeudor", Direccion);
                    cmd.Parameters.AddWithValue("@EmailDeudor", Email);
                    cmd.Parameters.AddWithValue("@CuantiaDeuda", cuantia);
                    cmd.Parameters.AddWithValue("@Observaciones", Observaciones);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                System.Windows.MessageBox.Show("Algun dato no es valido para su campo");
            }

        }

        internal void ModificarTicketAPagado(int Id_Ticket, int NumeroOperacion, string Nombre, string Identificador, string Matricula, int Telefono, string Direccion, string Email,
            double cuantia, string estado, string Observaciones)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlCommand cmd = new SqlCommand("ModificarTicketAPagado", ConexionBD.conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Ticket", Id_Ticket);
                    cmd.Parameters.AddWithValue("@NumeroOperacion", NumeroOperacion);
                    cmd.Parameters.AddWithValue("@NombreDeudor", Nombre);
                    cmd.Parameters.AddWithValue("@IdentificacionDeudor", Identificador);
                    cmd.Parameters.AddWithValue("@Matricula", Matricula);
                    cmd.Parameters.AddWithValue("@Telefono", Telefono);
                    cmd.Parameters.AddWithValue("@DireccionDeudor", Direccion);
                    cmd.Parameters.AddWithValue("@EmailDeudor", Email);
                    cmd.Parameters.AddWithValue("@CuantiaDeuda", cuantia);
                    cmd.Parameters.AddWithValue("@FechaDePago", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EstadoDeuda", estado);
                    cmd.Parameters.AddWithValue("@Observaciones", Observaciones);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                System.Windows.MessageBox.Show("Algun dato no es valido para su campo");
            }
        }

        public static void ExportToExcelTickets(DataTable dataTable)
        {
            //Instanciamos la clase
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //añadimos los filtros  que solo sean de extension .xlsx
            saveFileDialog.Filter = "Archivos de Excel (*.xlsx)|*.xlsx";
            //titulo de la ventana
            saveFileDialog.Title = "Guardar como";
            //Abrimos la pantalla para elegir la ruta donde se va a guardar el archivo
            saveFileDialog.ShowDialog();
            //ponemos la licencia que obligan a ponerla
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // si no han metido nombre no entra
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {   // capturamos el path que nos genera
                string filePath = saveFileDialog.FileName;
                // Crear un nuevo archivo de Excel utilizando la biblioteca EPPlus
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregar una nueva hoja al archivo de Excel
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    // Escribir encabezados de columna en la primera fila del archivo Excel
                    for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                    {
                        // Obtener el nombre de la columna del DataTable y escribirlo en la celda correspondiente
                        worksheet.Cells[1, columnIndex + 1].Value = dataTable.Columns[columnIndex].ColumnName;
                    }

                    // Escribir datos a partir de la segunda fila del archivo Excel
                    for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                    {
                        for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                        {
                            // Obtener el valor de cada celda del DataTable y escribirlo en el archivo de Excel
                            if (columnIndex == 11 || columnIndex == 12)
                            {
                                if (dataTable.Rows[rowIndex][columnIndex] != DBNull.Value)
                                {
                                    // Convertir el valor a DateTime
                                    DateTime fecha = (DateTime)dataTable.Rows[rowIndex][columnIndex];

                                    // Asignar el valor a la celda como DateTime
                                    worksheet.Cells[rowIndex + 2, columnIndex + 1].Value = fecha;

                                    // Definir el formato de la celda para fechas
                                    worksheet.Cells[rowIndex + 2, columnIndex + 1].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                                }
                            }
                            else if (columnIndex == 14)
                            {
                                if (dataTable.Rows[rowIndex][columnIndex] != DBNull.Value)
                                {
                                    byte[] firma = dataTable.Rows[rowIndex][columnIndex] as byte[];
                                    string firmaHexadecimal = BitConverter.ToString(firma);
                                    
                                    worksheet.Cells[rowIndex + 2, columnIndex + 1].Value = firmaHexadecimal;
                                }
                            }
                            else
                            {
                                worksheet.Cells[rowIndex + 2, columnIndex + 1].Value = dataTable.Rows[rowIndex][columnIndex];
                            }
                        }
                    }

                    // Guardar el archivo de Excel en el disco con la ruta especificada
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);
                }

            }
        }
        public void MostrarTodosTicketsPlus(DataTable dt)
        {
            try
            {
                ConexionBD.abrir();
                using (SqlDataAdapter cmd = new SqlDataAdapter("MostrarTodosTicketsPlus", ConexionBD.conexion))
                {
                    cmd.Fill(dt);
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.close();
            }
        }
    }
}
