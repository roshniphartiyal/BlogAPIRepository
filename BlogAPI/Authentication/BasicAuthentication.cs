using BlogDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;


namespace Authentication
{
    public class BasicAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization != null)
                {

                    var authToken = actionContext.Request.Headers.Authorization.Parameter;
                    var decoAuthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                    var UserNameAndPassword = decoAuthToken.Split(':');
                    if (IsAuthorizedUser(UserNameAndPassword[0], UserNameAndPassword[1]))
                    {
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserNameAndPassword[0]), null);
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        public static bool IsAuthorizedUser(string Username, string Password)
        {
            using (BLogDatabaseEntities context = new BLogDatabaseEntities())
            {
                var entity = context.UserTables.FirstOrDefault(x => x.LoginName == Username && x.Password == Password);
                if (entity == null)
                {
                    ErrorLog eo = new ErrorLog() {ErrorMessage="Invalid Username or Password. For Username - "+Username,Severity="high"};
                    context.ErrorLogs.Add(eo);
                    context.SaveChanges();
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
    }
}