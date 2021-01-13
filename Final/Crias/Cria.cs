using M14_15_TrabalhoModelo_2021_WIP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Crias
{
    class Cria
    {
        public string ncria { get; set; }
        public char sexo { get; set; }
        public DateTime data_nasc { get; set; }
        public double peso_nasc { get; set; }
        public double peso_2semanas { get; set; }
        public string mae { get; set; }
        public string observacao { get; set; }
        public bool obito { get; set; }

        public Cria(string ncria, char sexo, DateTime data_nasc, double peso_nasc, double peso_2semanas, string mae, string observacao, bool obito)
        {
            this.ncria = ncria;
            this.sexo = sexo;
            this.data_nasc = data_nasc;
            this.peso_nasc = peso_nasc;
            this.peso_2semanas = peso_2semanas;
            this.mae = mae;
            this.observacao = observacao;
            this.obito = obito;
        }

        public void Adicionar(BaseDados bd)
        {
            string sql = "";
            //iniciar transação
            SqlTransaction transacao = bd.iniciarTransacao();
            //registar o empréstimo
            sql = $"INSERT INTO crias(ncria,sexo,data_nasc, peso_nasc, peso_2semanas, mae, observacao, obito)" +
                $" VALUES (@ncria,@sexo, @data_nasc, @peso_nasc, @peso_2semanas, @mae, @observacao,@obito)";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ncria",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.ncria
                },
                new SqlParameter()
                {
                    ParameterName="@sexo",
                    SqlDbType=System.Data.SqlDbType.Char,
                    Value=this.sexo
                },
                new SqlParameter()
                {
                    ParameterName="@data_nasc",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_nasc
                },
                new SqlParameter()
                {
                    ParameterName="@peso_nasc",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.peso_nasc
                },
                new SqlParameter()
                {
                    ParameterName="@peso_2semanas",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.peso_2semanas
                },
                new SqlParameter()
                {
                    ParameterName="@mae",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.mae
                },
                new SqlParameter()
                {
                    ParameterName="@observacao",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.observacao
                },
                new SqlParameter()
                {
                    ParameterName="@obito",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=this.obito
                }
            };
            bd.executaSQL(sql, parametros, transacao);

            //terminar transação
            transacao.Commit();
        }

        public static List<Cria> ListaTodasCrias(BaseDados bd)
        {
            string sql = "SELECT * FROM crias";

            DataTable dados = bd.devolveSQL(sql);
            List<Cria> lista = new List<Cria>();
            foreach (DataRow linha in dados.Rows)
            {
                string ncria = linha["ncria"].ToString();
                char sexo = char.Parse(linha["sexo"].ToString());
                string obs = linha["observacao"].ToString();
                DateTime data = DateTime.Parse(linha["data_nasc"].ToString());
                double peso_nasc = double.Parse(linha["peso_nasc"].ToString());
                double peso_2semanas = double.Parse(linha["peso_2semanas"].ToString());
                string mae = linha["mae"].ToString();
                bool obito = bool.Parse(linha["obito"].ToString());
                Cria novo = new Cria(ncria, sexo, data, peso_nasc, peso_2semanas, mae, obs, obito);
                lista.Add(novo);
            }
            return lista;
        }

        //get
        public static Cria Get(BaseDados bd, int id)
        {
            string sql = "SELECT * FROM cria WHERE ncria=" + id;
            DataTable dados = bd.devolveSQL(sql);
            string ncria = dados.Rows[0]["ncria"].ToString();
            char sexo = char.Parse(dados.Rows[0]["sexo"].ToString());
            string obs = dados.Rows[0]["observacao"].ToString();
            DateTime data = DateTime.Parse(dados.Rows[0]["data_nasc"].ToString());
            double peso_nasc = double.Parse(dados.Rows[0]["peso_nasc"].ToString());
            double peso_2semanas = double.Parse(dados.Rows[0]["peso_2semanas"].ToString());
            string mae = dados.Rows[0]["mae"].ToString();
            bool obito = bool.Parse(dados.Rows[0]["obito"].ToString());
            Cria an = new Cria(ncria, sexo, data, peso_nasc, peso_2semanas, mae, obs, obito);
            return an;
        }

        internal void Atualizar(BaseDados bd)
        {
            string sql = $@"UPDATE crias SET sexo=@sexo,
                        data_nasc=@data_nasc,peso_nasc=@peso_nasc,peso_2semanas=@peso_2semanas, 
                        mae=@mae, observacao=@observacao, obito=@obito
                        WHERE ncria=@ncria";
            //parametros
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ncria",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.ncria
                },
                new SqlParameter()
                {
                    ParameterName="@sexo",
                    SqlDbType=System.Data.SqlDbType.Char,
                    Value=this.sexo
                },
                new SqlParameter()
                {
                    ParameterName="@data_nasc",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_nasc
                },
                new SqlParameter()
                {
                    ParameterName="@peso_nasc",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.peso_nasc
                },
                new SqlParameter()
                {
                    ParameterName="@peso_2semanas",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.peso_2semanas
                },
                new SqlParameter()
                {
                    ParameterName="@mae",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.mae
                },
                new SqlParameter()
                {
                    ParameterName="@observacao",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.observacao
                },
                new SqlParameter()
                {
                    ParameterName="@obito",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=this.obito
                }
            };
            //executar
            bd.executaSQL(sql, parametros);
        }
    }
}
