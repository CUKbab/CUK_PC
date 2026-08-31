using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CUK.Models;
using CUK.Services;

namespace CUK.ViewModels;

public partial class BuonPranzoViewModel : ViewModelBase
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

        // Morning Group
        var morningItems = new List<MenuCardItem>();
        AddCategoryItem(morningItems, menuData, "Morning", "category_1000_won_morning", dateString, _loc.Get("operating_morning"), noMenuString);

        if (morningItems.Count > 0)
        {
            MealGroups.Add(new MealGroupItem
            {
                GroupTitle = _loc.Get("group_morning"),
                OperatingHours = showOperatingHours ? $"({_loc.Get("operating_morning")})" : string.Empty,
                Items = morningItems
            });
        }

        // Lunch Group
        var lunchItems = new List<MenuCardItem>();
        AddCategoryItem(lunchItems, menuData, "Pranzo-Korean", "category_korean_cuisine", dateString, _loc.Get("operating_lunch"), noMenuString);
        AddCategoryItem(lunchItems, menuData, "Pranzo-Global-Noodle", "category_global_noodle", dateString, _loc.Get("operating_lunch"), noMenuString);
        AddCategoryItem(lunchItems, menuData, "Pranzo-Plus-Corner", "category_plus_corner", dateString, _loc.Get("operating_lunch"), noMenuString);

        if (lunchItems.Count > 0)
        {
            MealGroups.Add(new MealGroupItem
            {
                GroupTitle = _loc.Get("group_lunch"),
                OperatingHours = showOperatingHours ? $"({_loc.Get("operating_lunch")})" : string.Empty,
                Items = lunchItems
            });
        }

        // Dinner Group
        var dinnerItems = new List<MenuCardItem>();
        AddCategoryItem(dinnerItems, menuData, "Pranzo-Dinner", "category_dinner", dateString, _loc.Get("operating_dinner"), noMenuString);

        if (dinnerItems.Count > 0)
        {
            MealGroups.Add(new MealGroupItem
            {
                GroupTitle = _loc.Get("group_dinner"),
                OperatingHours = showOperatingHours ? $"({_loc.Get("operating_dinner")})" : string.Empty,
                Items = dinnerItems
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
