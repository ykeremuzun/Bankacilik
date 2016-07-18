using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfPakBank_OOP
{
    public partial class frmParaIslemleri : Form
    {
        public frmParaIslemleri()
        {
            InitializeComponent();
        }

        private void frmParaIslemleri_Load(object sender, EventArgs e)
        {
            lblTarih.Text = DateTime.Now.ToShortDateString();
            cbIslemTipleri.SelectedIndex = 1; //cekilen işlem tipini seçili hale getiriyoruz.
            lblHesapID.Text = frmHesapDokumu.HesapID;
            lblHesapNo.Text = frmHesapDokumu.HesapNo;
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
                    cHesapHareket hh = new cHesapHareket();
                    hh.ID = Convert.ToInt32(lblHesapID.Text);
                    hh.HesapNo = lblHesapNo.Text;
                    hh.Tarih = lblTarih.Text;
                    hh.Tutar = Convert.ToDouble(txtTutar.Text);
                    hh.IslemTipi = cbIslemTipleri.SelectedItem.ToString();
                    if (hh.HesapHareketEkle(hh))
                    {
                        MessageBox.Show("Yeni Hesap Bilgileri oluşturuldu.");
                        this.Close();
                    }
        }
    }
}
