using M14_15_TrabalhoModelo_2021_WIP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Obitos
{
    public class Obito
    {
        //public int nobito { get; set; }
        public string nanimal { get; set; }
        public string ncria { get; set; }
        public string relatorio { get; set; }

        public Obito(/*int nobito,*/ string nanimal, string ncria, string relatorio)
        {
            //this.nobito = nobito;
            this.nanimal = nanimal;
            this.ncria = ncria;
            this.relatorio = relatorio;
        }


        public void Adicionar(BaseDados bd)
        {
            string sql = "";
            //iniciar transação
            SqlTransaction transacao = bd.iniciarTransacao();
            //registar o empréstimo
            if (nanimal==null)
            {
                sql = $"INSERT INTO obitos(ncria,relatorio)"+
                $" VALUES (@ncria,@relatorio)";
            }
            else
            {
                sql = $"INSERT INTO obitos(nanimal,relatorio)"+
                $" VALUES (@nanimal,@relatorio)";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@relatorio",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.relatorio
                }
            };

            if (nanimal != null)
                parametros.Add(
                new SqlParameter()
                {
                    ParameterName = "@nanimal",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = this.nanimal
                });

            if (ncria != null)
                parametros.Add(
                new SqlParameter()
                {
                    ParameterName = "@ncria",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = this.ncria
                });

            bd.executaSQL(sql, parametros, transacao);
            
            //terminar transação
            transacao.Commit();
        }

        public void Registo(BaseDados bd, string id) 
        {
            string sql1;
            bool o = id.Contains("PT");
            if (o == true)
            {
                sql1 = "update animais set obito = 1 where nanimal = '" + id + "'";
            }
            else
            {
                sql1 = "update crias set obito = 1 where ncria = '" + id + "'";
            }

            bd.executaSQL(sql1);
        }


        public static List<Obito> ListaTodos(BaseDados bd)
        {
            string sql = "SELECT * FROM obitos";

            DataTable dados = bd.devolveSQL(sql);
            List<Obito> lista = new List<Obito>();
            foreach (DataRow linha in dados.Rows)
            {

                string nanimal = linha["nanimal"].ToString();
                string ncria = linha["ncria"].ToString();
                string rel = linha["relatorio"].ToString();
                Obito novo = new Obito(nanimal, ncria, rel);
                lista.Add(novo);
            }
            return lista;
        }
    }
}
