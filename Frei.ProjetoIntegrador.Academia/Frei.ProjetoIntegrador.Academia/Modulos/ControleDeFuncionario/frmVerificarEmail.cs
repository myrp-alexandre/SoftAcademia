﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frei.ProjetoIntegrador.Academia.Modulos.ControleDeFuncionario
{
    public partial class frmVerificarEmail : Form
    {
        public frmVerificarEmail()
        {
            InitializeComponent();
            Verificado = false;
        }

        int codVerificacao = 0;
        public bool Verificado { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            Verificado = false;
            Close();
        }

        public void Codigo(int cod)
        {
            codVerificacao = cod;
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            if (txtCod.Text != codVerificacao.ToString())
                    throw new ArgumentException("O código de segurança está incorreto!");
            else
            {
                Verificado = true;
                Close();
            }
        }
    }
}
