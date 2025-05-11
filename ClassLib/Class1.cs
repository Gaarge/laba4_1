using ClassLib.Models;
using HtmlAgilityPack;
using Jint;
using Esprima;
using Jint.Runtime;
using Jint;
using Esprima;


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
            if (firstOpen < 0 && firstClose < 0 && firstClose < firstOpen)
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
        public LintResult LintHtml(string html, LintOptions opts) => throw new NotImplementedException();
        public LintResult LintCss(string css, LintOptions opts) => throw new NotImplementedException();
        public LintResult LintJs(string js, LintOptions opts) => throw new NotImplementedException();
    }
    public class PerformanceAnalyzer
    {
        public PerformanceMetrics Measure(string fullHtml) => throw new NotImplementedException();
    }
}