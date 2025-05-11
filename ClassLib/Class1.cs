using ClassLib.Models;
using HtmlAgilityPack;
using Jint;
using Esprima;
using Jint.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ClassLib
{
    public class CodeValidator
    {
        public ValidationResult ValidateHtml(string html)
        {
            var result = new ValidationResult();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 1) Синтаксические ошибки
            foreach (var err in doc.ParseErrors)
                result.Errors.Add(new ValidationError
                {
                    Line = err.Line,
                    Column = err.LinePosition,
                    Message = err.Code + ": " + err.Reason
                });

            // 2) Структурные проверки
            bool hasHtml = doc.DocumentNode.SelectSingleNode("//html") != null;
            bool hasHead = doc.DocumentNode.SelectSingleNode("//head") != null;
            bool hasBody = doc.DocumentNode.SelectSingleNode("//body") != null;
            if (!hasHtml) result.Errors.Add(new ValidationError { Message = "Отсутствует <html>" });
            if (!hasHead) result.Errors.Add(new ValidationError { Message = "Отсутствует <head>" });
            if (!hasBody) result.Errors.Add(new ValidationError { Message = "Отсутствует <body>" });

            // 3) Если вообще нет видимых тегов — это тоже ошибка
            if (!doc.DocumentNode.ChildNodes.Any(n => n.Name != "#text" && n.Name != "#comment"))
                result.Errors.Add(new ValidationError { Message = "HTML не содержит тегов" });

            // 4) img без alt
            foreach (var img in doc.DocumentNode.SelectNodes("//img") ?? Enumerable.Empty<HtmlNode>())
            {
                if (img.GetAttributeValue("alt", null) == null)
                    result.Errors.Add(new ValidationError
                    {
                        Line = img.Line,
                        Column = img.LinePosition,
                        Message = "<img> без атрибута alt"
                    });
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }
        public ValidationResult ValidateCss(string css)
        {
            var result = new ValidationResult();

            // 0) Пустой или null
            if (string.IsNullOrWhiteSpace(css))
            {
                result.Errors.Add(new ValidationError
                {
                    Message = "CSS пустой"
                });
                result.IsValid = false;
                return result;
            }

            // 1) Баланс фигурных скобок
            int openBraces = css.Count(c => c == '{');
            int closeBraces = css.Count(c => c == '}');
            if (openBraces != closeBraces)
            {
                result.Errors.Add(new ValidationError
                {
                    Message = $"Несбалансированные скобки: '{{'={openBraces}, '}}'={closeBraces}"
                });
            }

            // 2) Находим первое '{' и первое '}'
            int firstOpen = css.IndexOf('{');
            int firstClose = css.IndexOf('}');

            // Если хотя бы один из индексов не валиден — сообщаем и НЕ лезем в Substring
            if (firstOpen < 0 || firstClose < 0 || firstClose < firstOpen)
    {
                result.Errors.Add(new ValidationError
                {
                    Message = "Отсутствует корректный блок правил вида {…}"
                });
            }
    else
            {
                // 3) Берём содержимое между скобками — здесь length гарантированно >=0
                int insideLen = firstClose - firstOpen - 1;  // >= 0
                var inside = css.Substring(firstOpen + 1, insideLen);

                // Проверяем базовые вещи внутри одного блока:
                if (!inside.Contains(":"))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Message = "Внутри блока нет двоеточия ':'"
                    });
                }
                if (!inside.Trim().EndsWith(";"))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Message = "Внутри блока отсутствует точка с запятой ';' в конце декларации"
                    });
                }
            }


            result.IsValid = !result.Errors.Any();
            return result;
        }
        public ValidationResult ValidateJs(string js)
        {
            var result = new ValidationResult();

            // 0) Баланс фигурных скобок — ловим отсутствие '}' до парсинга
            int openBraces = js.Count(c => c == '{');
            int closeBraces = js.Count(c => c == '}');
            if (openBraces != closeBraces)
            {
                result.Errors.Add(new ValidationError
                {
                    Line = 0,
                    Column = 0,
                    Message = $"Несбалансированные фигурные скобки: '{{'={openBraces}, '}}'={closeBraces}"
                });
                result.IsValid = false;
                return result;
            }

            // 1) Создаём движок и пытаемся парсить + выполнять
            var engine = new Engine(cfg => cfg.LimitRecursion(100).Strict());
            try
            {
                engine.Execute(js);
            }
            catch (ParserException ex)
            {
                // Синтаксические проблемы (включая неправильное ключевое слово func)
                result.Errors.Add(new ValidationError
                {
                    Line = ex.LineNumber,
                    Column = ex.Column,
                    Message = ex.Description
                });
                result.IsValid = false;
                return result;
            }
            catch (JavaScriptException jex)
            {
                // Runtime-ошибки, например ReferenceError
                var msgParts = jex.Message.Split(' ');
                var name = msgParts.Length > 0 ? msgParts[0] : "";
                int line = 1;
                // Ищем первое вхождение name в строках кода
                if (!string.IsNullOrEmpty(name))
                {
                    var lines = js.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                        if (lines[i].Contains(name))
                        {
                            line = i + 1;
                            break;
                        }
                }

                result.Errors.Add(new ValidationError
                {
                    Line = line,
                    Column = 0,
                    Message = jex.Message
                });
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }
    }
    public class CodeLinter
    {
        public LintResult LintHtml(string html, LintOptions opts)
        {
            var result = new LintResult();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 1) Теги только в нижнем регистре
            foreach (var node in doc.DocumentNode.Descendants()
                       .Where(n => n.NodeType == HtmlNodeType.Element))
            {
                var tag = node.OriginalName;
                if (tag != tag.ToLowerInvariant())
                    result.Error.Add(new LintError
                    {
                        RuleId = "lowercase-tags",
                        Message = $"Тег <{node.Name}> должен быть в нижнем регистре",
                        Line = node.Line,
                        Column = node.LinePosition
                    });
            }

            // 2) Уникальность id
            var seenIds = new HashSet<string>();
            foreach (var node in doc.DocumentNode.Descendants()
                       .Where(n => n.Attributes["id"] != null))
            {
                var id = node.Attributes["id"].Value;
                if (!seenIds.Add(id))
                    result.Error.Add(new LintError
                    {
                        RuleId = "duplicate-id",
                        Message = $"Дублированный id=\"{id}\"",
                        Line = node.Line,
                        Column = node.LinePosition
                    });
            }

            // 3) <img> должен иметь alt
            foreach (var img in doc.DocumentNode.Descendants("img"))
            {
                if (img.Attributes["alt"] == null)
                    result.Error.Add(new LintError
                    {
                        RuleId = "img-require-alt",
                        Message = "<img> без атрибута alt",
                        Line = img.Line,
                        Column = img.LinePosition
                    });
            }

            // 4) В <head> должен быть <title>
            if ((doc.DocumentNode.SelectSingleNode("//head/title") == null) && (doc.DocumentNode.SelectSingleNode("//head") != null))
            {
                var head = doc.DocumentNode.SelectSingleNode("//head");
                result.Error.Add(new LintError
                {
                    RuleId = "missing-title",
                    Message = "Отсутствует <title> в <head>",
                    Line = head?.Line ?? 0,
                    Column = head?.LinePosition ?? 0
                });
            }

            result.HasIssues = result.Error.Any();
            return result;
        }

        public LintResult LintCss(string css, LintOptions opts)
        {
            var result = new LintResult();
            var lines = css.Split(new[] { '\n' }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                // Числовое значение без единицы (кроме 0)
                if (line.Contains(":"))
                {
                    var parts = line.Split(':');
                    if (parts.Length > 1)
                    {
                        var value = parts[1].Trim().TrimEnd(';');
                        if (value != "0" && decimal.TryParse(value, out _))
                        {
                            result.Error.Add(new LintError
                            {
                                RuleId = "missing-unit",
                                Message = "Числовое значение без единицы измерения",
                                Line = i + 1,
                                Column = line.IndexOf(value) + 1
                            });
                        }
                    }
                }
            }

            result.HasIssues = result.Error.Any();
            return result;
        }

        public LintResult LintJs(string js, LintOptions opts)
        {
            var result = new LintResult();
            var lines = js.Split(new[] { '\n' }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var trimmed = line.Trim();

                // 1) console.log
                if (trimmed.Contains("console.log"))
                    result.Error.Add(new LintError
                    {
                        RuleId = "no-console",
                        Message = "Использование console.log",
                        Line = i + 1,
                        Column = trimmed.IndexOf("console.log") + 1
                    });

                // 2) Отсутствует ';' в конце строки кода
                if (!string.IsNullOrEmpty(trimmed) &&
                    !trimmed.EndsWith(";") &&
                    !trimmed.EndsWith("{") &&
                    !trimmed.EndsWith("}") &&
                    !trimmed.StartsWith("//") &&
                    !trimmed.StartsWith("/*"))
                {
                    result.Error.Add(new LintError
                    {
                        RuleId = "missing-semicolon-js",
                        Message = "Отсутствует ';' в конце JS-строки",
                        Line = i + 1,
                        Column = line.Length
                    });
                }
            }

            result.HasIssues = result.Error.Any();
            return result;
        }
    }
}
public class PerformanceAnalyzer
    {
        public PerformanceMetrics Measure(string fullHtml) => throw new NotImplementedException();
    }