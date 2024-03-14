using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_de_personal.Logica
{
    public class LTickets
    {
       public int Id_Ticket {  get; set; }
      public int Id_Empresa {  get; set; }
        public int Id_Personal { get; set; }
        public int NumeroOperacion {  get; set; }
      public string NombreDeudor { get; set; }
      public string IdentificacionDeudor { get; set; }
      public string Matricula {  get; set; }
      public int Telefono { get; set; }
      public string DireccionDeudor { get; set; }
      public string EmailDeudor {  get; set; }
      public double CuantiaDeuda {  get; set; }
      public DateTime FechaDeDeuda {  get; set; }
      public DateTime FechaDePago {  get; set; }
      public string EstadoDeuda {  get; set; }
      public byte[] Firma { get; set; }
       public string Observaciones { get; set; }
    }
}