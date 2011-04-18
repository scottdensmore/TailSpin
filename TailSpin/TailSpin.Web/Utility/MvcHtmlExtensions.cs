




 




namespace TailSpin.Web.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using TailSpin.Web.Survey.Shared;

    public static class MvcHtmlExtensions
    {
        public static MvcHtmlString SurveyLink(this HtmlHelper htmlHelper, string linkText, string tenant, string surveySlug)
        {
            string publicSurveysWebsiteUrl = CloudConfiguration.GetConfigurationSetting("PublicSurveyWebsiteUrl", string.Empty, true);
            
            var surveyLink = string.Format(CultureInfo.InvariantCulture, "{0}/survey/{1}/{2}", publicSurveysWebsiteUrl, tenant, surveySlug);
            var tagBuilder = new TagBuilder("a");
            tagBuilder.InnerHtml = !string.IsNullOrEmpty(linkText) ? HttpUtility.HtmlEncode(linkText) : string.Empty;
            tagBuilder.MergeAttribute("href", surveyLink);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                values.Select(value => new SelectListItem
                                           {
                                               Text = CamelCaseToSentence(value.ToString()),
                                               Value = value.ToString(),
                                               Selected = value.Equals(metadata.Model)
                                           });

            return htmlHelper.DropDownListFor(
                expression,
                items);
        }

        private static string CamelCaseToSentence(this string value)
        {
            var sb = new StringBuilder();
            var firstWord = true;

            foreach (var match in Regex.Matches(value, "([A-Z][a-z]+)|[0-9]+"))
            {
                if (firstWord)
                {
                    sb.Append(match.ToString());
                    firstWord = false;
                }
                else
                {
                    sb.Append(" ");
                    sb.Append(match.ToString().ToLowerInvariant());
                }
            }

            return sb.ToString();
        }
    }
}