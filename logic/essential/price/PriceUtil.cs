using System.Collections.Generic;

namespace Assets.logic.essential.price
{
    public static class PriceUtil
    {
        public static IPrice[] Decomposite(IPrice price, List<IPrice> prices = null)
        {
            if (prices == null)
                prices = new List<IPrice>();

            if (price is CompositePrice cp)
            {
                foreach (var pr in cp.prices)
                    Decomposite(pr.Value, prices);
            }
            else
                prices.Add(price);
            
            return prices.ToArray();
        }
    }
}
