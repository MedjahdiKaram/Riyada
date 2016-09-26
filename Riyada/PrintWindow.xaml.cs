using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Riyada
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        public PrintDialog PrintDlg { get; set; }
    
        public void Print()
        {
            var idocument = BigFlowDocument as IDocumentPaginatorSource;


            if (idocument != null)
            {
                //var count = idocument.DocumentPaginator.PageCount;
                //while (!FlowDocReader.CanGoToPage(count+1))
                //{
                //    top += 5;
                //    Debug.WriteLine(top);
                //    var thick = FooterContainer.Margin;
                //    thick.Top = top;
                //    FooterContainer.Margin = thick;
                //}
                //FooterContainer.Margin = new Thickness { Top = top - 5 };

                PrintDlg.PrintDocument(idocument.DocumentPaginator, "Cartes");
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Print();
        }

        public PrintWindow()
        {
            InitializeComponent();
            PrintDlg = new PrintDialog();

        }
        
    }
}
