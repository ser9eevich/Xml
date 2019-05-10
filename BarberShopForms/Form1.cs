﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Barbershop;
using System.IO;
using System.Xml.Serialization;

namespace BarberShopForms
{
    public partial class MainForm : Form
    {
        public List<Client> Clients;
        public List<Workers> Workers;
        public MainForm()
        {
            InitializeComponent();
            Clients = new List<Client>();
            Workers = new List<Workers>();
            FillInWorkersBase();
            WorkersListBox.DataSource = Workers;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var form = new AddClient(Clients, ClientListBox);
            form.Show();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (ClientListBox.SelectedItem is Client)
            {
                var client = (Client)ClientListBox.SelectedItem;
                var form = new EditClient(client, Clients, ClientListBox);
                form.Show();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (ClientListBox.SelectedItem is Client)
            {
                var client = (Client)ClientListBox.SelectedItem;
                Clients.Remove(client);
                ClientListBox.DataSource = null;
                ClientListBox.DataSource = Clients;
            }
        }

        private void ClientListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = ClientListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var client = (Client)ClientListBox.Items[index];
                var form = new AppointmentForm(client, Workers);
                form.Show();
            }
        }

        private void InformationButton_Click(object sender, EventArgs e)
        {
            if (ClientListBox.SelectedItem is Client)
            {
                var client = (Client)ClientListBox.SelectedItem;
                var form = new Info(client);
                form.Show();
            }
        }

        private void FillInWorkersBase()
        {
            Workers.Add(new Workers()
            {
                WorkerName = "Виталик",
                IndividualNumber = 228,
                Position = "Про-барбер",
                StartDate = new DateTime(2010, 12, 5)
            });

            Workers.Add(new Workers()
            {
                WorkerName = "Эдуард",
                IndividualNumber = 212,
                Position = "Нью-барбер",
                StartDate = new DateTime(2018, 5, 24)
            });

            Workers.Add(new Workers()
            {
                WorkerName = "Валерий",
                IndividualNumber = 47,
                Position = "Мастер бороды",
                StartDate = new DateTime(2015, 9, 15)
            });

            Workers.Add(new Workers()
            {
                WorkerName = "Степан",
                IndividualNumber = 007,
                Position = "Барбер-стажёр",
                StartDate = new DateTime(2019, 3, 7)
            });
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog() { Filter = "Клиенты|*.clients" };

            if (sfd.ShowDialog(this) != DialogResult.OK)
                return;

            var clients = new List<Client>();
            clients = Clients;

            var stream = new MemoryStream();
            var xs = new XmlSerializer(typeof(List<Client>));
            var file = File.Create(sfd.FileName);

            xs.Serialize(file, clients);
            file.Close();
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() { Filter = "Клиенты|*.clients" };

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            var xs = new XmlSerializer(typeof(List<Client>));
            var file = File.OpenRead(ofd.FileName);
            var clients = (List<Client>)xs.Deserialize(file);
            file.Close();

            Clients = clients;

            ClientListBox.DataSource = null;
            ClientListBox.DataSource = clients;
        }
    }
}
