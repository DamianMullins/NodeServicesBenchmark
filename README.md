# NodeServices Benchmarks

The benchmarks test the performance of using NodeServices in a .Net Core web application to compile & serve [Handlebars templates](https://github.com/wycats/handlebars.js/) versus the standard [Razor templates](https://github.com/aspnet/Razor).


## Running The Benchmarks

To run the benchmarks, first clone or download the project, then run `dotnet run -c release` in the `/NodeServicesBenchmark/NodeServicesBenchmark.BenchmarkRunner` directory.


## Benchmark Application

The [benchmark application](NodeServicesBenchmark.BenchmarkRunner) uses [`BenchmarkDotNet`](https://github.com/dotnet/BenchmarkDotNet) to run the benchmarks. The benchmarks [set use a TestServer](NodeServicesBenchmark.BenchmarkRunner/TestHttpServer.cs), which uses the `Microsoft.AspNetCore.TestHost` package, to create a test instance of the benchmark website.

There are two classes which contain benchmarks; [`LoopBenchmarks.cs`](NodeServicesBenchmark.BenchmarkRunner/LoopBenchmarks.cs) and [`RandomLoopBenchmarks.cs`](NodeServicesBenchmark.BenchmarkRunner/RandomLoopBenchmarks.cs), these call out to the routes described in the [Web App Details](#web-app-details) section.


## Web App Details

In the web project there are six routes with the following in each:

- Razor template with the same data each time the page is loaded
- Handlebars template with the same data each time the page is loaded
- Cached Handlebars template with the same data each time the page is loaded
- Razor template with random data each time the page is loaded
- Handlebars template with random data each time the page is loaded
- Cached Handlebars template with random data each time the page is loaded


### View Components

Both the random and non-random sets of routes are [served via two view components](NodeServicesBenchmark.Website/ViewComponents). Each view component contains logic which decides how it is going to serve up the HTML — Razor or Handlebars.


### Handlebars templates

The handlebars templates are [served via NodeServices in `TemplateService.cs`](NodeServicesBenchmark.Website/Services/TemplateService.cs) which calls a [node module called `templates.js`](NodeServicesBenchmark.Website/templates/templates.js).

`templates.js` is a self contained, pre-bundled version of [the Just Eat `f-templates` module](https://github.com/justeat/f-templates) which looks up Handlebars templates, partials, and resources (translations) which live in [a `templates` directory](NodeServicesBenchmark.Website/templates). Because it has been pre-bundled there is no need to deploy a node_modules directory to a production server.


### Node Services

Node services can be called in one of two ways

#### TemplateService

The [`TemplateService`](NodeServicesBenchmark.Website/Services/TemplateService.cs) calls `INodeServices.InvokeExportAsync()`, passing down some options, which then returns a Handlebars template.

#### CachedTemplateService

The [`CachedTemplateService`](NodeServicesBenchmark.Website/Services/CachedTemplateService.cs) calls down to the [`TemplateService`](NodeServicesBenchmark.Website/Services/TemplateService.cs) and then uses `IMemoryCache` to store the result in an in-memory cache, so any subsequent requests will return the result from the cache.

