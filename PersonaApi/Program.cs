using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

var lambdaFunction = new LambdaFunction();
lambdaFunction.Init(builder);

var host = builder.Build();
host.Run();