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
using Final.Obitos;

namespace Final
{
    /// <summary>
    /// Interaction logic for DialogoObito.xaml
    /// </summary>
    public partial class DialogoObito : Window
    {
        BaseDados bd;
        public DialogoObito(string id, BaseDados bd)
        {
            this.bd = bd;
            InitializeComponent();
            TB_NR.Text = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string rel = TB_Rel.Text;

            Obito n;
            bool o = TB_NR.Text.Contains("PT");
            if (o == true)
            {
                n = new Obito(TB_NR.Text, null, rel);
            }
            else
            {
                n = new Obito(null, TB_NR.Text, rel);
            }

            n.Adicionar(bd);
            n.Registo(bd, TB_NR.Text);

            this.Close();
        }

    }
}
