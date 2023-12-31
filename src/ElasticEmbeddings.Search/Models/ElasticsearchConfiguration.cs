﻿namespace ElasticEmbeddings.Search.Models;

public class ElasticsearchConfiguration
{
    public required string Endpoint { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}