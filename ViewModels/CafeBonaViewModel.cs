using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CUK.Models;
using CUK.Services;

namespace CUK.ViewModels;

public partial class CafeBonaViewModel : ViewModelBase
{
    private readonly LocalizationService _loc = LocalizationService.Instance;

    [ObservableProperty]
    private ObservableCollection<MealGroupItem> _mealGroups = new();

    public void UpdateMenu(MenuData? menuData, string dateString, bool showOperatingHours)
    {
        MealGroups.Clear();

        if (menuData == null)
            return;

        var noMenuString = _loc.Get("no_menu");

        // Cafe Bona Group
        var items = new List<MenuCardItem>();
        AddCategoryItem(items, menuData, "Bona-Rice-Bowl", "category_rice_bowl", dateString, _loc.Get("operating_cafe_bona"), noMenuString);

        if (items.Count > 0)
        {
            MealGroups.Add(new MealGroupItem
            {
                GroupTitle = _loc.Get("screen_cafe_bona"),
                OperatingHours = showOperatingHours ? $"({_loc.Get("operating_cafe_bona")})" : string.Empty,
                Items = items
            });
        }
    }

    private void AddCategoryItem(
        List<MenuCardItem> list,
        MenuData menuData,
        string categoryKey,
        string nameLocKey,
        string dateString,
        string hours,
        string noMenuString)
    {
        var rawText = string.Empty;
        if (menuData.TryGetValue(categoryKey, out var dates) && dates.TryGetValue(dateString, out var val))
        {
            rawText = val;
        }

        var isNoMenu = string.IsNullOrWhiteSpace(rawText) || rawText.Trim().Equals("No Menu", StringComparison.OrdinalIgnoreCase);
        var menuDisplay = isNoMenu ? noMenuString : rawText.Trim();

        list.Add(new MenuCardItem
        {
            CategoryKey = categoryKey,
            CategoryDisplayName = _loc.Get(nameLocKey),
            MenuText = menuDisplay,
            OperatingHours = hours,
            HasMenu = !isNoMenu
        });
    }
}
