using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CUK.Models;

namespace CUK.Services;

public class MenuService
{
    private static MenuService? _instance;
    public static MenuService Instance => _instance ??= new MenuService();

    private const string BaseRawUrl = "https://raw.githubusercontent.com/";
    private const string LatestMenuUrl = $"{BaseRawUrl}CUKbab/CUK_Menu/refs/heads/main/latest.json";
    
    private readonly HttpClient _httpClient;
    private readonly string _cacheDirectory;

    public MenuService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };
        _httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CUK_PC_Client/1.0");

        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _cacheDirectory = Path.Combine(appData, "CUK");
        Directory.CreateDirectory(_cacheDirectory);
    }

    public async Task<MenuData> GetMenuAsync(DateTime date, bool forceRefresh = false)
    {
        var dateString = date.ToString("yyyy-MM-dd");
        var cacheFilePath = GetCacheFilePath(date);

        if (!forceRefresh)
        {
            var cached = await LoadCachedMenuAsync(cacheFilePath);
            if (cached != null && HasDate(cached, dateString))
            {
                return cached;
            }
        }

        try
        {
            // 1. Try to fetch latest.json first
            var response = await _httpClient.GetStringAsync(LatestMenuUrl);
            var rawData = JsonSerializer.Deserialize<MenuData>(response);

            if (rawData != null)
            {
                var processed = ProcessMenuData(rawData);

                // 2. Check if latest.json contains the requested date
                if (HasDate(processed, dateString))
                {
                    await SaveCacheAsync(cacheFilePath, processed);
                    return processed;
                }
            }

            // 3. Fallback to archive if requested date is not in latest.json
            var archivedData = await FetchArchivedMenuAsync(date);
            if (archivedData != null)
            {
                var processed = ProcessMenuData(archivedData);
                await SaveCacheAsync(cacheFilePath, processed);
                return processed;
            }
        }
        catch (Exception)
        {
            // Fallback to cache if network fails
            var cached = await LoadCachedMenuAsync(cacheFilePath);
            if (cached != null)
            {
                return cached;
            }
            throw;
        }

        var finalFallback = await LoadCachedMenuAsync(cacheFilePath);
        return finalFallback ?? throw new Exception("Unable to load menu data for the selected date.");
    }

    private async Task<MenuData?> FetchArchivedMenuAsync(DateTime date)
    {
        var year = ISOWeek.GetYear(date);
        var week = ISOWeek.GetWeekOfYear(date);
        var archiveUrl = $"{BaseRawUrl}CUKbab/CUK_Menu/refs/heads/main/menus/{year}/{week}/menu.json";

        try
        {
            var response = await _httpClient.GetStringAsync(archiveUrl);
            return JsonSerializer.Deserialize<MenuData>(response);
        }
        catch
        {
            return null;
        }
    }

    private static bool HasDate(MenuData data, string dateString)
    {
        return data.Values.Any(dates => dates.ContainsKey(dateString));
    }

    private static MenuData ProcessMenuData(MenuData data)
    {
        var processed = new MenuData();
        foreach (var (category, dates) in data)
        {
            var dateMap = new Dictionary<string, string>();
            foreach (var (dateStr, menuText) in dates)
            {
                var cleanedText = (menuText ?? string.Empty)
                    .Replace("\\n", "\n")
                    .Trim();
                dateMap[dateStr] = cleanedText;
            }
            processed[category] = dateMap;
        }
        return processed;
    }

    private string GetCacheFilePath(DateTime date)
    {
        var year = ISOWeek.GetYear(date);
        var week = ISOWeek.GetWeekOfYear(date);
        return Path.Combine(_cacheDirectory, $"cached_menu_json_{year}_{week}.json");
    }

    private static async Task<MenuData?> LoadCachedMenuAsync(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                var json = await File.ReadAllTextAsync(path);
                var data = JsonSerializer.Deserialize<MenuData>(json);
                return data != null ? ProcessMenuData(data) : null;
            }
        }
        catch
        {
            // Ignore corrupted cache
        }
        return null;
    }

    private static async Task SaveCacheAsync(string path, MenuData data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(path, json);
        }
        catch
        {
            // Ignore cache write failures
        }
    }
}
