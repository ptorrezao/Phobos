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
            builder.AddCssClass("fa-" + value.ToLower());
            if (color != null)
            {
                builder.AddCssClass("text-" + color.ToString().ToLower());
            }
            return MvcHtmlString.Create(builder.ToString());
        }
        public static MvcHtmlString FontAwesome(this HtmlHelper htmlHelper, string link, string value, TextColor? color = null)
        {
            var builder = new TagBuilder("a");
            builder.InnerHtml = HtmlHelpers.FontAwesome(htmlHelper, value, color).ToString();
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
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            var value = "";
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
            {
                value = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2 * MINUTE)
            {
                value = "a minute ago";
            }
            if (delta < 45 * MINUTE)
            {
                value = ts.Minutes + " minutes ago";
            }
            if (delta < 90 * MINUTE)
            {
                value = "an hour ago";
            }
            if (delta < 24 * HOUR)
            {
                value = ts.Hours + " hours ago";
            }
            if (delta < 48 * HOUR)
            {
                value = "yesterday";
            }
            if (delta < 30 * DAY)
            {
                value = ts.Days + " days ago";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                value = months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                value = years <= 1 ? "one year ago" : years + " years ago";
            }

            return MvcHtmlString.Create(value);
        }
        public static HelperResult RenderHelper(this ViewDataDictionary<IGridPager> viewDataDictionary, string helperName)
        {
            Func<HelperResult> helper = viewDataDictionary[helperName] as Func<HelperResult>;
            if (helper != null)
            {
                return helper();
            }

            return null;
        }
        public static HelperResult SetPartialHelper(this ViewDataDictionary<MessageMailBoxFolderViewModel> viewDataDictionary, string helperName, Func<HelperResult> partial)
        {
            viewDataDictionary.Add(helperName, partial);
            return null;
        }
    }
}