using System.Net.Http;
using System.Text.Json;

public class Geo {
	public string ip { get; set; }
	public string filtro { get; set; }

	public Geo(string _ip, string _filtro){
		this.ip = _ip;
		this.filtro = _filtro;
	}

	public string GeraGeoLocation(){
		var url = @"http://api.ipapi.com/api/" + ip + "?access_key=fb9251d6c3c272af4b860ee6d680207a";

		HttpClient Http = new HttpClient();

		var resposta_consulta = ConsultaGeoLocation(Http, url).Result;

		if(resposta_consulta.IsSuccessStatusCode){
			return RespostaFinal(JsonSerializer.Deserialize<Root>(PegaResposta(resposta_consulta).Result));
		}
		else{
			return "A Soliticação falhou devido: " + resposta_consulta.StatusCode;
		}
	}

	private async Task<HttpResponseMessage> ConsultaGeoLocation(HttpClient cliente, string url){
		return await cliente.GetAsync(url);
	}

	private async Task<string> PegaResposta(HttpResponseMessage resposta){
		return await resposta.Content.ReadAsStringAsync();
	}

	private string RespostaFinal(Root objeto){
		var objResposta = new Resposta();

		if(filtro == "all"){
			objResposta.continente = objeto.continent_name;
			objResposta.pais = objeto.country_name;
			objResposta.estado = objeto.region_code;
			objResposta.lat = objeto.latitude;
			objResposta.lon = objeto.longitude;
			objResposta.cep = objeto.zip;
		}

		return JsonSerializer.Serialize(objResposta);
	}
}