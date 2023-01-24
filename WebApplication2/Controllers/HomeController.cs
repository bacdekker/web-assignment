using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApplication2.Models;
using WebApplication2.Data;

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
    }
}
