using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nancy.Helpers;
using Nancy.ViewEngines.Razor.HtmlHelpersUnofficial.MvcBits;

namespace Nancy.ViewEngines.Razor.HtmlHelpersUnofficial
{
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class HtmlHelpersLabelExtensions
    {
        public static IHtmlString LabelFor<TModel, TProperty>(this HtmlHelpers<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return Label(helper, ExpressionHelper.GetExpressionText(expression));
        }

        public static IHtmlString LabelFor<TModel, TProperty>(this HtmlHelpers<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return Label(helper, ExpressionHelper.GetExpressionText(expression), htmlAttributes);
        }

        public static IHtmlString LabelFor<TModel, TProperty>(this HtmlHelpers<TModel> helper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            return Label(helper, ExpressionHelper.GetExpressionText(expression), TypeHelper.ObjectToDictionary(htmlAttributes));
        }

        public static IHtmlString Label<TModel>(this HtmlHelpers<TModel> helper, string labelText)
        {
            return Label(helper, labelText, null, null);
        }

        public static IHtmlString Label<TModel>(this HtmlHelpers<TModel> helper, string labelText, string labelFor)
        {
            return Label(helper, labelText, labelFor, null);
        }

        public static IHtmlString Label<TModel>(this HtmlHelpers<TModel> helper, string labelText, object attributes)
        {
            return Label(helper, labelText, null, TypeHelper.ObjectToDictionary(attributes));
        }

        public static IHtmlString Label<TModel>(this HtmlHelpers<TModel> helper, string labelText, string labelFor, object attributes)
        {
            return Label(helper, labelText, labelFor, TypeHelper.ObjectToDictionary(attributes));
        }

        //private static string GetProperty()
        //{

        //}

        private static string GetDisplayNameFor(object obj, string propertyToGetDisplayNameFor)
        {
            if (propertyToGetDisplayNameFor.Contains("."))
            {
                var propertyList = propertyToGetDisplayNameFor.Split('.').ToList();
                var propertyToCheck = propertyList.FirstOrDefault();
                propertyList.RemoveAll(x => x.Equals(propertyToCheck));

                var property = obj.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyToCheck);
                if (property != null)
                {
                    var nextLevel = property.GetValue(obj, null);
                    var properties = string.Join(".", propertyList.Select(x => x));

                    return GetDisplayNameFor(nextLevel, properties);
                }
            }

            var attribute =
                obj.GetType()
                    .GetProperty(propertyToGetDisplayNameFor)
                    .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .FirstOrDefault();

            return attribute != null ? ((DisplayNameAttribute)attribute).DisplayName : string.Empty;
        }

        public static IHtmlString Label<TModel>(this HtmlHelpers<TModel> helper, string labelText, string labelFor, IDictionary<string, object> attributes)
        {
            string displayName = GetDisplayNameFor(helper.Model, labelText);

            if (string.IsNullOrEmpty(displayName) && string.IsNullOrWhiteSpace(labelText))
            {
                throw new ArgumentException("Argument_Cannot_Be_Null_Or_Empty", "labelText");
            }

            labelFor = labelFor ?? labelText;
            if (!string.IsNullOrEmpty(displayName))
            {
                labelText = displayName;
            }

            var tag = new TagBuilder("label")
            {
                InnerHtml = string.IsNullOrEmpty(labelText) ? string.Empty : HttpUtility.HtmlEncode(labelText)
            };

            if (!string.IsNullOrEmpty(labelFor))
            {
                tag.MergeAttribute("for", labelFor);
            }

            tag.MergeAttributes(attributes, false);

            return tag.ToHtmlString(TagRenderMode.Normal);
        }
    }
}
