using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using Inshapardaz.Api.Helpers;
using Inshapardaz.Api.Model;
using Inshapardaz.Api.Renderers;
using Inshapardaz.Domain.CommandHandlers;
using Inshapardaz.Domain.Model;
using Inshapardaz.Domain.Queries;
using Inshapardaz.Domain.QueryHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using paramore.brighter.commandprocessor;
using Swashbuckle.AspNetCore.Swagger;
using Hangfire;

namespace Inshapardaz.Api.Configuration
{
    public static class AppConfiguration
    {
        public static IConfigurationRoot InitialiseConfiguration(this IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            if (env.IsEnvironment("Development"))
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", false, true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

            builder.AddEnvironmentVariables();
            return builder.Build();
        }

        public static IServiceCollection RegisterFramework(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);

            services.AddMvc();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddCors();

            var connectionString = configuration["ConnectionStrings:DefaultDatabase"];
            services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            return services;
        }

        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Inshapardaz API", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection RegisterRenderes(this IServiceCollection services)
        {
            services.AddTransient<IRenderResponse<EntryView>, EntryRenderer>();
            services.AddTransient<IRenderResponseFromObject<IEnumerable<Dictionary>, DictionariesView>, DictionariesRenderer>();
            services.AddTransient<IRenderResponseFromObject<Dictionary, DictionaryView>, DictionaryRenderer>();
            services.AddTransient<IRenderEnum, EnumRenderer>();
            services.AddTransient<IRenderLink, LinkRenderer>();
            services.AddTransient<IRenderResponseFromObject<Word, WordView>, WordRenderer>();
            services.AddTransient<IRenderResponseFromObject<Word, WordView>, WordIndexRenderer>();
            services.AddTransient<IRenderResponseFromObject<WordDetail, WordDetailView>, WordDetailRenderer>();
            services.AddTransient<IRenderResponseFromObject<Translation, TranslationView>, TranslationRenderer>();
            services.AddTransient<IRenderResponseFromObject<WordRelation, RelationshipView>, RelationRenderer>();
            services.AddTransient<IRenderResponseFromObject<WordDetail, IEnumerable<MeaningContextView>>, WordMeaningRenderer>();
            services.AddTransient<IRenderResponseFromObject<Meaning, MeaningView>, MeaningRenderer>();
            services.AddTransient<IRenderResponseFromObject<PageRendererArgs<Word>, PageView<WordView>>, WordIndexPageRenderer>();

            return services;
        }

        public static IServiceCollection RegisterDomain(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var connectionString = configuration["ConnectionStrings:DefaultDatabase"];

            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<Domain.DatabaseContext>(options => options.UseSqlServer(connectionString, o => o.UseRowNumberForPaging()));
            services.AddTransient<Domain.IDatabaseContext, Domain.DatabaseContext>();

            services.AddTransient<AddDictionaryCommandHandler>();
            services.AddTransient<AddWordCommandHandler>();
            services.AddTransient<AddWordDetailCommandHandler>();
            services.AddTransient<AddWordRelationCommandHandler>();
            services.AddTransient<AddWordTranslationCommandHandler>();
            services.AddTransient<AddWordMeaningCommandHandler>();

            services.AddTransient<UpdateDictionaryCommandHandler>();
            services.AddTransient<UpdateWordCommandHandler>();
            services.AddTransient<UpdateWordDetailCommandHandler>();
            services.AddTransient<UpdateWordRelationCommandHandler>();
            services.AddTransient<UpdateWordTranslationCommandHandler>();
            services.AddTransient<UpdateWordMeaningCommandHandler>();

            services.AddTransient<DeleteDictionaryCommandHandler>();
            services.AddTransient<DeleteWordCommandHandler>();
            services.AddTransient<DeleteWordDetailCommandHandler>();
            services.AddTransient<DeleteWordRelationCommandHandler>();
            services.AddTransient<DeleteWordTranslationCommandHandler>();
            services.AddTransient<DeleteWordMeaningCommandHandler>();

            // Darker Handlers
            services.AddTransient<GetDictionariesByUserQueryHandler>();
            services.AddTransient<GetDictionaryByIdQueryHandler>();
            services.AddTransient<WordStartingWithQueryHandler>();
            services.AddTransient<WordMeaningByWordQueryHandler>();
            services.AddTransient<WordMeaningByIdQueryHandler>();
            services.AddTransient<WordContainingTitleQuery>();
            services.AddTransient<WordIndexContainingTitleQueryHandler>();
            services.AddTransient<WordDetailsByWordQueryHandler>();
            services.AddTransient<WordDetailByIdQueryHandler>();
            services.AddTransient<GetWordsPagesQueryHandler>();
            services.AddTransient<WordByTitleQueryHandler>();
            services.AddTransient<WordByIdQueryHandler>();
            services.AddTransient<TranslationsByWordIdQueryHandler>();
            services.AddTransient<TranslationsByLanguageQueryHandler>();
            services.AddTransient<TranslationByIdQueryHandler>();
            services.AddTransient<RelationshipByWordIdQueryHandler>();
            services.AddTransient<RelationshipByIdQueryHandler>();
            services.AddTransient<GetDictionaryByWordIdQueryHandler>();
            services.AddTransient<DictionaryByWordIdQuery>();
            services.AddTransient<GetDictionaryByMeaningIdQueryHandler>();
            services.AddTransient<GetDictionaryByTranslationIdQueryHandler>();

            services.ConfigureCommandProcessor();
            services.ConfigureDarker();

            return services;
        }

        public static void ConfigureLogging(this ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
        }

        public static IApplicationBuilder ConfigureApplication(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api"))
                {
                    context.Request.Path = "/";
                    await next();
                }
            });

            /* Always register CORS bedore Mvc  */
            app.UseCors(policy => policy.AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowCredentials());
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inshapardaz API");
            });

            return app;
        }

        public static IApplicationBuilder ConfigureHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            return app;
        }

        public static IApplicationBuilder ConfigureApiAuthentication(this IApplicationBuilder app, IConfigurationRoot configuration)
        {
            var options = new JwtBearerOptions
            {
                Audience = configuration["auth0:audience"],
                Authority = configuration["auth0:authority"]
            };
            app.UseJwtBearerAuthentication(options);

            return app;
        }

        public static IApplicationBuilder ConfigureObjectMappings(this IApplicationBuilder app)
        {
            Mapper.Initialize(c => c.AddProfile(new MappingProfile()));
            Mapper.AssertConfigurationIsValid();
            return app;
        }

        public static void ConfigureCommandProcessor(this IServiceCollection services)
        {
            var commandProcessor = CommandProcessorBuilder.With()
                .Handlers(new HandlerConfiguration(new CommandHandlerRegistry(), new ServiceHandlerFactory(services.BuildServiceProvider())))
                .DefaultPolicy()
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build();
            services.AddSingleton<IAmACommandProcessor>(commandProcessor);
        }

        public static void ConfigureDarker(this IServiceCollection services)
        {
            var config = new DarkerConfig(services, services.BuildServiceProvider());
            config.RegisterDefaultDecorators();
            config.RegisterQueriesAndHandlersFromAssembly(typeof(DictionariesByUserQuery).GetTypeInfo().Assembly);

            var queryProcessor = Darker.Builder.QueryProcessorBuilder.With()
                .Handlers(config.HandlerRegistry, config, config)
                .InMemoryQueryContextFactory()
                .Build();

            services.AddSingleton(queryProcessor);
        }
    }
}