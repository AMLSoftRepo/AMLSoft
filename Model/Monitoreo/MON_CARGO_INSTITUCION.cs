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
    
    public partial class MON_CARGO_INSTITUCION:Entity<int>
    {
        public MON_CARGO_INSTITUCION()
        {
            this.MON_CONTACTO_INSTITUCION = new HashSet<MON_CONTACTO_INSTITUCION>();
        }
    
        
        public int ID_INSTITUCION { get; set; }
        public string NOMBRE { get; set; }
    
        public virtual MON_CAT_INSTITUCION MON_CAT_INSTITUCION { get; set; }
        public virtual ICollection<MON_CONTACTO_INSTITUCION> MON_CONTACTO_INSTITUCION { get; set; }
    }
}
