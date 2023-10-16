using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Emerge.Models;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace Emerge.Controllers
{
    [Controller]
    public class LevAlloController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LevAlloController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
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
            TempData["Token"] = token; 


            if (string.IsNullOrEmpty(token))
            {
                // Redirect to the login page or handle as needed when the token is missing.
                return RedirectToAction("Login", "Authentication");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiBaseUrl = _configuration["LeaveAllocation"];
                var apiUrl = $"{apiBaseUrl}";

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var leaveRecords = await response.Content.ReadFromJsonAsync<List<LeaveAllocation>>();
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
        public async Task<IActionResult> Create(LeaveAllocation leaveAllocation)
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
                var apiBaseUrl = _configuration["LeaveAllocation"];
                var apiUrl = $"{apiBaseUrl}";

                var response = await client.PostAsJsonAsync(apiUrl, leaveAllocation);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error creating leave record");
                    return View(leaveAllocation);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(leaveAllocation);
            }
        }


        [HttpGet("LevAllo/Edit/{id}")]
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
                var apiBaseUrl = _configuration["LeaveAllocation"]; // Replace with your API endpoint
                var apiUrl = $"{apiBaseUrl}/{id}"; // Adjust the endpoint URL as needed

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var leaveAllocation = await response.Content.ReadFromJsonAsync<LeaveAllocation>();
                    if (leaveAllocation == null)
                    {
                        return NotFound();
                    }

                    return View(leaveAllocation);
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

        public async Task<IActionResult> Edit(int id, [Bind("Id,LeaveTypeId,EmployeeId,NumberOfDays")] LeaveAllocation leaveAllocation)
        {
            if (id != leaveAllocation.Id)
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
                var apiBaseUrl = _configuration["LeaveAllocation"];
                var apiUrl = $"{apiBaseUrl}/{id}";

                var response = await client.PutAsJsonAsync(apiUrl, leaveAllocation);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error updating leave record");
                    return View(leaveAllocation);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(leaveAllocation);
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
                var apiBaseUrl = _configuration["LeaveAllocation"];
                var apiUrl = $"{apiBaseUrl}/{deleteId}";

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var leaveAllocation = await response.Content.ReadFromJsonAsync<LeaveAllocation>();
                    if (leaveAllocation == null)
                    {
                        return NotFound();
                    }

                    return View(leaveAllocation);
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
                var apiBaseUrl = _configuration["LeaveAllocation"];
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
