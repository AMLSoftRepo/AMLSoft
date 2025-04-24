using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Model
{
    /// <summary>
    /// Clase base para las entidades que implementen llaves primarias 
    /// numericas autoincrementales, para tal fin implementa IEntity.
    /// </summary>
    /// <typeparam name="T">Parámetro generico para especificarle a las 
    /// entidades hijas el tipo de datos que sera aplicado a la llave 
    /// primaria "Id"</typeparam>
    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        /// <summary>
        /// Identificador Unico de cada instancia de clase. Propiedad Generica 
        /// para facilitar a todas las clases la obtención de una llave 
        /// primaria ya sea numerica o cadena de caracteres.
        /// </summary>
        [Key, Column(Order = 0)]
        [DataMember]
        public virtual T ID { get; set; }

    }
}
