using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CUK.Models;

public class MenuData : Dictionary<string, Dictionary<string, string>>
{
}

public enum AppTheme
{
    System,
    Light,
    Dark
}

public enum FontSizeOption
{
    Small,
    Medium,
    Large
}

public class AppSettings
{
    public AppTheme Theme { get; set; } = AppTheme.System;
    public FontSizeOption FontSize { get; set; } = FontSizeOption.Medium;
    public string Language { get; set; } = "system"; // "system", "ko", "en", "ja", "zh"
    public bool ShowOperatingHours { get; set; } = true;
    public string LastSeenVersion { get; set; } = "0.0.0";
}

public class IssueRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    [JsonPropertyName("labels")]
    public List<string> Labels { get; set; } = new();

    [JsonPropertyName("userEmail")]
    public string? UserEmail { get; set; }

    [JsonPropertyName("userId")]
    public string? UserId { get; set; }
}

public class IssueResponse
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }
}

public class MenuCardItem
{
    public string CategoryKey { get; set; } = string.Empty;
    public string CategoryDisplayName { get; set; } = string.Empty;
    public string MenuText { get; set; } = string.Empty;
    public string OperatingHours { get; set; } = string.Empty;
    public bool HasMenu { get; set; } = true;
}

public class MealGroupItem
{
    public string GroupTitle { get; set; } = string.Empty;
    public string OperatingHours { get; set; } = string.Empty;
    public List<MenuCardItem> Items { get; set; } = new();
}
