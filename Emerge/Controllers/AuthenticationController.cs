using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Emerge.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = "http://103.6.239.161/EmergeIdentity.API/Authentication/login";
            

            if (username == "Test")
            {
                var requestBody = new { Username = username, Password = password };
                var response = await client.PostAsJsonAsync(apiUrl, requestBody);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();



                    HttpContext.Session.SetString("Token", token);


                    // Set a success message in ViewBag
                    ViewBag.LoginMessage = "Login successful!";
                    return RedirectToAction("Index", "Home"); // Redirect to a protected page
                }
                else
                {
                    // Handle authentication failure
                    ModelState.AddModelError(string.Empty, "Invalid credentials");

                    // Set a failure message in ViewBag
                    ViewBag.LoginMessage = "Login failed! Invalid credentials";
                    return View();
                }
            }
            else
            {
                var requestBody = new { Username = username, Password = password };
                var response = await client.PostAsJsonAsync(apiUrl, requestBody);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();



                    HttpContext.Session.SetString("Token", token);


                    // Set a success message in ViewBag
                    ViewBag.LoginMessage = "Login successful!";
                    return RedirectToAction("Index", "Home"); // Redirect to a protected page
                }
                else
                {
                    // Handle authentication failure
                    ModelState.AddModelError(string.Empty, "Invalid credentials");

                    // Set a failure message in ViewBag
                    ViewBag.LoginMessage = "Login failed! Invalid credentials";
                    return View();
                }

            }
          


        
          

           
        }
        [HttpGet]
        public IActionResult Logout()
        {
            // Sign out the user
            HttpContext.SignOutAsync();

            // Redirect to the login page
            return RedirectToAction("Login");
        }

    }
}
