using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using B7;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace B7
{
    public static class API
    {
        public static List<Book> Books = new List<Book>();

        [FunctionName("GetAll")]
        public static IActionResult GetAll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get")] HttpRequest req)
        {
            return new OkObjectResult(Books);
        }

        [FunctionName("Post")]
        public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "post")] HttpRequest req)
        {
            string name = req.Query["name"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Book data = JsonConvert.DeserializeObject<Book>(requestBody);
            data.Id = System.Guid.NewGuid().ToString();
            Books.Add(data);

            return new OkObjectResult(data);
        }

        [FunctionName("GetOne")]
        public static IActionResult GetOne(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get/{id}")] HttpRequest req, string id)
        {
            var book = Books.FirstOrDefault(s => s.Id == id);
            if (book == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(book);
        }

        [FunctionName("Put")]
        public static async Task<IActionResult> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "put/{id}")] HttpRequest req, string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedBook = JsonConvert.DeserializeObject<Book>(requestBody);

            var existingBook = Books.FirstOrDefault(s => s.Id == id);
            if (existingBook == null)
            {
                return new NotFoundResult();
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.PageCount = updatedBook.PageCount;

            return new OkObjectResult(existingBook);
        }


        [FunctionName("Delete")]
        public static IActionResult Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delete/{id}")] HttpRequest req, string id)
        {
            var book = Books.FirstOrDefault(s => s.Id == id);
            if (book == null)
            {
                return new NotFoundResult();
            }

            Books.Remove(book);

            return new NoContentResult();
        }

    }
}