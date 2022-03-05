using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore6APIDataTransfer.Models;
using static NetCore6APIDataTransfer.ApiRoutes;

namespace NetCore6WebAPI.Controllers.Controllers
{
    /// <summary>
    /// The HttpGet, HttpPost, HttpPut, HttpDelete Attributes aren't necessary for the methods the way I am using the routes.
    /// But if you added a base route to the class Route Attribute they can be used properly.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("")]
    public class APITestController : Controller
    {
        [HttpGet(APITestRoute.GetApiTestItems)]
        public List<ItemModel> GetAll()
        {
            return new List<ItemModel>
            {
                new ItemModel() { ID = 1, ItemName = "Dashboard Item 1", ItemPrice = 1.50m  },
                new ItemModel() { ID = 2, ItemName = "Dashboard Item 2", ItemPrice = 4.50m  },
                new ItemModel() { ID = 3, ItemName = "Dashboard Item 3", ItemPrice = 2.75m  },
            };
        }

        [HttpGet(APITestRoute.GetApiTestItem)]
        public string Get(int id)
        {
            return $"Item {id}";
        }

        [HttpPost(APITestRoute.SaveApiTestItem)]
        public IActionResult Post([FromBody]int id)
        {
            if (id != 0)
            {
                return Ok(id);
            }

            return BadRequest(id);
        }

        [HttpPut(APITestRoute.UpdateApiTestItem)]
        public void Put(int id, string value)
        {
        }

        [HttpDelete(APITestRoute.DeleteApiTestItem)]
        public void Delete(int id)
        {
        }
    }
}