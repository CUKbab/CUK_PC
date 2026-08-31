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

            ["category_1000_won_morning"] = "1,000 KRW Morning",
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
            ["no_menu"] = "No menu available for this day.",
            ["loading_menu"] = "Loading menu...",
            ["failed_to_fetch_menu"] = "Failed to load menu. Showing cached menu if available.",
            
            ["display_settings"] = "Display Settings",
            ["theme_selection"] = "Theme",
            ["theme_light"] = "Light",
            ["theme_dark"] = "Dark",
            ["theme_system"] = "System Default",
            
            ["font_size_selection"] = "Font Size",
            ["font_size_small"] = "Small",
            ["font_size_medium"] = "Medium",
            ["font_size_large"] = "Large",

            ["language_selection"] = "Language",
            ["lang_system"] = "System Default",
            ["lang_ko"] = "한국어 (Korean)",
            ["lang_en"] = "English",
            ["lang_ja"] = "日本語 (Japanese)",
            ["lang_zh"] = "简体中文 (Chinese)",

            ["show_operating_hours"] = "Show Operating Hours",
            ["show_operating_hours_desc"] = "Display meal time badges next to category headers.",

            ["whats_new"] = "What's New!",
            ["feedback_support"] = "Feedback & Support",
            ["report_menu_error"] = "Report Menu Error",
            ["suggest_feature"] = "Suggest New Feature",
            ["report_bug"] = "Report a Bug",

            ["issue_title"] = "Title",
            ["issue_description"] = "Description",
            ["cancel"] = "Cancel",
            ["submit"] = "Submit",
            ["submitting"] = "Submitting...",
            ["close"] = "Close",
            ["back"] = "Back",

            ["report_success"] = "Report submitted successfully! Thank you.",
            ["report_duplicate"] = "This issue has already been reported.",
            ["report_error"] = "An error occurred while submitting the report.",

            ["about"] = "About CUK밥 PC",
            ["about_desc"] = "Cross-platform Catholic University of Korea cafeteria meal viewer.",
            ["github_source"] = "GitHub Repository"
        },
        ["ko"] = new()
        {
            ["app_name"] = "CUK밥",
            ["screen_buon_pranzo"] = "부온 프란초 (학생식당)",
            ["screen_cafe_bona"] = "카페 보나 (교직원식당)",
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
            ["no_menu"] = "해당 날짜에 등록된 식단이 없습니다.",
            ["loading_menu"] = "식단을 불러오는 중입니다...",
            ["failed_to_fetch_menu"] = "식단을 불러오는데 실패했습니다. 캐시된 식단이 표시됩니다.",

            ["display_settings"] = "화면 설정",
            ["theme_selection"] = "테마 설정",
            ["theme_light"] = "라이트 모드",
            ["theme_dark"] = "다크 모드",
            ["theme_system"] = "시스템 설정 따름",

            ["font_size_selection"] = "글자 크기",
            ["font_size_small"] = "작게",
            ["font_size_medium"] = "보통",
            ["font_size_large"] = "크게",

            ["language_selection"] = "언어 설정",
            ["lang_system"] = "시스템 설정 따름",
            ["lang_ko"] = "한국어",
            ["lang_en"] = "English (영어)",
            ["lang_ja"] = "日本語 (일본어)",
            ["lang_zh"] = "简体中文 (중국어)",

            ["show_operating_hours"] = "운영 시간 표시",
            ["show_operating_hours_desc"] = "식단 카테고리 헤더 옆에 배식 시간을 표시합니다.",

            ["whats_new"] = "업데이트 소식",
            ["feedback_support"] = "문의 및 피드백",
            ["report_menu_error"] = "식단 오류 제보",
            ["suggest_feature"] = "기능 제안",
            ["report_bug"] = "버그 제보",

            ["issue_title"] = "제목",
            ["issue_description"] = "내용",
            ["cancel"] = "취소",
            ["submit"] = "제출",
            ["submitting"] = "제출 중...",
            ["close"] = "닫기",
            ["back"] = "뒤로",

            ["report_success"] = "제보가 성공적으로 제출되었습니다!",
            ["report_duplicate"] = "이미 제보된 내용입니다.",
            ["report_error"] = "제보 제출 중 오류가 발생했습니다.",

            ["about"] = "CUK밥 PC 정보",
            ["about_desc"] = "가톨릭대학교 학식 조회 프로그램 (Windows / Linux / macOS)",
            ["github_source"] = "GitHub 저장소"
        },
        ["ja"] = new()
        {
            ["app_name"] = "CUK밥",
            ["screen_buon_pranzo"] = "ブオン・フランツォ",
            ["screen_cafe_bona"] = "カフェ・ボナ",
            ["screen_settings"] = "設定",

            ["category_1000_won_morning"] = "1000ウォンの朝食",
            ["category_korean_cuisine"] = "韓国料理",
            ["category_global_noodle"] = "グローバル・ヌードル",
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
            ["no_menu"] = "この日の献立はありません。",
            ["loading_menu"] = "献立を読み込み中...",
            ["failed_to_fetch_menu"] = "献立の取得に失敗しました。",

            ["display_settings"] = "表示設定",
            ["theme_selection"] = "テーマ設定",
            ["theme_light"] = "ライト",
            ["theme_dark"] = "ダーク",
            ["theme_system"] = "システム",

            ["font_size_selection"] = "フォントサイズ",
            ["font_size_small"] = "小",
            ["font_size_medium"] = "中",
            ["font_size_large"] = "大",

            ["language_selection"] = "言語選択",
            ["lang_system"] = "システム",
            ["lang_ko"] = "한국어 (韓国語)",
            ["lang_en"] = "English (英語)",
            ["lang_ja"] = "日本語",
            ["lang_zh"] = "简体中文 (中国語)",

            ["show_operating_hours"] = "営業時間を表示",
            ["show_operating_hours_desc"] = "カテゴリ見出しの横に食事時間を表示します。",

            ["whats_new"] = "最新情報",
            ["feedback_support"] = "フィードバックとサポート",
            ["report_menu_error"] = "メニューの誤りを報告",
            ["suggest_feature"] = "新機能を提案",
            ["report_bug"] = "バグを報告",

            ["issue_title"] = "タイトル",
            ["issue_description"] = "説明",
            ["cancel"] = "キャンセル",
            ["submit"] = "送信",
            ["submitting"] = "送信中...",
            ["close"] = "閉じる",
            ["back"] = "戻る",

            ["report_success"] = "レポートが送信されました！",
            ["report_duplicate"] = "この問題は既に報告されています。",
            ["report_error"] = "レポートの送信中にエラーが発生しました。",

            ["about"] = "CUK밥 PCについて",
            ["about_desc"] = "カトリック大学学食ビューアー (Windows / Linux / macOS)",
            ["github_source"] = "GitHub リポジトリ"
        },
        ["zh"] = new()
        {
            ["app_name"] = "CUK밥",
            ["screen_buon_pranzo"] = "Buon Pranzo (学生食堂)",
            ["screen_cafe_bona"] = "Cafe Bona (教工食堂)",
            ["screen_settings"] = "设置",

            ["category_1000_won_morning"] = "1000韩元早餐",
            ["category_korean_cuisine"] = "韩式料理",
            ["category_global_noodle"] = "国际面食",
            ["category_plus_corner"] = "加餐专区",
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
            ["no_menu"] = "该日期暂无菜单。",
            ["loading_menu"] = "正在加载菜单...",
            ["failed_to_fetch_menu"] = "获取菜单失败，显示缓存菜单。",

            ["display_settings"] = "显示设置",
            ["theme_selection"] = "主题设置",
            ["theme_light"] = "浅色模式",
            ["theme_dark"] = "深色模式",
            ["theme_system"] = "跟随系统",

            ["font_size_selection"] = "字体大小",
            ["font_size_small"] = "小",
            ["font_size_medium"] = "中",
            ["font_size_large"] = "大",

            ["language_selection"] = "语言选择",
            ["lang_system"] = "跟随系统",
            ["lang_ko"] = "한국어 (韩语)",
            ["lang_en"] = "English (英语)",
            ["lang_ja"] = "日本語 (日语)",
            ["lang_zh"] = "简体中文",

            ["show_operating_hours"] = "显示营业时间",
            ["show_operating_hours_desc"] = "在类别标题旁显示用餐时间段。",

            ["whats_new"] = "更新日志",
            ["feedback_support"] = "反馈与支持",
            ["report_menu_error"] = "反馈菜单错误",
            ["suggest_feature"] = "建议新功能",
            ["report_bug"] = "报告程序缺陷",

            ["issue_title"] = "标题",
            ["issue_description"] = "内容描述",
            ["cancel"] = "取消",
            ["submit"] = "提交",
            ["submitting"] = "正在提交...",
            ["close"] = "关闭",
            ["back"] = "返回",

            ["report_success"] = "反馈已成功提交！",
            ["report_duplicate"] = "此问题已有人反馈。",
            ["report_error"] = "提交反馈时出错。",

            ["about"] = "关于 CUK밥 PC",
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
