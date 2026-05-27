using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcGestionUsuarios : UserControl
    {
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private IList<Usuario> _usuarios;

        public UcGestionUsuarios()
        {
            InitializeComponent();
            // Problema visual que resuelve: filtros y grilla de usuarios usan el mismo lenguaje visual del admin.
            AppTheme.ApplyView(this);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplyCard(pnlFiltros, 8);
            AppTheme.ApplyTextBox(txtBuscar);
            AppTheme.ApplyCombo(cboEstado);
            AppTheme.ApplyPrimaryButton(btnCambiarEstado, AppTheme.Azul);
            AppTheme.ApplyDataGrid(dgvUsuarios);
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                _usuarios = _usuarioSvc.ObtenerTodos();
                FiltrarYMostrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarYMostrar()
        {
            string busqueda = txtBuscar.Text.Trim().ToLower();
            string estadoFiltro = cboEstado.SelectedItem?.ToString();

            var filtrados = _usuarios.AsEnumerable();

            if (!string.IsNullOrEmpty(busqueda))
                filtrados = filtrados.Where(u =>
                    (u.Nombre1 ?? "").ToLower().Contains(busqueda) ||
                    (u.Nombre2 ?? "").ToLower().Contains(busqueda) ||
                    (u.Apellido1 ?? "").ToLower().Contains(busqueda) ||
                    (u.Username ?? "").ToLower().Contains(busqueda));

            if (!string.IsNullOrEmpty(estadoFiltro) && estadoFiltro != "Todos")
                filtrados = filtrados.Where(u =>
                    (u.Estado ?? "").Equals(estadoFiltro, StringComparison.OrdinalIgnoreCase));

            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = filtrados.ToList();

            if (dgvUsuarios.Columns.Count > 0)
            {
                if (dgvUsuarios.Columns.Contains("IdUsuario"))
                    dgvUsuarios.Columns["IdUsuario"].HeaderText = "ID";
                if (dgvUsuarios.Columns.Contains("Username"))
                    dgvUsuarios.Columns["Username"].HeaderText = "Username";
                if (dgvUsuarios.Columns.Contains("Nombre1"))
                    dgvUsuarios.Columns["Nombre1"].HeaderText = "Nombre";
                if (dgvUsuarios.Columns.Contains("Correo"))
                    dgvUsuarios.Columns["Correo"].HeaderText = "Correo";
                if (dgvUsuarios.Columns.Contains("Saldo"))
                {
                    dgvUsuarios.Columns["Saldo"].HeaderText = "Saldo";
                    dgvUsuarios.Columns["Saldo"].DefaultCellStyle.Format = "C2";
                }
                if (dgvUsuarios.Columns.Contains("Estado"))
                    dgvUsuarios.Columns["Estado"].HeaderText = "Estado";

                string[] ocultar = { "Password", "Nombre2", "Apellido1", "Apellido2", "FechaNacimiento", "FechaRegistro", "IdRol" };
                foreach (string col in ocultar)
                    if (dgvUsuarios.Columns.Contains(col))
                        dgvUsuarios.Columns[col].Visible = false;
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            FiltrarYMostrar();
        }

        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarYMostrar();
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Usuario u = dgvUsuarios.Rows[e.RowIndex].DataBoundItem as Usuario;
            if (u == null) return;

            if (u.IdRol == 1)
            {
                MessageBox.Show("No se puede editar un administrador desde esta pantalla.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (FrmEditarUsuario form = new FrmEditarUsuario(u))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    CargarUsuarios();
            }
        }

        private void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario u = dgvUsuarios.CurrentRow.DataBoundItem as Usuario;
            if (u == null) return;

            if (u.IdRol == 1)
            {
                MessageBox.Show("No se puede modificar el estado de un administrador.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstado = u.Estado == "activo" ? "suspendido" : "activo";
            string resultado = _usuarioSvc.CambiarEstado(u.IdUsuario, nuevoEstado);

            bool ok = resultado == "Guardado correctamente.";
            MessageBox.Show(ok ? $"Estado cambiado a '{nuevoEstado}'." : resultado,
                ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok) CargarUsuarios();
        }
    }
}
