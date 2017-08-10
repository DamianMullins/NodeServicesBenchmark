# Microsoft.AspNetCore.NodeServices Benchmarks

Quick and dirty benchmarks to test the performance when using NodeServices inside of a .Net Core application.

## Details

In the web project there are three routes with the following in the Razor view for each:

- A view with static HTML
- A view which injects `ITemplateService` and requests a template from the `f-header` npm package.
- A view which injects `ICachedTemplateService` and requests a template from the `f-header` npm package.

### ITemplateService

`ITemplateService` calls `INodeServices.InvokeAsync()` which returns a handlebars template.

### ICachedTemplateService

`ICachedTemplateService` uses `IMemoryCache` to provide an in-memory cache, the `IMemoryCache.GetOrCreateAsync()` method is called and will return a cached template if one is found, otherwise it will call `INodeServices.InvokeAsync()` which returns a handlebars template, add the template to the cache, then return the template.

## Benchmark Application

The benchmark application uses the `Microsoft.AspNetCore.TestHost` package to create a test server instance which will then call a specified url a set number of times, recording the time taken for each request, then returning the average of those times.

> Note: The first request is omitted from the average calculation as it has to spin up the test server and this skews the average time taken.

## Local Test Run Results

_**Run on a Dell XPS 15, Win 10 Pro 64-bit, Intel Core i7-4712HQ 2.3GHz CPU, 16GB RAM**_

Each test run performs 1000 requests against the an endpoint.

| Run # | No Template Service | With Template Service | With Cached Template Service |
|-------|---------------------|-----------------------|------------------------------|
| 1     | 0.04ms              | 2.38ms                | 0.03ms                       |
| 2     | 0.05ms              | 2.36ms                | 0.03ms                       |
| 3     | 0.04ms              | 2.24ms                | 0.03ms                       |
| 4     | 0.03ms              | 2.34ms                | 0.03ms                       |
| 5     | 0.03ms              | 2.35ms                | 0.02ms                       |
