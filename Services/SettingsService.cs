using System;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Styling;
using CUK.Models;

namespace CUK.Services;

public class SettingsService
{
    private static SettingsService? _instance;
    public static SettingsService Instance => _instance ??= new SettingsService();

    private readonly string _settingsFilePath;
    private AppSettings _settings = new();

    public AppSettings Settings => _settings;

    public SettingsService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "CUK");
        Directory.CreateDirectory(dir);
        _settingsFilePath = Path.Combine(dir, "settings.json");

        LoadSettings();
    }

    public void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsFilePath))
            {
                var json = File.ReadAllText(_settingsFilePath);
                var loaded = JsonSerializer.Deserialize<AppSettings>(json);
                if (loaded != null)
                {
                    _settings = loaded;
                }
            }
        }
        catch
        {
            _settings = new AppSettings();
        }

        ApplyTheme(_settings.Theme);
        LocalizationService.Instance.CurrentLanguage = _settings.Language;
    }

    public void SaveSettings()
    {
        try
        {
            var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsFilePath, json);
        }
        catch
        {
            // Ignore write errors
        }
    }

    public void ApplyTheme(AppTheme theme)
    {
        _settings.Theme = theme;
        if (Application.Current != null)
        {
            Application.Current.RequestedThemeVariant = theme switch
            {
                AppTheme.Light => ThemeVariant.Light,
                AppTheme.Dark => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
        }
        SaveSettings();
    }

    public void SetLanguage(string lang)
    {
        _settings.Language = lang;
        LocalizationService.Instance.CurrentLanguage = lang;
        SaveSettings();
    }

    public void SetFontSize(FontSizeOption size)
    {
        _settings.FontSize = size;
        SaveSettings();
    }

    public void SetShowOperatingHours(bool show)
    {
        _settings.ShowOperatingHours = show;
        SaveSettings();
    }
}
