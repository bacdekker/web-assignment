using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApplication2.Models;
using WebApplication2.Data;
//using RestSharp;


namespace WebApplication2.Controllers
{
    //[Microsoft.AspNetCore.Mvc.Route("/Addresses")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressReposity;
        public AddressController(IAddressRepository addressRepository)
        {
            _addressReposity = addressRepository;
        }

        [HttpGet("/addresses")]
        [ProducesResponseType(200, Type = typeof(List<Address>))]
        public IActionResult getAddresses([FromQuery] Address address, bool orderByAscending)
        {
            (List<Address>, List<int>) addresses = _addressReposity.getAddresses(address, orderByAscending);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(addresses.Item1);
        }

        [HttpPost("/addresses")]
        public IActionResult addAddress(Address address)
        { 
            if (!ModelState.IsValid)
                    return BadRequest(ModelState);

            _addressReposity.InsertAddress(address);

            return Ok();
        }

        [HttpGet("/addresses/{id}")]
        [ProducesResponseType(200, Type = typeof(Address))]
        public IActionResult getSingleAddress(int id) 
        {
            if(!_addressReposity.existAddress(id))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_addressReposity.getSingleAddress(id));
        }
        [HttpPut("/addresses/{id}")]
        public IActionResult updateAddress(Address address, int id) 
        {
            if (!_addressReposity.existAddress(id))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _addressReposity.UpdateAddress(address, id);

            return Ok();
        }

        [HttpDelete("/addresses/{id}")]
        public IActionResult deleteAddress(int id) 
        {
            if (!_addressReposity.existAddress(id))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _addressReposity.DeleteAddress(id);

            return Ok();

        }
        /*
        [HttpGet("/addresses/distance")]
        public IActionResult getDistance(int id1, int id2)
        {
            if(!_addressReposity.existAddress(id1) || !_addressReposity.existAddress(id2))
                return BadRequest(ModelState);

            Address address1 = _addressReposity.getSingleAddress(id1);
            Address address2 = _addressReposity.getSingleAddress(id2);

            string request_body = $@"{address1.Street} {address1.HouseNumber.ToString()} {address1.City}\n{address2.Street} {address2.HouseNumber.ToString()} {address2.City}";

            var client = new RestClient("https://redline-redline-zipcode.p.rapidapi.com/rest/multi-radius.json/10/mile");
            var request = new RestRequest(Method.POST);
            request.AddHeader("X-RapidAPI-Key", "fb5d635ac7mshd85c2c013e2c9cbp185f6cjsn754469ec0762");
            request.AddHeader("X-RapidAPI-Host", "redline-redline-zipcode.p.rapidapi.com");
            request.AddParameter("units", "km");
            request.AddParameter("distance", 100);
            request.AddBody("addrs", request_body);
            IRestResponse response = client.Execute(request);
            return View();
        }*/
    }
}
