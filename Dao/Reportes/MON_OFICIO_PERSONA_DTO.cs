using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitoreo
{
    class MON_OFICIO_PERSONA_DTO
    {
        public long ID_OFICIO { get; set; }
        public string NOMBRE { get; set; }
        public string GENERALES { get; set; }
        public string DUI { get; set; }
        public string NIT { get; set; }
        public Nullable<long> TIPO_DOCUMENTO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string TIPO_PERSONA { get; set; }
        public string RESULTADO { get; set; }
        public bool COTIZACIONES { get; set; }
        public bool SOLICITUDES { get; set; }
        public bool PRESTAMOS { get; set; }
        public bool CHEQUES { get; set; }
        public bool AEX { get; set; }

        public string INSTITUCIONES { get; set; }

        public virtual MON_OFICIO MON_OFICIO { get; set; }

    }
}
