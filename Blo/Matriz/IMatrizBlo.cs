using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Matriz;

namespace Blo.Matriz
{
    public interface IMatrizBlo : IGenericBlo<MAT_MATRIZ>
    {
        /// <summary>
        /// Metodo que permite generar matriz por agencia, partiendo de los eventos y controles
        /// que se asignaron a dicho evento, finalmente este proceso deja limpia estas dos entidades
        /// para luego iniciar nuevamente el proceso de creación de la matriz de riesgo
        /// </summary>
        /// <param name="idAgencia">I</param>
        long GenerarMatriz();

    }
}
