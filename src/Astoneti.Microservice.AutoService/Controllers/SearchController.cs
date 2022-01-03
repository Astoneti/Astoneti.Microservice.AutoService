using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Models.Car;
using Astoneti.Microservice.AutoService.Models.Owner;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Controllers
{
    [Route("search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;

        public SearchController(ICarService carServise, IOwnerService ownerServise, IMapper mapper)
        {
            _carService = carServise;
            _ownerService = ownerServise;
            _mapper = mapper;
        }

        [HttpGet("owners/carsList")]
        [ProducesResponseType(typeof(IEnumerable<OwnerModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOwnersWithCarsList()
        {
            return Ok(
               _mapper.Map<IList<OwnerModel>>(
                   _ownerService.GetCarsList()
               )
           );
        }

        [HttpGet("owners/moreThenOneCar")]
        [ProducesResponseType(typeof(IEnumerable<OwnerModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOwnerWhereMoreThenOneCar()
        {
            return Ok(
               _mapper.Map<IList<OwnerModel>>(
                   _ownerService.GetOwnerWhereMoreThenOneCar()
               )
           );
        }

        [HttpGet("cars/ownersList")]
        [ProducesResponseType(typeof(IEnumerable<CarModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCarsWhithOwnersList()
        {
            return Ok(
                _mapper.Map<IList<CarModel>>(
                    _carService.GetOwnersList()
                )
            );
        }

        [HttpGet("cars/withoutOwners")]
        [ProducesResponseType(typeof(IEnumerable<CarModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCarsWithoutOwners()
        {
            return Ok(
                _mapper.Map<IList<CarModel>>(
                    _carService.GetCarsWithoutOwners()
                )
            );
        }

        [HttpGet("cars/by{number}")]
        [ProducesResponseType(typeof(IEnumerable<CarModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCarsByLicensePlate(string number)
        {
            var list = _carService.GetCarsByLicensePlate(number);

            var resultValue = list;

            if (resultValue == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<IList<CarModel>>(
                    resultValue
                )
            );
        }

        [HttpGet("cars/bycarbrand")]
        [ProducesResponseType(typeof(IEnumerable<CarModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCarsByCarBrand(string brand)
        {
            var list = _carService.GetByCarBrandWhithOwner(brand);

            var resultValue = list;

            if (resultValue == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<IList<CarModel>>(
                    resultValue
                )
            );
        }

        [HttpGet("cars/bycarmodel")]
        [ProducesResponseType(typeof(IEnumerable<CarModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCarsByCarModel(string model)
        {
            var list = _carService.GetByCarModel(model);

            var resultValue = list;

            if (resultValue == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<IList<CarModel>>(
                    resultValue
                )
            );
        }

        [HttpGet("cars/withOwners{id}")]
        [ProducesResponseType(typeof(CarModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCarsWhithOwnersById(int id)
        {
            var item = _carService.GetCarsWhithOwners(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<CarModel>(
                    item
                )
            );
        }
    }
}
