using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class AcessoMiddleware {
    private readonly RequestDelegate _next;

    public AcessoMiddleware(RequestDelegate next){
        _next = next;
    }

    public async Task Invoke(HttpContext context){
            await _next(context);
    }
}
