using pujcovna.Classes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using pujcovna.Extensions;
using pujcovna.Data.Models;

namespace Eshop.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent RenderFlashMessages(this IHtmlHelper helper)
        {
            // pod klíčem "Messages" najdeme pole se všemi zprávami ve formátu json, které rovnou pomocí extension metody deserializujeme
            // pozn.: pokud jsme žádné zprávy neuložili, metoda vrátí prázdnou instanci Listu - což nám ale vůbec nevadí
            var messageList = helper.ViewContext.TempData
                .DeserializeToObject<List<FlashMessage>>("Messages");

            var html = new HtmlContentBuilder();

            // procházíme všechny zprávy a vytvoříme HTML
            foreach (var msg in messageList)
            {
                var container = new TagBuilder("div");
                container.AddCssClass($"alert alert-{ msg.Type.ToString().ToLower() }"); //přidáme CSS z Bootstrap
                container.InnerHtml.SetContent(msg.Message);

                html.AppendHtml(container);
            }

            return html;
        }

        //odkazuje na eshop.Data.Models Category.cs - pokud nebude postupem lekcí dotvořena, vyřešit (je tu obdobná třída v podobné složce - akorát bez Data, ale k té se mi to nechce přiřadit)
        public static IHtmlContent RenderCategories(this IHtmlHelper helper, IEnumerable<Category> categories, string parentUrl = "")
        {
            var ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("nav nav-list tree");

            foreach (var category in categories)
            {
                string url = parentUrl + "/" + category.Url;
                var liTag = new TagBuilder("li");

                if (category.ChildCategories.Count > 0)
                {
                    var h4Tag = new TagBuilder("h4");
                    h4Tag.AddCssClass("text-left");
                    h4Tag.Attributes.Add("style", "font-weight:700");
                    var anchorTag = new TagBuilder("a");
                    anchorTag.Attributes.Add("style", "margin-left: -5px");
                    anchorTag.Attributes.Add("href", "/Product/Index?categoryId=" + category.CategoryId);
                    anchorTag.Attributes.Add("data-path", url);
                    anchorTag.InnerHtml.SetContent(category.Title);

                    liTag.InnerHtml.SetHtmlContent(h4Tag);
                    h4Tag.InnerHtml.SetHtmlContent(anchorTag);
                    liTag.InnerHtml.AppendHtml(RenderCategories(helper, category.ChildCategories.OrderBy(c => c.OrderNo), url));
                }

                else
                {
                    var h4Tag = new TagBuilder("h4");
                    h4Tag.AddCssClass("text-left");
                    var anchorTag = new TagBuilder("a");
                    anchorTag.Attributes.Add("style", "color: #b43eda");
                    anchorTag.Attributes.Add("href", "/Product/Index?categoryId=" + category.CategoryId);
                    anchorTag.Attributes.Add("data-path", url);
                    anchorTag.InnerHtml.SetContent(category.Title);

                    liTag.InnerHtml.SetHtmlContent(h4Tag);
                    h4Tag.InnerHtml.SetHtmlContent(anchorTag);
                }

                ulTag.InnerHtml.AppendHtml(liTag);
            }

            return new HtmlContentBuilder().AppendHtml(ulTag);
        }
    }
}