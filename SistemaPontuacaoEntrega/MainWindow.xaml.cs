using DatabaseAccess.DbSet;
using DatabaseAccess.Queries;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static System.Configuration.ConfigurationManager;

namespace SistemaPontuacaoEntrega
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MontarColunasGrid();
            ValorPontuacao.Text = DoubleToString(StringToDouble(AppSettings["ValorFrete"].ToString()));
        }
        
        private void Limpar_Click(object sender, RoutedEventArgs e)
        {
            LimparTudo();
        }

        private void Calcular_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInt(CEPInicial.Text) || !IsInt(CEPFinal.Text))
            {
                MessageBox.Show("Informe Códigos de CEP Válidos!");
                return;
            }
            else if (int.Parse(CEPInicial.Text) > 99 || int.Parse(CEPFinal.Text) > 99)
            {
                MessageBox.Show("Informe Códigos de CEP Válidos! (010 - 099)");
                return;
            }
            CalcularPontos();
        }

        private void CalcularPontos()
        {
            if (!IsDouble(ValorPontuacao.Text))
            {
                MessageBox.Show("Valor Frete Inválido!");
                ValorPontuacao.Focus();
                return;
            }

            var ponto = new BuscarPontuacao("[spe]").RetornarPontoDistanciaPercorrida(CEPInicial.Text, CEPFinal.Text);
            dgPontosDeslocamentos.Items.Add(new PontuacaoDeslocamentoDescricao()
            {
                zonaDistritoCepInicioDescricao = ponto.zonaDistritoCepInicioDescricao,
                zonaDistritoCepFinalDescricao = ponto.zonaDistritoCepFinalDescricao,
                Ponto = ponto.Ponto
            });

            double calculoPontos = 0;
            foreach (var t in dgPontosDeslocamentos.Items)
            {
                (t as PontuacaoDeslocamentoDescricao).Ponto = (t as PontuacaoDeslocamentoDescricao).Ponto ?? "0,5";
                calculoPontos += (double.Parse((t as PontuacaoDeslocamentoDescricao).Ponto));
            }
            TotalPontuacao.Text = DoubleToString((1.0 + calculoPontos) * StringToDouble(ValorPontuacao.Text));
        }

        private void LimparTudo()
        {
            CEPInicial.Text = string.Empty;
            CEPFinal.Text = string.Empty;
            TotalPontuacao.Text = "0";
            for(int item=0;dgPontosDeslocamentos.Items.Count>0;item++)
            {
                dgPontosDeslocamentos.Items.RemoveAt(0);
            }
        }

        private void MontarColunasGrid()
        {
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "Zona Partida";
            c1.Binding = new Binding("zonaDistritoCepInicioDescricao");
            c1.Width = 110;
            c1.IsReadOnly = true;
            dgPontosDeslocamentos.Columns.Add(c1);

            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "Zona Chegada";
            c2.Binding = new Binding("zonaDistritoCepFinalDescricao");
            c2.Width = 110;
            c2.IsReadOnly = true;
            dgPontosDeslocamentos.Columns.Add(c2);

            DataGridTextColumn c3 = new DataGridTextColumn();
            c3.Header = "Ponto";
            c3.Binding = new Binding("Ponto");
            c3.Width = 110;
            c3.IsReadOnly = true;
            dgPontosDeslocamentos.Columns.Add(c3);
        }

        private void GravarNovoFrete()
        {
            if(IsDouble(ValorPontuacao.Text))
            { 
                var configFile = OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings["ValorFrete"].Value = DoubleToString(StringToDouble(ValorPontuacao.Text));
                configFile.Save();
                return;
            }
            MessageBox.Show("Valor Frete Inválido!");
        }

        private void GravarNovoFrete_Click(object sender, RoutedEventArgs e)
        {
            GravarNovoFrete();
        }

        private bool IsDouble(string valor)
        {
            double valorFrete = 0.0;
            return double.TryParse(valor, out valorFrete);
        }

        private bool IsInt(string valor)
        {
            double valorFrete = 0.0;
            return double.TryParse(valor, out valorFrete);
        }

        private double StringToDouble(string valor)
        {
            if (IsDouble(valor))
                return double.Parse(valor);
            return 0.0;
        }

        private string DoubleToString(double valor)
        {
            return valor.ToString("N2");
        }
    }
}
