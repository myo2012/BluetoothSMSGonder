using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace BluetoothSMSGonder
{
    public partial class Form1 : Form
    {

    
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                if (sp.IsOpen)
                {//Serial port açıksa aşağıdaki mesajı verdiriyoruz.
                    MessageBox.Show("Şu an bir bağlantı noktasına zaten bağlı");
                    label5.Text = sp.PortName + "  Baglandı";
                    button1.Enabled = false;
                    button3.Enabled = true;
                }

                else
                {

                    sp.PortName = textBox1.Text;
                    sp.Open();
                    //Serial portun port numarası olarak TextBox1 e girilen değeri atıyor ve bağlantı noktasını açıyoruz.
                    if (sp.IsOpen)
                    {//Bağlantı kurulunca forma aşağıdaki değişikleri yaptım.
                        MessageBox.Show("Bağlantı Kuruldu");
                        label5.Text = sp.PortName + "  Baglandı";
                        
                        button2.Enabled = true;
                        button1.Enabled = false;
                        button3.Enabled = true;

                    }
                    else
                    {
                        //Bağlantı kurulamadıysa aşağıdaki değişiklikleri yaptım.

                        MessageBox.Show("Bağlantı Kurulamadı");
                        label5.Text = "Bağlı Değil";
                        button2.Enabled = false;
                        button1.Enabled = true;
                        button3.Enabled = false;
                        textBox1.Text = "";

                    }


                }
            }
            catch(Exception hata)
            {

                //Bağlantı kurulamadıysa , COM portu bağlantı noktası bulunamadıysa oluacak her türlü hatayı try-catch ile yakalatak
                //Oluan hata mesajını verdiriyorum.
                MessageBox.Show(hata.Message);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sp.Close();
            button1.Enabled = true;
            button3.Enabled = false;
            MessageBox.Show("Bağlantı Kesildi.");
            label5.Text = "Bağlı Değil";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sp.WriteLine("AT" + "\r");
            //Telefona AT komutları kullanacağımzı bildiriyoruz.
            System.Threading.Thread.Sleep(250);
            sp.WriteLine("AT+CMGF=1" + "\r");
            //Yeni Mesaj göndermek istediğimizi bildiriyoruz.
            System.Threading.Thread.Sleep(250);
            sp.WriteLine("AT+CMGS=\"" + numara.Text + "\"" + "\r\n");
            //Mesajın hangi numaraya gideceğini bildiriyoruz.
            System.Threading.Thread.Sleep(250);
            sp.WriteLine(turkceKarakterTemizle(mesaj.Text) + Char.ConvertFromUtf32(26) + "\r");
            //Türkçe karakter desteği olmadğı için Türkçe karakterleri temizliyoruz.
            //Mesajın bittiğine dair komutu CTRL+Z komutunun ASCII kod karşılığı ile bildiriyoru.
            System.Threading.Thread.Sleep(3000);
            sp.WriteLine("AT+CMSS=1" + "\r");
            //Mesajı gönder diyoruz.
            MessageBox.Show("Mesaj Gönderildi");
            temizle();
        }

        private void temizle()
        {
            numara.Text = "+90";
            mesaj.Text = "";
        }

        private void mesaj_TextChanged(object sender, EventArgs e)
        {
            //Mesajın kaç karakterden oluştuğunu yazdırıyorum.
            label6.Text = mesaj.Text.Length.ToString();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private string turkceKarakterTemizle(string mesaj)
        {
            //Mesaj olarak girilen içeriğin Türkçe karakterlerini temizliyorum.
            string geriDonecekDeger = mesaj.ToUpper().Replace('Ç','C').Replace('Ğ','G').Replace('İ','I').Replace ('Ö','O').Replace('Ş','S').Replace ('Ü','U');
            return geriDonecekDeger;

        
        }


    }
}