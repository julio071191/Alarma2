using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace alarma2
{
    using System.Timers;
    public partial class Form1 : Form
    {
        private Timer minuteTimer;
 
        public List<List<string>> alarms = new List<List<string>>();
        public int counter = 0;
        public int currentAlarm = -1;
        public List<string> daysOfWeek = new List<string>() { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"};

        public Form1()
        {
            InitializeComponent();
            this.Text = "Alarma";

            day.Items.Add("Domingo");
            day.Items.Add("Lunes");
            day.Items.Add("Martes");
            day.Items.Add("Miercoles");
            day.Items.Add("Jueves");
            day.Items.Add("Viernes");
            day.Items.Add("Sabado");
            
            day.SelectedIndex = (int)DateTime.Now.DayOfWeek;
           
            for (int i = 1; i < 25; i++)
            {
                if (i < 10)
                {
                    hour.Items.Add("0"+i);
                }
                else
                {
                    hour.Items.Add(i);
                }
                
            }
            
            hour.SelectedItem = DateTime.Now.Hour;
            for (int i = 00; i < 60; i++)
            {
                if (i < 10)
                {
                    minute.Items.Add("0"+i);
                }
                else { minute.Items.Add(i); }
            }
            minute.SelectedItem = DateTime.Now.Minute;

            minuteTimer = new Timer(10000);
            minuteTimer.Elapsed += oneMinute;
            minuteTimer.AutoReset = true;
            minuteTimer.Start();
        }

        private void oneMinute(object sender, ElapsedEventArgs e)
        {
            updatePanel();
        }
        
        private void add_Click(object sender, EventArgs e)
        {
            if (currentAlarm == -1)
            {
                List<string> list = new List<string>();
                list.Add(name.Text);
                list.Add(day.Text);
                list.Add(hour.Text);
                list.Add(minute.Text);
                list.Add("0");
                list.Add("0");
                alarms.Add(list);
            }
            else
            {
                alarms[currentAlarm][0] = name.Text;
                alarms[currentAlarm][1] = day.Text;
                alarms[currentAlarm][2]=hour.Text;
                alarms[currentAlarm][3] = minute.Text;
                alarms[currentAlarm][4] = "0";
                alarms[currentAlarm][5] = "0";
            }
            updatePanel();

        }
        public void updatePanel()
        {
            
            if (panelAlarms.InvokeRequired)
            {
                panelAlarms.Invoke(new MethodInvoker(updatePanel));
            }
            else
            {
                panelAlarms.Controls.Clear();
            
            int i = 0;
                foreach (var alarmV in alarms)
                {
                    Label labelTemp = new Label();
                    labelTemp.Text = alarmV[0] + "     " + alarmV[1] + "     " + alarmV[2] + ":" + alarmV[3];
                    if (alarmV[4] == "1" && alarmV[5] == "1")
                    {
                        alarmV[4] = "0";
                        popUpText.Text = "Alarma " + alarmV[0] + " desactivada";
                        popUp.Visible = true;
                    }
                    if (alarmV[1] == daysOfWeek[(int)DateTime.Now.DayOfWeek] && int.Parse(alarmV[2]) == DateTime.Now.Hour && int.Parse(alarmV[3]) == DateTime.Now.Minute && alarmV[4] == "0" && alarmV[5] == "0")
                    {
                        alarmV[5] = "1";
                        alarmV[4] = "1";
                        popUpText.Text = "Alarma " + alarmV[0] + " activada";
                        popUp.Visible = true;
                        
                    }
                    if (alarmV[4] == "1") {
                        labelTemp.Text += "   Activada";
                    }
                    else if (alarmV[4] == "0" && alarmV[5] == "1")
                    {
                        labelTemp.Text += "   Desactivada";
                    }
                labelTemp.Location = new Point(10, i*30);
                labelTemp.Size = new Size(300, 25);
                if (i == currentAlarm)
                {
                    labelTemp.BackColor = Color.Gray;
                }
                else
                {
                    labelTemp.BackColor = Color.LightGray;
                }
                panelAlarms.Controls.Add(labelTemp);
                panelAlarms.AutoScroll = true;
                labelTemp.Tag = i;
                labelTemp.Click += new EventHandler(alarmClick);
                i += 1;
            }
            }
        }

        
        private void alarmClick(object sender, EventArgs e)
        {
            add.Text = "Editar";
            delete.Enabled = true;
            foreach (Control control in panelAlarms.Controls)
            {
                control.BackColor = Color.LightGray;
            }
            (sender as Label).BackColor = Color.Gray;
            int count = (int)(sender as Label).Tag;
            currentAlarm = count;
            name.Text = alarms[count][0];
            day.SelectedItem = alarms[count][1];
            hour.SelectedItem = alarms[count][2];
            minute.SelectedItem = alarms[count][3];

        }

        private void delete_Click(object sender, EventArgs e)
        {
            
            alarms.RemoveAt(currentAlarm);
            currentAlarm = -1;
            delete.Enabled = false;
            add.Text = "Agregar";
            updatePanel();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if(!panelAlarms.Bounds.Contains(e.Location))
            {
                delete.Enabled = false;
                currentAlarm = -1;
                delete.Enabled = false;
                add.Text = "Agregar";
                updatePanel();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            popUp.Visible = false;
            updatePanel();
        }
    }
}
