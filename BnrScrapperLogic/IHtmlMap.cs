using HtmlAgilityPack;

namespace BnrScrapperLogic
{
    public interface IHtmlMap<T>
    {
        T Map(HtmlNode node);
    }
}