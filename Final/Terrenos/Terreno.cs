using M14_15_TrabalhoModelo_2021_WIP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Terrenos
{
    public class Terreno
    {

        public string nome { get; set; }
        public int tamanho { get; set; }
        public string localização { get; set; }
        public string estado { get; set; }
        public string semente { get; set; }
        public DateTime? datacultivo { get; set; }
        public string observacao { get; set; }

        public Terreno(string nome, int tamanho, string localização, string estado, string semente, DateTime? datacultivo, string observacao)
        {
            this.nome = nome;
            this.tamanho = tamanho;
            this.localização = localização;
            this.estado = estado;
            this.semente = semente;
            this.datacultivo = datacultivo;
            this.observacao = observacao;
        }

        public void Adicionar(BaseDados bd)
        {
            string sql = "";
            //iniciar transação
            SqlTransaction transacao = bd.iniciarTransacao();
            //registar o empréstimo
            if (datacultivo != null)
            {
                sql = $"INSERT INTO terrenos(nome,tamanho,localizacao, estado, semente, datacultivo, observacao)" +
                        $" VALUES (@nome,@tamanho,@localizacao, @estado, @semente, @datacultivo, @observacao)";
            }
            else
            {
                sql = $"INSERT INTO terrenos(nome,tamanho,localizacao, estado, semente, observacao)" +
                        $" VALUES (@nome,@tamanho,@localizacao, @estado, @semente, @observacao)";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nome",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.nome
                },
                new SqlParameter()
                {
                    ParameterName="@tamanho",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.tamanho
                },
                new SqlParameter()
                {
                    ParameterName="@localizacao",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.localização
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.estado
                },
                new SqlParameter()
                {
                    ParameterName="@semente",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.semente
                },
                new SqlParameter()
                {
                    ParameterName="@observacao",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.observacao
                }
            };

            if (datacultivo != null)
                parametros.Add(
                new SqlParameter()
                {
                    ParameterName = "@datacultivo",
                    SqlDbType = System.Data.SqlDbType.DateTime,
                    Value = this.datacultivo
                });


            bd.executaSQL(sql, parametros, transacao);

            //terminar transação
            transacao.Commit();
        }

        public static List<Terreno> ListaTodosTerrenos(BaseDados bd)
        {
            string sql = "SELECT * FROM terrenos";

            DataTable dados = bd.devolveSQL(sql);
            List<Terreno> lista = new List<Terreno>();
            foreach (DataRow linha in dados.Rows)
            {
                string nome = linha["nome"].ToString();
                int tamanho = int.Parse(linha["tamanho"].ToString());
                string localizacao = linha["localizacao"].ToString();
                string estado = linha["estado"].ToString();
                string semente = linha["semente"].ToString();
                DateTime datacultivo = DateTime.Parse(linha["datacultivo"].ToString());
                string obs = linha["observacao"].ToString();
                Terreno novo = new Terreno(nome, tamanho, localizacao, estado, semente, datacultivo, obs);
                lista.Add(novo);
            }
            return lista;
        }

        public static Terreno Get(BaseDados bd, string id)
        {
            string sql = "SELECT * FROM terreno WHERE nome=" + id;
            DataTable dados = bd.devolveSQL(sql);
            string nome = dados.Rows[0]["nome"].ToString();
            int tamanho = int.Parse(dados.Rows[0]["tamanho"].ToString());
            string localizacao = dados.Rows[0]["localizacao"].ToString();
            string estado = dados.Rows[0]["estado"].ToString();
            string semente = dados.Rows[0]["semente"].ToString();
            DateTime datacultivo = DateTime.Parse(dados.Rows[0]["datacultivo"].ToString());
            string obs = dados.Rows[0]["observacao"].ToString();
            Terreno an = new Terreno(nome, tamanho, localizacao, estado, semente, datacultivo, obs);
            return an;
        }

        internal void Atualizar(BaseDados bd)
        {
            string sql = $@"UPDATE terrenos SET nome=@nome, tamanho=@tamanho,
                        localizacao=@localizacao,estado=@estado,semente=@semente, 
                        datacultivo=@datacultivo, observacao=@observacao
                        WHERE nome=@nome";
            //parametros
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nome",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.nome
                },
                new SqlParameter()
                {
                    ParameterName="@tamanho",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.tamanho
                },
                new SqlParameter()
                {
                    ParameterName="@localizacao",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.localização
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.estado
                },
                new SqlParameter()
                {
                    ParameterName="@semente",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.semente
                },
                new SqlParameter()
                {
                    ParameterName="@datacultivo",
                    SqlDbType=System.Data.SqlDbType.DateTime,
                    Value=this.datacultivo
                },
                new SqlParameter()
                {
                    ParameterName="@observacao",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.observacao
                },
            };
            //executar
            bd.executaSQL(sql, parametros);
        }

        internal void Remover(BaseDados bd, string nome)
        {
            string sql = "DELETE FROM terrenos WHERE nome =" + nome;

            bd.executaSQL(sql);
        }
    }
}
