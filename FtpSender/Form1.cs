using MediaInfoNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace FtpSender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (FormatChecker(openFileDialog1.FileName))
            {

                //if Ok Add to list:
                dataGridView1.Rows.Add(openFileDialog1.FileName, "Waiting");
            }



        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            }
        }

        protected bool FormatChecker(string VidFile)
        {
            bool Vlu = true;

            MediaFile VideoFile = new MediaFile(VidFile);
            List<Presets> PrstList = new List<Presets>();
            PrstList = LoadSets();
            dataGridView2.ClearSelection();
            int Rw = 0;
            foreach (Presets item in PrstList)
            {
                List<bool> BoolLst = new List<bool>();
                DataGridViewRow Rr = new DataGridViewRow();
                Rr = dataGridView2.Rows[Rw];
              
                #region VIDEO
                if (VideoFile.Video.Count > 0)
                {
                    if (VideoFile.Video[0].Format == item.vformat)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[0].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[0].Style.BackColor = Color.Red;
                    }



                    if (VideoFile.Video[0].FormatID == item.vformatid)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[1].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[1].Style.BackColor = Color.Red;
                    }


                    if (VideoFile.Video[0].Height == item.height)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[2].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[2].Style.BackColor = Color.Red;
                    }

                    if (VideoFile.Video[0].Width == item.width)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[3].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[3].Style.BackColor = Color.Red;
                    }



                    if (VideoFile.Video[0].Bitrate >= item.vbitratemin)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[4].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[4].Style.BackColor = Color.Red;
                    }


                    if (VideoFile.Video[0].Bitrate <= item.vbitratemax)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[5].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[5].Style.BackColor = Color.Red;
                    }


                    if (VideoFile.Video[0].FrameRate == item.vframerate)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[6].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[6].Style.BackColor = Color.Red;
                    }


                } 
                #endregion

                #region AUDIO
                if (VideoFile.Audio.Count > 0)
                {
                    if (VideoFile.Audio[0].Format == item.aformat)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[7].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[7].Style.BackColor = Color.Red;
                    }


                    if (VideoFile.Audio[0].Bitrate >= item.abitratemin)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[8].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[8].Style.BackColor = Color.Red;
                    }


                    if (VideoFile.Audio[0].Bitrate <= item.abitratemax)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[9].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[9].Style.BackColor = Color.Red;
                    }


                    if (VideoFile.Audio[0].SamplingRate == item.asamplerate)
                    {
                        BoolLst.Add(true);
                        Rr.Cells[10].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        BoolLst.Add(false);
                        Rr.Cells[10].Style.BackColor = Color.Red;
                    }

                } 
                #endregion

               foreach (bool Bitem in BoolLst)
               {
                   if(!Bitem)
                   {
                       Vlu = false;
                   }
               }

                if(Vlu)
                {  
                    return Vlu;
                }
                else
                {
                    if(BtnPresets.Text==">>")
                    {
                        BtnPresets_Click(new object(), new EventArgs());
                    }
                }
               
                Rw++;
            }
            return Vlu;
        }
        protected List<Presets> LoadSets()
        {
            List<Presets> PrstList = new List<Presets>();
            dataGridView2.Rows.Clear();
            XmlDocument XDoc = new XmlDocument();
            string XmlPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Presets.xml";
            if (System.IO.File.Exists(XmlPath))
            {
                XDoc.Load(XmlPath);
                XmlNodeList PrXmlLst = XDoc.GetElementsByTagName("Preset");
                foreach (XmlNode Nd in PrXmlLst)
                {
                    Presets Prst = new Presets();
                    Prst.abitratemax = double.Parse(Nd.Attributes["abitratemax"].Value.ToString());
                    Prst.abitratemin = double.Parse(Nd.Attributes["abitratemin"].Value.ToString());
                    Prst.aformat = Nd.Attributes["aformat"].Value.ToString();
                    Prst.asamplerate = double.Parse(Nd.Attributes["asamplerate"].Value.ToString());
                    Prst.height = double.Parse(Nd.Attributes["height"].Value.ToString());
                    Prst.vbitratemax = double.Parse(Nd.Attributes["vbitratemax"].Value.ToString());
                    Prst.vbitratemin = double.Parse(Nd.Attributes["vbitratemin"].Value.ToString());
                    Prst.vformat = Nd.Attributes["vformat"].Value.ToString();
                    Prst.vformatid = Nd.Attributes["vformatid"].Value.ToString();
                    Prst.vframerate = double.Parse(Nd.Attributes["vframerate"].Value.ToString());
                    Prst.width = double.Parse(Nd.Attributes["width"].Value.ToString());
                    PrstList.Add(Prst);

                    dataGridView2.Rows.Add(Prst.vformat,
                        Prst.vformatid,
                         Prst.height,
                          Prst.width,
                           Prst.vbitratemin,
                            Prst.vbitratemax,
                             Prst.vframerate,
                              Prst.aformat,
                               Prst.abitratemin,
                                Prst.abitratemax,
                                 Prst.asamplerate

                        );
                }
            }

            return PrstList;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "STARTED";

            int Indx = ListFinder();
            if (Indx >= 0)
            {
                CheckDir();
                Upload(Indx);
            }

            button3.Text = "START";
        }
        protected int ListFinder()
        {
            int RetIndx = -1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value.ToString() == "Waiting")
                {
                    RetIndx = i;
                    break;
                }
            }
            return RetIndx;
        }
        protected bool CheckDir()
        {
            try
            {
                string Dir = DateConversion.GD2JD(DateTime.Now).ToString().Replace("/", "-");
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(System.Configuration.ConfigurationSettings.AppSettings["Server"].Trim()
                   + "/" + Dir);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(System.Configuration.ConfigurationSettings.AppSettings["UserName"].Trim(),
                    System.Configuration.ConfigurationSettings.AppSettings["PassWord"].Trim());
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    if (resp.StatusCode == FtpStatusCode.PathnameCreated)
                    {
                        // MessageBox.Show("Directoy " + textBox2.Text + " created.");
                        //  Console.WriteLine(resp.StatusCode);
                        resp.Close();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }


        }
        protected void Upload(int Index)
        {
            try
            {
                dataGridView1.Rows[Index].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = Index;
                dataGridView1.Rows[Index].Cells[1].Value = "Progress";

                string Dir = DateConversion.GD2JD(DateTime.Now).ToString().Replace("/", "-");
                var ftpWebRequest = (FtpWebRequest)WebRequest.Create(System.Configuration.ConfigurationSettings.AppSettings["Server"].Trim() +
              "/" + Dir + "/" + System.IO.Path.GetFileName(dataGridView1.Rows[Index].Cells[0].Value.ToString().Trim()));

                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpWebRequest.Credentials = new NetworkCredential(System.Configuration.ConfigurationSettings.AppSettings["UserName"].Trim(),
                    System.Configuration.ConfigurationSettings.AppSettings["PassWord"].Trim());
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.KeepAlive = false;
                //  ftpWebRequest. = 10000;
                using (var inputStream = System.IO.File.OpenRead(dataGridView1.Rows[Index].Cells[0].Value.ToString().Trim()))
                using (var outputStream = ftpWebRequest.GetRequestStream())
                {
                    var buffer = new byte[32 * 1024];
                    int totalReadBytesCount = 0;
                    int readBytesCount;

                    long length = inputStream.Length;
                    long bfr = 0;
                    progressBar1.Maximum = 100;
                    richTextBox1.Text += "Start Upload : " + ftpWebRequest.RequestUri.ToString() + "\n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    Application.DoEvents();
                    while ((readBytesCount = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bfr += readBytesCount;
                        int Pr = (int)Math.Ceiling(double.Parse(((bfr * 100) / length).ToString()));
                        progressBar1.Value = Pr;
                        outputStream.Write(buffer, 0, readBytesCount);
                        label1.Text = Pr.ToString() + "%";
                        Application.DoEvents();
                    }
                    richTextBox1.Text += "End Upload : " + ftpWebRequest.RequestUri.ToString() + "\n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    Application.DoEvents();
                }

                dataGridView1.Rows[Index].Cells[1].Value = "Done";
                button3_Click(new object(), new EventArgs());
                try
                {
                    //System.IO.File.Delete(_TempDir + FileName);
                    //richTextBox2.Text += "Delete Local Image: " + _TempDir + FileName + "\n";
                    //richTextBox2.SelectionStart = richTextBox2.Text.Length;
                    //richTextBox2.ScrollToCaret();
                    //Application.DoEvents();
                }
                catch
                {

                }

            }
            catch (Exception Exp)
            {
                dataGridView1.Rows[Index].Cells[1].Value = "Error";
                Application.DoEvents();
                richTextBox1.Text += "Error Upload To Ftp\n";
                richTextBox1.Text += Exp.Message + "\n";
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
                Application.DoEvents();
                Thread.Sleep(5000);
                Upload(Index);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            BtnPresets.Text = ">>";
            this.Width=628;
            LoadSets();
          
        }
        private void BtnPresets_Click(object sender, EventArgs e)
        {
            if (BtnPresets.Text == "<<")
            {
                BtnPresets.Text = ">>";
                this.Width = 632;
            }
            else
            {
                BtnPresets.Text = "<<";
                this.Width = 1339;
            }
        }      
    }
}
