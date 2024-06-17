using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TescoSWUkol
{
        [XmlRoot(ElementName = "vuz")]
        public class Car
        {

            [XmlElement(ElementName = "nazevModelu")]
            public string? ModelName { get; set; }

            [XmlIgnore]
            public DateTime SaleDate { get; set; }

            [XmlElement(ElementName = "datumProdeje")]
            public string SaleDateString
            {
                get { return this.SaleDate.ToString("dd.MM.yyyy"); }
                set { this.SaleDate = DateTime.Parse(value); }
            }

            [XmlElement(ElementName = "cena")]
            public double Price { get; set; }

            [XmlElement(ElementName = "dph")]
            public double DPH { get; set; }
         }

        [XmlRoot(ElementName = "prodanevozy")]
        public class CarList
        {

            [XmlElement(ElementName = "vuz")]
            public List<Car> Cars { get; set; }

            public void ListClear()
            {
            Cars.Clear();
            }
    }
 
}
