using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CUK.Models;
using CUK.Services;

namespace CUK.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly MenuService _menuService = MenuService.Instance;
    private readonly SettingsService _settingsService = SettingsService.Instance;
    private readonly LocalizationService _loc = LocalizationService.Instance;

    public LocalizationService Loc => _loc;

    public BuonPranzoViewModel BuonPranzoVm { get; } = new();
    public CafeBonaViewModel CafeBonaVm { get; } = new();
    public SettingsViewModel SettingsVm { get; } = new();

    [ObservableProperty]
    private int _selectedScreenIndex = 0; // 0 = BuonPranzo, 1 = CafeBona, 2 = Settings

    [ObservableProperty]
    private ViewModelBase _currentView;

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private string _formattedDate = string.Empty;

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private double _baseFontSize = 14.0;

    [ObservableProperty]
    private bool _canGoPreviousDay = true;

    [ObservableProperty]
    private bool _canGoNextDay = true;

    private MenuData? _menuData;

    public MainViewModel()
    {
        _currentView = BuonPranzoVm;

        // Skip weekend to Monday
        if (_selectedDate.DayOfWeek == DayOfWeek.Saturday)
        {
            _selectedDate = _selectedDate.AddDays(2);
        }
        else if (_selectedDate.DayOfWeek == DayOfWeek.Sunday)
        {
            _selectedDate = _selectedDate.AddDays(1);
        }

        UpdateFormattedDate();
        UpdateDayNavigationState();
        UpdateFontSize();

        SettingsVm.SettingsChanged += OnSettingsChanged;

        _ = LoadMenuAsync(forceRefresh: false);
    }

    private void OnSettingsChanged()
    {
        UpdateFormattedDate();
        UpdateFontSize();
        UpdateCurrentMenuView();
    }

    private void UpdateFontSize()
    {
        BaseFontSize = _settingsService.Settings.FontSize switch
        {
            FontSizeOption.Small => 12.0,
            FontSizeOption.Large => 16.0,
            _ => 14.0
        };
    }

    partial void OnSelectedScreenIndexChanged(int value)
    {
        CurrentView = value switch
        {
            0 => BuonPranzoVm,
            1 => CafeBonaVm,
            2 => SettingsVm,
            _ => BuonPranzoVm
        };
        UpdateCurrentMenuView();
    }

    [RelayCommand]
    public void SelectScreen(string indexStr)
    {
        if (int.TryParse(indexStr, out var index))
        {
            SelectedScreenIndex = index;
        }
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        UpdateFormattedDate();
        UpdateDayNavigationState();
        _ = LoadMenuAsync(forceRefresh: false);
    }

    private void UpdateDayNavigationState()
    {
        CanGoPreviousDay = SelectedDate.DayOfWeek != DayOfWeek.Monday;
        CanGoNextDay = SelectedDate.DayOfWeek != DayOfWeek.Friday;
    }

    private void UpdateFormattedDate()
    {
        try
        {
            var culture = new CultureInfo(_loc.EffectiveLanguage switch
            {
                "ko" => "ko-KR",
                "ja" => "ja-JP",
                "zh" => "zh-CN",
                _ => "en-US"
            });
            FormattedDate = SelectedDate.ToString("yyyy-MM-dd (ddd)", culture);
        }
        catch
        {
            FormattedDate = SelectedDate.ToString("yyyy-MM-dd");
        }
    }

    public async Task LoadMenuAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && _menuData != null && HasDateForSelected(_menuData))
        {
            UpdateCurrentMenuView();
            return;
        }

        IsLoading = true;
        HasError = false;
        ErrorMessage = null;

        try
        {
            _menuData = await _menuService.GetMenuAsync(SelectedDate, forceRefresh);
            HasError = false;
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = _loc.Get("failed_to_fetch_menu") + $"\n({ex.Message})";
        }
        finally
        {
            IsLoading = false;
            UpdateCurrentMenuView();
        }
    }

    private bool HasDateForSelected(MenuData data)
    {
        var dateString = SelectedDate.ToString("yyyy-MM-dd");
        foreach (var categoryMap in data.Values)
        {
            if (categoryMap.ContainsKey(dateString))
                return true;
        }
        return false;
    }

    [RelayCommand]
    public async Task RefreshMenu()
    {
        if (IsRefreshing) return;

        IsRefreshing = true;
        HasError = false;
        ErrorMessage = null;

        try
        {
            _menuData = await _menuService.GetMenuAsync(SelectedDate, forceRefresh: true);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = _loc.Get("failed_to_fetch_menu") + $"\n({ex.Message})";
        }
        finally
        {
            IsRefreshing = false;
            UpdateCurrentMenuView();
        }
    }

    [RelayCommand]
    public void PreviousDay()
    {
        if (CanGoPreviousDay)
        {
            SelectedDate = SelectedDate.AddDays(-1);
        }
    }

    [RelayCommand]
    public void NextDay()
    {
        if (CanGoNextDay)
        {
            SelectedDate = SelectedDate.AddDays(1);
        }
    }

    [RelayCommand]
    public void Today()
    {
        var today = DateTime.Today;
        if (today.DayOfWeek == DayOfWeek.Saturday)
            today = today.AddDays(2);
        else if (today.DayOfWeek == DayOfWeek.Sunday)
            today = today.AddDays(1);

        SelectedDate = today;
    }

    private void UpdateCurrentMenuView()
    {
        var dateString = SelectedDate.ToString("yyyy-MM-dd");
        var showHours = _settingsService.Settings.ShowOperatingHours;

        BuonPranzoVm.UpdateMenu(_menuData, dateString, showHours);
        CafeBonaVm.UpdateMenu(_menuData, dateString, showHours);
    }
}
