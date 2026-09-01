using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CUK.Services;

public class LocalizationService : INotifyPropertyChanged
{
    private static LocalizationService? _instance;
    public static LocalizationService Instance => _instance ??= new LocalizationService();

    private string _currentLanguage = "system";

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                OnPropertyChanged(string.Empty); // Notify all string properties
            }
        }
    }

    public string EffectiveLanguage
    {
        get
        {
            if (_currentLanguage != "system")
                return _currentLanguage;

            var current = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
            return current switch
            {
                "ko" => "ko",
                "ja" => "ja",
                "zh" => "zh",
                _ => "en"
            };
        }
    }

    private readonly Dictionary<string, Dictionary<string, string>> _strings = new()
    {
        ["en"] = new()
        {
            ["app_name"] = "CUK밥",
            ["screen_buon_pranzo"] = "Buon Pranzo",
            ["screen_cafe_bona"] = "Cafe Bona",
            ["screen_settings"] = "Settings",

            ["category_1000_won_morning"] = "1000 Won Morning",
            ["category_korean_cuisine"] = "Korean Cuisine",
            ["category_global_noodle"] = "Global Noodle",
            ["category_plus_corner"] = "Plus Corner",
            ["category_dinner"] = "Dinner",
            ["category_rice_bowl"] = "Rice Bowl",

            ["group_morning"] = "Morning",
            ["group_lunch"] = "Lunch",
            ["group_dinner"] = "Dinner",

            ["operating_morning"] = "08:00 ~ 09:30",
            ["operating_lunch"] = "11:30 ~ 14:00",
            ["operating_dinner"] = "17:30 ~ 19:00",
            ["operating_cafe_bona"] = "11:30 ~ 14:00",

            ["refresh"] = "Refresh",
            ["previous_day"] = "Previous Day",
            ["next_day"] = "Next Day",
            ["today"] = "Today",
            ["no_menu"] = "No Menu",
            ["loading_menu"] = "Loading menu...",
            ["failed_to_fetch_menu"] = "Failed to fetch menu",
            
            ["display_settings"] = "Display Settings",
            ["theme_selection"] = "Theme Selection",
            ["theme_light"] = "Light",
            ["theme_dark"] = "Dark",
            ["theme_system"] = "System",
            
            ["font_size_selection"] = "Font Size Selection",
            ["font_size_small"] = "Small",
            ["font_size_medium"] = "Medium",
            ["font_size_large"] = "Large",

            ["language_selection"] = "Language Selection",
            ["lang_system"] = "System Default",
            ["lang_ko"] = "한국어",
            ["lang_en"] = "English",
            ["lang_ja"] = "日本語",
            ["lang_zh"] = "简体中文",

            ["show_operating_hours"] = "Show Operating Hours",
            ["show_operating_hours_desc"] = "Display meal times next to category headers.",

            ["whats_new"] = "What's New!",
            ["feedback_support"] = "Feedback & Support",
            ["report_menu_error"] = "Report menu error",

            ["close"] = "Close",
            ["back"] = "Back",

            ["report_success"] = "Report submitted successfully!",
            ["report_duplicate"] = "This issue has already been reported.",
            ["report_error"] = "An error occurred while submitting the report.",
            ["open_github"] = "Open on GitHub",

            ["about"] = "About",
            ["about_desc"] = "Catholic University of Korea cafeteria meal viewer.",
            ["github_source"] = "GitHub Repository"
        },
        ["ko"] = new()
        {
            ["app_name"] = "CUK밥",
            ["screen_buon_pranzo"] = "부온 프란조",
            ["screen_cafe_bona"] = "카페 보나",
            ["screen_settings"] = "설정",

            ["category_1000_won_morning"] = "천원의 아침",
            ["category_korean_cuisine"] = "한식",
            ["category_global_noodle"] = "글로벌 & 누들",
            ["category_plus_corner"] = "플러스 코너",
            ["category_dinner"] = "석식",
            ["category_rice_bowl"] = "덮밥",

            ["group_morning"] = "조식",
            ["group_lunch"] = "중식",
            ["group_dinner"] = "석식",

            ["operating_morning"] = "08:00 ~ 09:30",
            ["operating_lunch"] = "11:30 ~ 14:00",
            ["operating_dinner"] = "17:30 ~ 19:00",
            ["operating_cafe_bona"] = "11:30 ~ 14:00",

            ["refresh"] = "새로고침",
            ["previous_day"] = "이전 날짜",
            ["next_day"] = "다음 날짜",
            ["today"] = "오늘",
            ["no_menu"] = "메뉴 없음",
            ["loading_menu"] = "식단을 불러오는 중입니다...",
            ["failed_to_fetch_menu"] = "메뉴를 불러오는데 실패했습니다",

            ["display_settings"] = "화면 설정",
            ["theme_selection"] = "테마 설정",
            ["theme_light"] = "라이트",
            ["theme_dark"] = "다크",
            ["theme_system"] = "시스템 설정",

            ["font_size_selection"] = "폰트 크기 설정",
            ["font_size_small"] = "작게",
            ["font_size_medium"] = "보통",
            ["font_size_large"] = "크게",

            ["language_selection"] = "언어 설정",
            ["lang_system"] = "시스템 기본값",
            ["lang_ko"] = "한국어",
            ["lang_en"] = "English",
            ["lang_ja"] = "日本語",
            ["lang_zh"] = "简体中文",

            ["show_operating_hours"] = "운영 시간 표시",
            ["show_operating_hours_desc"] = "카테고리 제목 옆에 식사 시간을 표시합니다.",

            ["whats_new"] = "업데이트 소식",
            ["feedback_support"] = "문의 및 피드백",
            ["report_menu_error"] = "식단 오류 제보",

            ["close"] = "닫기",
            ["back"] = "뒤로",

            ["report_success"] = "제보가 성공적으로 제출되었습니다!",
            ["report_duplicate"] = "이미 제보된 내용입니다.",
            ["report_error"] = "제보 제출 중 오류가 발생했습니다.",
            ["open_github"] = "GitHub에서 열기",

            ["about"] = "정보",
            ["about_desc"] = "가톨릭대학교 학식 조회 프로그램 (Windows / Linux / macOS)",
            ["github_source"] = "GitHub 저장소"
        },
        ["ja"] = new()
        {
            ["app_name"] = "CUK밥",
            ["screen_buon_pranzo"] = "Buon Pranzo",
            ["screen_cafe_bona"] = "Cafe Bona",
            ["screen_settings"] = "設定",

            ["category_1000_won_morning"] = "1000ウォンの朝食",
            ["category_korean_cuisine"] = "韓国料理",
            ["category_global_noodle"] = "Global Noodle",
            ["category_plus_corner"] = "プラスコーナー",
            ["category_dinner"] = "夕食",
            ["category_rice_bowl"] = "丼物",

            ["group_morning"] = "朝食",
            ["group_lunch"] = "昼食",
            ["group_dinner"] = "夕食",

            ["operating_morning"] = "08:00 ~ 09:30",
            ["operating_lunch"] = "11:30 ~ 14:00",
            ["operating_dinner"] = "17:30 ~ 19:00",
            ["operating_cafe_bona"] = "11:30 ~ 14:00",

            ["refresh"] = "更新",
            ["previous_day"] = "前日",
            ["next_day"] = "翌日",
            ["today"] = "今日",
            ["no_menu"] = "メニューなし",
            ["loading_menu"] = "献立を読み込み中...",
            ["failed_to_fetch_menu"] = "メニューの取得に失敗しました",

            ["display_settings"] = "表示設定",
            ["theme_selection"] = "テーマ選択",
            ["theme_light"] = "ライト",
            ["theme_dark"] = "ダーク",
            ["theme_system"] = "システム",

            ["font_size_selection"] = "フォントサイズ選択",
            ["font_size_small"] = "小",
            ["font_size_medium"] = "中",
            ["font_size_large"] = "大",

            ["language_selection"] = "言語選択",
            ["lang_system"] = "システムデフォルト",
            ["lang_ko"] = "한국어",
            ["lang_en"] = "English",
            ["lang_ja"] = "日本語",
            ["lang_zh"] = "简体中文",

            ["show_operating_hours"] = "営業時間を表示",
            ["show_operating_hours_desc"] = "カテゴリ見出しの横に食事時間を表示します。",

            ["whats_new"] = "最新情報",
            ["feedback_support"] = "フィードバックとサポート",
            ["report_menu_error"] = "メニューの誤りを報告",

            ["close"] = "閉じる",
            ["back"] = "戻る",

            ["report_success"] = "レポートが送信されました！",
            ["report_duplicate"] = "この問題は既に報告されています。",
            ["report_error"] = "レポートの送信中にエラーが発生しました。",
            ["open_github"] = "GitHubで開く",

            ["about"] = "このアプリについて",
            ["about_desc"] = "カトリック大学学食ビューアー (Windows / Linux / macOS)",
            ["github_source"] = "GitHub リポジトリ"
        },
        ["zh"] = new()
        {
            ["app_name"] = "CUK밥",
            ["screen_buon_pranzo"] = "Buon Pranzo",
            ["screen_cafe_bona"] = "Cafe Bona",
            ["screen_settings"] = "设置",

            ["category_1000_won_morning"] = "1000韩元早餐",
            ["category_korean_cuisine"] = "韩餐",
            ["category_global_noodle"] = "Global Noodle",
            ["category_plus_corner"] = "Plus角落",
            ["category_dinner"] = "晚餐",
            ["category_rice_bowl"] = "盖饭",

            ["group_morning"] = "早餐",
            ["group_lunch"] = "午餐",
            ["group_dinner"] = "晚餐",

            ["operating_morning"] = "08:00 ~ 09:30",
            ["operating_lunch"] = "11:30 ~ 14:00",
            ["operating_dinner"] = "17:30 ~ 19:00",
            ["operating_cafe_bona"] = "11:30 ~ 14:00",

            ["refresh"] = "刷新",
            ["previous_day"] = "前一天",
            ["next_day"] = "后一天",
            ["today"] = "今天",
            ["no_menu"] = "暂无菜单",
            ["loading_menu"] = "正在加载菜单...",
            ["failed_to_fetch_menu"] = "获取菜单失败",

            ["display_settings"] = "显示设置",
            ["theme_selection"] = "主题选择",
            ["theme_light"] = "浅色",
            ["theme_dark"] = "深色",
            ["theme_system"] = "系统默认",

            ["font_size_selection"] = "字体大小选择",
            ["font_size_small"] = "小",
            ["font_size_medium"] = "中",
            ["font_size_large"] = "大",

            ["language_selection"] = "语言选择",
            ["lang_system"] = "系统默认",
            ["lang_ko"] = "한국어",
            ["lang_en"] = "English",
            ["lang_ja"] = "日本語",
            ["lang_zh"] = "简体中文",

            ["show_operating_hours"] = "显示营业时间",
            ["show_operating_hours_desc"] = "在分类标题旁显示用餐时间。",

            ["whats_new"] = "更新日志",
            ["feedback_support"] = "反馈与支持",
            ["report_menu_error"] = "报告菜单错误",

            ["close"] = "关闭",
            ["back"] = "返回",

            ["report_success"] = "反馈已成功提交！",
            ["report_duplicate"] = "此问题已被报告过。",
            ["report_error"] = "提交反馈时出错。",
            ["open_github"] = "在 GitHub 上查看",

            ["about"] = "关于",
            ["about_desc"] = "韩国天主教大学食堂菜单查询器 (Windows / Linux / macOS)",
            ["github_source"] = "GitHub 源码仓库"
        }
    };

    public string this[string key] => Get(key);

    public string Get(string key)
    {
        var lang = EffectiveLanguage;
        if (_strings.TryGetValue(lang, out var langDict) && langDict.TryGetValue(key, out var value))
        {
            return value;
        }

        if (_strings["en"].TryGetValue(key, out var enValue))
        {
            return enValue;
        }

        return key;
    }

    public string Format(string key, params object[] args)
    {
        var template = Get(key);
        try
        {
            return string.Format(template, args);
        }
        catch
        {
            return template;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
