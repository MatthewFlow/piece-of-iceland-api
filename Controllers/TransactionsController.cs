using Microsoft.AspNetCore.Mvc;
using piece_of_iceland_api.Models;
using piece_of_iceland_api.Services;

namespace piece_of_iceland_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionsController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> Get() =>
        await _transactionService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Transaction>> Get(string id)
    {
        var transaction = await _transactionService.GetAsync(id);
        return transaction is null ? NotFound() : transaction;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Transaction newTransaction)
    {
        await _transactionService.CreateAsync(newTransaction);
        return CreatedAtAction(nameof(Get), new { id = newTransaction.Id }, newTransaction);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var transaction = await _transactionService.GetAsync(id);
        if (transaction is null) return NotFound();

        await _transactionService.DeleteAsync(id);
        return NoContent();
    }
}
