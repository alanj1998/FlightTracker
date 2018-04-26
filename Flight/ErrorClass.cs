using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;

namespace FlightTracker
{
    /// <summary>
    /// Error Class used to show error messages.
    /// Created to show web errors
    /// </summary>
    class ErrorClass
    {
        /// <summary>
        /// Method used to show the error message
        /// </summary>
        /// <typeparam name="TypeOfError"></typeparam>
        /// <param name="error"></param>
        public void InvokeErrorMessage<TypeOfError>(TypeOfError error)
        {      
            string errorTitle = Application.Current.Resources["errorTitle"].ToString(),
                   errorText = "";
                        
            if(typeof(TypeOfError) == typeof(HttpStatusCode?))
            {
                errorText = GetWebError(error as HttpStatusCode?);
            }


            MessageBox.Show(errorText, errorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Method used to sort out the message related to web errors
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private string GetWebError(HttpStatusCode? error)
        {
            string message = "";

            switch(error.Value)
            {
                case HttpStatusCode.ProxyAuthenticationRequired:
                    message += Application.Current.Resources["proxyError"];
                    break;
                case HttpStatusCode.RequestTimeout:
                    message += Application.Current.Resources["timeoutError"];
                    break;
                case HttpStatusCode.BadRequest:
                    message += Application.Current.Resources["requestError"];
                    break;
                case HttpStatusCode.Conflict:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.Unauthorized:
                    message += Application.Current.Resources["serverError"];
                    break;
                default:
                    message += Application.Current.Resources["genericError"];
                    break;
            }

            return message;
        }
    }
}
