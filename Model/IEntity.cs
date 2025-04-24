namespace Model
{

    /// <summary>
    /// Interface utilizada para definir el Id como atributo numerico 
    /// autoincremental.
    /// </summary>
    public interface IEntity<T>
    {
        /// <summary>
        /// Identificador Unico de cada instancia de clase. Propiedad Generica 
        /// para facilitar a todas las clases la obtención de una llave 
        /// primaria ya sea numerica o cadena de caracteres.
        /// </summary>
        T ID { get; set; }

    }
}
