using System;
using System.Collections.Generic;
using System.Linq;
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
using App_de_personal.Logica;
using App_de_personal.DB;
using System.Data;
using System.Windows.Forms;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.ExceptionServices;
using Microsoft.Data.SqlClient;
using System.IO;

namespace App_de_personal.Windows
{
    /// <summary>
    /// Lógica de interacción para PrePlantilla.xaml
    /// </summary>
    public partial class PrePlantilla
    {
        int Id_cargo = 0;
        int desde = 0;
        int hasta = 10;
        int contador;
        int pag = 0;
        byte[] imagenByte;
        int Idpersonal;
        private int item_por_pagina = 10;
        string Estado;
        int totalPaginas;
        bool mostrado = false;

        LEmpresa LEmpresa;
        public PrePlantilla(LEmpresa lEmpresa)
        {
            this.LEmpresa = lEmpresa;
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Mostrar_Personal();
            lblNumPagina.Content = (ConteoPaginas() / 10).ToString();

        }
        private void btnAgregarUsuario_Click(object sender, RoutedEventArgs e)
        {
            GridCargos.Visibility = Visibility.Hidden;
            PanelPaginado.Visibility = Visibility.Visible;
            PanelRegistros.Visibility = Visibility.Visible;
            btnGuardarPersonal.Visibility = Visibility.Visible;
            panelDatos.Height =Double.NaN;
            btnMostrarTodos.Visibility = Visibility.Visible;
            txtBuscador.Visibility = Visibility.Hidden;
            
            if (contador == 0)
            {
                PanelRegistros.Children.RemoveAt(1);
                contador++;
            }

            limpiar();
        }
        private void limpiar()
        {
            txtNombre.Clear();
            txtIdentificacion.Clear();
            txtCargo.Clear();
            txtSueldoPorHora.Clear();
            txtNameUser.Clear();
            txtPass.Clear();
            BuscarCargos();
        }

        private void btnGuardarPersonal_Click(object sender, RoutedEventArgs e)
        {
            /* Este eventio se encarga de al pulsar el boton insertar en base de datos el personal que se a escrito
               en los campos */
            //Instanciamos la clase Lpersonal para acceder a sus propiedades
            Lpersonal parametro = new Lpersonal();
            // Instanciamos la clase Dpersonal para acceder a sus funciones
            Dpersonal funciones =   new Dpersonal();
            parametro.Nombre = txtNombre.Text;
            parametro.Identificacion = txtIdentificacion.Text;
            parametro.Pais = boxPaises.Text;
            parametro.Id_cargo = Id_cargo;
            parametro.SueldoPoHora = Convert.ToDouble(txtSueldoPorHora.Text);
            
            
            //Validamos que ningun campo sea vacio
            if(!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtIdentificacion.Text) 
            && !string.IsNullOrEmpty(boxPaises.Text) && Id_cargo>0 && !string.IsNullOrEmpty(txtSueldoPorHora.Text) 
            && !string.IsNullOrEmpty(txtNameUser.Text)&& !string.IsNullOrEmpty(txtPass.Text))
            {
                if (funciones.InsertarPersonal(parametro,LEmpresa.Id_Empresa))
                {
                    
                    var lPersonal = Dpersonal.BuscarIdPersonal(txtIdentificacion.Text);
                    DUser dUser = new DUser();
                    string pass = dUser.CreateHash(txtPass.Text);
                    
                    if(dUser.InsertarUser((int)lPersonal.Id_personal, txtNameUser.Text, pass))
                    {
                        if (!imagenByte.IsNullOrEmpty())
                        {
                            Dpersonal dpersonal = new Dpersonal();
                            dpersonal.InsertarImagenEnBD(imagenByte, lPersonal.Id_personal);
                        }
                        mostrado = true;                   
                        System.Windows.MessageBox.Show("Personal registrado correctamente", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
            }

        }
        private void Mostrar_Personal(int desde = 0, int hasta = 10)
        {
            /*Mostramos el personal de 10 en 10*/
            DataTable dt = new DataTable();
            Dpersonal funcion = new Dpersonal();
            funcion.MostrarPersonal(ref dt, desde, hasta);
            ListadoPersonal listadoPersonal = new ListadoPersonal();
            listadoPersonal.dataListadoPersonal.ItemsSource = dt.DefaultView;           
            panelDatos.Height = 0;
            txtBuscador.Visibility = Visibility.Visible;
            PanelRegistros.Children.Insert(PanelRegistros.Children.IndexOf(PanelPaginado), listadoPersonal);
            
        }

        private void btnGuardarAgregarCargo_Click(object sender, RoutedEventArgs e)
        {
            //Esto comprueba si es un string vacio o si es un null en caso de ser no entra
            
            if (!string.IsNullOrEmpty(txtAgregarCargo.Text))//comprobamos el campo de cargo
            {
                if (!string.IsNullOrEmpty(txtAgregarSueldo.Text))//comprobamos el campo de SueldoPorHora
                {
                    Lcargos parametro = new Lcargos();
                    Dcargos funcion = new Dcargos();
                    parametro.Cargo = txtAgregarCargo.Text;
                    parametro.SueldoPorHora = Convert.ToDouble(txtAgregarSueldo.Text);
                    if (funcion.Insertar_Cargos(parametro) == true)
                    {
                        BuscarCargos();
                        GridCargos.Visibility = Visibility.Hidden;
                        
                    }
                    
                }
                else
                {
                    System.Windows.MessageBox.Show("Agrege el Sueldo");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Agrege el cargo");
            }
            

        }
        private void BuscarCargos()
        {
            //instanciamos un datatble
            DataTable dt = new DataTable();
            //intanciamos la clase Dcargos para traer sus funciones
            Dcargos funcion = new Dcargos();
            //usamos la funcion buscar_cargo en el que le pasamos la tabla creada y el parametro por el que vamos a realizar
            //la busqueda 
            funcion.Buscar_Cargo(ref dt, txtCargo.Text);
            //insertamos los datos en el dataGrid llamado DataListadocargo
            DataListadoCargos.ItemsSource = dt.DefaultView;
            // Esta funcion creada en la clase Base  hace que el width de las columnas se ajuste automaticamente respecto al texto
            Bases.DiseñoDtv(ref DataListadoCargos);
            
        }

        private void txtCargo_TextChanged(object sender, TextChangedEventArgs e)
        {
            //cuando escribimos en el campo de cargos la tabla de cargo se modifica sola
            //esto realiza una busqueda cadavez que metemos un caracter
            BuscarCargos();
        }

        private void btnAgregarCargo_Click(object sender, RoutedEventArgs e)
        {
            GridCargos.Visibility = Visibility.Visible;
            btnGuardarCambiosCargo.Visibility = Visibility.Hidden;
            btnGuardarAgregarCargo.Visibility = Visibility.Visible;
            txtAgregarCargo.Clear();
            txtAgregarSueldo.Clear();
        }

        private void txtAgregarSueldo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //esta función valida cada caracter que introducimos y solo deja escribir numeros y si es un punto lo sustituye
            //por una coma
            Bases.Decimales(txtAgregarSueldo,e);
        }

        private void DataListadoCargos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Este evento se ejecuta cada vez que sleccionamos una fila del datagrid al que pertenece el evento

            DataRowView rowView = (DataRowView)DataListadoCargos.SelectedItem;
            try
            {
                int id_cargo = (int)rowView["Id_cargo"];
                string cargo = (string)rowView["Cargo"];
                decimal sueldo = (decimal)rowView["SueldoPorHora"];
                Id_cargo = id_cargo;
                if (GridCargos.Visibility == Visibility)
                {
                    txtAgregarCargo.Text = cargo;
                    txtAgregarSueldo.Text = Convert.ToString(sueldo);
                    
                    txtCargo.Clear();
                    txtSueldoPorHora.Clear();
                }
                else
                {
                    txtCargo.Text = cargo;
                    txtSueldoPorHora.Text = Convert.ToString(sueldo);
                }                                            
            }
            catch(Exception ex)
            {
                
            }


        }

        private void btnEditarCargo1_Click(object sender, RoutedEventArgs e)
        {
                GridCargos.Visibility=Visibility.Visible;
                btnGuardarCambiosCargo.Visibility=Visibility.Visible;
                btnGuardarAgregarCargo.Visibility=Visibility.Hidden;
        }

        private void btnGuardarCambiosCargo_Click(object sender, RoutedEventArgs e)
        {
            Lcargos propiedades = new Lcargos();
            Dcargos funciones = new Dcargos();
            propiedades.Id_cargo = Id_cargo;
            propiedades.Cargo = txtAgregarCargo.Text;
            propiedades.SueldoPorHora = Convert.ToDouble(txtAgregarSueldo.Text);
            funciones.Update_Cargo(propiedades);
            BuscarCargos();
            
        }

        private void txtAgregarCargo_TextChanged(object sender, TextChangedEventArgs e)
        {
            BuscarCargos();
        }



        private void btnCloseCargos_Click(object sender, RoutedEventArgs e)
        {
            GridCargos.Visibility = Visibility.Hidden;

        }

        private void btnMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            MostrarTodos();
            mostrado = true;
            //lo volvemos a 0 para que lo vuelva a borrar de la pantalla de registros
            contador = 0;
        }
        private void MostrarTodos()
        {
            //Si el indice de PanelPaginado es 1 es que ya esta creado el Datagrid y si es 0 pues creamos uno
            if(PanelRegistros.Children.IndexOf(PanelPaginado) <= 1)
            {
                Mostrar_Personal();
                panelDatos.Height = 0;
            }
            else
            {
                panelDatos.Height = 0;
            }

        }
        

        private void BuscarPersona()
        {

            if (txtBuscador.Text != "")
            {
                PanelRegistros.Children.RemoveAt(1);
                DataTable dt = new DataTable();
                Dpersonal funcion = new Dpersonal();
                funcion.BuscarPersonal(ref dt, txtBuscador.Text);
                ListadoPersonal listadoPersonal = new ListadoPersonal();
                listadoPersonal.dataListadoPersonal.ItemsSource = dt.DefaultView;
                PanelRegistros.Children.Insert(PanelRegistros.Children.IndexOf(PanelPaginado), listadoPersonal);
            }

        }

        private void txtBuscador_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            BuscarPersona();
        }

        private void btnPaginaAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (pag > 0 )
            {
                if (PanelRegistros.Children.IndexOf(PanelPaginado) <= 2 )
                {
                    PanelRegistros.Children.RemoveAt(1);
                    panelDatos.Height = 0;
                    desde -= 10;
                    hasta -= 10;
                    Mostrar_Personal(desde, hasta);
                    pag -= 1;
                    lblContadorPagina.Content = pag;
                }
            }
             
        }

        private void btnPaginaSigiente_Click(object sender, RoutedEventArgs e)
        {
            
            if (pag != Convert.ToInt32(lblNumPagina.Content))
            {
                if (PanelRegistros.Children.IndexOf(PanelPaginado) <= 2)
                {
                    PanelRegistros.Children.RemoveAt(1);
                    panelDatos.Height = 0;
                    desde += 10;
                    hasta += 10;
                    Mostrar_Personal(desde, hasta);
                    pag += 1;
                    lblContadorPagina.Content = pag;
                }
            }
            

            
        }
        private int ConteoPaginas()
        {
            int count = 0;
            try
            {
                string contenido = ConexionBD.LeerConexion();
                using (SqlConnection connection = new SqlConnection(contenido))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Select count(Nombre) from Personal;", connection);
                    count = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
                return count;
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.Message);
                return 0;
            }
            
    
        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //añadimos el filtro de los archivos validos
                openFileDialog.Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //sacamos el path del archivo seleccionado 
                    string rutaImagen = openFileDialog.FileName;
                    //convetimos la imagen a un array de bytes
                    imagenByte = File.ReadAllBytes(rutaImagen);
                    
                }
            }
        }

    }
}
