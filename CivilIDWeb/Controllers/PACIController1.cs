using CivilIDWeb.DB.APPContext;
using CivilIDWeb.Models;
using CivilIDWeb.PACIReponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIDAuthSignContract.Entities;
using PACI.MobileId.IntegrationServices.Client;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CivilIDWeb.Controllers
{
    public class PACIController1 : Controller
    {
        public static ConcurrentDictionary<string, TaskCompletionSource<PACICallbackResponse>> requestTcses = new ConcurrentDictionary<string, TaskCompletionSource<PACICallbackResponse>>();
        private IConfiguration configuration;
        private PACIDBContext context;

        private ILogger<PACIController1> Logger { get; }

        public PACIController1(ILogger<PACIController1> logger, IConfiguration config, PACIDBContext pACIDBContext)
        {
            Logger = logger;
            configuration = config;
            context = pACIDBContext;
        }

        // GET: HomeController1
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/auth/getperson/callback")]
        public IActionResult PaciCallback([FromBody] PACICallbackResponse callbackResponse)
        {
            Logger.LogDebug("called auth/getperson/callback");
            var requestId = callbackResponse.MIDAuthSignResponse.RequestDetails.RequestID;
            var tcs = requestTcses[requestId];
            bool isRemoved = requestTcses.Remove(requestId, out _);
            tcs.TrySetResult(callbackResponse);

            // find and remove completed tasks - intermittent tasks
            //logic starts
            //tcs.Task.Creat

            return Ok();
        }

        [HttpGet]
        [Route("/getperson/{civilno}")]
        public async Task<IActionResult> GetPersonDetails(string civilno)
        {
            var check = context.PacirequestLogs.ToList();

            var certThumbprint = configuration.GetValue<string>("certThumbprint");
            var paciServiceHostName = configuration.GetValue<string>("paciServiceHostName");
            var paciServicePort = configuration.GetValue<string>("paciServicePort");

            var authRequest = configuration.GetSection("AuthenticateRequest").Get<AuthenticateRequest>();

            var callbackUrl = "https://" + Request.Host + "auth/getperson/callback";

#pragma warning disable SYSLIB0026 // Type or member is obsolete
            X509Certificate2 cert = new X509Certificate2();
#pragma warning restore SYSLIB0026 // Type or member is obsolete

            //X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            //x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            //var certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, false);
            //if (certificate2Collection == null || certificate2Collection.Count == 0 ||
            //certificate2Collection.Count > 1)
            //{
            //    throw new Exception("Certificate not found");
            //}
            //cert = certificate2Collection[0];
            //var authClient = new MIDAuthServiceClient(cert, paciServiceHostName, paciServicePort);

            //var resPACI = authClient.InitiateAuthRequestPN(
            //    new AuthenticateRequest
            //    {
            //        ServiceProviderId = authRequest.ServiceProviderId,
            //        ServiceDescriptionEN = authRequest.ServiceDescriptionEN,
            //        ServiceDescriptionAR = authRequest.ServiceDescriptionAR,
            //        AuthenticationReasonEn = authRequest.AuthenticationReasonEn,
            //        AuthenticationReasonAr = authRequest.AuthenticationReasonAr,
            //        RequestUserDetails = true,
            //        SPCallbackURL = callbackUrl,
            //        PersonCivilNo = civilno
            //    });

            var tcs = new TaskCompletionSource<PACICallbackResponse>();
            
            requestTcses.TryAdd(/*resPACI.Data*/"7a80c36f-95d1-4406-889e-13adc9fecf61", tcs);

            return Ok(await tcs.Task);
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
