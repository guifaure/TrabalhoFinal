using M14_15_TrabalhoModelo_2021_WIP;
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

namespace Final.Obitos
{
    /// <summary>
    /// Interaction logic for J_obitos.xaml
    /// </summary>
    public partial class J_obitos : Window
    {
        BaseDados bd;
        public J_obitos(BaseDados bd)
        {
            this.bd = bd;
            InitializeComponent();
            AtualizaGrid();
        }

        private void BT_Voltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow j = new MainWindow();
            this.Close();
            j.Show();
        }

        private void AtualizaGrid()
        {
            DG_Animais.ItemsSource = Obito.ListaTodos(bd);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Utils.printDG<Obito>(DG_Animais, "Obitos");
        }
    }
}
