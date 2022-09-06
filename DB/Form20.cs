﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DB
{
    public partial class Form20 : Form
    {
        NpgsqlConnection npgsqlConnection1;

        public Form20(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();

            npgsqlConnection1 = npgsqlConnection;
            npgsqlConnection1.Close();
            NpgsqlCommand cmd = npgsqlConnection.CreateCommand();
            DataTable dt = new DataTable();
            cmd.CommandText = "SELECT name_dish FROM dish WHERE id_dish IN (SELECT id_dish FROM dish_client);";
            npgsqlConnection.Open();
            cmd.ExecuteNonQuery();
            npgsqlConnection.Close();
            NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(cmd);
            npgsqlDataAdapter.Fill(dt);
            CopyDataTableToList(dt, listView1);
        }

        public static void CopyDataTableToList(DataTable data, ListView lv)
        {
            lv.BeginUpdate();

            if (lv.Columns.Count != data.Columns.Count)
            {
                lv.Columns.Clear();

                foreach (DataColumn column in data.Columns)
                {
                    lv.Columns.Add(column.ColumnName);
                }
            }

            lv.Items.Clear();

            foreach (DataRow row in data.Rows)
            {
                var list = row.ItemArray.Select(ob => ob.ToString()).ToArray();
                ListViewItem item = new ListViewItem(list);
                lv.Items.Add(item);
            }

            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            lv.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(npgsqlConnection1, "povar");
            form4.Show();
            this.Close();
        }
    }
}
