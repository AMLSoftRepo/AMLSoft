using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Principal;

namespace View.Controllers
{
    /// <summary>
    /// Permite validar si el usuario actual tiene permiso para ?
    /// Ejecutar una determinada accion dentro del sistema
    /// </summary>
    public class Autorizacion : AuthorizeAttribute
    {
        /// <summary>
        /// Metodo de autorizacion
        /// </summary>
        /// <param name="httpContext">Contexto actual</param>
        /// <returns>true o false</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            if (httpContext.Session["MyCodeUser"] != null)
              authorize = true;

           return authorize;
        }


        /// <summary>
        /// En caso que el metodo AuthorizeCore retorne false redirecciona al LOGIN
        /// </summary>
        /// <param name="filterContext">Contexto actual</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Login");
        }

    }
}