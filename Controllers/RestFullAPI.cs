using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Controllers







{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RestFullAPI : ControllerBase
    {
        private readonly GarageContext _context;

        public RestFullAPI(GarageContext context)
        {
            _context = context;
        }

        // Create or Edit an appointment
        // POST request to create or edit an appointment
        // Example curl command:
        // curl -X POST "http://localhost:5162/api/RestFullAPI/CreateEdit" -H "Content-Type: application/json" -d '{"AppointmentId":1,"CarID":1,"AppointmentDate":"2024-08-27T10:00:00","RequiredService":"Service A","Status":"Scheduled"}'
        [HttpPost]
        public JsonResult CreateEdit([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return new JsonResult("Error while creating appointment");
            }

            _context.Appointment.Add(appointment);
            _context.SaveChanges();

            return new JsonResult("Appointment created successfully");
        }

        // Get an appointment by ID
        // GET request to retrieve an appointment by its ID
        // Example curl command:
        // curl -X GET "http://localhost:5162/api/RestFullAPI/Get?id=1"
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Appointment.Find(id);
            if (result == null)
            {
                return new JsonResult("Appointment not found");
            }
            else
            {
                return new JsonResult(Ok(result));
            }
        }

        // Delete an appointment by ID
        // DELETE request to remove an appointment by its ID
        // Example curl command:
        // curl -X DELETE "http://localhost:5162/api/RestFullAPI/Delete?id=1"
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Appointment.Find(id);
            if (result == null)
            {
                return new JsonResult("Appointment not found");
            }
            else
            {
                _context.Appointment.Remove(result);
                _context.SaveChanges();
                return new JsonResult("Appointment deleted successfully");
            }
        }

        // Get all appointments
        // GET request to retrieve a list of all appointments
        // Example curl command:
        // curl -X GET "http://localhost:5162/api/RestFullAPI/GetAll"
        [HttpGet()]
        public JsonResult GetAll()
        {
            var result = _context.Appointment.ToList();
            if (result == null)
            {
                return new JsonResult("Appointment not found");
            }
            else
            {
                return new JsonResult(Ok(result));
            }
        }
    }
}



////1. Get All Appointments
//Method: GET
//URL: http://localhost:5162/api/RestFullAPI/GetAll
//Description: Retrieves all appointments.

//Steps in Postman:

//Open Postman.
//Set the request method to GET.
//Enter the URL http://localhost:5162/api/RestFullAPI/GetAll.
//Click Send.
//Check the response in the Postman window.
//2. Get Appointment by ID
//Method: GET
//URL: http://localhost:5162/api/RestFullAPI/Get?id=1 (replace 1 with the actual ID of the appointment you want to retrieve)
//Description: Retrieves a specific appointment by its ID.

//Steps in Postman:

//Set the request method to GET.
//Enter the URL http://localhost:5162/api/RestFullAPI/Get?id=1.
//Click Send.
//Check the response in the Postman window.
//3. Create/Edit Appointment
//Method: POST
//URL: http://localhost:5162/api/RestFullAPI/CreateEdit
//Description: Creates a new appointment or updates an existing one.

//Body: Use JSON format and include the Appointment data. For example:

//json
//Copy code
//{
//    "AppointmentId": 0, // Use 0 for new appointments
//    "CarID": 1,
//    "AppointmentDate": "2024-08-27T15:30:00", // Use appropriate date and time format
//    "RequiredService": "Oil Change",
//    "Status": "Scheduled"
//}
//Steps in Postman:

//Set the request method to POST.
//Enter the URL http://localhost:5162/api/RestFullAPI/CreateEdit.
//Go to the Body tab.
//Select raw and then JSON from the dropdown.
//Paste the JSON data into the text area.
//Click Send.
//Check the response in the Postman window.
//4. Delete Appointment
//Method: DELETE
//URL: http://localhost:5162/api/RestFullAPI/Delete?id=1 (replace 1 with the actual ID of the appointment you want to delete)
//Description: Deletes a specific appointment by its ID.

//Steps in Postman:

//Set the request method to DELETE.
//Enter the URL http://localhost:5162/api/RestFullAPI/Delete?id=1.
//Click Send.
//Check the response in the Postman window.
//Additional Tips:
//Ensure API is Running: Make sure your API is running and accessible on localhost:5162.You can check this by navigating to the base URL in your browser (e.g., http://localhost:5162).
//Check API Routes: Double - check that the routes defined in your API controller ([Route("api/[controller]/[action]")]) match the URLs you're using in Postman.
//Handle Authentication: If your API requires authentication, make sure to include any necessary tokens or credentials in the headers of your requests.
