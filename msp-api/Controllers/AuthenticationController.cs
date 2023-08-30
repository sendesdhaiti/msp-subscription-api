using System;
using Microsoft.AspNetCore.Mvc;
[EnableCors("MyAllowCorsPolicy")]
[ApiController]
[Route("[controller]")]
public class AuthenticationController: ControllerBase
{
	public AuthenticationController()
	{
	}

	[HttpGet]
	public ActionResult Home()
	{
		return Ok("Hello World!");
	}
}