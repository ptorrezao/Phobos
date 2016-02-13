using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Phobos.Helpers
{
    public static class HtmlHelpers
    {
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
    }
}