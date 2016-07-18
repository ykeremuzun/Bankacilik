using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfPakBank
{
    public partial class frmHesapAcilisi : Form
    {
        public frmHesapAcilisi()
        {
            InitializeComponent();
        }
        Random rnd = new Random();

        private void frmHesapAcilisi_Load(object sender, EventArgs e)
        {
            this.Top = 50;
            this.Left = 15;
            lblTarih.Text = DateTime.Now.ToShortDateString();
            cbHesapTurleri.SelectedIndex = 0;
            SonHesapIDBul();
            HesapNumarasiOlustur();
        }
        private void SonHesapIDBul()
        {
            StreamWriter DosyaAc = new StreamWriter("HesapKartlari.txt", true);
            DosyaAc.Close();

            StreamReader DosyaOku = new StreamReader("HesapKartlari.txt");
            string okunan = DosyaOku.ReadLine();
            if (okunan == null)
                lblHesapID.Text = "1";
            else
            {
                while (okunan != null)
                {
                    string[] Degerler = okunan.Split(';');
                    lblHesapID.Text = (Convert.ToInt32(Degerler[0]) + 1).ToString();
                    okunan = DosyaOku.ReadLine();
                }
            } DosyaOku.Close();
        }
        private void HesapNumarasiOlustur()
        {
            bool Varmi = false;
            do
            {
                Varmi = HesapVarmi(); //HesapVarmi metodundan true döndüğü sürece tekrar hesapno ouşturulup baştamn kontrol edilmeye devam ediyor.
            } while (Varmi);
        }
        private bool HesapVarmi()
        {
            lblHesapNo.Text = "ACC" + rnd.Next(1000, 10000).ToString();
            StreamReader DosyaOku = new StreamReader("HesapKartlari.txt");
            string okunan = DosyaOku.ReadLine();
            while (okunan != null)
            {
                string[] Degerler = okunan.Split(';');
                if (lblHesapNo.Text == Degerler[1]) //Eğer yeni hesapnoya sahip önceden tanımlı bir kayıt bulursa, dosyayı kapatıp, yeniden hesapolustur metodundaki döngüye true değeriyle dönüyor.
                {
                    DosyaOku.Close();
                    return true;
                    //HesapNumarasiOlustur(); recursive methods(kendini çalıştıran metotlar)
                }
                okunan = DosyaOku.ReadLine();
            }
            DosyaOku.Close();
            return false;
        }

        private void btnHesapAc_Click(object sender, EventArgs e)
        {
            //FileStream fs = new FileStream("HesapKartlari.txt", FileMode.Append);
            //StreamWriter sw = new StreamWriter(fs);
            if (txtAdi.Text.Trim() != "" && txtSoyadi.Text.Trim() != "" && txtTCKNo.Text.Trim() != "" && txtBakiye.Text.Trim() != "")
            {
                StreamWriter DosyaAc = new StreamWriter("HesapKartlari.txt", true);
                DosyaAc.WriteLine(lblHesapID.Text + ";" + lblHesapNo.Text + ";" + lblTarih.Text + ";" + txtAdi.Text + ";" + txtSoyadi.Text + ";" + txtTCKNo.Text + ";" + txtBakiye.Text + ";" + cbHesapTurleri.SelectedItem.ToString());
                DosyaAc.Close();

                StreamWriter HareketYaz = new StreamWriter("HesapHareketleri.txt", true);
                HareketYaz.WriteLine(lblHesapID.Text + ";" + lblHesapNo.Text + ";" + lblTarih.Text + ";" + txtBakiye.Text + ";" + "yatan");
                HareketYaz.Close();

                MessageBox.Show("Yeni Hesap Bilgileri oluşturuldu.");
                Temizle();
                SonHesapIDBul();
                HesapNumarasiOlustur();
            }
            else { MessageBox.Show("Girilmesi zorunlu alanları boş bırakmayınız!", "DİKKAT! Eksik Bilgi!"); }
        }
        private void Temizle()
        {
            txtAdi.Clear();
            txtSoyadi.Clear();
            txtTCKNo.Clear();
            txtBakiye.Clear();
            txtAdi.Focus();
        }
    }
}
