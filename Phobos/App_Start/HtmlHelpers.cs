using NonFactors.Mvc.Grid;
using Phobos.Library.Models.Enums;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Phobos.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString FontAwesome(this HtmlHelper htmlHelper, string value, TextColor? color = null)
        {
            var builder = new TagBuilder("i");
            builder.AddCssClass("fa");
            if (value != null)
            {
                builder.AddCssClass("fa-" + value.ToLower());
            }
            if (color != null)
            {
                builder.AddCssClass("text-" + color.ToString().ToLower());
            }
            return MvcHtmlString.Create(builder.ToString());
        }
        public static MvcHtmlString FontAwesome(this HtmlHelper htmlHelper, string link, string value, TextColor? color = null)
        {
            var builder = new TagBuilder("a");
            if (value != null)
            {
                builder.InnerHtml = HtmlHelpers.FontAwesome(htmlHelper, value, color).ToString();
            }
            builder.Attributes["href"] = link;

            return MvcHtmlString.Create(builder.ToString());
        }
        public static MvcHtmlString FirstWordInBold(this HtmlHelper htmlHelper, string value)
        {
            var finalValue = value;
            var builder = new TagBuilder("b");
            var valueAlreadySetted = false;
            var preserveAcronyms = false;

            if (!string.IsNullOrWhiteSpace(value))
            {
                StringBuilder newText = new StringBuilder(value.Length * 2);
                newText.Append(value[0]);
                for (int i = 1; i < value.Length; i++)
                {
                    if (char.IsUpper(value[i]))
                    {
                        if ((value[i - 1] != ' ' && !char.IsUpper(value[i - 1]) && !valueAlreadySetted) ||
                            (preserveAcronyms && char.IsUpper(value[i - 1]) &&
                             i < value.Length - 1 && !char.IsUpper(value[i + 1]) && !valueAlreadySetted))
                        {
                            builder.SetInnerText(newText.ToString());
                            valueAlreadySetted = true;
                        }
                    }
                    newText.Append(value[i]);
                }

                if (string.IsNullOrEmpty(builder.InnerHtml))
                {
                    builder.SetInnerText(newText.ToString());
                }

                newText = newText.Replace(builder.InnerHtml, "");
                finalValue = builder.ToString(TagRenderMode.Normal) + newText;
            }

            return MvcHtmlString.Create(finalValue);
        }
        public static MvcHtmlString TimeAgo(this HtmlHelper htmlHelper, DateTime date)
        {
            var timeSpan = DateTime.Now - date;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return MvcHtmlString.Create(string.Format("{0} seconds ago", timeSpan.Seconds));

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return MvcHtmlString.Create(timeSpan.Minutes > 1 ? String.Format("about {0} minutes ago", timeSpan.Minutes) : "about a minute ago");

            if (timeSpan <= TimeSpan.FromHours(24))
                return MvcHtmlString.Create(timeSpan.Hours > 1 ? String.Format("about {0} hours ago", timeSpan.Hours) : "about an hour ago");

            if (timeSpan <= TimeSpan.FromDays(30))
                return MvcHtmlString.Create(timeSpan.Days > 1 ? String.Format("about {0} days ago", timeSpan.Days) : "yesterday");

            if (timeSpan <= TimeSpan.FromDays(365))
                return MvcHtmlString.Create(timeSpan.Days > 30 ? String.Format("about {0} months ago", timeSpan.Days / 30) : "about a month ago");

            return MvcHtmlString.Create(timeSpan.Days > 365 ? String.Format("about {0} years ago", timeSpan.Days / 365) : "about a year ago");

        }
        public static HelperResult SetPartialHelper(this HtmlHelper htmlHelper, string helperName, Func<HelperResult> partial)
        {
            htmlHelper.ViewData.Add(helperName, partial);
            return null;
        }
        public static HelperResult RenderHelper(this HtmlHelper htmlHelper, string helperName)
        {
            Func<HelperResult> helper = htmlHelper.ViewData[helperName] as Func<HelperResult>;
            if (helper != null)
            {
                return helper();
            }

            return null;
        }
    }
}