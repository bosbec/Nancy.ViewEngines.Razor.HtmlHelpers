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

        private static string GetProperty()
        {

        }

        private static string GetDisplayNameFor(object obj, string propertyToGetNameFor)
        {
            // TODO: koll för null
            if (propertyToGetNameFor.Contains("."))
            {
                var propNames = propertyToGetNameFor.Split('.');
                var nameParts = new Queue<string>();

                foreach (var proppy in propNames)
                {
                    nameParts.Enqueue(proppy);
                }

                PropertyInfo propInfo;
                while (nameParts.Count > 0)
                {
                    var objType = obj.GetType();
                    propInfo = objType.GetProperty(nameParts.Dequeue());
                    var val = propInfo.GetValue(obj, null);
                }

                //var propertName = propertyToGetNameFor.Substring(0, propertyToGetNameFor.IndexOf("."));
                //var property = obj.GetType().GetProperties().FirstOrDefault(x => x.Name == propertName);
                //// todo null-check
                //var val = property.GetValue(obj, null);
                
                return GetDisplayNameFor("", propertyToGetNameFor.Substring(propertyToGetNameFor.IndexOf(".") + 1));
            }

            var attribute =
                obj.GetType()
                    .GetProperty(propertyToGetNameFor)
                    .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .FirstOrDefault();

            return attribute != null ? ((DisplayNameAttribute)attribute).DisplayName : string.Empty;
        }

        public static IHtmlString Label<TModel>(this HtmlHelpers<TModel> helper, string labelText, string labelFor, IDictionary<string, object> attributes)
        {
            var displayName = string.Empty;

            displayName = GetDisplayNameFor(helper.Model, labelText);

            if (String.IsNullOrEmpty(displayName) && string.IsNullOrWhiteSpace(labelText))
            {
                throw new ArgumentException("Argument_Cannot_Be_Null_Or_Empty", "labelText");
            }

            labelFor = labelFor ?? labelText;
            labelText = displayName ?? labelText;

            var tag = new TagBuilder("label")
            {
                InnerHtml = String.IsNullOrEmpty(labelText) ? String.Empty : HttpUtility.HtmlEncode(labelText)
            };

            if (!String.IsNullOrEmpty(labelFor))
            {
                tag.MergeAttribute("for", labelFor);
            }

            tag.MergeAttributes(attributes, false);

            return tag.ToHtmlString(TagRenderMode.Normal);
        }
    }
}
