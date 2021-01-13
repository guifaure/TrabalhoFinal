using System;
using System.Collections.Generic;
using System.IO;
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
using M14_15_TrabalhoModelo_2021_WIP;
using Final.Animais;

namespace Final.Animais
{
    /// <summary>
    /// Interaction logic for J_animais.xaml
    /// </summary>
    public partial class J_animais : Window
    {
        BaseDados bd;
        char sexo;
        string obs;
        bool leite;
        public J_animais(BaseDados bd)
        {
            this.bd = bd;
            InitializeComponent();
            BT_Alterar.Visibility = Visibility.Hidden;
            TB_nanimal.IsReadOnly = false;
            BT_Obitor.Visibility = Visibility.Hidden;
            Grid_Leite.Visibility = Visibility.Hidden;
            AtualizaGrid();
        }

        private void BT_Voltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow j = new MainWindow();
            this.Close();
            j.Show();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Grid_Leite.Visibility = Visibility.Visible;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            Grid_Leite.Visibility = Visibility.Hidden;
        }

        private void BT_Adicionar_Click(object sender, RoutedEventArgs e)
        {
            //validar dados do form
            string nanimal = TB_nanimal.Text;
            if (nanimal.Trim().Length == 0)
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

            double med = 0;
            if (TB_Med.Text != "")
            {
                med = double.Parse(TB_Med.Text);
            }

            obs = "";
            if (TB_Obs.Text != "")
            {
                obs = TB_Obs.Text;
            }

            if (CB_Leite.IsChecked == true)
            {
                leite = (bool)CB_Leite.IsChecked;
            }


            //criar objeto
            Animal an = new Animal(nanimal, sexo, obs, leite, med, false);

            //guardar na bd
            an.Adicionar(bd);

            //limpar form
            LimparForm();

            //atualizar grid
            AtualizaGrid();
        }

        private void AtualizaGrid()
        {
            DG_Animais.ItemsSource = Animal.ListaTodosAnimais(bd);
            
        }

        private void LimparForm()
        {
            TB_nanimal.Text = "";
            RB_Femea.IsChecked = false;
            RB_Macho.IsChecked = false;
            CB_Leite.IsChecked = false;
            TB_Med.Text = "";
            Grid_Leite.Visibility = Visibility.Hidden;
            TB_Obs.Text = "";
            DG_Animais.SelectedItem = null;
            BT_Adicionar.IsEnabled = true;
        }

        private void BT_Limpar_Click(object sender, RoutedEventArgs e)
        {
            LimparForm();
            TB_nanimal.IsReadOnly = false;
            BT_Obitor.Visibility = Visibility.Hidden;
            BT_Alterar.Visibility = Visibility.Hidden;
        }

        private void BT_Obitor_Click(object sender, RoutedEventArgs e)
        {
            Animal n = (Animal)DG_Animais.SelectedItem;

            DialogoObito j = new DialogoObito(n.nanimal, bd);

            j.ShowDialog();
            AtualizaGrid();
            LimparForm();
        }

        private void BT_Alterar_Click(object sender, RoutedEventArgs e)
        {
            Animal cr = (Animal)DG_Animais.SelectedItem;
            if (cr == null) return;

            //atualizar dados
            cr.nanimal = TB_nanimal.Text;
            cr.med_leite = double.Parse(TB_Med.Text);
            cr.observacao = TB_Obs.Text;

            if (CB_Leite.IsChecked == true)
            {
                cr.leite = true;
            }
            else
            {
                cr.leite = false;
            }
            if (RB_Femea.IsChecked == true)
            {
                cr.sexo = 'F';
            }
            else if (RB_Macho.IsChecked == true)
            {
                cr.sexo = 'M';
            }
            cr.observacao = TB_Obs.Text;

            cr.Atualizar(bd);
            LimparForm();
            AtualizaGrid();
            BT_Alterar.Visibility = Visibility.Hidden;
            BT_Obitor.Visibility = Visibility.Hidden;
        }

        private void DG_Animais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Animal lv = (Animal)DG_Animais.SelectedItem;
            if (lv == null) return;
            TB_nanimal.Text = lv.nanimal;
            TB_Med.Text = lv.med_leite.ToString();
            TB_Obs.Text = lv.observacao;
            if (lv.leite != true)
            {
                CB_Leite.IsChecked = false;
            }
            else
            {
                CB_Leite.IsChecked = true;
            }

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
                BT_Obitor.IsEnabled = true;
                BT_Alterar.IsEnabled = true;
            }
            else
            {
                BT_Obitor.IsEnabled = false;
                BT_Alterar.IsEnabled = false;
            }

            //mostrar o botão atualizar
            BT_Alterar.Visibility = Visibility.Visible;
            BT_Obitor.Visibility = Visibility.Visible;
            TB_nanimal.IsReadOnly = true;
            BT_Adicionar.IsEnabled = false;

        }

        private void DG_Animais_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "nanimal")
            {
                e.Column.Header = "Nº de registo";
            }
            else if (e.Column.Header.ToString() =="sexo")
            {
                e.Column.Header = "Sexo";
            }
            else if (e.Column.Header.ToString() == "observacao")
            {
                e.Column.Header = "Observação";
            }
            else if (e.Column.Header.ToString() == "leite")
            {
                e.Column.Header = "Produção de leite";
            }
            else if (e.Column.Header.ToString() == "med_leite")
            {
                e.Column.Header = "Média de leite";
            }
            else if (e.Column.Header.ToString() == "obito")
            {
                e.Column.Header = "Óbito";
            }


        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Utils.printDG<Animal>(DG_Animais, "Animais");
        }
    }
}
