using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Internal.HttpClient
{
    #region

    using System;
    using System.Net;

    #endregion

    /// <summary>
    ///     The call ret.
    /// </summary>
    internal class CallResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallResponse"/> class.
        /// </summary>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public CallResponse(HttpStatusCode statusCode, string response)
        {
            this.StatusCode = statusCode;
            this.Response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallResponse"/> class.
        /// </summary>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        public CallResponse(HttpStatusCode statusCode, Exception e)
        {
            this.StatusCode = statusCode;
            this.Exception = e;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallResponse"/> class.
        /// </summary>
        /// <param name="ret">
        /// The ret.
        /// </param>
        public CallResponse(CallResponse ret)
        {
            this.StatusCode = ret.StatusCode;
            this.Exception = ret.Exception;
            this.Response = ret.Response;
        }

        /// <summary>
        ///     Gets or sets the status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; protected set; }

        /// <summary>
        ///     Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; protected set; }

        /// <summary>
        ///     Gets or sets the response.
        /// </summary>
        public string Response { get; protected set; }

        /// <summary>
        ///     Gets a value indicating whether ok.
        /// </summary>
        public bool OK
        {
            get
            {
                return (int)this.StatusCode / 100 == 2;
            }
        }
    }
}
