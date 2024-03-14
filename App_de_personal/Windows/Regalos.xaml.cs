using App_de_personal.DB;
using App_de_personal.Logica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace App_de_personal.Windows
{
    /// <summary>
    /// Lógica de interacción para Regalos.xaml
    /// </summary>
    public partial class Regalos : UserControl
    {
        LRegalos lRegalos = new LRegalos();
        public Regalos()
        {
            InitializeComponent();
        }
        public static string btnActivo;
        private void txtBuscadorRegalos_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            LRegalos lRegalos = new LRegalos();
            DRegalos dRegalos = new DRegalos();
            DataTable dt = new DataTable();
            lRegalos.Descripcion = txtBuscadorRegalos.Text;
            dRegalos.MostrarRegalo(dt,lRegalos.Descripcion);
            dataListadoArticulos.ItemsSource = dt.DefaultView;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblNombreArticulo.Visibility = Visibility.Hidden;
            lblCodigo.Visibility = Visibility.Hidden;
            lblCantidad.Visibility = Visibility.Hidden;
            txtNombreArticulo.Visibility = Visibility.Hidden;
            txtCodigo.Visibility = Visibility.Hidden;
            txtCantidad.Visibility = Visibility.Hidden;
            DRegalos dRegalos = new DRegalos();
            DataTable dt = new DataTable();
            dRegalos.MostrarTodosRegalos(dt);
            dataListadoArticulos.ItemsSource = dt.DefaultView;
        }

        private void btnMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            DRegalos dRegalos = new DRegalos();
            dRegalos.MostrarTodosRegalos(dt);
            dataListadoArticulos.ItemsSource = dt.DefaultView;
        }

        private void btnMasUno_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                DataRowView dataArticulos = (DataRowView)dataListadoArticulos.SelectedItem;
                DRegalos dRegalos = new DRegalos();
                dRegalos.RegaloMasUno((string)dataArticulos["Descripcion"]);
                DataTable dt = new DataTable();
                dRegalos.MostrarRegalo(dt, (string)dataArticulos["Descripcion"]);
                dataListadoArticulos.ItemsSource = dt.DefaultView;
            }
            catch 
            {
                MessageBox.Show("Seleciona un articulo de la lista");
            }


        }

        private void btnMenosUno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataArticulos = (DataRowView)dataListadoArticulos.SelectedItem;
                DRegalos dRegalos = new DRegalos();
                dRegalos.RegaloMenosUno((string)dataArticulos["Descripcion"]);
                DataTable dt = new DataTable();
                dRegalos.MostrarRegalo(dt, (string)dataArticulos["Descripcion"]);
                dataListadoArticulos.ItemsSource = dt.DefaultView;
            }
            catch 
            {
                MessageBox.Show("Seleciona un articulo de la lista");
            }


        }

        private void btnInsertarRegalo_Click(object sender, RoutedEventArgs e)
        {
            lblNombreArticulo.Visibility = Visibility.Visible;
            lblCodigo.Visibility = Visibility.Visible;
            lblCantidad.Visibility = Visibility.Visible;
            txtNombreArticulo.Visibility = Visibility.Visible;
            txtCodigo.Visibility = Visibility.Visible;
            txtCantidad.Visibility = Visibility.Visible;
            btnInsertarRegalo.Background = Brushes.Green;
            btnModificarRegalo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF333333"));
            btnBorrarRegalo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF333333"));
            btnActivo = "Insertar";
            limpiar();
        }

        private void btnModificarRegalo_Click_1(object sender, RoutedEventArgs e)
        {
            lblNombreArticulo.Visibility = Visibility.Visible;
            lblCodigo.Visibility = Visibility.Visible;
            lblCantidad.Visibility = Visibility.Visible;
            txtNombreArticulo.Visibility = Visibility.Visible;
            txtCodigo.Visibility = Visibility.Visible;
            txtCantidad.Visibility = Visibility.Visible;
            btnInsertarRegalo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF333333"));
            btnModificarRegalo.Background = Brushes.Green;
            btnBorrarRegalo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF333333"));
            btnActivo = "Modificar";
            txtNombreArticulo.Text = lRegalos.Descripcion;
            txtCodigo.Text = Convert.ToString(lRegalos.CodigoArticulo);
            txtCantidad.Text = Convert.ToString(lRegalos.Cantidad);
        }
        private void btnBorrarRegalo_Click(object sender, RoutedEventArgs e)
        {
            lblNombreArticulo.Visibility = Visibility.Hidden;
            lblCodigo.Visibility = Visibility.Visible;
            lblCantidad.Visibility = Visibility.Hidden;
            txtNombreArticulo.Visibility = Visibility.Hidden;
            txtCodigo.Visibility = Visibility.Visible;
            txtCantidad.Visibility = Visibility.Hidden;
            btnInsertarRegalo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF333333"));
            btnModificarRegalo.Background =new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF333333"));
            btnBorrarRegalo.Background = Brushes.Green;
            btnActivo = "Eliminar";
        }

        private void btnGuardarArticulo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblNombreArticulo.Visibility = Visibility.Hidden;
                lblCodigo.Visibility = Visibility.Hidden;
                lblCantidad.Visibility = Visibility.Hidden;
                txtNombreArticulo.Visibility = Visibility.Hidden;
                txtCodigo.Visibility = Visibility.Hidden;
                txtCantidad.Visibility = Visibility.Hidden;
                DRegalos dRegalos = new DRegalos();
                string nombre = txtNombreArticulo.Text;
                int codigo = Convert.ToInt32(txtCodigo.Text);
                int cantidad = Convert.ToInt32(txtCantidad.Text);
                DataTable dt = new DataTable();

                if (btnActivo == "Insertar")
                {
                    
                    if (!string.IsNullOrEmpty(txtNombreArticulo.Text) && !string.IsNullOrEmpty(txtCodigo.Text) && !string.IsNullOrEmpty(txtCantidad.Text))
                    {
                        dRegalos.InsertarRegalo(nombre, codigo, cantidad);
                        dRegalos.MostrarTodosRegalos(dt);
                        dataListadoArticulos.ItemsSource = dt.DefaultView;
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("Deves de rellenar todos los campos");
                    }
                }
                else if (btnActivo == "Modificar")
                {

                    if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(txtCodigo.Text) && !string.IsNullOrEmpty(txtCantidad.Text))
                    {
                        
                        dRegalos.ModificarRegalo(lRegalos.Id_Regalo,nombre, codigo, cantidad);
                        dRegalos.MostrarTodosRegalos(dt);
                        dataListadoArticulos.ItemsSource = dt.DefaultView;
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("Deves de rellenar todos los campos");
                    }
                }
                else if (btnActivo == "Eliminar")
                {

                    if (!string.IsNullOrEmpty(txtCodigo.Text))
                    {
                        string nombreArticulo = dRegalos.BuscarRegaloCodigo(Convert.ToInt32(txtCodigo.Text));
                        MessageBoxResult result = MessageBox.Show($"Estas seguro que quieres eliminar  el articulo {nombreArticulo} ", "Eliminar Articulo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            dRegalos.EliminarRegalo(codigo);
                            dRegalos.MostrarTodosRegalos(dt);
                            dataListadoArticulos.ItemsSource = dt.DefaultView;
                            limpiar();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Codigo invalido");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Algun dato es erroneo");
            }

        }

        private void dataListadoArticulos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = (DataRowView)dataListadoArticulos.SelectedItem;
                if (dataRowView != null)
                {
                    lRegalos.Id_Regalo = (int)dataRowView["Id_Regalo"];
                    txtNombreArticulo.Text = lRegalos.Descripcion = (string)dataRowView["Descripcion"];
                    txtCodigo.Text = (lRegalos.CodigoArticulo = (int)dataRowView["CodigoArticulo"]).ToString();
                    txtCantidad.Text =(lRegalos.Cantidad = (int)dataRowView["Cantidad"]).ToString();
                }
            }
            catch (Exception)
            {
            }



        }
        private void limpiar()
        {
            txtNombreArticulo.Text = null;
            txtCantidad.Text = null;
            txtCodigo.Text = null;
        }


    }
}

