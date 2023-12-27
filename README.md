# ElasticEmbeddings

ElasticEmbeddings is a simple application, which exposes an API for [semantic search](https://www.elastic.co/what-is/semantic-search).

The semantic search is done with [OpenAI embeddings](https://platform.openai.com/docs/guides/embeddings) and [Elasticsearch](https://www.elastic.co/guide/en/elasticsearch/reference/current/semantic-search.html).

## Usage

This application is intended to run in docker. 

For the sake of convenience, a [`docker-compose.yml`](docker-compose.yml) file is provided, containing an Elasticsearch instance and the application itself.

### Running the application

To configure the dependencies, copy the [`.env.template`](.env.template) file to `.env` and fill in the values.

Then, run the application with:

```bash
docker-compose up -d
```
