using Audit_management_portal.Models;
using Authorization_service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Audit_management_portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(HomeController));

        public  AuditRequest REQ=new AuditRequest();

        private readonly DataBaseContext db;

        private IConfiguration _config;
        private static string token;

        public HomeController(DataBaseContext context, IConfiguration config)
        {
            db = context;
            _config = config;
        }

        public IActionResult Index()
        {
            var user=new User();
            _log4net.Info("Login Successful");
            return View("Login",user);
        }




        public ActionResult Authenticate(User user)
        {
            token = GetToken(_config["Links:Authorization"], user);

            if (token == null)
            {
                ViewBag.Message = String.Format("Invalid Username or Password");
                return View("Login", user);
            }
            _log4net.Info("Token Generated");

            var AuditModel=new AuditRequest();
            return View("Form",AuditModel);

        }








        public async Task<IActionResult> AuditResult(AuditRequest Request)
        {
            REQ = Request;
            string Result=null;
            using (var client = new HttpClient())
            {
                client.BaseAddress=new Uri(_config["Links:AuditChecklist"]);
                //client.BaseAddress = new Uri("https://mitem.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.GetAsync("AuditCheckList/?AuditType=" + Request.AuditDetails.AuditType);
                if (response.IsSuccessStatusCode)
                {
                    Result = response.Content.ReadAsStringAsync().Result;
                }
                
            }
            List<string> Questions = JsonConvert.DeserializeObject<List<string>>(Result);
            Dictionary<string,string> QuestionAndAnswer= new Dictionary<string, string>();
            foreach (var item in Questions)
            {
                QuestionAndAnswer.Add(item,null);
            }
            return View("Questionnaire",QuestionAndAnswer);
        }






        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> Audit(Dictionary<string, string> QuestionAndAnswer)
        {
            QuestionAndAnswer.Remove(QuestionAndAnswer.ElementAt(QuestionAndAnswer.Count - 1).Key);
            REQ.AuditDetails.AuditQuestions = QuestionAndAnswer;
            REQ.AuditDetails.AuditDate=DateTime.Now;
            string Result = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.PostAsync(_config["Links:AuditSeverity"],new StringContent(JsonConvert.SerializeObject(REQ),Encoding.UTF8,"application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Result = response.Content.ReadAsStringAsync().Result;
                }

            }
            AuditResponse resp = JsonConvert.DeserializeObject<AuditResponse>(Result);
            var CheckIfAlreadyExists = db.AuditResponse.FirstOrDefault(x => x.AuditId == resp.AuditId);
            if (CheckIfAlreadyExists != null)
            {
                while (true)
                {
                    Random r = new Random();
                    resp.AuditId = r.Next(1, 99999);
                    var NewNumber = db.AuditResponse.FirstOrDefault(x => x.AuditId == resp.AuditId);
                    if (NewNumber == null)
                        break;
                }
            }
            db.AuditResponse.Add(resp);
            db.SaveChanges();
            return View("Result",resp);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string GetToken(string url,User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url, data).Result;
                string name = response.Content.ReadAsStringAsync().Result;
                dynamic details = JObject.Parse(name);
                return details.token;
            }
        }
    }
}
