using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Models.Owner;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Controllers
{
    [Route("owners")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerService ownerServise, IMapper mapper)
        {
            _ownerService = ownerServise;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<OwnerModel>), StatusCodes.Status200OK)]
        public IActionResult GetList()
        {
            return Ok(
                _mapper.Map<IList<OwnerModel>>(
                    _ownerService.GetList()
                )
            );
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OwnerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var item = _ownerService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<OwnerModel>(
                    item
                )
            );
        }

        [HttpPost]
        [ProducesResponseType(typeof(OwnerModel), StatusCodes.Status201Created)]
        public IActionResult Post(OwnerPostModel model)
        {
            var item = _ownerService.Add(model);

            return CreatedAtAction(
                nameof(Get),
                new { id = item.Id },
                _mapper.Map<OwnerModel>(
                    item
                )
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Put(int id, OwnerPutModel model)
        {
            if (id != model.CarId)
            {
                return BadRequest();
            }

            var item = _ownerService.Edit(model);

            if (item == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var result = _ownerService.Delete(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
