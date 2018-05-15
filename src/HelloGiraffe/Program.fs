module HelloGiraffe.App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open HelloGiraffe.HttpHandlers
open Giraffe.ComputationExpressions

// ---------------------------------
// Web app
// ---------------------------------

open HelloGiraffe.Models
open System.Runtime.CompilerServices

// handleRoutef and handleRoutef' are equivalent
let handleRoutef (s, i) =
  fun (next : HttpFunc) (ctx : Microsoft.AspNetCore.Http.HttpContext) ->
    let result = {
      Text = sprintf "s = %s, i = %i" s i
    }
    json result next ctx

// handleRoutef and handleRoutef' are equivalent
let handleRoutef' (s, i) =
  let result = {
    Text = sprintf "s' = %s, i' = %i" s i
  }
  json result

let abort = System.Threading.Tasks.Task.FromResult None

let handleQuery1 (next : HttpFunc) (ctx : Microsoft.AspNetCore.Http.HttpContext) =
  let query = opt {
    let! foo = ctx.TryGetQueryStringValue "foo"
    let! bar = ctx.TryGetQueryStringValue "bar"
    return (foo, bar)
  }
  match query with
  | Some (foo, bar) -> text (sprintf "foo = %s, bar = %s" foo bar) next ctx
  | None -> abort

let handleQuery2 (next : HttpFunc) (ctx : Microsoft.AspNetCore.Http.HttpContext) =
  let query = opt {
    let! bat = ctx.TryGetQueryStringValue "bat"
    let! car = ctx.TryGetQueryStringValue "car"
    return (bat, car)
  }
  match query with
  | Some (bat, car) -> text (sprintf "bat = %s, car = %s" bat car) next ctx
  | None -> abort


let webApp =
    choose [
        subRoute "/api"
            (choose [
                GET >=> choose [
                    route "/hello" >=> handleGetHello
                    routef "/routef/%s/%i" handleRoutef'
                    route "/query" >=> choose [
                      handleQuery1
                      handleQuery2
                    ]
                ]
            ])
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseCors(configureCors)
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    let filter (l : LogLevel) = l.Equals LogLevel.Error
    builder.AddFilter(filter).AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    WebHostBuilder()
        .UseKestrel()
        .UseIISIntegration()
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0
