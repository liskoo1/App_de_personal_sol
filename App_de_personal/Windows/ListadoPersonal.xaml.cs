using App_de_personal.DB;
using App_de_personal.Logica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
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
    /// Lógica de interacción para ListadoPersonal.xaml
    /// </summary>
    public partial class ListadoPersonal : UserControl
    {
        public ListadoPersonal()
        {
            InitializeComponent();
        }
        int desde = 0;
        int hasta = 10;
        public Lpersonal propiedades = new Lpersonal();
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            PersonalSeleccionado();
            if (propiedades.Id_personal != 0)
            {
                var result = MessageBox.Show("Estas seguro que quieres actualizar los datos?", "Actualización", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    
                    Dpersonal funcion = new Dpersonal();
                    funcion.UpdatePersonal(propiedades);
                    actualizarData();
                }
            }
            else
            {
                MessageBox.Show("Debes de seleccionar una fila de los empleados");
            }


        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dataListadoPersonal.SelectedItem != null)
                {

                    Dpersonal funciones = new Dpersonal();
                    DataRowView rowView = (DataRowView)dataListadoPersonal.SelectedItem;
                    propiedades.Id_personal = (int)rowView["Id_Personal"];
                    funciones.EliminarPersonal(propiedades);
                    actualizarData();
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Selecciona una fila");
            }

        }


        private void actualizarData()
        {
            //Volvemos a insertar los datos en el datagrid
            DataTable dt = new DataTable();
            Dpersonal funcion = new Dpersonal();
            funcion.MostrarPersonal(ref dt, desde, hasta);
            dataListadoPersonal.ItemsSource = dt.DefaultView;
        }


        private void MostrarCargo()
        {
            DataTable dt = new DataTable();
            Dcargos funcion = new Dcargos();
            string buscador = "";
            funcion.Buscar_Cargo(ref dt, buscador);
            dataListadoCargo.ItemsSource = dt.DefaultView;
        }
        private void btnActualizarCargo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataPersonal = (DataRowView)dataListadoPersonal.SelectedItem;
                DataRowView dataCargo = (DataRowView)dataListadoCargo.SelectedItem;
                Dcargos funcion = new Dcargos();
                int Id_Personal_P = (int)dataPersonal["Id_Personal"];
                string Nombre_P = (string)dataPersonal["Nombre"];
                string Cargo_P = (string)dataPersonal["Cargo"];
                int Id_cargo_C = (int)dataCargo["Id_cargo"];
                string Cargo_C = (string)dataCargo["Cargo"];
                double Sueldo = Convert.ToDouble(dataCargo["SueldoPorHora"]);
                var resul = MessageBox.Show($"Vas a actializar el cargo de {Nombre_P} de {Cargo_P} a {Cargo_C}", "Actializar", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                if (resul == MessageBoxResult.OK)
                {
                    funcion.Update_Cargo_personal(Id_Personal_P, Id_cargo_C, Sueldo);
                    actualizarData();
                }
            }
            catch
            {
                MessageBox.Show("Para cambiar el cargo de algun empleado debes de selecionar el empleado y despues el cargo nuevo");
            }


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarCargo();
        }

        private void PersonalSeleccionado()
        {
            try
            {

                DataRowView rowView = (DataRowView)dataListadoPersonal.SelectedItem;
                if (dataListadoPersonal.SelectedItem != null)
                {
                    propiedades.Id_personal = (int)rowView["Id_Personal"];
                    propiedades.Nombre = (string)rowView["Nombre"];
                    propiedades.Identificacion = (string)rowView["Identificacion"];
                    propiedades.Pais = (string)rowView["Pais"];
                    propiedades.Id_cargo = (int)rowView["Id_cargo"];
                    propiedades.SueldoPoHora = Convert.ToDouble(rowView["SueldoPorHora"]);
                    propiedades.Estado = rowView["Estado"].ToString().ToUpper();

                }

            }
            catch
            {
                MessageBox.Show("Selecciona una fila");

            }
        }

    }
}    