using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Project.Server.Services.StackExchange.Tags;
using Project.Server.Services.StackExchange.Tags.Models;

namespace Project.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class TagsController : ControllerBase {
    private readonly ITagsService _tagsService;

    public TagsController(ITagsService tagsService) {
        _tagsService = tagsService;
    }

    [EnableCors("ClientPermission")]
    [HttpPost(nameof(GetTagsFromStackOverflow))]
    public async Task<IActionResult> GetTagsFromStackOverflow(Pagination pagination) {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _tagsService.GetTagsFromStackOverflow(accessToken, pagination);

        if (response == null)
            return NoContent();

        return Ok(response);
    }
}