using SMC_CLIENTE.Models;
using SMC_CLIENTE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC_CLIENTE.Forms
{
    public partial class PacientesForm : Form
    {
        private DataGridView dgvPacientes;
        private TextBox txtBuscar;
        private Button btnNuevo, btnEditar, btnEliminar, btnBuscar;
        private Paciente pacienteSeleccionado;

        public PacientesForm()
        {
            ConfigurarInterfaz();
            CargarPacientes();
        }

        private void ConfigurarInterfaz() // Renombrado desde InitializeComponent
        {
            this.Size = new Size(1000, 600);
            this.Text = "Gestión de Pacientes";

            // Panel superior con controles
            var panelSuperior = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(10)
            };

            var lblTitulo = new Label
            {
                Text = "GESTIÓN DE PACIENTES",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(10, 10),
                AutoSize = true
            };

            // Controles de búsqueda
            var lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(10, 45),
                Size = new Size(50, 20)
            };

            txtBuscar = new TextBox
            {
                Location = new Point(70, 42),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10)
            };

            btnBuscar = new Button
            {
                Text = "🔍 Buscar",
                Location = new Point(280, 40),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9)
            };

            btnBuscar.Click += BtnBuscar_Click;
            txtBuscar.KeyPress += (s, e) => { if (e.KeyChar == (char)13) BtnBuscar_Click(s, e); };

            // Botones de acción
            btnNuevo = new Button
            {
                Text = "➕ Nuevo",
                Location = new Point(400, 40),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9)
            };

            btnEditar = new Button
            {
                Text = "✏️ Editar",
                Location = new Point(490, 40),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9),
                Enabled = false
            };

            btnEliminar = new Button
            {
                Text = "🗑️ Eliminar",
                Location = new Point(580, 40),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9),
                Enabled = false
            };

            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            panelSuperior.Controls.Add(lblTitulo);
            panelSuperior.Controls.Add(lblBuscar);
            panelSuperior.Controls.Add(txtBuscar);
            panelSuperior.Controls.Add(btnBuscar);
            panelSuperior.Controls.Add(btnNuevo);
            panelSuperior.Controls.Add(btnEditar);
            panelSuperior.Controls.Add(btnEliminar);

            // DataGridView para mostrar pacientes
            dgvPacientes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(0, 122, 204),
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold)
                }
            };

            // Configurar columnas
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdPaciente",
                HeaderText = "ID",
                Width = 50,
                Visible = false
            });

            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Nombre Completo",
                Width = 200
            });

            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cedula",
                HeaderText = "Cédula",
                Width = 120
            });

            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Telefono",
                HeaderText = "Teléfono",
                Width = 100
            });

            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                Width = 180
            });

            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Edad",
                HeaderText = "Edad",
                Width = 60
            });

            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Genero",
                HeaderText = "Género",
                Width = 80
            });

            dgvPacientes.SelectionChanged += DgvPacientes_SelectionChanged;
            dgvPacientes.DoubleClick += (s, e) => BtnEditar_Click(s, e);

            this.Controls.Add(dgvPacientes);
            this.Controls.Add(panelSuperior);
        }

        private void CargarPacientes()
        {
            try
            {
                var pacientes = PacienteService.ObtenerTodos();
                dgvPacientes.DataSource = pacientes;
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pacientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    CargarPacientes();
                }
                else
                {
                    var pacientes = PacienteService.Buscar(txtBuscar.Text.Trim());
                    dgvPacientes.DataSource = pacientes;
                }
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar pacientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            using (var form = new PacienteFormulario())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CargarPacientes();
                }
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (pacienteSeleccionado != null)
            {
                using (var form = new PacienteFormulario(pacienteSeleccionado))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        CargarPacientes();
                    }
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (pacienteSeleccionado != null)
            {
                if (MessageBox.Show($"¿Está seguro que desea eliminar al paciente {pacienteSeleccionado.NombreCompleto}?",
                                  "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Aquí implementarías la lógica de eliminación (soft delete)
                    MessageBox.Show("Funcionalidad de eliminación en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DgvPacientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPacientes.SelectedRows.Count > 0)
            {
                pacienteSeleccionado = (Paciente)dgvPacientes.SelectedRows[0].DataBoundItem;
            }
            else
            {
                pacienteSeleccionado = null;
            }
            ActualizarEstadoBotones();
        }

        private void ActualizarEstadoBotones()
        {
            btnEditar.Enabled = pacienteSeleccionado != null;
            btnEliminar.Enabled = pacienteSeleccionado != null;
        }
    }
}