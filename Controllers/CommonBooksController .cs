using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using ODataTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataTest.Controllers
{
    [Route("odata/Common/[Controller]")]
    //[ODataRoutePrefix("CommonBooks")]
    [Produces("application/json")]
    public class CommonBooksController : ODataController
    {
        private BookStoreContext _db;

        public CommonBooksController(BookStoreContext context)
        {
            _db = context;
            if (context.Books.Count() == 0)
            {
                foreach (var b in DataSource.GetBooks())
                {
                    context.Books.Add(b);
                    context.Presses.Add(b.Press);
                }
                context.SaveChanges();
            }
        }

        //[ODataRoute("Common/Books")]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Books);
        }
    }
}
