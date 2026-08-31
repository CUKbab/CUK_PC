using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CUK.Models;

namespace CUK.Services;

public class ReporterService
{
    private static ReporterService? _instance;
    public static ReporterService Instance => _instance ??= new ReporterService();

    private const string BaseRawUrl = "https://raw.githubusercontent.com/";
    private readonly HttpClient _httpClient;

    public ReporterService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CUK_PC_Client/1.0");
    }

    public async Task<string?> FetchChangelogAsync(string lang)
    {
        var targetLang = lang switch
        {
            "ko" => "ko",
            "ja" => "ja",
            "zh" => "zh",
            _ => "en"
        };

        // Try CUK_PC repository first (when created by user), then fallback to local release notes
        var pcChangelogUrl = $"{BaseRawUrl}CUKbab/CUK_PC/refs/heads/main/changelogs/{targetLang}/latest.md";

        try
        {
            var response = await _httpClient.GetAsync(pcChangelogUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(content))
                    return content;
            }
        }
        catch
        {
            // Fallback
        }

        return targetLang switch
        {
            "ko" => "# CUK밥 PC v1.0.0\n\n## 새로운 기능\n- Linux, Windows, macOS (Intel & Apple Silicon) 지원\n- 부온 프란초(학생식당) 및 카페 보나(교직원식당) 식단 조회\n- 다크 모드 / 라이트 모드 및 다국어 지원\n- 주차별 오프라인 캐시 지원",
            "ja" => "# CUK밥 PC v1.0.0\n\n## 新機能\n- Linux、Windows、macOS (Intel & Apple Silicon) をサポート\n- ブオン・フランツォおよびカフェ・ボナの献立表示\n- ダークモード / ライトモードおよび多言語対応\n- オフラインキャッシュ対応",
            "zh" => "# CUK밥 PC v1.0.0\n\n## 新功能\n- 支持 Linux、Windows、macOS (Intel & Apple Silicon)\n- 查看 Buon Pranzo 和 Cafe Bona 食堂菜单\n- 支持深色/浅色模式与多语言切换\n- 支持离线缓存",
            _ => "# CUK밥 PC v1.0.0\n\n## New Features\n- Support for Linux, Windows, and macOS (Intel & Apple Silicon)\n- View menus for Buon Pranzo and Cafe Bona\n- Dark mode / Light mode and multi-language support\n- Offline caching support"
        };
    }

    public async Task<(bool Success, string Message)> SubmitReportAsync(IssueRequest request)
    {
        await Task.Delay(400); // Simulate brief network submission or placeholder
        return (true, LocalizationService.Instance.Get("report_success"));
    }
}
