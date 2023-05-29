using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]
public class GeoLocalizacao : ControllerBase {

	[HttpGet("api/location/{ip}/{filtro}")]
	public ActionResult<string> PegarGeoLocation(string ip, string filtro){
		//Chama a Model
		var geo = new Geo(ip, filtro);

		return Content(geo.GeraGeoLocation(), "application/json");
	}

} 