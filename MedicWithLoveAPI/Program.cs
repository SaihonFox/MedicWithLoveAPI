using MedicWithLoveAPI.Hubs;
using MedicWithLoveAPI.ModelsContext;
using MedicWithLoveAPI.Services;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Scalar.AspNetCore;

using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace MedicWithLoveAPI;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

		// Add services to the container.

		builder.Services.AddControllers(options => options.Conventions.Add(new SnakeCaseControllerModelConvention())).AddNewtonsoftJson(options =>
		{
			options.SerializerSettings.Converters.Add(new DateTimeConverter());

			options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;

			options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
			options.SerializerSettings.Formatting = Formatting.Indented;

			options.SerializerSettings.Culture = CultureInfo.GetCultureInfo("ru-RU");
			options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		});

		builder.Services.AddDbContext<PgSQLContext>(options =>
		{
			options
				.UseNpgsql(string.Format(Environment.GetEnvironmentVariable("LocalhostDBPgSQL", EnvironmentVariableTarget.Machine)!, "diplom-path"))
				.LogTo(Console.WriteLine)
				.EnableDetailedErrors()
				.UseLazyLoadingProxies();
		});

		builder.Services.AddSwaggerGen();
		builder.Services.AddSignalR(options => options.EnableDetailedErrors = true);

		builder.Services.AddCors(options =>
		{
			options.AddPolicy("AllowAllOrigins",
				builder =>
				{
					builder.AllowAnyOrigin()
						   .AllowAnyHeader()
						   .AllowAnyMethod();
				});
		});

		builder.Services.AddScoped<EmailService>();

		builder.Services.AddResponseCompression(options =>
		{
			options.EnableForHttps = true;
			options.Providers.Add<GzipCompressionProvider>();
			options.Providers.Add<BrotliCompressionProvider>();
		});
		builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
		builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.SmallestSize);

		var app = builder.Build();

		app.UseResponseCompression();

		app.MapHub<AnalysisHub>("/hub/analysis");
		app.MapHub<AnalysisCategoryHub>("/hub/analysis_category");
		app.MapHub<PatientHub>("/hub/patient");
		app.MapHub<RequestHub>("/hub/request");
		app.MapHub<UserHub>("/hub/user");

		if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.MapOpenApi();
			app.UseSwagger(options =>
			{
				options.RouteTemplate = "openapi/{documentName}.json";
			});
			app.MapScalarApiReference(options =>
			{
				options.Title = "MedicWithLove API";
				options.Theme = ScalarTheme.BluePlanet;
				options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.RestSharp);
			});
		}

		app.UseStatusCodePages(async builder => {
			HttpResponse response = builder.HttpContext.Response;
			string path = builder.HttpContext.Request.Path;
			response.ContentType = "text/html; charset=UTF-8";

			await response.WriteAsync(
				@$"
				<!DOCTYPE html>
				<html lang=""en"">
				<head>
					<meta charset=""UTF-8"">
					<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
					<style>
						body, html {{
							height: 100%;
							margin: 0;
							font-size: 56px;
							font-family: ""Nunito"";
							display: flex;
							justify-content: center;
							align-items: center;
							text-align: center;
						}}
					</style>
				</head>
				<body>
					<h1>{response.StatusCode}<br>Your path must be: */scalar/v1</h1>
				</body>
				</html>
				"
			);
		});

		app.UseCors("AllowAllOrigins");

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}

internal partial class SnakeCaseControllerModelConvention : IControllerModelConvention
{
	[GeneratedRegex(@"(?<!^)(?=[A-Z])", RegexOptions.Compiled)]
	private static partial Regex MyRegex();

	public void Apply(ControllerModel controller)
	{
		string controllerName = controller.ControllerName;
		if (controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
			controllerName = controllerName[..^"Controller".Length];

		string snakeCaseName = MyRegex().Replace(controllerName, "_").ToLowerInvariant();

		controller.ControllerName = snakeCaseName;
	}
}

internal class DateTimeConverter : JsonConverter<DateTime>
{
	private static readonly string[] formats = [
		"dd.MM.yyyy'T'HH:mm:ss",
		"dd.MM.yyyy HH:mm:ss",
		"dd.MM.yyyy'T'HH:mm:ss.fff",
		"dd.MM.yyyy'T'HH:mm:ss.fff zzz",
		"dd.MM.yyyy'T'HH:mm:ss zzz",
		"dd.MM.yyyy'T'H:mm:ss zzz",
		"dd.MM.yyyy H:mm:ss zzz",
	];

	private string writeFormat = "dd.MM.yyyy'T'HH:mm:ss.fff";

	public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		//File.WriteAllText(@"C:\Users\ILNAR\Desktop\datetime read api.txt", reader.Value!.ToString());
		if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Date)
		{
			var dateString = reader.Value?.ToString();
			if (DateTime.TryParseExact(dateString, formats, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out DateTime dateTime))
				return dateTime;
		}

		string message = $"Unexpected token or cannot parse datetime of value: {reader.Value}. Avaliable formats: {string.Join(", ", formats)}";

		File.WriteAllText(@"C:\Users\ILNAR\Desktop\datetime error api.txt", message);
		throw new JsonSerializationException(message);
	}

	public override async void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
	{
		await writer.WriteValueAsync(value.ToString(writeFormat));
	}
}