using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Models.Car;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Contollers
{
    [Route("cars")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public CarController(ICarService carServise, IMapper mapper)
        {
            _carService = carServise;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<CarModel>), StatusCodes.Status200OK)]
        public IActionResult GetList()
        {
            return Ok(
                _mapper.Map<IList<CarModel>>(
                    _carService.GetList()
                )
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CarModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var item = _carService.Get(id);

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

        [HttpPost]
        [ProducesResponseType(typeof(CarModel), StatusCodes.Status201Created)]
        public IActionResult Post(CarPostModel model)
        {
            var item = _carService.Add(model);

            return CreatedAtAction(
                nameof(Get),
                new { id = item.Id },
                _mapper.Map<CarModel>(
                    item
                )
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Put(int id, CarPutModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var item = _carService.Edit(model);

            if (item == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Delete(int id)
        {
            var result = _carService.Delete(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
