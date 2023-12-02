using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.API.Data;
using BookStore.API.Models.Author;
using AutoMapper;
using BookStore.API.Static;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorsController : ControllerBase
{
    private readonly BookStoreContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(
        BookStoreContext context,
        IMapper mapper,
        ILogger<AuthorsController> logger
    )
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
    {
        try
        {
            var authorsDto = await _context.Authors
                .ProjectTo<AuthorReadOnlyDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(authorsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    // GET: api/Authors/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthorDetailsDto>> GetAuthor(int id)
    {
        try
        {
            var author = await _context.Authors
                .Include(auth => auth.Books)
                .ProjectTo<AuthorDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(auth => auth.Id == id);

            if (author == null)
            {
                _logger.LogWarning($"Record Not Found: {nameof(GetAuthor)} - ID: {id}");
                return NotFound();
            }

            return Ok(author);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    // PUT: api/Authors/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
    {
        if (id != authorDto.Id)
        {
            _logger.LogWarning($"Update ID invalid in {nameof(PutAuthor)} - ID: {id}");
            return BadRequest();
        }

        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            _logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} - ID: {id}");
            return NotFound();
        }

        _mapper.Map(authorDto, author);
        _context.Entry(author).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await AuthorExists(id))
            {
                return NotFound();
            }
            else
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(PutAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        return NoContent();
    }

    // POST: api/Authors
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
    {
        try
        {
            var author = _mapper.Map<Author>(authorDto);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error Performing POST in {nameof(PostAuthor)}", authorDto);
            return StatusCode(500, Messages.Error500Message);
        }

    }

    // DELETE: api/Authors/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        try
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                _logger.LogWarning($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} - ID: {id}");
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error Performing DELETE in {nameof(DeleteAuthor)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    private async Task<bool> AuthorExists(int id)
    {
        return await _context.Authors.AnyAsync(e => e.Id == id);
    }
}