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
    public class LeaveController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LeaveController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
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


            if (string.IsNullOrEmpty(token))
            {
                // Redirect to the login page or handle as needed when the token is missing.
                return RedirectToAction("Login", "Authentication");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiBaseUrl = _configuration["LeaveApi"];
                var apiUrl = $"{apiBaseUrl}";

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var leaveRecords = await response.Content.ReadFromJsonAsync<List<LeaveInfo>>();
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
        public async Task<IActionResult> Create(LeaveInfo leaveInfo)
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
                var apiBaseUrl = _configuration["LeaveApi"];
                var apiUrl = $"{apiBaseUrl}";

                var response = await client.PostAsJsonAsync(apiUrl, leaveInfo);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error creating leave record");
                    return View(leaveInfo);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(leaveInfo);
            }
        }


        [HttpGet("Leave/Edit/{id}")]
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
                var apiBaseUrl = _configuration["LeaveApi"]; 
                var apiUrl = $"{apiBaseUrl}/{id}"; 

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var leaveInfo = await response.Content.ReadFromJsonAsync<LeaveInfo>();
                    if (leaveInfo == null)
                    {
                        return NotFound();
                    }

                    return View(leaveInfo);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Description,NoOfDays,MaxSlot,Gender,StartValidityDays,CarryForwardDays,Designation")] LeaveInfo leaveInfo)
        {
            if (id != leaveInfo.Id)
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
                var apiBaseUrl = _configuration["LeaveApi"];
                var apiUrl = $"{apiBaseUrl}/{id}";

                var response = await client.PutAsJsonAsync(apiUrl, leaveInfo);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error updating leave record");
                    return View(leaveInfo);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(leaveInfo);
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
                var apiBaseUrl = _configuration["LeaveApi"];
                var apiUrl = $"{apiBaseUrl}/{deleteId}";

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var leaveInfo = await response.Content.ReadFromJsonAsync<LeaveInfo>();
                    if (leaveInfo == null)
                    {
                        return NotFound();
                    }

                    return View(leaveInfo);
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
                var apiBaseUrl = _configuration["LeaveApi"];
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
