using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using TextBox = System.Windows.Controls.TextBox;
using App_de_personal.Windows;

namespace App_de_personal.Logica
{
    public class Bases
    {
        public static void DiseñoDtv(ref DataGrid dataGrid)
        {
            // Por ejemplo, si estás utilizando un DataGrid llamado dataGrid
            foreach (var column in dataGrid.Columns)
            {
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                
            }

        }
        static int contador {  get; set; }
        public static void Decimales(object sender, TextCompositionEventArgs e)
        {
            
            TextBox textBox = sender as TextBox;
            try
            {
                //si el caracter actual es punto o coma entro
                if (e.Text == "." || e.Text == ",")
                {
                    //si el texbox contiene punto o coma no le dejo escribir
                    if (textBox.Text.Contains(",") || textBox.Text.Contains("."))
                    {
                        e.Handled = true;
                    }
                }
                else if (char.IsLetter(e.Text, 0))
                {
                    e.Handled = true;
                }
                else if (true)
                {
                    try//lo meto en un try por que el primer caracter me va a dar error al no estar dentro de los indices
                    {   //si el caracter anterior es un punto lo remuevo y le pongo una coma
                        if (textBox.Text[textBox.CaretIndex - 1] == '.')
                        {
                            //eliminamos el ultimo caracter
                            textBox.Text = textBox.Text.Remove(textBox.CaretIndex - 1);
                            //agregamos una coma
                            textBox.Text += ",";
                            //ponemos el cursor al final del text
                            textBox.CaretIndex = textBox.Text.Length;
                        }
                    }
                    catch
                    {

                    }
                    finally { contador++; }
                }
            }
            catch
            {
                MessageBox.Show("Los putnos no son validos");
            }
            
        }
    }
}
