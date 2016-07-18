using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfPakBank
{
    public partial class frmHesapDokumu : Form
    {
        public frmHesapDokumu()
        {
            InitializeComponent();
        }
        Font fntBaslik = new Font("Times New Roman", 16, FontStyle.Bold);
        Font fntDetay = new Font("Times New Roman", 12, FontStyle.Regular);
        SolidBrush sb = new SolidBrush(Color.Black);
        public static string HesapID; //static olarak tanımlanan değişkenler, class'ın new ile örneği (instance) oluşturulmadan çağrılabilir.
        public static string HesapNo;

        private void frmHesapDokumu_Load(object sender, EventArgs e)
        {
            this.Top = 50;
            this.Left = 15;
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            if (txtHesapNo.Text.Trim() != "")
            {
                HesapBilgileriGoster();
                HesapHareketleriGoster();
                ToplamlariGoster();
            }
        }
        private void HesapBilgileriGoster()
        {
            StreamReader DosyaOku = new StreamReader("HesapKartlari.txt");
            string okunan = DosyaOku.ReadLine();
            while (okunan != null)
            {
                string[] Degerler = okunan.Split(';');
                if (txtHesapNo.Text == Degerler[1]) 
                {
                    txtAdi.Text = Degerler[3];
                    txtSoyadi.Text = Degerler[4];
                    txtTCKNo.Text = Degerler[5];
                    txtHesapTuru.Text = Degerler[7];
                    txtTarih.Text = Degerler[2];
                    break;
                }
                okunan = DosyaOku.ReadLine();
            }
            DosyaOku.Close();
        }
        private void HesapHareketleriGoster()
        {
            lvHareketler.Items.Clear();
            StreamReader DosyaOku = new StreamReader("HesapHareketleri.txt");
            string okunan = DosyaOku.ReadLine();
            int i = 0;
            while (okunan != null)
            {
                string[] Degerler = okunan.Split(';');
                if (txtHesapNo.Text == Degerler[1])
                {
                    lvHareketler.Items.Add(Degerler[0]);
                    lvHareketler.Items[i].SubItems.Add(Degerler[1]);
                    lvHareketler.Items[i].SubItems.Add(Degerler[2]);
                    lvHareketler.Items[i].SubItems.Add(Degerler[3]);
                    lvHareketler.Items[i].SubItems.Add(Degerler[4]);
                    i++;
                }
                okunan = DosyaOku.ReadLine();
            }
            DosyaOku.Close();
        }
        private void ToplamlariGoster()
        {
            double TopYatan = 0;
            double TopCekilen = 0;
            for (int i = 0; i < lvHareketler.Items.Count; i++)
            {
                if (lvHareketler.Items[i].SubItems[4].Text == "yatan")
                    TopYatan += Convert.ToDouble(lvHareketler.Items[i].SubItems[3].Text);            
                else
                    TopCekilen += Convert.ToDouble(lvHareketler.Items[i].SubItems[3].Text);
            }
            txtToplamYatan.Text = TopYatan.ToString();
            txtToplamCekilen.Text = TopCekilen.ToString();
            txtBakiye.Text = (TopYatan - TopCekilen).ToString();
        }

        private void btnParaIslemleri_Click(object sender, EventArgs e)
        {
            //HesapID = lvHareketler.Items[0].SubItems[0].Text;
            //HesapNo = txtHesapNo.Text;
            frmParaIslemleri frm = new frmParaIslemleri();
            //ParaIslemleri ekranındaki HesapID ve HesapNo labellarının modifiers özelliklerini Public yaptık.
            //frm.lblHesapID.Text = lvHareketler.Items[0].SubItems[0].Text;
            //frm.lblHesapNo.Text = lvHareketler.Items[0].SubItems[1].Text;
            frm.HesapBilgileriAl(lvHareketler.Items[0].SubItems[0].Text, txtHesapNo.Text);
            frm.ShowDialog();
            //ParaIslemleri formu kapatıldığında programın işleyişi burdan devam eder.
            HesapHareketleriGoster();
            ToplamlariGoster();
        }
        int k = 0;
        private void pdocHesap_PrintPage(object sender, PrintPageEventArgs e)
        {
            StringFormat fmt = new StringFormat();
            fmt.Alignment = StringAlignment.Near;
            e.Graphics.DrawString("Müşteri : " + txtAdi.Text + " " + txtSoyadi.Text, fntBaslik, sb, 100, 100, fmt);
            e.Graphics.DrawString("HesapNo : " + txtHesapNo.Text, fntBaslik, sb, 100, 130, fmt);
            e.Graphics.DrawString(DateTime.Now.ToShortDateString(), fntBaslik, sb, 650, 110, fmt);
            e.Graphics.DrawString("HESAP HAREKETLERİ", fntBaslik, sb, 300, 200, fmt);
            e.Graphics.DrawString("  ID    HesapNo       İşlemTarihi        İşlemTutarı       İşlemTipi", fntBaslik, sb, 100, 250, fmt);
            e.Graphics.DrawString("________________________________________________________", fntBaslik, sb, 100, 270);
            int j = 0;
            for (int i = k; i < lvHareketler.Items.Count; i++)
            {
                e.Graphics.DrawString(lvHareketler.Items[i].SubItems[0].Text, fntDetay, sb, 114, 300 + j * 30, fmt);
                e.Graphics.DrawString(lvHareketler.Items[i].SubItems[1].Text, fntDetay, sb, 175, 300 + j * 30, fmt);
                e.Graphics.DrawString(lvHareketler.Items[i].SubItems[2].Text, fntDetay, sb, 290, 300 + j * 30, fmt);
                fmt.Alignment = StringAlignment.Far;
                e.Graphics.DrawString(lvHareketler.Items[i].SubItems[3].Text, fntDetay, sb, 520, 300 + j * 30, fmt);
                fmt.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(lvHareketler.Items[i].SubItems[4].Text, fntDetay, sb, 600, 300 + j * 30, fmt);

                if(i % 22 == 0 && i != 0)
                {
                    e.HasMorePages = true;
                    k++;
                    return;
                }else
                {
                    e.HasMorePages = false;
                    j++;
                    k++;
                }
            }
            e.Graphics.DrawString("________________________________________________________", fntBaslik, sb, 100, 300 + j * 30);
            j++;
            e.Graphics.DrawString("Toplam Yatan   ", fntBaslik, sb, 290, 300 + j * 30, fmt);
            fmt.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(txtToplamYatan.Text, fntBaslik, sb, 520, 300 + j * 30, fmt);
            fmt.Alignment = StringAlignment.Near;
            j++;
            e.Graphics.DrawString("Toplam Çekilen ", fntBaslik, sb, 290, 300 + j * 30, fmt);
            fmt.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(txtToplamCekilen.Text, fntBaslik, sb, 520, 300 + j * 30, fmt);
            fmt.Alignment = StringAlignment.Near;
            j++;
            e.Graphics.DrawString("Bakiye", fntBaslik, sb, 290, 300 + j * 30, fmt);
            fmt.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(txtBakiye.Text, fntBaslik, sb, 520, 300 + j * 30, fmt);
            k = 0;
        }

        private void btnYazici_Click(object sender, EventArgs e)
        {
            ppdHesap.ShowDialog();
        }
    }
}
