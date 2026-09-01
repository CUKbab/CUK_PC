using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CUK.Models;
using CUK.Services;

namespace CUK.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly SettingsService _settingsService = SettingsService.Instance;
    private readonly LocalizationService _loc = LocalizationService.Instance;
    private readonly ReporterService _reporterService = ReporterService.Instance;

    public LocalizationService Loc => _loc;

    [ObservableProperty]
    private int _selectedThemeIndex;

    [ObservableProperty]
    private int _selectedFontSizeIndex;

    [ObservableProperty]
    private int _selectedLanguageIndex;

    [ObservableProperty]
    private bool _showOperatingHours;

    // Modal Dialog states
    [ObservableProperty]
    private bool _isChangelogOpen;

    [ObservableProperty]
    private string _changelogContent = string.Empty;

    [ObservableProperty]
    private bool _isChangelogLoading;

    [ObservableProperty]
    private bool _isReporting;

    [ObservableProperty]
    private bool _isReportStatusOpen;

    [ObservableProperty]
    private bool _reportStatusSuccess;

    [ObservableProperty]
    private string _reportStatusTitle = string.Empty;

    [ObservableProperty]
    private string _reportStatusMessage = string.Empty;

    [ObservableProperty]
    private string? _reportStatusGithubUrl;

    public event Action? SettingsChanged;

    public SettingsViewModel()
    {
        _selectedThemeIndex = (int)_settingsService.Settings.Theme;
        _selectedFontSizeIndex = (int)_settingsService.Settings.FontSize;
        _showOperatingHours = _settingsService.Settings.ShowOperatingHours;

        _selectedLanguageIndex = _settingsService.Settings.Language switch
        {
            "ko" => 1,
            "en" => 2,
            "ja" => 3,
            "zh" => 4,
            _ => 0
        };
    }

    partial void OnSelectedThemeIndexChanged(int value)
    {
        var theme = (AppTheme)value;
        _settingsService.ApplyTheme(theme);
        SettingsChanged?.Invoke();
    }

    partial void OnSelectedFontSizeIndexChanged(int value)
    {
        var size = (FontSizeOption)value;
        _settingsService.SetFontSize(size);
        SettingsChanged?.Invoke();
    }

    partial void OnSelectedLanguageIndexChanged(int value)
    {
        var lang = value switch
        {
            1 => "ko",
            2 => "en",
            3 => "ja",
            4 => "zh",
            _ => "system"
        };
        _settingsService.SetLanguage(lang);
        SettingsChanged?.Invoke();
    }

    partial void OnShowOperatingHoursChanged(bool value)
    {
        _settingsService.SetShowOperatingHours(value);
        SettingsChanged?.Invoke();
    }

    [RelayCommand]
    public async Task OpenChangelog()
    {
        IsChangelogOpen = true;
        IsChangelogLoading = true;
        ChangelogContent = string.Empty;

        var content = await _reporterService.FetchChangelogAsync(_loc.EffectiveLanguage);
        ChangelogContent = !string.IsNullOrWhiteSpace(content) ? content : "No changelog available.";
        IsChangelogLoading = false;
    }

    [RelayCommand]
    public void CloseChangelog()
    {
        IsChangelogOpen = false;
    }

    [RelayCommand]
    public async Task ReportMenuError()
    {
        if (IsReporting) return;

        IsReporting = true;
        var (success, msg, githubUrl) = await _reporterService.ReportMenuErrorAsync();
        IsReporting = false;

        ReportStatusSuccess = success;
        ReportStatusTitle = success ? _loc.Get("report_success") : _loc.Get("report_error");
        ReportStatusMessage = msg;
        ReportStatusGithubUrl = githubUrl;
        IsReportStatusOpen = true;
    }

    [RelayCommand]
    public void CloseReportStatus()
    {
        IsReportStatusOpen = false;
    }

    [RelayCommand]
    public void OpenReportGithub()
    {
        if (!string.IsNullOrWhiteSpace(ReportStatusGithubUrl))
        {
            OpenUrl(ReportStatusGithubUrl);
        }
        else
        {
            OpenUrl("https://github.com/CUKbab");
        }
    }

    [RelayCommand]
    public void OpenGitHub()
    {
        OpenUrl("https://github.com/CUKbab");
    }

    private static void OpenUrl(string url)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }
        catch
        {
            // Ignore error
        }
    }
}
