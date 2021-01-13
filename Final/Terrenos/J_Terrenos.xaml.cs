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

namespace Final.Terrenos
{
    /// <summary>
    /// Interaction logic for J_Terrenos.xaml
    /// </summary>
    public partial class J_Terrenos : Window
    {
        BaseDados bd;
        public J_Terrenos(BaseDados bd)
        {
            this.bd = bd;
            InitializeComponent();
            BT_Alterar.Visibility = Visibility.Hidden;
            DateP.SelectedDate = DateTime.Now;
            AtualizaGrid();
            G_Semente.Visibility = Visibility.Hidden;
        }

        private void AtualizaGrid()
        {
            DG_Terreno.ItemsSource = Terreno.ListaTodosTerrenos(bd);
        }

        private void DG_Terreno_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Terreno lv = (Terreno)DG_Terreno.SelectedItem;
            if (lv == null) return;
            TB_Nome.Text = lv.nome;
            TB_Tamanho.Text = lv.tamanho.ToString();
            TB_Localizacao.Text = lv.localização;
            TB_Estado.Text = lv.estado;
            TB_Semente.Text = lv.semente;
            TB_Obs.Text = lv.observacao;
            

            //mostrar o botão atualizar
            BT_Alterar.Visibility = Visibility.Visible;
            BT_Adicionar.IsEnabled = false;
        }

        private void DG_Terreno_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void BT_Voltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow j = new MainWindow();
            this.Close();
            j.Show();
        }

        private void BT_Alterar_Click(object sender, RoutedEventArgs e)
        {
            Terreno cr = (Terreno)DG_Terreno.SelectedItem;
            if (cr == null) return;

            //atualizar dados
            cr.nome = TB_Nome.Text;
            cr.tamanho = int.Parse(TB_Tamanho.Text);
            cr.localização = TB_Localizacao.Text;
            cr.estado = TB_Estado.Text;
            cr.semente = TB_Semente.Text;
            cr.datacultivo = DateP.SelectedDate.Value;
            cr.observacao = TB_Obs.Text;

            cr.Atualizar(bd);
            LimparForm();
            AtualizaGrid();
            BT_Alterar.Visibility = Visibility.Hidden;
        }

        private void BT_Adicionar_Click(object sender, RoutedEventArgs e)
        {
            //validar dados do form
            string nome = TB_Nome.Text;
            if (nome.Trim().Length == 0)
            {
                MessageBox.Show("O nome é obrigatório.");
                return;
            }

            int tamanho = 0;
            if (TB_Tamanho.Text != "")
            {
                tamanho = int.Parse(TB_Tamanho.Text);
            }

            string obs = "";
            if (TB_Obs.Text != "")
            {
                obs = TB_Obs.Text;
            }

            string localizacao = "";
            if (TB_Localizacao.Text != "")
            {
                localizacao = TB_Localizacao.Text;
            }

            string estado = "";
            if (TB_Estado.Text != "")
            {
                estado = TB_Estado.Text;
            }

            string tiposemente = "";
            if (TB_Semente.Text != "")
            {
                tiposemente = TB_Semente.Text;
            }

            DateTime? datacultivo = null;
            if (G_Semente.IsVisible == true)
            {
                datacultivo = DateP.SelectedDate.Value;
            }
            else if (G_Semente.IsVisible == false)
            {
                datacultivo = null;
            }
            

            //criar objeto
            Terreno an = new Terreno(nome, tamanho, localizacao, estado, tiposemente, datacultivo, obs);

            //guardar na bd
            an.Adicionar(bd);

            //limpar form
            LimparForm();

            //atualizar grid
            AtualizaGrid();
        }

        private void BT_Limpar_Click(object sender, RoutedEventArgs e)
        {
            LimparForm();
            BT_Alterar.Visibility = Visibility.Hidden;
        }

        private void LimparForm()
        {
            TB_Nome.Text = "";
            TB_Tamanho.Text = "";
            TB_Localizacao.Text = "";
            TB_Estado.Text = "";
            G_Semente.Visibility = Visibility.Hidden;
            DG_Terreno.SelectedItem = null;
            BT_Adicionar.IsEnabled = true;
        }

        private void TB_Estado_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_Estado.Text.Contains("Semear"))
            {
                G_Semente.Visibility = Visibility.Visible;
            }
            else
            {
                G_Semente.Visibility = Visibility.Hidden;
            }
        }

        private void BT_Eliminar_Click(object sender, RoutedEventArgs e)
        {
            Terreno terreno = (Terreno)DG_Terreno.SelectedItem;
            if (terreno == null)
            {
                MessageBox.Show("Selecione o terreno a remover.");
                return;
            }
            terreno.Remover(bd, terreno.nome);
            LimparForm();
            AtualizaGrid();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Utils.printDG<Terreno>(DG_Terreno, "Terrenos");
        }
    }
}
