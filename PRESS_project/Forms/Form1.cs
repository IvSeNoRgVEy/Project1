using PRESS_project.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


//PASSWORD = 123

namespace PRESS_project
{
    public partial class Form1 : Form
    {
        
        DataSet dataSet = new DataSet();
        SqlCommandBuilder builder;
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-TUS4TSK\SQLEXPRESS;Initial Catalog=Press;Integrated Security=true;");
        SqlDataAdapter adapter;
        AdminForm adminForm;
        RequestsForm requestsForm;
        Thread thread1;
        Thread thread2;

        
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            connection.Open();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowTables(comboBox1.SelectedItem.ToString());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowTables(comboBox1.SelectedItem.ToString());
        }


        private void textBox1_TextChanged(object sender, EventArgs e) //ПОИСК ПО ИЗДАНИЮ И НАЗВАНИЮ
        {
            if (dataSet.Tables.Count > 0)
            {
                dataSet.Tables[0].Rows.Clear();
                dataSet.Tables[0].Columns.Clear();
            }
            try
            {
                adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]" +
                $"from Products,Type,Publishing,Country " +
                $"where Name like'{textBox1.Text}%' and Type.Id=Type_FK and Publishing.Id=Publishing_FK and Country.Id=Country_FK " +
                $"or Name_PUBL like'{textBox1.Text}%' and Type.Id=Type_FK and Publishing.Id=Publishing_FK and Country.Id=Country_FK;", connection);
                builder = new SqlCommandBuilder(adapter);
                adapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }


        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ShowTables(comboBox1.SelectedItem.ToString());
        }


        private string OrderByTable(int x) // МЕТОД ДЛЯ СОРТИРОВКИ В ЗАПРОСАХ
        {
            if (x == 0) return "Name";
            else if (x == 1) return "Price";
            else return "Circulation";
        }
        private void ShowTables(string x) // ВЫВОД НА ЭКРАН ПРЕССЫ ПО ТИПУ. СОРТИРОВКА. 
        {
            if (dataSet.Tables.Count > 0)
            {
                dataSet.Tables[0].Rows.Clear();
                dataSet.Tables[0].Columns.Clear();
            }
            try
            {
                string temp = "";
                if (checkBox1.Checked == false) temp = "ASC";
                else temp = "DESC";

                if (comboBox1.SelectedIndex == 0)
                {

                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна] " +
                        $"from Products,Type,Publishing,Country " +
                        $"where Type.Id=Type_FK and Publishing.Id=Publishing_FK and Country.Id=Country_FK " +
                        $"order by {OrderByTable(comboBox2.SelectedIndex)} {temp};", connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна] " +
                        $"from Products,Type,Publishing,Country " +
                        $"where Name_TP='{comboBox1.SelectedItem.ToString()}' and Type.Id=Type_FK and Publishing.Id=Publishing_FK and Country.Id=Country_FK " +
                        $"order by {OrderByTable(comboBox2.SelectedIndex)} {temp} ;", connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) // ПРОВЕРКА ПАРОЛЯ
        {
            if (textBox2.Text != "123") MessageBox.Show("Неправильный пароль");
            else
            {
                thread1 = new Thread(NewFormAdmin);
                thread1.IsBackground=true;
                thread1.Start();
            }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            thread2 = new Thread(NewFormRequest);
            thread2.IsBackground = true;
            thread2.Start();
        }
        void NewFormRequest() // ФОРМА ЗАПРОСОВ
        {
            try
            {
                requestsForm = new RequestsForm();
                requestsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        void NewFormAdmin() // ФОРМА РЕДАКТИРОВАНИЯ
        {
            try
            {
                adminForm = new AdminForm();
                adminForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }

}
    

