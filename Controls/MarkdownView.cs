using System;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;

namespace CUK.Controls;

public class MarkdownView : StackPanel
{
    public static readonly StyledProperty<string?> MarkdownProperty =
        AvaloniaProperty.Register<MarkdownView, string?>(nameof(Markdown));

    public string? Markdown
    {
        get => GetValue(MarkdownProperty);
        set => SetValue(MarkdownProperty, value);
    }

    static MarkdownView()
    {
        MarkdownProperty.Changed.AddClassHandler<MarkdownView>((x, e) => x.OnMarkdownChanged());
    }

    public MarkdownView()
    {
        Spacing = 2;
    }

    private void OnMarkdownChanged()
    {
        Children.Clear();

        if (string.IsNullOrWhiteSpace(Markdown))
            return;

        var rawLines = Markdown.Replace("\r\n", "\n").Split('\n');
        bool inCodeBlock = false;
        var codeBlockContent = new System.Text.StringBuilder();

        foreach (var rawLine in rawLines)
        {
            var trimmed = rawLine.Trim();

            // Code block delimiter
            if (trimmed.StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    // End code block
                    var codeBorder = new Border
                    {
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(12, 10),
                        Margin = new Thickness(0, 6, 0, 8)
                    };
                    codeBorder.Bind(Border.BackgroundProperty, new DynamicResourceExtension("SidebarBackgroundBrush"));
                    codeBorder.Bind(Border.BorderBrushProperty, new DynamicResourceExtension("CardBorderBrush"));
                    codeBorder.BorderThickness = new Thickness(1);

                    var codeText = new TextBlock
                    {
                        Text = codeBlockContent.ToString().TrimEnd(),
                        FontFamily = new FontFamily("Cascadia Code, Consolas, Courier New, monospace"),
                        FontSize = 12,
                        TextWrapping = TextWrapping.Wrap
                    };
                    codeText.Bind(TextBlock.ForegroundProperty, new DynamicResourceExtension("TextControlForeground"));
                    codeBorder.Child = codeText;
                    Children.Add(codeBorder);

                    codeBlockContent.Clear();
                    inCodeBlock = false;
                }
                else
                {
                    inCodeBlock = true;
                    codeBlockContent.Clear();
                }
                continue;
            }

            if (inCodeBlock)
            {
                codeBlockContent.AppendLine(rawLine);
                continue;
            }

            // Empty line
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                Children.Add(new Control { Height = 4 });
                continue;
            }

            // Horizontal rule
            if (trimmed == "---" || trimmed == "***" || trimmed == "___")
            {
                var separator = new Border
                {
                    Height = 1,
                    Margin = new Thickness(0, 8, 0, 8)
                };
                separator.Bind(Border.BackgroundProperty, new DynamicResourceExtension("CardBorderBrush"));
                Children.Add(separator);
                continue;
            }

            // Headings
            if (trimmed.StartsWith("### "))
            {
                var text = trimmed.Substring(4).Trim();
                var tb = CreateHeadingTextBlock(text, 14, FontWeight.SemiBold, new Thickness(0, 10, 0, 4));
                Children.Add(tb);
                continue;
            }

            if (trimmed.StartsWith("## "))
            {
                var text = trimmed.Substring(3).Trim();
                var tb = CreateHeadingTextBlock(text, 16, FontWeight.Bold, new Thickness(0, 14, 0, 6));
                Children.Add(tb);
                continue;
            }

            if (trimmed.StartsWith("# "))
            {
                var text = trimmed.Substring(2).Trim();
                var tb = CreateHeadingTextBlock(text, 19, FontWeight.Bold, new Thickness(0, 4, 0, 8));
                Children.Add(tb);
                continue;
            }

            // Unordered list items: "- " or "* " or "+ "
            if (trimmed.StartsWith("- ") || trimmed.StartsWith("* ") || trimmed.StartsWith("+ "))
            {
                var itemText = trimmed.Substring(2).Trim();
                var listGrid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions("Auto,*"),
                    Margin = new Thickness(4, 2, 0, 4)
                };

                var bullet = new TextBlock
                {
                    Text = "•",
                    FontSize = 14,
                    FontWeight = FontWeight.Bold,
                    Margin = new Thickness(0, 0, 8, 0),
                    VerticalAlignment = VerticalAlignment.Top
                };
                bullet.Bind(TextBlock.ForegroundProperty, new DynamicResourceExtension("AppAccentBrush"));
                Grid.SetColumn(bullet, 0);

                var contentTb = new TextBlock
                {
                    FontSize = 13,
                    LineHeight = 20,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center
                };
                contentTb.Bind(TextBlock.ForegroundProperty, new DynamicResourceExtension("TextControlForeground"));
                PopulateInlines(contentTb, itemText);
                Grid.SetColumn(contentTb, 1);

                listGrid.Children.Add(bullet);
                listGrid.Children.Add(contentTb);
                Children.Add(listGrid);
                continue;
            }

            // Regular paragraph
            var paraTb = new TextBlock
            {
                FontSize = 13,
                LineHeight = 20,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 2, 0, 4)
            };
            paraTb.Bind(TextBlock.ForegroundProperty, new DynamicResourceExtension("TextControlForeground"));
            PopulateInlines(paraTb, trimmed);
            Children.Add(paraTb);
        }
    }

    private static TextBlock CreateHeadingTextBlock(string text, double fontSize, FontWeight fontWeight, Thickness margin)
    {
        var tb = new TextBlock
        {
            FontSize = fontSize,
            FontWeight = fontWeight,
            Margin = margin,
            TextWrapping = TextWrapping.Wrap
        };
        tb.Bind(TextBlock.ForegroundProperty, new DynamicResourceExtension("HeaderPrimaryBrush"));
        PopulateInlines(tb, text);
        return tb;
    }

    private static void PopulateInlines(TextBlock textBlock, string text)
    {
        textBlock.Inlines?.Clear();
        if (string.IsNullOrEmpty(text))
            return;

        var inlines = new InlineCollection();
        // Regex matches **bold**, *italic*, `code`, and [label](url)
        var pattern = @"(\*\*(.*?)\*\*|\*(.*?)\*|`(.*?)`|\[(.*?)\]\((.*?)\))";
        int lastIndex = 0;

        foreach (Match match in Regex.Matches(text, pattern))
        {
            if (match.Index > lastIndex)
            {
                inlines.Add(new Run(text.Substring(lastIndex, match.Index - lastIndex)));
            }

            if (match.Value.StartsWith("**") && match.Value.EndsWith("**"))
            {
                var bold = new Bold();
                bold.Inlines.Add(new Run(match.Groups[2].Value));
                inlines.Add(bold);
            }
            else if (match.Value.StartsWith("*") && match.Value.EndsWith("*"))
            {
                var italic = new Italic();
                italic.Inlines.Add(new Run(match.Groups[3].Value));
                inlines.Add(italic);
            }
            else if (match.Value.StartsWith("`") && match.Value.EndsWith("`"))
            {
                var codeSpan = new Span();
                codeSpan.FontFamily = new FontFamily("Cascadia Code, Consolas, Courier New, monospace");
                codeSpan.Inlines.Add(new Run(match.Groups[4].Value));
                inlines.Add(codeSpan);
            }
            else if (match.Value.StartsWith("[") && match.Value.Contains("](") && match.Value.EndsWith(")"))
            {
                var label = match.Groups[5].Value;
                var linkRun = new Run(label)
                {
                    TextDecorations = TextDecorations.Underline
                };
                linkRun.Bind(Run.ForegroundProperty, new DynamicResourceExtension("AppAccentBrush"));
                inlines.Add(linkRun);
            }

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < text.Length)
        {
            inlines.Add(new Run(text.Substring(lastIndex)));
        }

        textBlock.Inlines = inlines;
    }
}
