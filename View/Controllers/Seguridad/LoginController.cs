using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Seguridad;
using System.Text;
using System.Web.Security;
using hbehr.AdAuthentication;
using System.Configuration;

namespace View.Controllers.Seguridad
{
    public class LoginController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IAccesoUsuarioBlo _accesoUsuarioBlo;
        private readonly IModuloBlo _moduloBlo;
        private readonly IRolUsuarioBlo _rolUsuarioBlo;
        private readonly IRolBlo _rolBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public LoginController(IAccesoUsuarioBlo accesoUsuarioBlo, IModuloBlo moduloBlo, IRolUsuarioBlo rolUsuarioBlo, IRolBlo rolBlo)
        {
            _accesoUsuarioBlo = accesoUsuarioBlo;
            _moduloBlo = moduloBlo;
            _rolUsuarioBlo = rolUsuarioBlo;
            _rolBlo = rolBlo;
        }


        [AllowAnonymous]
        public ActionResult Index()
        {
            String usuarioIdentity = null;
            ViewBag.Error = null;

            Session["MyCodeUser"] = "Global16";
            Session["MyCodePerfil"] = 1;
            Session["MyNombre"] = "Global16";
            Session["MyPerfil"] = "Administración";
            Session["AlertaNotificada"] = true;
            Session["AlertaAnalizada"] = true;
            Session["AlertaOficio"] = true;
            Session["AlertaPersonaLista"] = true;
            Session["AlertaPerfilTransaccional"] = true;

            LoadMenu(1);

            return RedirectToAction("Index", "Home");

        }


        [Autorizacion]
        private void LoadMenu(int perfil)
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder();
                var opciones = _accesoUsuarioBlo.GetOpcionesxPerfil(perfil);


                if (!opciones.Any())
                    throw new System.ArgumentException("No tiene opciones asignadas a su perfil");

                if (opciones.Any())
                {
                    mBuilder.Append(@"<ul class=""sidebar-menu"">");
                    mBuilder.Append(@"<li class=""header"">MENU PRINCIPAL</li>");

                    //modulos
                    foreach (var m in _moduloBlo.GetAll().OrderBy(x => x.ID))
                    {
                        mBuilder.Append(@"<li class=""treeview"">");
                        mBuilder.Append(@"<a href=""#"">");
                        mBuilder.Append(@"<i class=""fa fa-th""></i>");
                        mBuilder.Append(@"<span>" + m.DESCRIPCION + @"</span>");
                        mBuilder.Append(@"<i class=""fa fa-angle-left pull-right""></i>");
                        mBuilder.Append(@"</a>");
                        mBuilder.Append(@"<ul class=""treeview-menu"">");

                        //opciones x modulo
                        //1 nivel
                        foreach (var o in opciones.Where(x => x.ID_MODULO == m.ID).OrderBy(x=>x.DESCRIPCION))
                        {
                            if (o.ID_PADRE == -1) // -1 si es una opcion padre
                            {
                                mBuilder.Append(@"<li>");
                                mBuilder.Append(@"<li><a href="""
                                                + o.URL + @"""><i class=""fa fa-circle-o""></i>"
                                                + o.DESCRIPCION
                                                + @"<span class=""pull-right-container"">"
                                                + @" <i class=""fa fa-angle-left pull-right""></i>"
                                                + @"</span>"
                                                + @"</a>");

                                mBuilder.Append(@"<ul class=""treeview-menu"">");

                                //2 nivel
                                foreach (var opcionesHijas in opciones.Where(x => x.ID_PADRE == o.ID).OrderBy(x=>x.DESCRIPCION))
                                {
                                    mBuilder.Append(@"<li>");

                                    //Valida que es una opcion padre
                                    if (opcionesHijas.URL.Trim() == "#")
                                    {
                                        mBuilder.Append(@"<li><a href="""
                                                    + opcionesHijas.URL + @"""><i class=""fa fa-circle-o""></i>"
                                                    + opcionesHijas.DESCRIPCION
                                                    + @"<span class=""pull-right-container"">"
                                                    + @" <i class=""fa fa-angle-left pull-right""></i>"
                                                    + @"</span>"
                                                    + @"</a>");
                                    }
                                    else // no es padre
                                    {
                                        mBuilder.Append(@"<li><a href="""
                                                        + opcionesHijas.URL + @"""><i class=""fa fa-circle-o""></i>"
                                                        + opcionesHijas.DESCRIPCION
                                                        + @"</a></li>");
                                    }

                                    //3 nivel
                                    mBuilder.Append(@"<ul class=""treeview-menu"">");
                                    foreach (var opcionesHijas2 in opciones.Where(x => x.ID_PADRE == opcionesHijas.ID).OrderBy(x=>x.DESCRIPCION))
                                    {
                                        mBuilder.Append(@"<li><a href="""
                                                        + opcionesHijas2.URL + @"""><i class=""fa fa-circle-o""></i>"
                                                        + opcionesHijas2.DESCRIPCION
                                                        + @"</a></li>");
                                    }
                                    mBuilder.Append(@"</ul>");
                                    mBuilder.Append(@"</li>");
                                }

                                mBuilder.Append(@"</ul>");
                                mBuilder.Append(@"</li>");
                            }
                            if (o.ID_PADRE == 0) // es 0 cuando una opcion no es padre ni hijo
                            {
                                mBuilder.Append(@"<li><a href="""
                                                + o.URL + @"""><i class=""fa fa-circle-o""></i>"
                                                + o.DESCRIPCION
                                                + @"</a>");
                            }
                        }

                        mBuilder.Append(@"</ul>");
                        mBuilder.Append(@"</li>");
                    }

                    mBuilder.Append("</ul>");
                    Session["bootstrapMenu"] = mBuilder.ToString();
                }


            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException(ex.Message);
            }
        }


    }
}