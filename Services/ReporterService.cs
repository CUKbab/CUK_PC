using System;
using System.Collections.Generic;
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
    private const string ReporterEndpointUrl = "https://github-reporter.cukbab.workers.dev/";
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
        var langFiles = lang switch
        {
            "ko" => new[] { "kr", "ko" },
            "ja" => new[] { "ja" },
            "zh" => new[] { "zn", "zh" },
            _ => new[] { "en" }
        };

        foreach (var file in langFiles)
        {
            var urls = new[]
            {
                $"{BaseRawUrl}CUKbab/CUK/refs/heads/main/pc_changelogs/{file}.md",
                $"{BaseRawUrl}CUKbab/CUK/main/pc_changelogs/{file}.md"
            };

            foreach (var url in urls)
            {
                try
                {
                    var response = await _httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(content))
                            return content;
                    }
                }
                catch
                {
                    // Fallback to next URL
                }
            }
        }

        return lang switch
        {
            "ko" => "# CUK밥 PC v1.0.0\n\n## 새로운 기능\n- Linux, Windows 및 macOS 지원\n- 부온 프란조(Buon Pranzo) 및 카페 보나(Cafe Bona) 메뉴 확인\n- 다크 모드 / 라이트 모드 및 다국어 지원\n- 오프라인 캐싱 지원",
            "ja" => "# CUK밥 PC v1.0.0\n\n## 新機能\n- Linux、Windows、macOSのサポート\n- Buon PranzoとCafe Bonaのメニュー表示\n- ダークモード / ライトモードおよび多言語対応\n- オフラインキャッシュのサポート",
            "zh" => "# CUK밥 PC v1.0.0\n\n## 新功能\n- 支持 Linux、Windows 和 macOS\n- 查看 Buon Pranzo 和 Cafe Bona 的菜单\n- 深色模式 / 浅色模式及多语言支持\n- 离线缓存支持",
            _ => "# CUK밥 PC v1.0.0\n\n## New Features\n- Support for Linux, Windows, and macOS\n- View menus for Buon Pranzo and Cafe Bona\n- Dark mode / Light mode and multi-language Support\n- Offline caching support"
        };
    }

    public async Task<(bool Success, string Message, string? HtmlUrl)> ReportMenuErrorAsync()
    {
        var request = new IssueRequest
        {
            Title = "Menu Error",
            Body = "Menu Error",
            Labels = new List<string> { "menu-error", "reported-via-pc" }
        };

        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ReporterEndpointUrl, content);

            if ((int)response.StatusCode == 201 || (int)response.StatusCode == 200)
            {
                var respBody = await response.Content.ReadAsStringAsync();
                IssueResponse? issueResp = null;
                try
                {
                    issueResp = JsonSerializer.Deserialize<IssueResponse>(respBody);
                }
                catch { }

                return (true, LocalizationService.Instance.Get("report_success"), issueResp?.HtmlUrl);
            }
            else if ((int)response.StatusCode == 409 || (int)response.StatusCode == 422)
            {
                return (false, LocalizationService.Instance.Get("report_duplicate"), null);
            }
            else
            {
                return (false, LocalizationService.Instance.Get("report_error"), null);
            }
        }
        catch (Exception)
        {
            return (false, LocalizationService.Instance.Get("report_error"), null);
        }
    }
}

