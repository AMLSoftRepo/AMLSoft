using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    /// <summary>
    /// Clase base para todas las entidades persistentes del sistema, esta
    /// proporciona los metodos comunes a estas.
    /// </summary>
    public abstract class BaseEntity
    {

        /// <summary>
        /// Obtiene un listado de objetos llave primaria, independientemente
        /// que sean llaves autoincrementales (long id) o llaves compuestas 
        /// por varios atributos numericos o de texto.
        /// </summary>
        public List<object> PrimaryKey
        {
            get
            {
                return (from property in this.GetType().GetProperties()
                        where Attribute.IsDefined(property, typeof(KeyAttribute))
                        orderby ((ColumnAttribute)property.GetCustomAttributes(false).Single(
                            attr => attr is ColumnAttribute)).Order ascending
                        select property.GetValue(this)).ToList();
            }
        }
    }
}
