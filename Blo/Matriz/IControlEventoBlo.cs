using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Matriz
{
    public interface IControlEventoBlo : IGenericBlo<MAT_CONTROL_EVENTO>
    {
        /// <summary>
        /// Metodo que permite guargar o asignar un control a un evento
        /// </summary>
        /// <param name="idEvento">Identificador único de EVENTO</param>
        /// <param name="idControl">Identificador único de CONTROL</param>
        void SaveControl(long idEvento, long idControl);


        /// <summary>
        /// Metodo que permite eliminar un control asociado a un evento
        /// </summary>
        /// <param name="idEvento">Identificador único de EVENTOS</param>
        /// <param name="idControl">Identificador único de CONTROLES</param>
        void EliminarControles(long idEvento, long idControl);


        /// <summary>
        /// Metodo que permite guardar un nuevo control
        /// y támbien agregarlo a un evento determinado
        /// </summary>
        /// <param name="idEvento">Identificador único de EVENTO</param>
        /// <param name="data">conjunto de datos del CONTROL</param>
        void GuargarNuevoControl(long idEvento, MAT_CONTROL data);

    }
}
