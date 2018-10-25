﻿using Frei.ProjetoIntegrador.Academia.DB.Clientes;
using Frei.ProjetoIntegrador.Academia.DB.PedidoItemVenda;
using Frei.ProjetoIntegrador.Academia.DB.PedidoVenda;
using Frei.ProjetoIntegrador.Academia.DB.ProdutoVenda;
using Frei.ProjetoIntegrador.Academia.DB.Usuario;
using Nsf._2018.Modulo3.App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frei.ProjetoIntegrador.Academia.Modulos.ControleDeVenda
{
    public partial class frmNovo : Form
    {
        public frmNovo()
        {
            InitializeComponent();
            CarregarProdutos();
            CarregarCampos();
            CarregarGrid();
        }

        BindingList<PedidoItemVendaDTO> produtosCarrinho = new BindingList<PedidoItemVendaDTO>();

        private void CarregarGrid()
        {
            dgvItens.AutoGenerateColumns = false;
            dgvItens.DataSource = produtosCarrinho;
        }

        private void CarregarCampos()
        {
            UsuarioDTO user = UserSession.UsuarioLogado;
            txtUsuario.Text = user.nm_Usuario;

            ClientesBusiness business = new ClientesBusiness();
            List<ClientesDTO> clientes = business.Consultar();

            cboClientes.DisplayMember = nameof(ClientesDTO.nm_Nome);
            cboClientes.ValueMember = nameof(ClientesDTO.id_Cliente);
            cboClientes.DataSource = clientes;
        }

        private int ValidarUsuario(string nome, string senha)
        {
            LoginBusiness business = new LoginBusiness();
            UsuarioDTO user = business.Autenticar(nome, senha);

            return user.id_Usuario;
        }

        private void CarregarProdutos()
        {
            ProdutoVendaBusiness business = new ProdutoVendaBusiness();
            List<ProdutoVendaDTO> produtos = business.Consultar();

            cboProdutos.ValueMember = nameof(ProdutoVendaDTO.id_produtoVenda);
            cboProdutos.DisplayMember = nameof(ProdutoVendaDTO.nm_Produto);
            cboProdutos.DataSource = produtos;
        }

        private void cboProdutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProdutoVendaBusiness business = new ProdutoVendaBusiness();
            ProdutoVendaDTO produto = business.ConsultarPorId(Convert.ToInt32(cboProdutos.SelectedValue));
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            ProdutoVendaDTO dto = cboProdutos.SelectedItem as ProdutoVendaDTO;
            PedidoItemVendaDTO item = new PedidoItemVendaDTO();

            item.qnt_Produto = Convert.ToInt32(txtQuantidade.Text);
            item.fk_PedidoItemVenda_ProdutoVenda = dto.id_produtoVenda;
            item.nm_Produto = dto.nm_Produto;

            produtosCarrinho.Add(item);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                int id_User = ValidarUsuario(txtUsuario.Text, txtConfSenha.Text);

                if (id_User == 0)
                    throw new ArgumentException("Usuário não autenticado!");

                PedidoVendaDTO pedido = new PedidoVendaDTO();
                pedido.dt_Pedido = DateTime.Now;
                pedido.fk_PedidoVenda_Filial = Program.id_Filial;
                pedido.fk_PedidoVenda_Usuario = id_User;
                pedido.fk_PedidoVenda_Cliente = Convert.ToInt32(cboClientes.SelectedValue);

                PedidoItemVendaBusiness business = new PedidoItemVendaBusiness();
                business.AdicionarCarrinho(pedido, produtosCarrinho);

                MessageBox.Show("Compra efetuada com sucesso!", "Black Fit LTDA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Black Fit LTDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocorreu um erro não identificado.", "Black Fit LTDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            produtosCarrinho.RemoveAt(0);
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            while (produtosCarrinho.Count > 0)
                produtosCarrinho.RemoveAt(0);
        }
    }
}
