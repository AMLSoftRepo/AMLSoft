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
    
    public partial class ALE_NOTIFICACION_ALERTA: Entity<int>
    {
        public int ID_TIPO_ALERTA { get; set; }
        public int ID_CONTACTO { get; set; }
    
        public virtual ALE_CONTACTO_ALERTA ALE_CONTACTO_ALERTA { get; set; }
        public virtual ALE_TIPO_ALERTA ALE_TIPO_ALERTA { get; set; }
    }
}
