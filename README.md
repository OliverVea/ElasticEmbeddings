# ElasticEmbeddings

ElasticEmbeddings is a simple application, which exposes an API for [semantic search](https://www.elastic.co/what-is/semantic-search).

The semantic search is done with [OpenAI embeddings](https://platform.openai.com/docs/guides/embeddings) and [Elasticsearch](https://www.elastic.co/guide/en/elasticsearch/reference/current/semantic-search.html).

## Usage

This application is intended to run in docker. 

For the sake of convenience, a [`docker-compose.yml`](docker-compose.yml) file is provided, containing an Elasticsearch instance and the application itself.

### Running the application

1. Ensure that you have a valid Azure OpenAI API key.
1. Copy the [`.env.template`](.env.template) file to `.env` and fill in the values.
2. Run the application with:
    ```bash
    docker-compose up -d
    ```
3. The API is now available at [`http://localhost:8080/swagger/index.html`](http://localhost:8080/swagger/index.html).

### Demo

An IPython notebook is provided to demonstrate the usage of the API. It can be found in [`demo.ipynb`](demo.ipynb).
