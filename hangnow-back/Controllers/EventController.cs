using hangnow_back.Models;
using Microsoft.AspNetCore.Mvc;

namespace hangnow_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly Context _context;
    
    public EventController(Context context)
    {
        _context = context;
    }
}