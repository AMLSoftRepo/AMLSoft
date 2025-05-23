//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class MON_OFICIO:Entity<long>
    {
        public MON_OFICIO()
        {
            this.MON_OFICIO_PERSONA = new HashSet<MON_OFICIO_PERSONA>();
        }

        public int ID_CONTACTO_SOLICITANTE { get; set; }
        public int ID_CONTACTO_INSTITUCION { get; set; }
        public int ID_CONTACTO_INSTITUCION_OFICIAL { get; set; }
        public string NUMERO_OFICIO { get; set; }
        public string REFERENCIA { get; set; }
        public string ORIGEN { get; set; }
        public System.DateTime FECHA_OFICIO { get; set; }
        public System.DateTime FECHA_RECIBIDO { get; set; }
        public string FORMATO_RESPUESTA { get; set; }
        public System.DateTime FECHA_RESPUESTA_UIF { get; set; }
        public Nullable<System.DateTime> FECHA_RECIBIDO_UIF { get; set; }
        public int DIAS_HABILES { get; set; }
        public int DIAS_MAX { get; set; }
        public System.DateTime FECHA_MAXIMA_UIF { get; set; }
        public int CUMPLIMIENTO { get; set; }
        public bool PROCESADO { get; set; }
        public Nullable<System.DateTime> FECHA_INVESTIGACION_INI { get; set; }
        public Nullable<System.DateTime> FECHA_INVESTIGACION_FIN { get; set; }
        public string COMENTARIO { get; set; }

        public virtual MON_CONTACTO_INSTITUCION MON_CONTACTO_INSTITUCION { get; set; }
        public virtual MON_CONTACTO_INSTITUCION MON_CONTACTO_INSTITUCION1 { get; set; }
        public virtual MON_CONTACTO_INSTITUCION MON_CONTACTO_INSTITUCION2 { get; set; }
        public virtual ICollection<MON_OFICIO_PERSONA> MON_OFICIO_PERSONA { get; set; }
        public object ID_INSTITUCION { get; set; }
    }
}
