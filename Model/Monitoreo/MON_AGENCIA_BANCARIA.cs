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
    
    public partial class MON_AGENCIA_BANCARIA : Entity<int>
    {
        public MON_AGENCIA_BANCARIA()
        {
            this.MON_ROS_OPERACION = new HashSet<MON_ROS_OPERACION>();
        }
        public int ID_FINANCIERA { get; set; }
        public string DESCRIPCION { get; set; }


        public virtual MON_CAT_FINANCIERA MON_CAT_FINANCIERA { get; set; }
        public virtual ICollection<MON_ROS_OPERACION> MON_ROS_OPERACION { get; set; }
    }
}
