using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;
using PRESS_project.Model;

namespace PRESS_project.Forms
{
    public partial class AdminForm : Form
    {
        DataSet dataSet = new DataSet();
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-TUS4TSK\SQLEXPRESS;Initial Catalog=Press;Integrated Security=true;");
        SqlDataAdapter adapter;
        SqlCommand command;
        private readonly PressDbContext pressDbContext;
        public AdminForm()
        {
            InitializeComponent();
           
            connection.Open();
            pressDbContext = new PressDbContext(); // ДОБАВЛЕНИЕ ЭЛЕМЕНТОВ В COMBOBOX
            pressDbContext.Countries.ToList().ForEach(country => comboBox2.Items.Add(country.Name_CNTR));
            pressDbContext.Types.ToList().ForEach(type => comboBox3.Items.Add(type.Name_TP));
            pressDbContext.Types.ToList().ForEach(type => comboBox1.Items.Add(type.Name_TP));
            pressDbContext.Publishings.ToList().ForEach(publ => comboBox4.Items.Add(publ.Name_PUBL));
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataSet.Tables.Count > 0)
            {
                dataSet.Tables[0].Rows.Clear();
                dataSet.Tables[0].Columns.Clear();
            }
            ShowTable();
        }

        private void button1_Click_1(object sender, EventArgs e) // РЕДАКТИРОВАНИЕ ДАННЫХ
        {

            string save = "update Products set Name = @pName,Circulation = @pCirculation,Price = @pPrice," +
            "Period = @pPeriod,Demand = @pDemand"+
            " where Products.ID = @pID;";
            command = new SqlCommand(save, connection);
                command.Parameters.Add(new SqlParameter("@pName", SqlDbType.NVarChar, 50));
                command.Parameters.Add(new SqlParameter("@pCirculation", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@pPrice", SqlDbType.Money));
                command.Parameters.Add(new SqlParameter("@pPeriod", SqlDbType.NVarChar, 50));
                command.Parameters.Add(new SqlParameter("@pDemand", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@pID", SqlDbType.Int));

                command.Parameters["@pName"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@pCirculation"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@pPrice"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@pPeriod"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@pDemand"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@pID"].SourceVersion = DataRowVersion.Original;

                command.Parameters["@pName"].SourceColumn = "Название";
                command.Parameters["@pCirculation"].SourceColumn = "Тираж";
                command.Parameters["@pPrice"].SourceColumn = "Цена";
                command.Parameters["@pPeriod"].SourceColumn = "Месяц продажи";
                command.Parameters["@pDemand"].SourceColumn = "Спрос";
                command.Parameters["@pID"].SourceColumn = "ID";

                adapter.UpdateCommand = command;
                adapter.Update(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
           

            if (dataSet.Tables.Count > 0)
            {
                dataSet.Tables[0].Rows.Clear();
            }
            ShowTable();
        }
        private void ShowTable() // ВЫВОД НА ЭКРАН ИНФОРМАЦИИ ПО ТИПУ
        {
            try
            {
                
                
                    adapter = new SqlDataAdapter($"select Products.Id as [ID],Name as [Название],Circulation as [Тираж],Price as [Цена],Period as [Месяц продажи],Demand as [Спрос],Name_PUBL as [Издательство],Name_CNTR as [Страна],Name_TP as [Тип] " +
                        $"from Products,Publishing,Country,Type" +
                        $" where Name_TP = '{comboBox1.SelectedItem.ToString()}'" +
                        $"and Type.Id=Type_FK " +
                        $"and Publishing.Id = Publishing_FK " +
                        $"and Country.Id = Products.Country_FK; ", connection);
                    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) //ДОБАВЛЕНИЕ СТРАНЫ В ТАБЛИЦУ
        {
            if (textBox1.Text == "") return;
            try
            {
                //Country country = new Country {Name_CNTR=textBox1.Text };
                //pressDbContext.Countries.Add(country);
                //pressDbContext.SaveChanges();
                comboBox2.Items.Add(textBox1.Text);
                command = new SqlCommand($"Insert Into Country (Name_CNTR) " +
                    $"Values(@pName_CNTR)", connection);
                command.Parameters.AddWithValue("@pName_CNTR", textBox1.Text);
                command.ExecuteNonQuery();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

            textBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e) // ДОБАВЛЕНИЕ ТИПА В ТАБЛИЦУ
        {
            if (textBox2.Text == "") return;
            try
            {
                //Model.Type type = new Model.Type { Name_TP = textBox2.Text };   // ПРОБОВАЛ ЧЕРЕЗ ENTITY. 
                //pressDbContext.Types.Add(type);                                 // ДОБАВЛЯЕТ,СОХРАНЯЕТ, НО НЕ ВЫВОДИТ НА ЭКРАН В ПРИЛОЖЕНИИ.  
                //pressDbContext.SaveChanges();                                   // В САМОЙ БАЗЕ ДОБАВЛЯЕТ. НЕ РАЗОБРАЛСЯ.   
                comboBox3.Items.Add(textBox2.Text);
                command = new SqlCommand($"Insert Into Type (Name_TP) " +
                    $"Values(@pName_TP)", connection);
                command.Parameters.AddWithValue("@pName_TP", textBox2.Text);
                command.ExecuteNonQuery();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

            textBox2.Text = "";
        }

        private void button4_Click(object sender, EventArgs e) // ДОБАВЛЕНИЕ ИЗДАТЕЛЬТСВА В ТАБЛИЦУ
        {
            if (textBox3.Text == "") return;
            try
            {
                //Publishing publishing = new Publishing { Name_PUBL = textBox3.Text };
                //pressDbContext.Publishings.Add(publishing);
                //pressDbContext.SaveChanges();
                comboBox4.Items.Add(textBox3.Text);
                command = new SqlCommand($"Insert Into Publishing (Name_PUBL) " +
                    $"Values(@pName_PUBL)", connection);
                command.Parameters.AddWithValue("@pName_PUBL", textBox3.Text);
                command.ExecuteNonQuery();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

            textBox2.Text = "";
        }
        private void button5_Click(object sender, EventArgs e) // ДОБАВЛЕНИЕ ПРОДУКЦИИ В ТАБЛИЦУ
        {
            if (comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1 || comboBox4.SelectedIndex == -1 || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            try
            {
                //Product product=new Product { Name = textBox4.Text,Circulation=Int32.Parse(textBox5.Text),Price=Int32.Parse(textBox6.Text),Period=textBox7.Text,Demand=Int32.Parse(textBox8.Text) };
                //pressDbContext.Products.Add(product); 
                //pressDbContext.SaveChanges();
                command = new SqlCommand($"insert into Products (Name,Circulation,Price,Period,Demand,Publishing_FK,Type_FK,Country_FK)" +
                     $"values(@pName,@pCirculation,@pPrice,@pPeriod,@pDemand,@pPublishing_FK,@pType_FK,@pCountry_FK)", connection);
                command.Parameters.AddWithValue("@pName", textBox4.Text);
                command.Parameters.AddWithValue("@pCirculation", textBox5.Text);
                command.Parameters.AddWithValue("@pPrice", textBox6.Text);
                command.Parameters.AddWithValue("@pPeriod", textBox7.Text);
                command.Parameters.AddWithValue("@pDemand", textBox8.Text);
                command.Parameters.AddWithValue("@pPublishing_FK", comboBox4.SelectedIndex + 1);
                command.Parameters.AddWithValue("@pType_FK", comboBox3.SelectedIndex + 1);
                command.Parameters.AddWithValue("@pCountry_FK", comboBox2.SelectedIndex + 1);
                command.ExecuteNonQuery();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

            
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";

        }
    }
    
}
