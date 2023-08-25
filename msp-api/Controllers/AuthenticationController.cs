using System;
using Microsoft.AspNetCore.Mvc;

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