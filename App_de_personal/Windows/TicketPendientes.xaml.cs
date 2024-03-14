using App_de_personal.DB;
using App_de_personal.Logica;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace App_de_personal.Windows
{
    /// <summary>
    /// Lógica de interacción para TicketPendientes.xaml
    /// </summary>
    public partial class TicketPendientes : UserControl
    {
        public DataTable dt = new DataTable();
        public DTickets dTickets;
        public LTickets lTickets;
        public LEmpresa lEmpresa;
        int Id_CurrentUser;
        public TicketPendientes(LEmpresa _lEmpresa, int IdCurrentUser)
        {
            lEmpresa = _lEmpresa;
            Id_CurrentUser = IdCurrentUser;
            InitializeComponent();
        }
        private void ticketsPendientes_Loaded(object sender, RoutedEventArgs e)
        {
            dt.Rows.Clear();
            dTickets = new DTickets();
            dTickets.MostrarTodosTicketsPendientes(dt);
            dataListadoTicket.ItemsSource = dt.DefaultView;
        }
        private void btnMostrarTodasTicket_Click(object sender, RoutedEventArgs e)
        {
            dt.Rows.Clear();
        
            dTickets = new DTickets();
            dTickets.MostrarTodosTickets(dt);
            dataListadoTicket.ItemsSource = dt.DefaultView;
        }

        private void btnAgregarTicket_Click(object sender, RoutedEventArgs e)
        {
            AgregarTicket agregarTicket = new AgregarTicket(lEmpresa,Id_CurrentUser);
            agregarTicket.btnGuardar.Visibility = Visibility.Visible;
            agregarTicket.ShowDialog();

        }

        private void dataListadoTicket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView =(DataRowView)dataListadoTicket.SelectedItem;
                int Id_Ticket = (int)dataRowView["Id_Ticket"];
                lTickets = dTickets.BuscarTicketPorId(Id_Ticket);
            }
            catch (Exception)
            {

            }

        }

        private void btnModificarTicket_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                AgregarTicket agregarTicket = new AgregarTicket(lEmpresa, Id_CurrentUser, lTickets, true);
                agregarTicket.inkFirma.Visibility = Visibility.Hidden;
                agregarTicket.lblFirma.Visibility = Visibility.Hidden;
                agregarTicket.btnGuardar.Visibility = Visibility.Visible;
                agregarTicket.txtNumeroOperacion.Text = lTickets.NumeroOperacion.ToString();
                agregarTicket.txtNombreCompleto.Text = lTickets.NombreDeudor;
                agregarTicket.txtIdentificacion.Text = lTickets.IdentificacionDeudor;
                agregarTicket.txtMatricula.Text = lTickets.Matricula;
                agregarTicket.txtTelefono.Text = lTickets.Telefono.ToString();
                agregarTicket.txtDireccion.Text = lTickets.DireccionDeudor;
                agregarTicket.txtxEmail.Text = lTickets.EmailDeudor;
                agregarTicket.txtCuantia.Text = lTickets.CuantiaDeuda.ToString();
                agregarTicket.comboBoxEstado.Text = lTickets.EstadoDeuda;
                agregarTicket.ShowDialog();
            }
            catch (Exception)
            {

                MessageBox.Show("Antes debes de seleccionar una fila de la tabla de tickets");

            }

        }

        private void btnExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();
            dTickets.MostrarTodosTicketsPlus(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                if (row[11] != DBNull.Value)
                {
                    DateTime fechaEntrada = (DateTime)row[11];
                    DateTime fechaSalida = (DateTime)row[12];

                    // Especifica el formato que deseas para la fecha y hora
                    string formatoDeseado = "dd/MM/yyyy HH:mm:ss";
                    row[11] = fechaEntrada.ToString(formatoDeseado);
                    row[12] = fechaSalida.ToString(formatoDeseado);

                    MessageBox.Show(row[11].ToString());
                    MessageBox.Show(row[12].ToString());
                }
            }
            DTickets.ExportToExcelTickets(dataTable);
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {


            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument document = CreateFlowDocument(); // Método para crear el contenido del documento
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Documento a imprimir");
            }
        }

        private void txtBuscadorTicket_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            dt.Rows.Clear();
            dTickets = new DTickets();
            dTickets.BuscarTicketPorNombre(dt,txtBuscadorTicket.Text);
            dataListadoTicket.ItemsSource = dt.DefaultView;
        }

        private void btnTickestPendientes_Click(object sender, RoutedEventArgs e)
        {
            dt.Rows.Clear();
            dTickets = new DTickets();
            dTickets.MostrarTodosTicketsPendientes(dt);
            dataListadoTicket.ItemsSource = dt.DefaultView;
        }

        private void btnTicketsPagados_Click(object sender, RoutedEventArgs e)
        {
            dt.Rows.Clear();
            dTickets = new DTickets();
            dTickets.MostrarTodosTicketsPagados(dt);
            dataListadoTicket.ItemsSource = dt.DefaultView;
        }

        private FlowDocument CreateFlowDocument()
        {// Crear un nuevo documento de flujo
            FlowDocument document = new FlowDocument();

            // Configurar estilos para el título y el contenido
            Style titleStyle = new Style(typeof(Paragraph));
            titleStyle.Setters.Add(new Setter(Paragraph.FontSizeProperty, 18.0));
            titleStyle.Setters.Add(new Setter(Paragraph.FontWeightProperty, FontWeights.Bold));
            titleStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0, 10, 0, 5)));

            Style contentStyle = new Style(typeof(Paragraph));
            contentStyle.Setters.Add(new Setter(Paragraph.FontSizeProperty, 12.0));
            contentStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0, 0, 0, 5)));

            // Crear un título para el documento
            Paragraph titleParagraph = new Paragraph(new Run("Detalles del Ticket"));
            titleParagraph.Style = titleStyle;
            document.Blocks.Add(titleParagraph);

            // Agregar cada propiedad del ticket al documento
            document.Blocks.Add(CreateParagraph("ID de Ticket:", lTickets.Id_Ticket.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("ID de Empresa:", lTickets.Id_Empresa.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("ID de Personal:", lTickets.Id_Personal.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("Número de Operación:", lTickets.NumeroOperacion.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("Nombre del Deudor:", lTickets.NombreDeudor, contentStyle));
            document.Blocks.Add(CreateParagraph("Identificación del Deudor:", lTickets.IdentificacionDeudor, contentStyle));
            document.Blocks.Add(CreateParagraph("Matrícula:", lTickets.Matricula, contentStyle));
            document.Blocks.Add(CreateParagraph("Teléfono:", lTickets.Telefono.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("Dirección del Deudor:", lTickets.DireccionDeudor, contentStyle));
            document.Blocks.Add(CreateParagraph("Email del Deudor:", lTickets.EmailDeudor, contentStyle));
            document.Blocks.Add(CreateParagraph("Cuantía de Deuda:", lTickets.CuantiaDeuda.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("Fecha de Deuda:", lTickets.FechaDeDeuda.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("Fecha de Pago:", lTickets.FechaDePago.ToString(), contentStyle));
            document.Blocks.Add(CreateParagraph("Estado de Deuda:", lTickets.EstadoDeuda, contentStyle));

            // Agregar la firma como una imagen si está presente
            if (lTickets.Firma != null && lTickets.Firma.Length > 0)
            {
                BitmapImage bitmap = ConvertToBitmapImage(lTickets.Firma);
                if (bitmap != null)
                {
                    Image image = new Image();
                    image.Source = bitmap;
                    image.Width = 200; // Ajustar el ancho de la imagen según sea necesario
                    image.Margin = new Thickness(0, 10, 0, 0);
                    BlockUIContainer imageContainer = new BlockUIContainer(image);
                    document.Blocks.Add(imageContainer);
                }
            }

            return document;
        }
        private Paragraph CreateParagraph(string label, string value, Style style)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Style = style;
            paragraph.Inlines.Add(new Run(label) { FontWeight = FontWeights.Bold });
            paragraph.Inlines.Add(new Run(" " + value));
            return paragraph;
        }
        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;
            // Convertir los bytes de la imagen en un objeto BitmapImage
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(imageData);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            return bitmap;
        }

        private void btnVerTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AgregarTicket agregarTicket = new AgregarTicket(lEmpresa, Id_CurrentUser, lTickets, true);
                agregarTicket.inkFirma.Visibility = Visibility.Visible;
                agregarTicket.lblFirma.Visibility = Visibility.Hidden;
                agregarTicket.txtNumeroOperacion.Text = lTickets.NumeroOperacion.ToString();
                agregarTicket.txtNombreCompleto.Text = lTickets.NombreDeudor;
                agregarTicket.txtIdentificacion.Text = lTickets.IdentificacionDeudor;
                agregarTicket.txtMatricula.Text = lTickets.Matricula;
                agregarTicket.txtTelefono.Text = lTickets.Telefono.ToString();
                agregarTicket.txtDireccion.Text = lTickets.DireccionDeudor;
                agregarTicket.txtxEmail.Text = lTickets.EmailDeudor;
                agregarTicket.txtCuantia.Text = lTickets.CuantiaDeuda.ToString();
                agregarTicket.comboBoxEstado.Text = lTickets.EstadoDeuda;
                agregarTicket.txtNotas.Text = lTickets.Observaciones;
                agregarTicket.btnGuardar.Visibility = Visibility.Hidden;

                // Agregar la firma como una imagen si está presente
                if (lTickets.Firma != null && lTickets.Firma.Length > 0)
                {
                    BitmapImage bitmap = ConvertToBitmapImage(lTickets.Firma);
                    // Crear un nuevo objeto ImageBrush usando el BitmapImage
                    ImageBrush imageBrush = new ImageBrush(bitmap);

                    // Asignar el ImageBrush como el fondo del InkCanvas
                    agregarTicket.inkFirma.Background = imageBrush;
                }

                agregarTicket.ShowDialog();
            }
            catch (Exception)
            {

                MessageBox.Show("Antes debes de seleccionar una fila de la tabla de tickets");

            }
        }
    }
}
