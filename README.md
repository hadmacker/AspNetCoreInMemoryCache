# ASP.NET In-Memory Cache

* This solution is designed around [IMemoryCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-5.0)

# Getting Started

* This proof of concept is intended to allow for easy entry of KVP data into an Azure Table and allow for rapid lookup of this data using an in-memory cache.
* Create an Azure Storage Account and add an environment variable with the name `ancimcstorage` with value of the `Connection String`.

# Hosting & Resources

* While you can probably also use Azure functions for an in-memory cache, you're working against gb-sec pricing. Therefore this demo is designed as an ASP.NET Core web app.
* [Azure App Service](https://azure.microsoft.com/en-us/pricing/details/app-service/windows/)

# Technical Reference

* [Cache in-memory in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-5.0)
* [Azure Table Storage Tutorial](https://docs.microsoft.com/en-us/azure/cosmos-db/tutorial-develop-table-dotnet)
* [Azure Table Storage primer](https://www.c-sharpcorner.com/article/insert-data-into-azure-table-storage-using-asp-net-core-application/)
  * Uses outdated table provider. See Azure.Storage.Blobs instead.
* [Azure Cosmos DB: Table API (Used to access Table Storage)](https://docs.microsoft.com/en-us/azure/cosmos-db/table-introduction)
* [Azure.Storage.Blobs](https://github.com/Azure/azure-sdk-for-net/blob/Azure.Storage.Blobs_12.8.0/sdk/storage/Azure.Storage.Blobs/README.md)
* [Configuration in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0)
* [Secrets Management in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows)
  * Where have you been all my life??
  * ASP.NET MVC Reference
    * [Views + ViewModels](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/mvc-music-store/mvc-music-store-part-3)
    * [Edit Forms & Templating](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/mvc-music-store/mvc-music-store-part-5)