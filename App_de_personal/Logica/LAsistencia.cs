using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_de_personal.Logica
{
    public class LAsistencia
    {
        public int Id_Asistencia { get; set; }
        public int Id_Personal { get; set; }
        public DateTime Fecha_Entrada { get; set; }
        public DateTime Fecha_Salida { get; set; }
        public int Horas { get; set; }
        public string Observaciones { get; set; }
        public byte[] Firma {  get; set; }  
    }       
}
