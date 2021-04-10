using System.Configuration;

namespace TRMDesktopUI.Library.Helpers
{
    public interface IConfigHelper
    {
        /// <exception cref="ConfigurationErrorsException"/>
        decimal GetTaxRate();
    }

    public class ConfigHelper : IConfigHelper
    {
        /// <exception cref="ConfigurationErrorsException"/>
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];
            bool isValidTaxRate = decimal.TryParse(rateText, out var output);
            if (isValidTaxRate == false)
            {
                // TODO: Configuration exception is only thrown when the value is retrieved. Probably want that happen at startup. But going along with course...
                throw new ConfigurationErrorsException("The tax rate is not set up properly.");
            }

            return output; // TODO: This doesn't actually return a rate (value between [0.0, 1.0]), but going along with course...
        }
    }
}