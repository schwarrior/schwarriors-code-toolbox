using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

public static class HtmlHelpers
{

    private static MvcHtmlString AttributeInjectionHelper(MvcHtmlString baseEditorResult, dynamic htmlAttributes)
    {
        MvcHtmlString attribResult = baseEditorResult;
        //do not append if editor helper produced a hidden field
        if(!baseEditorResult.ToString().Contains("type=\"hidden\""))
        {
            IDictionary<string, Object> htmlAttributeDict = Utilities.DynamicToDictionary(htmlAttributes);
            attribResult = Utilities.AddHtmlAttributes(baseEditorResult, htmlAttributeDict);
        }
        //remove silly html encode from result
        //eg if the textbox value has an & it will become &amp;
        MvcHtmlString abbribResultDecoded =
            new MvcHtmlString(HttpContext.Current.Server.HtmlDecode(attribResult.ToString()));

        return abbribResultDecoded;
    }
    
    public static MvcHtmlString EditorWithAttributeInjection(
        this HtmlHelper html,
        string expression,
        dynamic htmlAttributes
        )
    {
        MvcHtmlString baseEditorResult = html.Editor(expression);
        return !String.IsNullOrEmpty(baseEditorResult.ToString()) ? AttributeInjectionHelper(baseEditorResult, htmlAttributes) : baseEditorResult;
    }

    public static MvcHtmlString EditorWithAttributeInjection(
        this HtmlHelper html,
        string expression,
        Object additionalViewData,
        dynamic htmlAttributes
    )
    {
        MvcHtmlString baseEditorResult = html.Editor(expression, additionalViewData);
        return AttributeInjectionHelper(baseEditorResult, htmlAttributes);
    }

    public static MvcHtmlString EditorWithAttributeInjection(
        this HtmlHelper html,
        string expression,
        string templateName,
        dynamic htmlAttributes
    )
    {
        MvcHtmlString baseEditorResult = html.Editor(expression, templateName);
        return AttributeInjectionHelper(baseEditorResult, htmlAttributes);
    }

    public static MvcHtmlString EditorWithAttributeInjection(
        this HtmlHelper html,
        string expression,
        string templateName,
        Object additionalViewData,
        dynamic htmlAttributes
    )
    {
        MvcHtmlString baseEditorResult = html.Editor(expression, templateName, additionalViewData);
        return AttributeInjectionHelper(baseEditorResult, htmlAttributes);
    }

    public static MvcHtmlString EditorWithAttributeInjection(
        this HtmlHelper html,
        string expression,
        string templateName,
        string htmlFieldName,
        dynamic htmlAttributes
    )
    {
        MvcHtmlString baseEditorResult = html.Editor(expression, templateName, htmlFieldName);
        return AttributeInjectionHelper(baseEditorResult, htmlAttributes);
    }

    public static MvcHtmlString EditorWithAttributeInjection(
        this HtmlHelper html,
        string expression,
        string templateName,
        string htmlFieldName,
        Object additionalViewData,
        dynamic htmlAttributes
    )
    {
        MvcHtmlString baseEditorResult = html.Editor(expression, templateName, htmlFieldName, additionalViewData);
        return AttributeInjectionHelper(baseEditorResult, htmlAttributes);
    }

    public static MvcHtmlString EditorForWithAttributeInjection<TModel, TValue>(
        this HtmlHelper<TModel> html,
        Expression<Func<TModel, TValue>> expression,
        dynamic htmlAttributes)
    {
        MvcHtmlString baseEditorResult = html.EditorFor(expression);
        return !String.IsNullOrEmpty(baseEditorResult.ToString()) ? AttributeInjectionHelper(baseEditorResult, htmlAttributes) : baseEditorResult;
    }

}
