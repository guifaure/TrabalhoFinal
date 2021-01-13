using M14_15_TrabalhoModelo_2021_WIP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Animais
{
    class Animal
    {
        public string nanimal { get; set; }
        public char sexo { get; set; }
        public string observacao { get; set; }
        public bool leite { get; set; }
        public double med_leite { get; set; }
        public bool obito { get; set; }


        public Animal(string nanimal, char sexo, string observacao, bool leite, double med_leite, bool obito)
        {
            this.nanimal = nanimal;
            this.sexo = sexo;
            this.observacao = observacao;
            this.leite = leite;
            this.med_leite = med_leite;
            this.obito = obito;
        }

        public void Adicionar(BaseDados bd)
        {
            string sql = "";
            //iniciar transação
            SqlTransaction transacao = bd.iniciarTransacao();
            //registar o empréstimo
            sql = $"INSERT INTO animais(nanimal,sexo,observacao, leite, media_leite, obito)" +
                $" VALUES (@nanimal,@sexo,@observacao,@leite, @media_leite, @obito)";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nanimal",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.nanimal
                },
                new SqlParameter()
                {
                    ParameterName="@sexo",
                    SqlDbType=System.Data.SqlDbType.Char,
                    Value=this.sexo
                },
                new SqlParameter()
                {
                    ParameterName="@observacao",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.observacao
                },
                new SqlParameter()
                {
                    ParameterName="@leite",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=this.leite
                },
                new SqlParameter()
                {
                    ParameterName="@media_leite",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.med_leite
                },
                new SqlParameter()
                {
                    ParameterName="@obito",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=false
                }
            };
            bd.executaSQL(sql, parametros, transacao);

            //terminar transação
            transacao.Commit();
        }

        //Mostrar todos
        public static List<Animal> ListaTodosAnimais(BaseDados bd)
        {
            string sql = $@"SELECT * FROM animais";

            DataTable dados = bd.devolveSQL(sql);
            List<Animal> lista = new List<Animal>();
            foreach (DataRow linha in dados.Rows)
            {
                string nanimal = linha["nanimal"].ToString();
                char sexo = char.Parse(linha["sexo"].ToString());
                string obs = linha["observacao"].ToString();
                bool leite = bool.Parse(linha["leite"].ToString());
                double med = double.Parse(linha["media_leite"].ToString());
                bool obito = bool.Parse(linha["obito"].ToString());
                Animal novo = new Animal(nanimal, sexo, obs, leite, med, obito);
                lista.Add(novo);
            }
            return lista;
        }

        //get
        public static Animal Get(BaseDados bd, int id)
        {
            string sql = $@"SELECT * FROM animais WHERE nanimal=" + id;
            DataTable dados = bd.devolveSQL(sql);
            string nanimal = dados.Rows[0]["nanimal"].ToString();
            char sexo = char.Parse(dados.Rows[0]["sexo"].ToString());
            string obs = dados.Rows[0]["observacao"].ToString();
            bool leite = bool.Parse(dados.Rows[0]["leite"].ToString());
            double med = double.Parse(dados.Rows[0]["media_leite"].ToString());
            bool obito = bool.Parse(dados.Rows[0]["obito"].ToString());
            Animal an = new Animal(nanimal, sexo, obs, leite, med, obito);
            return an;
        }

        internal void Atualizar(BaseDados bd)
        {
            string sql = $@"UPDATE animais SET sexo=@sexo,
                        leite=@leite,media_leite=@media_leite, observacao=@observacao, obito=@obito
                        WHERE nanimal=@nanimal";
            //parametros
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nanimal",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.nanimal
                },
                new SqlParameter()
                {
                    ParameterName="@sexo",
                    SqlDbType=System.Data.SqlDbType.Char,
                    Value=this.sexo
                },
                new SqlParameter()
                {
                    ParameterName="@leite",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=this.leite
                },
                new SqlParameter()
                {
                    ParameterName="@media_leite",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.med_leite
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
