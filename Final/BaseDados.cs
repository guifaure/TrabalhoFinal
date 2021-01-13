using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace M14_15_TrabalhoModelo_2021_WIP
{
    public class BaseDados
    {
        string BDName = "Pontinhas";
        string caminho;
        string strLigacao;
        SqlConnection ligacaoBD;

        public BaseDados()
        {
            strLigacao = ConfigurationManager.ConnectionStrings["servidor"].ToString();
            //se a bd existe
            //definir o caminho para bd
            caminho = Utils.pastaDoPrograma() + @"\" + BDName + ".mdf";
            if (File.Exists(caminho) == false)
                criarBD();

            //atualizar a string ligação
            ligacaoBD = new SqlConnection(strLigacao);
            //abrir a ligação a bd
            ligacaoBD.Open();
            ligacaoBD.ChangeDatabase(BDName);
        }

        internal SqlTransaction iniciarTransacao()
        {
            return ligacaoBD.BeginTransaction();
        }

        ~BaseDados()
        {
            try
            {
                ligacaoBD.Close();
            }
            catch { }
        }
        private void criarBD()
        {
            //nome da bd

            //abrir ligação
            ligacaoBD = new SqlConnection(strLigacao);
            ligacaoBD.Open();
            //criar bd
            string strSQL = $"CREATE DATABASE {BDName} ON PRIMARY (NAME={BDName}, FILENAME='{caminho}')";
            executaSQL(strSQL);
            ligacaoBD.ChangeDatabase(BDName);
            //criar as tabelas
            strSQL = @"create table animais(
	                    nanimal varchar(50) primary key,
	                    sexo char(1),
	                    observacao varchar(max),
	                    leite bit,
	                    media_leite decimal(3, 1),
	                    obito bit
                    );

                    create table crias(
	                    ncria varchar(50) primary key,
	                    sexo char(1),
	                    data_nasc date,
	                    peso_nasc decimal(4,2),
	                    peso_2semanas decimal(4,2),
	                    mae varchar(50),
	                    observacao varchar(max),
	                    obito bit
                    );

                    create table obitos(
	                    nobito int identity primary key,
	                    nanimal varchar(50) references animais(nanimal),
	                    ncria varchar(50) references crias(ncria),
	                    relatorio varchar(max)
                    );

                    create table terrenos(
	                    nome varchar(100) primary key,
	                    tamanho int,
	                    localizacao varchar(50),
	                    estado varchar(50),
	                    semente varchar(50),
                        datacultivo date,
                        observacao varchar(max)
                    )";
            executaSQL(strSQL);
            ligacaoBD.Close();
        }

        #region SQL
        public void executaSQL(string strSQL)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            comando.ExecuteNonQuery();
            comando.Dispose();
            comando = null;
        }
        public void executaSQL(string strSQL, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            comando.Parameters.AddRange(parametros.ToArray());
            comando.ExecuteNonQuery();
            comando.Dispose();
            comando = null;
        }
        public void executaSQL(string strSQL, List<SqlParameter> parametros, SqlTransaction transacao)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            if (parametros != null)
                comando.Parameters.AddRange(parametros.ToArray());
            comando.Transaction = transacao;
            comando.ExecuteNonQuery();
            comando.Dispose();
            comando = null;
        }
        public int executaEDeolveSQL(string strSQL, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            comando.Parameters.AddRange(parametros.ToArray());
            int id = (int)comando.ExecuteScalar();
            comando.Dispose();
            comando = null;
            return id;
        }
        public DataTable devolveSQL(string strSQL)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            DataTable dados = new DataTable();
            SqlDataReader registos = comando.ExecuteReader();
            dados.Load(registos);
            registos.Close();
            registos = null;
            comando.Dispose();
            comando = null;
            return dados;
        }
        public DataTable devolveSQL(string strSQL, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            DataTable dados = new DataTable();
            comando.Parameters.AddRange(parametros.ToArray());
            SqlDataReader registos = comando.ExecuteReader();
            dados.Load(registos);
            registos.Close();
            registos = null;
            comando.Dispose();
            comando = null;
            return dados;
        }
        public DataTable devolveSQL(string strSQL, List<SqlParameter> parametros, SqlTransaction transacao)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            DataTable dados = new DataTable();
            comando.Transaction = transacao;
            comando.Parameters.AddRange(parametros.ToArray());
            SqlDataReader registos = comando.ExecuteReader();
            dados.Load(registos);
            registos.Close();
            registos = null;
            comando.Dispose();
            comando = null;
            return dados;
        }

        #endregion

    }
}
