using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using System.Linq;

namespace TescoSWUkol
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? filePath = null;
        private CarList carList = new CarList();
        
        public MainWindow()
        {
            InitializeComponent();

        }

        private void BChooseFilepath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog selectPath = new OpenFileDialog();
            selectPath.Title = "Vyberte .xml soubor";
            selectPath.Filter = "Extensible Markup Language (*.xml)|*.xml";
            try
            {
                bool? success = selectPath.ShowDialog();
                if (success == true)
                {
                    filePath = selectPath.FileName;
                }
                else
                {
                    throw new Exception("Nebyl zvolen žádný .xml soubor, prosím opakujte volbu");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Chyba při výběru souboru", MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }

        private void BLoadData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadFileData();
                CarDataGrid.ItemsSource = carList.Cars;
                CarDataGrid.Columns[0].Header = "Model auta";
                CarDataGrid.Columns[1].Visibility = Visibility.Collapsed;
                CarDataGrid.Columns[2].Header = "Datum prodeje";
                CarDataGrid.Columns[3].Header = "Cena bez DPH";
                CarDataGrid.Columns[4].Header = "% DPH";
            }

            catch (Exception ex) { MessageBox.Show(ex.Message, "Chyba při načítání dat ze souboru", MessageBoxButton.OK, MessageBoxImage.Error); }
        }


        private void BWeekendSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadFileData();
                var weekendSales = (from car in carList.Cars
                                    where car.SaleDate.DayOfWeek == DayOfWeek.Saturday || car.SaleDate.DayOfWeek == DayOfWeek.Sunday
                                    select car)
                                   .GroupBy(c => c.ModelName)
                                   .Select(carSale => new CarSale { ModelName = carSale.First().ModelName, TotalPrice = carSale.Sum(c => c.Price), TotalPriceTaxed = carSale.Sum(c => (c.Price * ((c.DPH / 100) + 1))) }).
                                   ToList();


                CarDataGrid.ItemsSource = weekendSales;
                CarDataGrid.Columns[0].Header = "Název modelu";
                CarDataGrid.Columns[1].Header = "Cena bez DPH";
                CarDataGrid.Columns[2].Header = "Cena s DPH";
            }

            catch (Exception ex) { MessageBox.Show(ex.Message, "Chyba při načítání dat ze souboru", MessageBoxButton.OK, MessageBoxImage.Error); }
        }


        private void LoadFileData()
        {
            if (filePath == null)
            {
                throw new Exception("Nejdříve zvolte .xml soubor ");
            }

            XmlSerializer serializer = new XmlSerializer(typeof(CarList));
            using (StreamReader sr = new StreamReader(filePath))

            //using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                carList = (CarList)serializer.Deserialize(sr);
            }
        }
    }
}