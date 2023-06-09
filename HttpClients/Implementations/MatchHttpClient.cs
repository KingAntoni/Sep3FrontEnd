﻿using System.Text;
using System.Text.Json;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class MatchHttpClient : IMatchService
{
    private readonly HttpClient client;
    
    public MatchHttpClient(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<MatchDomainModel> findById(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"https://localhost:7093/Match/findById/{id}");
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        MatchDomainModel match = JsonSerializer.Deserialize<MatchDomainModel>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return match;
    }

    public async Task updateMatch(MatchDomainModel matchModel)
    {
        string json = JsonSerializer.Serialize(matchModel);
        StringContent body = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PatchAsync("https://localhost:7093/Match/update", body);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task deleteMatch(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"https://localhost:7093/Match/deleteMatch{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task<IEnumerable<MatchDomainModel>> findByUserId(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"https://localhost:7093/Match/findByUserId{id}");
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        IEnumerable<MatchDomainModel> matchs = JsonSerializer.Deserialize<IEnumerable<MatchDomainModel>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        MatchDomainModel firstMatch = matchs.FirstOrDefault();
        return matchs;
    }
}