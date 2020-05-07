using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data.SqlClient;
using DGVPrinterHelper;
using System.Drawing.Printing;
using System.Drawing.Printing;

namespace TrimAplikacija_V2._0
{
    public class UI
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        SqlDataReader sqlDataReader;
        public void CreateButton(string companyName, FlowLayoutPanel layoutPanel, EventHandler eventHandler)
        {
            string noSpaceName = companyName.Replace(" ", string.Empty);
            Button button = new Button();
            button.Location = new Point(22, 250);
            button.Name = $"btn{noSpaceName}";

            if (layoutPanel.Name == "flowLayoutPanel1")
                button.Name = $"btn{noSpaceName}";
            else if (layoutPanel.Name == "flowLayoutPanel2")
                button.Name = $"btn{noSpaceName}2";

            button.Text = $"{companyName}";
            button.Size = new Size(layoutPanel.ClientSize.Width - 5, 23);
            button.Font = new System.Drawing.Font("Segoe UI", 8);
            button.Padding = new Padding(0);
            button.BackColor = Color.White;
            button.FlatStyle = FlatStyle.Popup;
            button.Click += new EventHandler(eventHandler);
            button.Anchor = AnchorStyles.None;
            button.BringToFront();

            layoutPanel.Controls.Add(button);
        }

        public void LoadButtons(FlowLayoutPanel layoutPanel1, FlowLayoutPanel layoutPanel2, EventHandler eventHandler)
        {
            using (sqlConnection = Connection.AddConnection())
            {
                string querry = "SELECT dbo.firme.id_firme, dbo.firme.naziv FROM dbo.firme;";
                sqlCommand = new SqlCommand(querry, sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    string[] companies = new[] { sqlDataReader["naziv"].ToString() };

                    // Populates button in first flow layout panel ( In tab 'Zaposleni - Detalji')
                    foreach (var company in companies)
                    {
                        CreateButton(companyName: company, layoutPanel: layoutPanel1, eventHandler);
                    }

                    // Populates button in second flow layout panel ( In tab 'Datumi uplate')
                    foreach (var company in companies)
                    {
                        CreateButton(companyName: company, layoutPanel: layoutPanel2, eventHandler);
                    }
                }
            }
        }

        public void UIRemoveButtons(FlowLayoutPanel layoutPanel, DataGridView dataGrid)
        {
            foreach (Control c in layoutPanel.Controls)
            {
                if (c.GetType() == typeof(Button))
                {
                    if (((Button)c).Text == dataGrid.CurrentRow.Cells["naziv"].Value.ToString())
                    {
                        layoutPanel.Controls.Remove(c);
                    }
                }
            }
        }
        
        
        public static List<Button> GetAllButons(FlowLayoutPanel flowLayoutPanel)
        {
            List<Button> buttons = new List<Button>();
            foreach (Control c in flowLayoutPanel.Controls)
            {
                if (c.GetType() == typeof(Button))
                {
                    buttons.Add((Button)c);
                }
            }
            return buttons;
        }

        public static void SearchButton(FlowLayoutPanel flowLayoutPanel, TextBox textBox)
        {
            List<string> companies = Connection.SearchQuery($"SELECT naziv FROM dbo.firme WHERE naziv LIKE '%{textBox.Text}%';");
            List<Button> buttons = UI.GetAllButons(flowLayoutPanel);

            foreach (Button button in buttons)
            {
                foreach (var company in companies)
                {
                    if(button.Text.ToLower() == company.ToLower())
                    {
                        button.Visible = true;
                    }
                    else if (button.Text != company)
                    {
                        button.Visible = false;
                    }
                    else if (textBox.Text == string.Empty)
                    {
                        foreach (Button button1 in buttons)
                        {
                            button1.Visible = true;
                        }
                    }
                }
            }

            textBox.Text = string.Empty;
        }

        public static void ShowAllButtons(FlowLayoutPanel flowLayoutPanel)
        {
            List<Button> buttons = UI.GetAllButons(flowLayoutPanel);

            foreach (Button button in buttons)
            {
                if (button.Visible != true)
                {
                    button.Visible = true;
                }
            }
        }
    }
}