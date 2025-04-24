using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Blo.Seguridad;
using Dao.Seguridad;

namespace UnitTests.Blo
{

    [TestClass]
    public class TestAccesoUsuarioBlo
    {
        /// <summary>
        /// Propiedad que referencia e objeto de acceso a datos.
        /// </summary>
        private IAccesoUsuarioBlo _accesoUsuarioBlo;

        /// <summary>
        /// Iniciar los objetos necesarios para la realización de las pruebas
        /// </summary>
        [TestInitialize]
        public void UnitTestInitialize()
        {
            _accesoUsuarioBlo = new AccesoUsuarioBlo(new AccesoUsuarioDao());
        }

        /// <summary>
        /// Metodo para probar obtener un elemento por su ID
        /// </summary>
        [TestMethod]
        public void GetById()
        {
            var l = _accesoUsuarioBlo.GetById(4);
        }

        /// <summary>
        /// Metodo para probar Insertar un elemento 
        /// </summary>
        [TestMethod]
        public void Save()
        {
            //SEG_ACCESO_USUARIO neww = new SEG_ACCESO_USUARIO();

            //neww.ID_MODULO = 1;
            //neww.ID_OPCION = 5;
            //neww.ID_ROL = 1;
            //neww.ACCESO = true;

            //_accesoUsuarioBlo.Save(neww);
        }

        /// <summary>
        /// Metodo para probar la actualizacion de un elemento
        /// </summary>
        [TestMethod]
        public void Update()
        {
            //SEG_ACCESO_USUARIO update = new SEG_ACCESO_USUARIO();

            //update = _accesoUsuarioBlo.GetById(14);

            //update.ACCESO = true;

            //_accesoUsuarioBlo.Save(update);
        }

        /// <summary>
        /// Metodo para probar la eliminacion de un elemento
        /// </summary>
        [TestMethod]
        public void Delete()
        {
            //_accesoUsuarioBlo.Remove(14);
        }

    }
}
