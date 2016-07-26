using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services.results
{
    public class ExchangeRateResult
    {
        public string Base { get; set; }

        public DateTime Date { get; set; }
        public float Rate { get; set; }
        public string Target { get; set; }
    }
}