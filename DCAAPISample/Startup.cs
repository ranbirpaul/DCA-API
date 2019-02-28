﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service;
using Service.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository;
using Repository.Abstract;
using Business_Model;
using Swashbuckle.AspNetCore.Swagger;
using DCAAPISample.Middleware;
using System.Reflection;
using System.IO;

namespace DCAAPISample
{
	public static class ServiceExtensions
	{
		public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials());
			});
		}
		public static void ResolveDependencies(this IServiceCollection services)
		{
			services.AddTransient<IDCARepository, DCARepositry>();
			services.AddTransient<IDCATypeRepository, DCATypeRepositry>();
			services.AddTransient<IDCATypeService, DCATypeService>();
			services.AddTransient<IDCAService, DCAService>();
			services.AddTransient<IDCAReferencesRepository, DCAReferencesRepository>();
			services.AddTransient<IDCAReferencesService, DCAReferencesService>();
			services.AddTransient<IQueryRepository, QueryRepository>();
			services.AddTransient<IQueryService, QueryService>();
		}
		public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<Settings>(Options =>
			{
				Options.InfoModelBaseAddress = configuration.GetSection("AbilityModel:InfoModelBaseURI").Value;
				Options.TypeDefBaseAddress = configuration.GetSection("AbilityModel:TypeDefBaseURI").Value;
				Options.InfoObjects = configuration.GetSection("AbilityModel:CreateObjects").Value;
				Options.RetrieveObjects = configuration.GetSection("AbilityModel:RetrieveObjects").Value;
				Options.EditType = configuration.GetSection("AbilityModel:CreateTypes").Value;
				Options.EditModel = configuration.GetSection("InfoModel:CreateModel").Value;
				Options.QueryPath = configuration.GetSection("AbilityModel:DSLQueryPath").Value;
				Options.ReferencePath = configuration.GetSection("AbilityModel:ReferencePath").Value;
				Options.ReferenceRetrievePath = configuration.GetSection("AbilityModel:ReferenceRetrievePath").Value;


				Options.infoModelToken.GrantType = configuration.GetSection("AbilityToken:grant_type").Value;
				Options.infoModelToken.ClientId = configuration.GetSection("AbilityToken:client_id").Value;
				Options.infoModelToken.ClientSecret = configuration.GetSection("AbilityToken:client_secret").Value;
				Options.infoModelToken.Resource = configuration.GetSection("AbilityToken:resource").Value;
				Options.infoModelToken.Scope = configuration.GetSection("AbilityToken:scope").Value;
				Options.infoModelToken.password = configuration.GetSection("AbilityToken:password").Value;
				Options.infoModelToken.username = configuration.GetSection("AbilityToken:username").Value;
				Options.infoModelToken.test = configuration.GetSection("AbilityToken:test").Value;
				Options.infoModelToken.TokenURI = configuration.GetSection("AbilityToken:tokenURI").Value;
			});
		}
	}
		public class Startup
		{

			public Startup(IConfiguration configuration)
			{
				Configuration = configuration;
			}

			public IConfiguration Configuration { get; }

			// This method gets called by the runtime. Use this method to add services to the container.
			public void ConfigureServices(IServiceCollection services)
			{
				services.AddSwaggerGen(c =>
				{
					c.SwaggerDoc("v1", new Info { Title = "DCA Portal API Service", Version = "v1" });
					//Locate the XML file being generated by ASP.NET...
					var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

					//... and tell Swagger to use those XML comments.
					c.IncludeXmlComments(xmlPath);
				});

				//services.Configure<Settings>(Options =>
				//{
				//	Options.InfoModelBaseAddress = Configuration.GetSection("AbilityModel:InfoModelBaseURI").Value;
				//	Options.TypeDefBaseAddress = Configuration.GetSection("AbilityModel:TypeDefBaseURI").Value;
				//	Options.InfoObjects = Configuration.GetSection("AbilityModel:CreateObjects").Value;
				//	Options.InfoObjects = Configuration.GetSection("AbilityModel:RetrieveObjects").Value;
				//	Options.EditType = Configuration.GetSection("AbilityModel:CreateTypes").Value;
				//	Options.EditModel = Configuration.GetSection("InfoModel:CreateModel").Value;
				//	Options.QueryPath = Configuration.GetSection("AbilityModel:DSLQueryPath").Value;
				//	Options.ReferencePath = Configuration.GetSection("AbilityModel:ReferencePath").Value;

				//	Options.infoModelToken.GrantType = Configuration.GetSection("AbilityToken:grant_type").Value;
				//	Options.infoModelToken.ClientId = Configuration.GetSection("AbilityToken:client_id").Value;
				//	Options.infoModelToken.ClientSecret = Configuration.GetSection("AbilityToken:client_secret").Value;
				//	Options.infoModelToken.Resource = Configuration.GetSection("AbilityToken:resource").Value;
				//	Options.infoModelToken.Scope = Configuration.GetSection("AbilityToken:scope").Value;
				//	Options.infoModelToken.password = Configuration.GetSection("AbilityToken:password").Value;
				//	Options.infoModelToken.username = Configuration.GetSection("AbilityToken:username").Value;
				//	Options.infoModelToken.test = Configuration.GetSection("AbilityToken:test").Value;
				//	Options.infoModelToken.TokenURI = Configuration.GetSection("AbilityToken:tokenURI").Value;
				//});
				services.ConfigureSettings(Configuration);
				services.ConfigureCors();
				services.ResolveDependencies();
				services.AddMvc();
			}

			// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
			public void Configure(IApplicationBuilder app, IHostingEnvironment env)
			{
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "DCA API Service V1");
					c.RoutePrefix = string.Empty;
				});
				if (env.IsDevelopment())
				{
					app.UseDeveloperExceptionPage();
				}
				app.UseMiddleware(typeof(ErrorHandlingMidlleware));

				DefaultFilesOptions DefaultFile = new DefaultFilesOptions();
				DefaultFile.DefaultFileNames.Clear();
				DefaultFile.DefaultFileNames.Add("index.html");
				app.UseDefaultFiles(DefaultFile);

			app.UseCors("AllowMyOrigin");
			app.UseMvc();
			}
		}
	
}