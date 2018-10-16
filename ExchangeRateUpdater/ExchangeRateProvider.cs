using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        public readonly Currency Source = new Currency("CZK");
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, string url)
        {
            var doc = XDocument.Load(url);
            var lines = doc.Descendants().Where(node => node.Name == "radek");
            var rates = lines.Join(currencies, element => element.Attribute("kod")?.Value, currency => currency.Code, (element, currency) => new ExchangeRate(Source, currency, decimal.Parse(element.Attribute("kurz")?.Value))).ToArray();
            return rates;
        }
    }
}

