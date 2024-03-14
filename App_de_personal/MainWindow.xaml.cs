using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using App_de_personal.DB;
using App_de_personal.Logica;
using App_de_personal.Windows;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;

namespace App_de_personal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] imagenByte;

        string conexion;
        string nameDb;
        LEmpresa lEmpresa;
        public Lpersonal idCurrentUser = new Lpersonal();
        public MainWindow()
        {

            InitializeComponent();
        }

        private void btnPersonal_Click(object sender, RoutedEventArgs e)
        {
            
            PrePlantilla Plantilla = new PrePlantilla(lEmpresa);
            
            PanelPresentacion.Children.Clear();
            
            Plantilla.btnMostrarTodos.Visibility = Visibility.Hidden;
            PanelPresentacion.Children.Add(Plantilla);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            RegistroEntradaSalida registroEntradaSalida = new RegistroEntradaSalida(lEmpresa.Nombre);
            registroEntradaSalida.ShowDialog();
        }
   

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            LoginUser loginUser = new LoginUser(this);
            PanelPresentacion.Children.Clear();
            PanelPresentacion.Children.Add(loginUser);       
        }

        private void btnAgregarPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    //añadimos el filtro de los archivos validos
                    openFileDialog.Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.gif";
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //sacamos el path del archivo seleccionado 
                        string rutaImagen = openFileDialog.FileName;
                        //convetimos la imagen a un array de bytes
                        imagenByte = File.ReadAllBytes(rutaImagen);
                    }
                    Dpersonal dpersonal = new Dpersonal();
                    System.Windows.MessageBox.Show(idCurrentUser.ToString());
                    dpersonal.InsertarImagenEnBD(imagenByte, idCurrentUser.Id_personal);
                    DUser dUser = new DUser();
                    dUser.MostrarImagenDesdeBD(idCurrentUser.Id_personal, this);
                }
            }
            catch (Exception ex) 
            {
            
            }

        }

        private void btnUpdatePhoto_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //añadimos el filtro de los archivos validos
                openFileDialog.Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //sacamos el path del archivo seleccionado 
                    string rutaImagen = openFileDialog.FileName;
                    //convetimos la imagen a un array de bytes
                    imagenByte = File.ReadAllBytes(rutaImagen);
                }
                Dpersonal dpersonal = new Dpersonal();

                dpersonal.UpdateImagen(imagenByte, idCurrentUser.Id_personal);
                DUser dUser = new DUser();
                dUser.MostrarImagenDesdeBD(idCurrentUser.Id_personal, this);
            }
        }

        private void MinWin_Loaded(object sender, RoutedEventArgs e)
        {
            
            btnAgregarPhoto.Visibility = Visibility.Hidden;
            btnUpdatePhoto.Visibility = Visibility.Hidden;
            btnRegistros.Visibility = Visibility.Hidden;
            btnExportarDb.Visibility = Visibility.Hidden;
            btnTicket.Visibility = Visibility.Hidden;
            btnRespaldos.Visibility = Visibility.Hidden;
            imgEstacion.Visibility = Visibility.Hidden;
            imgPersonal.Visibility = Visibility.Hidden;
            imgTicket.Visibility = Visibility.Hidden;
            imgRespaldos.Visibility = Visibility.Hidden;
            btnRestaurarDb.Visibility = Visibility.Hidden;
            imgRestauracion.Visibility = Visibility.Hidden;
            imgRegistro.Visibility = Visibility.Hidden;
            btnPersonal.Visibility = Visibility.Hidden;
            btnRegalos.Visibility = Visibility.Hidden;
            imgRegalos.Visibility = Visibility.Hidden;
            DEmpresa dEmpresa = new DEmpresa();
            if (comprobarConexion() == false)
            {
                btnRestaurarDb.Visibility = Visibility.Visible;
                imgRestauracion.Visibility = Visibility.Visible;
                ConexionBaseDeDatos conexionBaseDeDatos = new ConexionBaseDeDatos();
                conexionBaseDeDatos.ShowDialog();
                
                lEmpresa = dEmpresa.BuscarEmpresa();
                
            }
            else
            {
                lEmpresa = dEmpresa.BuscarEmpresa();
                Title = lEmpresa.Nombre;
            }
        }
        public bool comprobarConexion()
        {
            try
            {
                return ConexionBD.abrir();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                ConexionBD.close();
            }
        }

        private void btnRespaldos_Click(object sender, RoutedEventArgs e)
        {
            //Para recibir las fechas en un formato legible la conversion la hago en el procedure con un conver()
            /*
            alter proc mostrarTodasAsistencias
            as
            SELECT p.Nombre,p.Identificacion,a.Id_Personal, CONVERT(varchar, a.Fecha_Entrada, 120) AS Fecha_Entrada,
                    CONVERT(varchar, a.Fecha_Salida, 120) AS Fecha_Salida,a.Observaciones from Personal as p
            INNER JOIN Asistencia as a on a.Id_Personal = p.Id_Personal;
             */
            DataTable dt = new DataTable();
            Dpersonal dpersonal = new Dpersonal();
            dpersonal.mostrarTodasAsistencias(ref dt);
            ExportToExcel(dt);
        }
        public static void ExportToExcel(DataTable dataTable)
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
                             
                            if(columnIndex == 7)
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
        private void btnExportarDb_Click(object sender, RoutedEventArgs e)
        {
            exportarDb();
        }
        private void btnRestaurarDb_Click(object sender, RoutedEventArgs e)
        {
            RestaurarDataBase restaurarDataBase = new RestaurarDataBase();
            restaurarDataBase.ShowDialog();
        }
        private void exportarDb()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos de copia de seguridad (*.bak)|*.bak";
            saveFileDialog.Title = "Exportar base de datos";
            saveFileDialog.ShowDialog();
            string path = saveFileDialog.FileName;

            try 
            {
                ConexionBD.abrir();
                nameDb = extraerNombreDb();

                using (SqlCommand cmd = new SqlCommand($"BACKUP DATABASE {nameDb} TO DISK = '{path}';", ConexionBD.conexion))
                {
                    cmd.ExecuteNonQuery();
                    System.Windows.MessageBox.Show("Base de Datos exportada.");
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
        
        public  string extraerNombreDb()
        {
            conexion = ConexionBD.LeerConexion();
            int index = conexion.IndexOf("Initial Catalog=");
            // Si se encuentra "Initial Catalog="
            if (index >= 0)
            {
                // Avanzar al índice después del texto "Initial Catalog="
                index += "Initial Catalog=".Length;

                // Buscar el siguiente punto y coma (;)
                int endIndex = conexion.IndexOf(";", index);

                // Si se encuentra un punto y coma (;)
                if (endIndex >= 0)
                {
                    // Extraer el nombre de la base de datos
                    string databaseName = conexion.Substring(index, endIndex - index);

                    // Imprimir el nombre de la base de datos
                    System.Windows.MessageBox.Show("Nombre de la base de datos: " + databaseName);
                    return databaseName;
                }
                else
                {
                    System.Windows.MessageBox.Show("Formato de cadena incorrecto: no se encontró el punto y coma (;) después de 'Initial Catalog='");
                    return "";
                }
            }
            else
            {

                System.Windows.MessageBox.Show("Formato de cadena incorrecto: no se encontró 'Initial Catalog='");
                return "";
            }
        }

        private void btnRegalos_Click(object sender, RoutedEventArgs e)
        {
            Regalos regalos = new Regalos();
            PanelPresentacion.Children.Clear();
            PanelPresentacion.Children.Add(regalos);
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            TicketPendientes ticket = new TicketPendientes(lEmpresa, idCurrentUser.Id_personal);
            PanelPresentacion.Children.Clear();
            PanelPresentacion.Children.Add(ticket);
        }
        
    }
    
}