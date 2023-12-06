# ElasticEmbeddings

ElasticEmbeddings is a simple application, exposing an API for [semantic search](https://www.elastic.co/what-is/semantic-search).

The semantic search is done with [OpenAI embeddings](https://platform.openai.com/docs/guides/embeddings) and [Elasticsearch](https://www.elastic.co/guide/en/elasticsearch/reference/current/semantic-search.html).

## Usage

### Configuration

The application required access to elasticsearch and an Azure instance of the OpenAI API.

These configurations can be set in e.g. [appsettings.json](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0#appsettingsjson) or [environment variables](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0#non-prefixed-environment-variables).

#### Elasticsearch

To configure elasticsearch, add the following section to your configuration, replacing values with appropriate values for your environment.

```json
{
  ...
  "ElasticSearch":{
    "Endpoint": "https://localhost:9200",
    "Username": "elastic",
    "Password": "ELASTIC_PASSWORD"
  }
}
```

#### OpenAI

To configure the OpenAI API access, add the following section to your configuration, replacing values with appropriate values for your environment.

```json
{
  ...
  "OpenAI": {
    "Deployment": "<DEPLOYMENT>",
    "ApiKey": "<YOUR API KEY>",
    "Endpoint": "<OPEN AI AZURE ENDPOINT>"
  }
}  
```

> Please note that only the Azure OpenAI API is supported.

### Running the application

Simply `dotnet run` or `dotnet publish` the project `src/ElasticEmbeddings.API/ElasticEmbeddings.csproj`.

See the [demo notebook](./demo.ipynb) for an example on using the search.

