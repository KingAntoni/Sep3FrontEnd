﻿using System.Text.Json;
using Domain.Models;

namespace FileData;

public class FileContext
{
    private const string filePath = "data.json";
    private DataContainer? dataContainer;

    public ICollection<Post> Posts
    {
        get
        {
            LoadData();
            return dataContainer!.Posts;
        }
    }

    public ICollection<User> Users
    {
        get
        {
            LoadData();
            return dataContainer!.Users;
        }
    }

    public ICollection<AnswerMessage> AnswerMessages
    {
        get
        {
            LoadData();
            return dataContainer!.AnswerMessages;
        }
    }
    private void LoadData()
    {
        if (dataContainer != null) return;
        
        if (!File.Exists(filePath))
        {
            dataContainer = new ()
            {
                Posts = new List<Post>(),
                Users = new List<User>(),
                AnswerMessages = new List<AnswerMessage>()
            };
            return;
        }
        string content = File.ReadAllText(filePath);
        dataContainer = JsonSerializer.Deserialize<DataContainer>(content);
        
        if (dataContainer.AnswerMessages == null)
        {
            dataContainer.AnswerMessages = new List<AnswerMessage>();
        }
    }

    public void SaveChanges()
    {
        string serialized = JsonSerializer.Serialize(dataContainer, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(filePath, serialized);
        dataContainer = null;
    }
}
