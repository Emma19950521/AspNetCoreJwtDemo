using System.Text.Json;

namespace MemberSystem.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				Console.WriteLine("抓到錯誤：" + ex.Message);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = 500;

				var errorResponse = new
				{
					Message = "系統錯誤",
					Detail = ex.Message  // 開發模式可以加 ex.StackTrace
				};

				var json = JsonSerializer.Serialize(errorResponse);
				await context.Response.WriteAsync(json);
			}
		}
	}
}
