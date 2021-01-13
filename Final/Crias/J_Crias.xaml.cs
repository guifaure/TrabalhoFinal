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

namespace Final.Crias
{
    /// <summary>
    /// Interaction logic for J_Crias.xaml
    /// </summary>
    public partial class J_Crias : Window
    {
        BaseDados bd;
        char sexo;
        string obs;
        public J_Crias(BaseDados bd)
        {
            this.bd = bd;
            InitializeComponent();
            BT_Alterar.Visibility = Visibility.Hidden;
            TB_ncria.IsReadOnly = false;
            BT_Obito.Visibility = Visibility.Hidden;
            DateP.SelectedDate = DateTime.Now;
            AtualizaGrid();

        }

        private void BT_Voltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow j = new MainWindow();
            this.Close();
            j.Show();
        }

        private void BT_Limpar_Click(object sender, RoutedEventArgs e)
        {
            LimparForm();
            TB_ncria.IsReadOnly = false;
            BT_Obito.Visibility = Visibility.Hidden;
            BT_Alterar.Visibility = Visibility.Hidden;
        }

        private void LimparForm()
        {
            TB_ncria.Text = "";
            RB_Femea.IsChecked = false;
            RB_Macho.IsChecked = false;
            TB_mae.Text = "";
            TB_Peso1.Text = "";
            TB_Peso2.Text = "";
            TB_Obs.Text = "";
            DG_Animais.SelectedItem = null;
            BT_Adicionar.IsEnabled = true;
        }

        private void BT_Obito_Click(object sender, RoutedEventArgs e)
        {
            Cria n = (Cria)DG_Animais.SelectedItem;

            DialogoObito j = new DialogoObito(n.ncria, bd);

            j.ShowDialog();
            AtualizaGrid();
            LimparForm();
        }

        private void BT_Adicionar_Click(object sender, RoutedEventArgs e)
        {
            //validar dados do form
            string ncria = TB_ncria.Text;
            if (ncria.Trim().Length == 0)
            {
                MessageBox.Show("O número é obrigatório.");
                return;
            }

            if (RB_Femea.IsChecked == true)
            {
                sexo = 'F';
            }
            else if (RB_Macho.IsChecked == true)
            {
                sexo = 'M';
            }

            DateTime data = DateP.SelectedDate.Value;

            double peso1 = 0;
            if (TB_Peso1.Text != "")
            {
                peso1 = double.Parse(TB_Peso1.Text);
            }

            double peso2 = 0;
            if (TB_Peso2.Text != "")
            {
                peso2 = double.Parse(TB_Peso2.Text);
            }

            string mae = TB_mae.Text;

            obs = "";
            if (TB_Obs.Text != "")
            {
                obs = TB_Obs.Text;
            }


            //criar objeto
            Cria an = new Cria(ncria, sexo, data, peso1, peso2, mae, obs, false);

            //guardar na bd
            an.Adicionar(bd);

            //limpar form
            LimparForm();

            //atualizar grid
            AtualizaGrid();
        }

        private void AtualizaGrid()
        {
            DG_Animais.ItemsSource = Cria.ListaTodasCrias(bd);
        }

        private void BT_Alterar_Click(object sender, RoutedEventArgs e)
        {
            Cria cr = (Cria)DG_Animais.SelectedItem;
            if (cr == null) return;

            //atualizar dados
            cr.ncria = TB_ncria.Text;
            cr.mae = TB_mae.Text;
            cr.peso_2semanas = double.Parse(TB_Peso2.Text);
            cr.peso_nasc = double.Parse(TB_Peso1.Text);
            if (RB_Femea.IsChecked == true)
            {
                cr.sexo = 'F';
            }
            else if (RB_Macho.IsChecked == true)
            {
                cr.sexo = 'M';
            }
            cr.observacao = TB_Obs.Text;
            cr.data_nasc = DateP.SelectedDate.Value;

            cr.Atualizar(bd);
            LimparForm();
            AtualizaGrid();
            BT_Alterar.Visibility = Visibility.Hidden;
            BT_Obito.Visibility = Visibility.Hidden;
        }

        private void DG_Animais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cria lv = (Cria)DG_Animais.SelectedItem;
            if (lv == null) return;
            TB_ncria.Text = lv.ncria;
            TB_mae.Text = lv.mae;
            TB_Obs.Text = lv.observacao;
            TB_Peso1.Text = lv.peso_nasc.ToString();
            TB_Peso2.Text = lv.peso_2semanas.ToString();
            DateP.SelectedDate = lv.data_nasc;
            if (lv.sexo == 'F')
            {
                RB_Femea.IsChecked = true;
            }
            else if (lv.sexo == 'M')
            {
                RB_Macho.IsChecked = true;
            }

            if (lv.obito == false)
            {
                BT_Obito.IsEnabled = true;
                BT_Alterar.IsEnabled = true;
            }
            else
            {
                BT_Obito.IsEnabled = false;
                BT_Alterar.IsEnabled = false;
            }

            //mostrar o botão atualizar
            BT_Alterar.Visibility = Visibility.Visible;
            BT_Obito.Visibility = Visibility.Visible;
            TB_ncria.IsReadOnly = true;
            BT_Adicionar.IsEnabled = false;
        }

        private void DG_Animais_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if (col != null && e.PropertyType == typeof(DateTime))
            {
                if (col.Header.ToString() == "data_nasc")
                    col.Binding = new Binding(e.PropertyName) { StringFormat = "dd-MM-yyyy" };
            }

            if (e.Column.Header.ToString() == "ncria")
            {
                e.Column.Header = "Nº de registo";
            }
            else if (e.Column.Header.ToString() == "sexo")
            {
                e.Column.Header = "Sexo";
            }
            else if (e.Column.Header.ToString() == "observacao")
            {
                e.Column.Header = "Observação";
            }
            else if (e.Column.Header.ToString() == "peso_2semanas")
            {
                e.Column.Header = "Peso 15 dias após";
            }
            else if (e.Column.Header.ToString() == "peso_nasc")
            {
                e.Column.Header = "Peso nascimento";
            }
            else if (e.Column.Header.ToString() == "obito")
            {
                e.Column.Header = "Óbito";
            }
            else if (e.Column.Header.ToString() == "mae")
            {
                e.Column.Header = "Mãe";
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Utils.printDG<Cria>(DG_Animais, "Crias");
        }
    }
}
