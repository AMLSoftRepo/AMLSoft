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

    public partial class MAT_CAT_TIPO_CONTROL : Entity<int>
    {
        public MAT_CAT_TIPO_CONTROL()
        {
            this.MAT_CONTROL = new HashSet<MAT_CONTROL>();
        }
    
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<MAT_CONTROL> MAT_CONTROL { get; set; }
    }
}
