using Emerge.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Emerge.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CurrencyController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            string token = null;
            var tokenJson = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(tokenJson))
            {
                var tokenIndex = tokenJson.IndexOf("\"token\":\"");
                if (tokenIndex >= 0)
                {
                    tokenIndex += "\"token\":\"".Length;
                    var closingQuoteIndex = tokenJson.IndexOf("\"", tokenIndex);
                    if (closingQuoteIndex >= 0)
                    {
                        token = tokenJson.Substring(tokenIndex, closingQuoteIndex - tokenIndex);
                    }
                }


            }
            else
            {
                // Handle the case where 'Token' is not found or is empty
            }
            TempData["Token"] = token; // Store the token in TempData
         //   string token = HttpContext.Session.GetString("Token");


            if (string.IsNullOrEmpty(token))
            {
                // Redirect to the login page or handle as needed when the token is missing.
                return RedirectToAction("Login", "Authentication");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiBaseUrl = _configuration["Currency"];
                var apiUrl = $"{apiBaseUrl}";

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var leaveRecords = await response.Content.ReadFromJsonAsync<List<Currency>>();
                    return View(leaveRecords);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving leave records");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Currency currency)
        {
            // Retrieve the token from TempData

            string token = TempData["Token"] as string;

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where the token is not found or is empty
                return RedirectToAction("Login", "Authentication");
            }


            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiBaseUrl = _configuration["Currency"];
                var apiUrl = $"{apiBaseUrl}";

                var response = await client.PostAsJsonAsync(apiUrl, currency);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error creating leave record");
                    return View(currency);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(currency);
            }
        }


        [HttpGet("Currency/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string token = TempData["Token"] as string;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiBaseUrl = _configuration["Currency"]; // Replace with your API endpoint
                var apiUrl = $"{apiBaseUrl}/{id}"; // Adjust the endpoint URL as needed

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var currency = await response.Content.ReadFromJsonAsync<Currency>();
                    if (currency == null)
                    {
                        return NotFound();
                    }

                    return View(currency);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving leave record");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Code, Abbreviation")] Currency currency)
        {
            if (id != currency.Id)
            {
                return NotFound();
            }

            string token = TempData["Token"] as string;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiBaseUrl = _configuration["Currency"];
                var apiUrl = $"{apiBaseUrl}/{id}";

                var response = await client.PutAsJsonAsync(apiUrl, currency);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error updating leave record");
                    return View(currency);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(currency);
            }
        }

        public async Task<IActionResult> Delete(int? deleteId)
        {
            if (deleteId == null)
            {
                return NotFound();
            }
            string token = TempData["Token"] as string;
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiBaseUrl = _configuration["Currency"];
                var apiUrl = $"{apiBaseUrl}/{deleteId}";

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var currency = await response.Content.ReadFromJsonAsync<Currency>();
                    if (currency == null)
                    {
                        return NotFound();
                    }

                    return View(currency);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving leave record");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int deleteId)
        {
            string token = TempData["Token"] as string;



            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiBaseUrl = _configuration["Currency"];
                var apiUrl = $"{apiBaseUrl}/{deleteId}";

                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error deleting leave record");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View();
            }
        }
    }
}
