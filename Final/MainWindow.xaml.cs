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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Final.Terrenos;
using Final.Animais;
using Final.Obitos;
using Final.Crias;
using M14_15_TrabalhoModelo_2021_WIP;
using System.Data;

namespace Final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BaseDados bd = new BaseDados();
        public MainWindow()
        {
            InitializeComponent();
            CB_Pesquisa.Items.Add("Terrenos semeados");
            CB_Pesquisa.Items.Add("Terrenos lavrados");
            CB_Pesquisa.Items.Add("Terrenos parados");
            CB_Pesquisa.Items.Add("Crias e respetivas mães");
        }

        private void BT_Terr_Click(object sender, RoutedEventArgs e)
        {
            J_Terrenos j = new J_Terrenos(bd);

            this.Hide();
            j.Show();
        }

        private void BT_Crias_Click(object sender, RoutedEventArgs e)
        {
            J_Crias j = new J_Crias(bd);

            this.Hide();
            j.Show();
        }

        private void BT_Adultos_Click(object sender, RoutedEventArgs e)
        {
            J_animais j = new J_animais(bd);

            this.Hide();
            j.Show();
        }

        private void BT_Obitos_Click(object sender, RoutedEventArgs e)
        {
            J_obitos j = new J_obitos(bd);

            this.Hide();
            j.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Utils.printDG<MainWindow>(DG_Pesquisa, "Pesquisa");
        }

        private void CB_Pesquisa_SelecionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sql = "";
            if (CB_Pesquisa.Text == "Terrenos semeados")
            {
                sql = $@"select nome from terrenos where estado like 'Semear'";
            }
            else if (CB_Pesquisa.Text == "Terrenos lavrados")
            {
                sql = $@"select nome from terrenos where estado like 'Lavrar'";
            }
            else if (CB_Pesquisa.Text == "Terrenos parados")
            {
                sql = $@"select nome from terrenos where estado like 'Parado'";
            }
            else if (CB_Pesquisa.Text == "Crias e respetivas mães")
            {
                sql = $@"select ncria, nanimal from crias left join animais on mae = nanimal";
            }
            DataTable dados = bd.devolveSQL(sql);
            DG_Pesquisa.DataContext = dados.DefaultView;
        }
    }
}
