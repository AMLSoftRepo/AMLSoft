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

    public partial class MAT_CAT_UNIDAD : Entity<int>
    {
        public MAT_CAT_UNIDAD()
        {
            this.MAT_EVENTO_RIESGO = new HashSet<MAT_EVENTO_RIESGO>();
         
        }


        public int ID_AGENCIA { get; set; }
        public string DESCRIPCION { get; set; }

        public virtual MAT_CAT_AGENCIA MAT_CAT_AGENCIA { get; set; }
        public virtual ICollection<MAT_EVENTO_RIESGO> MAT_EVENTO_RIESGO { get; set; }
        
    }
}
