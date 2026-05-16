using ComputerManagementApi.DTOs;
using ComputerManagementApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagementApi.Controllers;

[ApiController]
[Route("api/pcs")]
public class PcsController : ControllerBase
{
    private readonly IPcService _service;

    public PcsController(IPcService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PcListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var pcs = await _service.GetAllAsync();
        return Ok(pcs);
    }

    [HttpGet("{id:int}/components")]
    [ProducesResponseType(typeof(PcWithComponentsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdWithComponents(int id)
    {
        var pc = await _service.GetByIdWithComponentsAsync(id);
        if (pc is null)
            return NotFound($"PC with id {id} not found.");

        return Ok(pc);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PcListDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] PcCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByIdWithComponents), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PcListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] PcUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound($"PC with id {id} not found.");

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound($"PC with id {id} not found.");

        return NoContent();
    }
}
