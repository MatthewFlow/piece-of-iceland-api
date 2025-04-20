using Microsoft.AspNetCore.Mvc;
using piece_of_iceland_api.Models;
using piece_of_iceland_api.Services;

namespace piece_of_iceland_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParcelsController : ControllerBase
{
    private readonly ParcelService _parcelService;

    public ParcelsController(ParcelService parcelService)
    {
        _parcelService = parcelService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Parcel>>> Get() =>
        await _parcelService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Parcel>> Get(string id)
    {
        var parcel = await _parcelService.GetAsync(id);
        return parcel is null ? NotFound() : parcel;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Parcel newParcel)
    {
        await _parcelService.CreateAsync(newParcel);
        return CreatedAtAction(nameof(Get), new { id = newParcel.Id }, newParcel);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Put(string id, Parcel updatedParcel)
    {
        var parcel = await _parcelService.GetAsync(id);
        if (parcel is null) return NotFound();

        updatedParcel.Id = parcel.Id;
        await _parcelService.UpdateAsync(id, updatedParcel);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var parcel = await _parcelService.GetAsync(id);
        if (parcel is null) return NotFound();

        await _parcelService.DeleteAsync(id);
        return NoContent();
    }
}
