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
    
    public partial class PER_TIPO_CALIFICACION: Entity<int>
    {
        public string DESCRIPCION { get; set; }
        public decimal VALORMIN { get; set; }
        public decimal VALORMAX { get; set; }
        public string COLOR { get; set; }
    }
}
