namespace SPaaS.Api.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal RoundToOneDecimalPlace(this decimal value)
        {
            return Math.Round(value, 1);
        }

        public static decimal CalculatePositivePercentageValue(this decimal value, decimal percentage)
        {
            if (percentage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be a number greater than or equal to 0.");
            }

            return value * (percentage / 100);
        }

        public static bool IsSmallerOrEqualToAnyInSubset(this decimal value, List<decimal> set)
        {
            foreach (var item in set)
            {
                if (item <= value)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsSubsetSum(this decimal value, List<decimal> set, int currentIndex)
        {
            if (value == 0)
            {
                return true;
            }

            // Si l'index actuel est inférieur à 0 ou le total est négatif, la combinaison n'est pas possible
            if (currentIndex < 0 || value < 0)
                return false;

            // Vérifier si le total peut être atteint en incluant l'élément à l'index actuel
            var includeCurrent = (value - set[currentIndex]).IsSubsetSum(set, currentIndex - 1);

            // Vérifier si le total peut être atteint sans inclure l'élément à l'index actuel
            var excludeCurrent = value.IsSubsetSum(set, currentIndex - 1);

            // Retourner true si l'une des deux conditions est vraie
            return includeCurrent || excludeCurrent;
        }
    }
}
