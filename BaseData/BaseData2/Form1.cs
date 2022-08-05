using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace BaseData2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FillTable();
            FillTable1();
        }
        string Connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Desktop\BaseData\BaseData2\Liberybase.mdf;Integrated Security=True";
        private void Form1_Load(object sender, EventArgs e)
        {
            
            // TODO: данная строка кода позволяет загрузить данные в таблицу "liberybaseDataSet2.TableLibery". При необходимости она может быть перемещена или удалена.
            this.tableLiberyTableAdapter1.Fill(this.liberybaseDataSet2.TableLibery);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "liberybaseDataSet1.TableJournal". При необходимости она может быть перемещена или удалена.
            this.tableJournalTableAdapter.Fill(this.liberybaseDataSet1.TableJournal);


        }
        private void FillTable()
        {
            string SqlText = "SELECT * FROM [TableLibery]";// Берем данные из таблицы TableLibery
            SqlDataAdapter da = new SqlDataAdapter(SqlText, Connect);// объявляем переменную класса SqlDataAdapter с именем da и  подключаем базу данных
            DataSet ds = new DataSet();// объявляем переменную класса DataSet с именем ds
            da.Fill(ds, "[TableLibery]");// используем метод Fill() класса SqlDataAdapter, для добавления или обновления строки в System.Data.DataSet
            dataGridView1.DataSource = ds.Tables["[TableLibery]"].DefaultView;// записываем в таблицу обновленные данные
        }
        public void MyExecuteNonQuery(string SqlText)
        {

            // выделение памяти с инициализацией строки соединения с базой данных
            SqlConnection con = new SqlConnection(Connect);
            con.Open(); // открыть источник данных
            SqlCommand cmd = con.CreateCommand(); // задать SQL-команду
            cmd.CommandText = SqlText; // задать командную строку
            cmd.ExecuteNonQuery(); // выполнить SQL-команду
            con.Close(); // закрыть источник данных
        }

        private void buttonAdd_Click(object sender, EventArgs e) //Добавить в БД
        {
            string SqlText;// сформировать SQL-строку
            bool p = true;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                    if ((textBoxName.Text == Convert.ToString(dataGridView1.Rows[i].Cells[1].Value)) && (textBoxAuthor.Text == Convert.ToString(dataGridView1.Rows[i].Cells[2].Value)) && (textBoxYears.Text == Convert.ToString(dataGridView1.Rows[i].Cells[3].Value)))
                        p = false;
            }
            if (p == false)
            {
                MessageBox.Show("Такая запись уже есть в БД");
                p = true;
            }
            else
            {
                if ((textBoxName.Text != "") && (textBoxAuthor.Text != "") && (textBoxYears.Text != "") && (textBoxPublish.Text != ""))
                {
                    SqlText = "INSERT INTO [TableLibery] ([Id], [Номер], [Название книги], [Автор], [Год], [Издательство], [Описание], [Наличие]) VALUES (";
                    SqlText = SqlText + "\'" + dataGridView1.RowCount + "\', N";
                    SqlText = SqlText + "\'" + dataGridView1.RowCount + "\', N";
                    SqlText = SqlText + "\'" + textBoxName.Text + "\', N";
                    SqlText = SqlText + "\'" + textBoxAuthor.Text + "\', ";
                    SqlText = SqlText + "\'" + textBoxYears.Text + "\', N";
                    SqlText = SqlText + "\'" + textBoxPublish.Text + "\', N";
                    SqlText = SqlText + "\'" + textBoxDescrip.Text + "\', ";
                    SqlText = SqlText + "\' true \')";

                    // выполнить SQL-команду
                    MyExecuteNonQuery(SqlText);
                    // отобразить таблицу Table 
                    FillTable();
                }
                else
                    MessageBox.Show("Введены не все данные");
            }
        }

        int l;
        int index;

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult delete = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
            if (delete == DialogResult.Cancel)
            {
                e.Cancel = true;

            }
            l = Convert.ToInt32(e.Row.Cells[0].Value);
            index = dataGridView1.CurrentRow.Index;
            string SqlText = "DELETE FROM [TableLibery] WHERE [TableLibery].Номер = " + Convert.ToString(dataGridView1[0, index].Value);

            MyExecuteNonQuery(SqlText);
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            for (int i = l + 1; i < dataGridView1.Rows.Count + 1; i++)
            {

                string SqlText1 = "UPDATE [TableLibery] SET [TableLibery].Номер =" + (i - 1) + " WHERE [TableLibery].Номер =" + i;
                MyExecuteNonQuery(SqlText1);
                string SqlText2 = "UPDATE [TableLibery] SET [TableLibery].Id =" + (i - 1) + " WHERE [TableLibery].ID =" + i;
                MyExecuteNonQuery(SqlText2);

            }

            FillTable();
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;
           textBoxName1.Text = Convert.ToString(dataGridView1.Rows[index].Cells[1].Value);
           textBoxAuthor1.Text = Convert.ToString(dataGridView1.Rows[index].Cells[2].Value);
        }

        public void FillTable1()
        {
            string SqlText = "SELECT * FROM [TableJournal]";// Берем данные из таблицы TableJournal
            SqlDataAdapter Sqlda = new SqlDataAdapter(SqlText, Connect); // объявляем переменную класса SqlDataAdapter с именем Sqlda и  подключаем базу данных
            DataSet ds = new DataSet(); // объявляем переменную класса DataSet с именем ds
            Sqlda.Fill(ds, "[TableJournal]"); // используем метод Fill() класса SqlDataAdapter, для добавления или обновления строки в System.Data.DataSet 
            dataGridView2.DataSource = ds.Tables["[TableJournal]"].DefaultView; // записываем в таблицу обновленные данные
        }
        private void buttonTake_Click(object sender, EventArgs e)// забрать из БД
        {
            int index = dataGridView1.CurrentRow.Index;

            if (Convert.ToString(dataGridView1.Rows[index].Cells[6].Value) == "false     ") MessageBox.Show("Такую книгу уже забрали");
            else if (textBoxFIO.Text == "") MessageBox.Show("Введите данные студента");
            else
            {

                string SqlText, data;

                data = dateTimePicker1.Value.ToString("dd/MM/yyyy");


                // сформировать SQL-строку

                SqlText = "INSERT INTO [TableJournal] ([Id], [Название книги], [Автор], [До какого числа], [Студент]) VALUES (";
                SqlText = SqlText + "\'" + dataGridView2.RowCount + "\', N";
                SqlText = SqlText + "\'" + textBoxName1.Text + "\', N";
                SqlText = SqlText + "\'" + textBoxAuthor1.Text + "\',";
                SqlText = SqlText + "\'" + data + "\',N";
                SqlText = SqlText + "\'" + textBoxFIO.Text + "\')";


                // выполнить SQL-команду
                MyExecuteNonQuery(SqlText);
                // отобразить таблицу Table 
                FillTable1();

                SqlText = "UPDATE [TableLibery] SET Наличие=" + "\'false     \'" + " WHERE Номер =" + "\'" + (index + 1) + "\'";

                MyExecuteNonQuery(SqlText);
                FillTable();
            }

        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            int index1 = dataGridView2.CurrentRow.Index;

            int a = 0;
            
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                if (Convert.ToString(dataGridView2.Rows[index1].Cells[0].Value) == Convert.ToString(dataGridView1.Rows[j].Cells[1].Value))
                    a = j;
                }
            

            string SqlText2 = "UPDATE [TableLibery] SET Наличие=" + "\'true     \'" + " WHERE Номер =" + "\'" + (a + 1) + "\'";

            MyExecuteNonQuery(SqlText2);
            FillTable();

            string SqlText = "DELETE FROM [TableJournal] WHERE Id =" + (index1 + 1);
            MyExecuteNonQuery(SqlText);
            FillTable1();

            for (int i = index1 + 1; i < dataGridView2.Rows.Count; i++)
            {
                    string SqlText1 = "UPDATE [TableJournal] SET [TableJournal].Id =" + i  + " WHERE [TableJournal].Id =" + (i + 1);
                    MyExecuteNonQuery(SqlText1);
            }
            
            FillTable1();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                    dataGridView1.Rows[i].Selected = false;

            }
            int g=0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    if ((textBoxSearch1.Text == Convert.ToString(dataGridView1.Rows[i].Cells[1].Value) && (textBoxSearch1.Text != "")) || (textBoxSearch2.Text == Convert.ToString(dataGridView1.Rows[i].Cells[2].Value) && (textBoxSearch2.Text != "")) || (textBoxSearch3.Text == Convert.ToString(dataGridView1.Rows[i].Cells[3].Value) && (textBoxSearch3.Text != "")))
                        dataGridView1.Rows[i].Selected = true;
                    else
                        g++;

                }
            }
            if (g == (dataGridView1.Rows.Count * dataGridView1.Columns.Count))
            {
                MessageBox.Show("Ничего не найдено");
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                            dataGridView1.Rows[i].Selected = false;

                }
            }

        }

        private void textBoxName_Enter(object sender, EventArgs e)
        {
            if (textBoxName.Text != "")
                textBoxName.Text = "";
        }

        private void textBoxAuthor_Enter(object sender, EventArgs e)
        {
            if (textBoxAuthor.Text != "")
                textBoxAuthor.Text = "";
        }

        private void textBoxYears_Enter(object sender, EventArgs e)
        {
            if (textBoxYears.Text != "")
                textBoxYears.Text = "";
        }

        private void textBoxPublish_Enter(object sender, EventArgs e)
        {
            if (textBoxPublish.Text != "")
                textBoxPublish.Text = "";
        }

        private void textBoxSearch1_Enter(object sender, EventArgs e)
        {
            if (textBoxSearch1.Text != "")
                textBoxSearch1.Text = "";
        }

        private void textBoxSearch2_Enter(object sender, EventArgs e)
        {
            if (textBoxSearch2.Text != "")
                textBoxSearch2.Text = "";
        }

        private void textBoxSearch3_Enter(object sender, EventArgs e)
        {
            if (textBoxSearch3.Text != "")
                textBoxSearch3.Text = "";
        }

        
    }
}
