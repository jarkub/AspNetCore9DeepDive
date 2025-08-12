using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    
    public class DepartmentsController
    {
            
        public string Index()
        {
            return "These are the departments.";
        }

        public string DetailsArrayFromHeader([FromHeader]int[] id)
        {
            var res = string.Join(", ", id);
            return $"DetailsArrayFromHeader: {res}";
        }

        public string DetailsArray(int[] id)
        {
            var res = string.Join(", ", id);
            return $"DetailsArray: {res}";
        }

        public string Details(int? id)
        {
            return $"Details: {id}";
        }

        [Route ("/reqreq/{id}")]
        public string ReqReq(int id)
        {
            return $"ReqReq {id}";
        }

        [Route("/reqopt/{id}")]
        public string ReqOpt(int? id)
        {
            return $"ReqOpt {id}";
        }

        [Route("/optreq/{id?}")]
        public string OptReq(int id)
        {
            return $"OptReq {id}";
        }

        [Route("/optopt/{id?}")]
        public string OptOpt(int? id)
        {
            return $"OptOpt {id}";
        }

        [HttpPost]
        public object Create(Department department)
        {
            return department;
        }

        [HttpPost]
        public object CreateFromBody([FromBody]Department department)
        {
            return department;
        }

        [HttpPost]
        public string Delete(int? id)
        {
            return $"Deleted department: {id}";
        }

        [HttpPost]
        public string Edit(int? id)
        {
            return $"Updated department: {id}";
        }
      
    }
}
