using App_de_personal.DB;
using App_de_personal.Logica;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
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

namespace App_de_personal.Windows
{
    /// <summary>
    /// Lógica de interacción para LoginUser.xaml
    /// </summary>
    public partial class LoginUser 
    {
        public bool registro = false;
        private MainWindow mainWindow;
        public int Id_Personal { get; set; }
        public LoginUser(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
            InitializeComponent();
            lblNombre.Content = mainWindow.Title;
        }

        private void btnEntrarUser_Click(object sender, RoutedEventArgs e)
        {
            bool showImage;
            DUser dUser = new DUser();
            
            string pass = dUser.CreateHash(password.Password);
            var lUser = dUser.BuscarUser(txtNombre.Text,pass);
            Id_Personal = lUser.Id_Personal;
            mainWindow.idCurrentUser.Id_personal = Id_Personal;
            mainWindow.imgPhoto.Visibility = Visibility.Hidden;
            if (lUser.NameUser.IsNullOrEmpty()) 
            {
                MessageBox.Show("El ususario o la contraseña no son correctos, vuelve a intentarlo.");
            }
            else
            {
                var dataUser = dUser.BuscarDatosUsuarioPorId(lUser.Id_Personal);

                
                if (dataUser.Id_cargo == 1)
                {
                    if (dataUser.Estado == "ACTIVO")
                    {
                        mainWindow.btnRegistros.Visibility = Visibility.Visible;
                        mainWindow.btnExportarDb.Visibility = Visibility.Visible;
                        mainWindow.btnRestaurarDb.Visibility = Visibility.Visible;
                        mainWindow.btnRespaldos.Visibility = Visibility.Visible;
                        mainWindow.btnTicket.Visibility = Visibility.Visible;
                        mainWindow.imgEstacion.Visibility = Visibility.Visible;
                        mainWindow.imgRespaldos.Visibility = Visibility.Visible;
                        mainWindow.imgRestauracion.Visibility = Visibility.Visible;
                        mainWindow.imgRegistro.Visibility = Visibility.Visible;
                        mainWindow.txtNombreUser.Text = txtNombre.Text;
                        mainWindow.btnPersonal.Visibility = Visibility.Visible;
                        mainWindow.btnRegalos.Visibility = Visibility.Visible;
                        mainWindow.imgRegalos.Visibility = Visibility.Visible;
                        mainWindow.imgPersonal.Visibility = Visibility.Visible;
                        mainWindow.imgTicket.Visibility = Visibility.Visible;
                        // mainWindow.PanelPresentacion.Children.Cleraar();



                        DUser dUser1 = new DUser();
                        try
                        {
                            showImage = dUser1.MostrarImagenDesdeBD(lUser.Id_Personal, mainWindow);
                            if (showImage == true)
                            {
                                mainWindow.btnAgregarPhoto.Visibility = Visibility.Hidden;
                                mainWindow.btnUpdatePhoto.Visibility = Visibility.Visible;
                                mainWindow.imgPhoto.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                mainWindow.btnUpdatePhoto.Visibility = Visibility.Hidden;
                                mainWindow.btnAgregarPhoto.Visibility = Visibility.Visible;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        mainWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Este usuario esta Inactivo. Consulta con tu administrador");
                    }

                }
                else
                {
                    if(dataUser.Estado == "ACTIVO")
                    {
                        DUser dUser1 = new DUser();
                        try
                        {
                            showImage = dUser1.MostrarImagenDesdeBD(lUser.Id_Personal, mainWindow);
                            if (showImage == true)
                            {
                                mainWindow.btnAgregarPhoto.Visibility = Visibility.Hidden;
                                mainWindow.btnUpdatePhoto.Visibility = Visibility.Visible;
                                mainWindow.imgPhoto.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                mainWindow.btnUpdatePhoto.Visibility = Visibility.Hidden;
                                mainWindow.btnAgregarPhoto.Visibility = Visibility.Visible;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        mainWindow.txtNombreUser.Text = txtNombre.Text;
                        mainWindow.btnRegistros.Visibility = Visibility.Visible;
                        mainWindow.imgRegistro.Visibility = Visibility.Visible;
                        mainWindow.btnExportarDb.Visibility = Visibility.Hidden;
                        mainWindow.btnRestaurarDb.Visibility = Visibility.Hidden;
                        mainWindow.btnRespaldos.Visibility = Visibility.Hidden;
                        mainWindow.imgEstacion.Visibility = Visibility.Hidden;
                        mainWindow.imgRespaldos.Visibility = Visibility.Hidden;
                        mainWindow.imgRestauracion.Visibility = Visibility.Hidden;
                        mainWindow.btnRegalos.Visibility = Visibility.Visible;
                        mainWindow.imgRegalos.Visibility = Visibility.Visible;
                        mainWindow.btnPersonal.Visibility = Visibility.Hidden;
                        mainWindow.imgPersonal.Visibility = Visibility.Hidden;
                        mainWindow.imgTicket.Visibility = Visibility.Visible;
                        mainWindow.btnTicket.Visibility = Visibility.Visible;
                        mainWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Este usuario esta Inactivo. Consulta con tu administrador");
                    }
                }
                    
            }
        }

        public int CurrentUserId()
        {
            return Id_Personal;
        }
    }
}
