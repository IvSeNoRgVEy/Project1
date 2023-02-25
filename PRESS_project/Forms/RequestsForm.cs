using PRESS_project.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PRESS_project.Forms
{
    public partial class RequestsForm : Form
    {
        DataSet dataSet=new DataSet();
        SqlDataAdapter adapter;
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-TUS4TSK\SQLEXPRESS;Initial Catalog=Press;Integrated Security=true;");
        private readonly PressDbContext pressDbContext;
        public RequestsForm()
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox4.Items.Clear();
            pressDbContext = new PressDbContext();
            pressDbContext.Types.ToList().ForEach(type => comboBox1.Items.Add(type.Name_TP));
            pressDbContext.Publishings.ToList().ForEach(publishing => comboBox4.Items.Add(publishing.Name_PUBL));
            pressDbContext.Countries.ToList().ForEach(country => comboBox6.Items.Add(country.Name_CNTR));        
        }
        private void ClearMenu()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox6.SelectedIndex= -1;
            numericUpDown3.Value = 0;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }
        private void ClearTable()
        {

            if (dataSet.Tables.Count > 0)
            {
                dataSet.Tables[0].Rows.Clear();
                dataSet.Tables[0].Columns.Clear();
            }

        }
        private void SearchPublidhing(int x, int y) // ПОИСК В ИЗДАТЕЛЬСТВЕ ПО ТИПУ И В ЦЕЛОМ
        {
            try
            {
                if (x == -1)
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                    $"from Products,Type,Publishing,Country " +
                    $"WHERE Publishing.Id = {y + 1} and Type.Id = Type_FK and Publishing.Id = Publishing_FK and Country.Id = Country_FK", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                    $"from Products,Type,Publishing,Country " +
                    $"WHERE Publishing.Id = {y + 1} and Type.Id={x + 1} and Type.Id = Type_FK and Publishing.Id = Publishing_FK and Country.Id = Country_FK", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        private void MinMax(string m,int x) // ПОИСК MIN\MAX ЦЕНЫ В ЦЕЛОМ И ПО ТИПУ
        {
            try
            {
                if (x == -1 || checkBox1.Checked == true)
                {
                    adapter = new SqlDataAdapter($"SELECT Name as [Название], {m}(Price) as [Цена] " +
                        $"FROM Products where Price=(select {m}(Price) from Products)  " +
                        $"group by Name", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else
                {
                    adapter = new SqlDataAdapter($"SELECT Name as [Название], {m}(Price) as [Цена] " +
                        $"FROM Products " +
                        $"where Price = (select {m}(Price) from Products, Type where Type.Id = Type_FK and  Type.Id ={x + 1})" +
                        $"group by Name", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void CurculationAndPriceRange(int a,int c,int x,int y) //ПОИСК В ДИАПАЗОНЕ ПО ЦЕНЕ И ТИРАЖУ
        {
            try 
            {
                string temp = " ";
                if (c == 0) temp = "Circulation";
                else temp = "Price";
                if (a == -1 || checkBox1.Checked == true)
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                    $"from Products,Type,Publishing,Country " +
                    $"where Publishing.Id=Publishing_FK and Type.Id=Type_FK and Country.Id=Country_FK and {temp} BETWEEN {x} AND {y}", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                    $"from Products,Type,Publishing,Country " +
                    $"where Publishing.Id=Publishing_FK and Type.Id=Type_FK and Country.Id=Country_FK and Type.Id={a + 1} and Circulation BETWEEN {x} AND {y}", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void AVG(int x,int y,int z,bool b) // ПОИСК СРЕДНЕЙ ЦЕНЫ В ЦЕЛОМ, ПО ТИПУ, ПО ИЗДАНИЮ, ВСЯ ПРЕССА ПОСТУПИВШАЯ ОТ ЗАДАННОГО ИЗДАНИЯ, ЧЬЯ СТОИМОСТЬ БОЛЬШЕ ЧЕМ СРЕДНЯЯ ЗАДАННОЙ ПРЕССЫ ИЗ ЗАДАННОЙ СТРАНЫ 
        {
            try
            {
                if (x == -1 && y == -1 && z == -1 || b == true)
                {
                    adapter = new SqlDataAdapter($"select AVG(Price) as [Средняя цена] from Products", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if (x != -1 && y == -1 && z == -1)
                {
                    adapter = new SqlDataAdapter($"select Name_TP as [Тип], AVG(Price) as [Средняя цена] " +
                        $"from Products,Type " +
                        $"where Type_FK=Type.Id and Type.Id={x + 1}" +
                        $"group by Name_TP;", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if (x == -1 && y != -1 && z == -1)
                {
                    adapter = new SqlDataAdapter($"select Name_PUBL as [Издательство], AVG(Price) as [Средняя цена] " +
                        $"from Products,Publishing " +
                        $"where Publishing_FK=Publishing.Id and Publishing.Id={y + 1}" +
                        $"group by Name_PUBL;", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if (x != -1 && y != -1 && z == -1)
                {
                    adapter = new SqlDataAdapter($"select Name_TP as [Тип],Name_PUBL as [Издательство],AVG(Price) as [Средняя цена] " +
                        $"from Products, Publishing, Type " +
                        $"where Publishing_FK = Publishing.Id and Type.Id = Type_FK and Publishing.Id={y + 1} and  Type.Id = {x + 1}" +
                        $"group by Name_TP,Name_PUBL", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if (x != -1 && y != -1 && z != -1)
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна] " +
                        $"from Products, Type, Publishing, Country " +
                        $"where Type.Id = Type_FK and Publishing.Id = Publishing_FK and Country.Id = Country_FK and Publishing.Id = {y + 1} and Price > (select AVG(Price)" +
                        $"from Products, Type, Publishing, Country " +
                        $"where Publishing_FK = Publishing.Id and Type.Id = {x + 1} and Country.Id = {z + 1})", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else
                    return;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void PriceUpAndDown(bool a, int x,int y) //ПОИСК ПРЕССЫ В ЦЕЛОМ ,ПО ТИПУ,ПО ИЗДАНИЮ ЧЬЯ СТОИМОСТЬ  < ИЛИ > ЗАДАНННОГО ЗНАЧЕНИЯ 
        {
            try
            {
                char T = ' ';
                if (a == false) T = '>';
                else T = '<';

                if (x == -1 && y==-1 || checkBox1.Checked==true)
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                        $"from Products,Type,Publishing,Country " +
                        $"where Publishing.Id=Publishing_FK and Type.Id=Type_FK and Country.Id=Country_FK and price{T}{(int)numericUpDown3.Value}", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if(x != -1 && y == -1)
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                        $"from Products,Type,Publishing,Country " +
                        $"where Publishing.Id=Publishing_FK and Type.Id=Type_FK and Country.Id=Country_FK and price{T}{(int)numericUpDown3.Value} and Type.Id={x + 1}", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if(x == -1 && y != -1)
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                        $"from Products,Type,Publishing,Country " +
                        $"where Publishing.Id=Publishing_FK and Type.Id=Type_FK and Country.Id=Country_FK and price{T}{(int)numericUpDown3.Value} and Publishing.Id={y + 1}", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if(x != -1 && y != -1)
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                        $"from Products,Type,Publishing,Country " +
                        $"where Publishing.Id=Publishing_FK and Type.Id=Type_FK and Country.Id=Country_FK and price{T}{(int)numericUpDown3.Value} and Type.Id={x + 1} and Publishing.Id={y + 1}", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else 
                    return;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void PeriodSearch(string x,int y) //ПОИСК ПРЕССЫ  ПО ПЕРИОДУ ПРОДАЖ
        {
            try
            {
                if (y == -1 || checkBox1.Checked==true)
                {
                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                        $"from Products,Type,Publishing,Country " +
                        $"where Publishing.Id=Publishing_FK and Type.Id=Type_FK and Country.Id=Country_FK and Period = '{x}'", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else
                {

                    adapter = new SqlDataAdapter($"select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Demand as[Спрос в %],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]  " +
                        $"from Products,Type,Publishing,Country " +
                        $"where Publishing.Id=Publishing_FK and Type.Id=Type_FK and Country.Id=Country_FK and Period = '{x}' and Type.Id = {y+1}", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void PopularPressa(int x,int y,int z,bool b) // ПОИСК САМОЙ ПРОДАВАЕМОЙ ПРЕССЫ
        {
            try
            {
                if (x == -1 && y == -1 && z == -1 || b == true)
                {
                    adapter = new SqlDataAdapter($"SELECT Name_TP as [Тип],Name as [Название], MAX(Demand) as [Процент продаж] " +
                        $"FROM Products,Type " +
                        $"where Demand=(select MAX(Demand) from Products) and Type_FK=Type.Id " +
                        $"group by Name_TP,Name", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if (x != -1 && y == -1 && z == -1)
                {
                    adapter = new SqlDataAdapter($"SELECT Name_TP as [Тип],Name as [Название], MAX(Demand) as [Процент продаж] " +
                        $"FROM Products,Type " +
                        $"where Demand=(select MAX(Demand) from Products,Type where Type.Id={x + 1} and Type_FK=Type.Id) and Type_FK=Type.Id and Type.Id={x + 1}  " +
                        $"group by Name_TP,Name", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if (x == -1 && y != -1 && z == -1)
                {
                    adapter = new SqlDataAdapter($"SELECT Name_PUBL as [Издательство],Name as [Название], MAX(Demand) as [Процент продаж] " +
                        $"FROM Products,Publishing " +
                        $"where Demand=(select MAX(Demand) from Products,Publishing where Publishing.Id={y + 1} and Publishing_FK = Publishing.Id) and Publishing_FK=Publishing.Id and Publishing.Id={y + 1}  " +
                        $"group by Name_PUBL,Name", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if (x == -1 && y == -1 && z != -1)
                {
                    adapter = new SqlDataAdapter($"SELECT Name_CNTR as [Страна],Name as [Название], MAX(Demand) as [Процент продаж] " +
                        $"FROM Products,Country " +
                        $"where Demand=(select MAX(Demand) from Products,Country where Country.Id={z + 1} and Country_FK = Country.Id) and Country_FK=Country.Id and Country.Id={z + 1}  " +
                        $"group by Name_CNTR,Name", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else if (x != -1 && y != -1 && z != -1)
                {
                    adapter = new SqlDataAdapter($"SELECT Name_CNTR as [Страна],Name_TP as [Тип],Name_PUBL as [Издательство],Name as [Название], MAX(Demand) as [Процент продаж] " +
                        $"FROM Products, Country, Type, Publishing" +
                        $"where Demand = (select MAX(Demand) from Products, Publishing, Country, Type where Publishing.Id = {y + 1} and Publishing.Id = Publishing_FK and Country.Id = {z + 1} and Country.Id = Country_FK and Type.Id = {x + 1} and Type.Id = Type_FK ) and Publishing.Id = {y + 1} and Publishing.Id = Publishing_FK and Country.Id = {z + 1} and Country.Id = Country_FK and Type.Id = {x + 1} and Type.Id = Type_FK" +
                        $"group by Name_CNTR, Name_TP, Name_PUBL, Name", connection);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                }
                else return;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearTable();
            AVG(comboBox1.SelectedIndex, comboBox4.SelectedIndex, comboBox6.SelectedIndex,checkBox1.Checked);
            ClearMenu();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked== true) comboBox1.Enabled = false;
            else comboBox1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearTable();
            if (numericUpDown3.Value == 0) return;
            PriceUpAndDown(checkBox2.Checked,comboBox1.SelectedIndex,comboBox4.SelectedIndex);
            ClearMenu();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            ClearTable();
            if (numericUpDown2.Value == 0) return;
            CurculationAndPriceRange(comboBox1.SelectedIndex,comboBox3.SelectedIndex,(int)numericUpDown1.Value,(int) numericUpDown2.Value);
            ClearMenu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearTable();
            SearchPublidhing(comboBox1.SelectedIndex,comboBox4.SelectedIndex);
            ClearMenu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearTable();
            MinMax(comboBox2.SelectedItem.ToString(), comboBox1.SelectedIndex);
          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClearTable();
            PeriodSearch(comboBox5.SelectedItem.ToString(),comboBox1.SelectedIndex);
            ClearMenu();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClearTable();
            PopularPressa(comboBox1.SelectedIndex, comboBox4.SelectedIndex, comboBox6.SelectedIndex, checkBox1.Checked);
            ClearMenu();
        }
    }
}
