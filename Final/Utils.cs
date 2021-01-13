using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace M14_15_TrabalhoModelo_2021_WIP
{
    class Utils
    {
        static public byte[] ImagemParaVetor(string ficheiro)
        {
            FileStream fs = new FileStream(ficheiro, FileMode.Open, FileAccess.Read);
            byte[] dados = new byte[fs.Length];
            fs.Read(dados, 0, (int)fs.Length);
            fs.Close();
            return dados;
        }
        static public void VetorParaImagem(byte[] imagem, string ficheiro)
        {
            FileStream fs = new FileStream(ficheiro, FileMode.Create, FileAccess.Write);
            fs.Write(imagem, 0, imagem.GetUpperBound(0));
            fs.Close();
        }
        static public string pastaDoPrograma()
        {
            string pastaInicial = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            pastaInicial += @"\TrabalhoFinal";
            if (System.IO.Directory.Exists(pastaInicial) == false)
                System.IO.Directory.CreateDirectory(pastaInicial);
            return pastaInicial;
        }
        /// <summary>
        /// Adaptado de
        /// https://www.c-sharpcorner.com/uploadfile/mahesh/printing-in-wpf/
        /// https://stackoverflow.com/questions/37698316/print-all-data-in-the-datagrid-in-wpf/50281632#50281632
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataGrid"></param>
        /// <param name="title"></param>
        static public void printDG<T>(DataGrid dataGrid, string title)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                FlowDocument fd = new FlowDocument();

                Paragraph p = new Paragraph(new Run(title));
                p.FontStyle = dataGrid.FontStyle;
                p.FontFamily = dataGrid.FontFamily;
                p.FontSize = 18;
                fd.Blocks.Add(p);

                Table table = new Table();
                TableRowGroup tableRowGroup = new TableRowGroup();
                TableRow r = new TableRow();
                fd.PageWidth = printDialog.PrintableAreaWidth;
                fd.PageHeight = printDialog.PrintableAreaHeight;
                fd.BringIntoView();

                fd.TextAlignment = TextAlignment.Center;
                fd.ColumnWidth = 500;
                table.CellSpacing = 0;

                var headerList = dataGrid.Columns.Select(e => e.Header.ToString()).ToList();
                List<dynamic> bindList = new List<dynamic>();

                for (int j = 0; j < headerList.Count; j++)
                {
                    r.Cells.Add(new TableCell(new Paragraph(new Run(headerList[j]))));
                    r.Cells[j].ColumnSpan = 4;
                    r.Cells[j].Padding = new Thickness(4);
                    r.Cells[j].BorderBrush = Brushes.Black;
                    r.Cells[j].FontWeight = FontWeights.Bold;
                    r.Cells[j].Background = Brushes.DarkGray;
                    r.Cells[j].Foreground = Brushes.White;
                    r.Cells[j].BorderThickness = new Thickness(1, 1, 1, 1);

                    var binding = (dataGrid.Columns[j] as DataGridBoundColumn).Binding as Binding;
                    bindList.Add(binding.Path.Path);
                }

                tableRowGroup.Rows.Add(r);
                table.RowGroups.Add(tableRowGroup);

                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    dynamic row;

                    if (dataGrid.ItemsSource.ToString().ToLower() == "system.data.linqdataview")
                    { 
                        row = (DataRowView)dataGrid.Items.GetItemAt(i); 
                    }
                    else
                    {
                        row = (T)dataGrid.Items.GetItemAt(i);
                    }

                    table.BorderBrush = Brushes.Gray;
                    table.BorderThickness = new Thickness(1, 1, 0, 0);
                    table.FontStyle = dataGrid.FontStyle;
                    table.FontFamily = dataGrid.FontFamily;
                    table.FontSize = 13;
                    tableRowGroup = new TableRowGroup();
                    r = new TableRow();
                    var lista = row.GetType().GetProperties();
                    for (int j = 0; j <lista.Length ; j++)
                    {
                        /*if (dataGrid.ItemsSource.ToString().ToLower() == "system.data.linqdataview")
                        {
                            r.Cells.Add(new TableCell(new Paragraph(new Run(row.Row.ItemArray[j].ToString()))));
                        }
                        else
                        {*/
                        //r.Cells.Add(new TableCell(new Paragraph(new Run(row.GetType().GetProperty(bindList[j]).GetValue(row, null)))));
                        // }.
                        if (lista[j].GetValue(row).ToString() == "False")
                        {
                            r.Cells.Add(new TableCell(new Paragraph(new Run("Não"))));
                        }
                        else if (lista[j].GetValue(row).ToString()== "True")
                        {
                            r.Cells.Add(new TableCell(new Paragraph(new Run("Sim"))));
                        }
                        else
                        {
                            r.Cells.Add(new TableCell(new Paragraph(new Run(lista[j].GetValue(row).ToString()))));
                        }
                        

                        r.Cells[j].ColumnSpan = 4;
                        r.Cells[j].Padding = new Thickness(4);
                        r.Cells[j].BorderBrush = Brushes.DarkGray;
                        r.Cells[j].BorderThickness = new Thickness(0, 0, 1, 1);
                    }

                    tableRowGroup.Rows.Add(r);
                    table.RowGroups.Add(tableRowGroup);
                }

                fd.Blocks.Add(table);
                printDialog.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "");
            }
        }
    }
}
