using SMC_CLIENTE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC_CLIENTE.Forms
{
    public partial class MainForm : Form
    {
        private Panel panelContenido;
        private Label lblUsuarioActual;

        public MainForm()
        {
            ConfigurarInterfaz();  // Nuestro método de configuración
            this.WindowState = FormWindowState.Maximized;
            this.Text = $"Sistema Consultorio Médico - {AutenticacionService.UsuarioActual.NombreCompleto}";
        }

        private void ConfigurarInterfaz() // Renombrado desde InitializeComponent
        {
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema Consultorio Médico";

            // Panel superior con información del usuario
            var panelSuperior = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(0, 122, 204)
            };

            var lblTitulo = new Label
            {
                Text = "SISTEMA CONSULTORIO MÉDICO",
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            lblUsuarioActual = new Label
            {
                Text = $"Usuario: {AutenticacionService.UsuarioActual.NombreCompleto} | Rol: {AutenticacionService.UsuarioActual.Rol}",
                ForeColor = Color.White,
                Font = new Font("Arial", 10),
                Location = new Point(20, 35),
                AutoSize = true
            };

            var btnCerrarSesion = new Button
            {
                Text = "Cerrar Sesión",
                Size = new Size(120, 30),
                Location = new Point(1050, 15),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            btnCerrarSesion.Click += (sender, e) =>
            {
                if (MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Cerrar Sesión",
                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AutenticacionService.Logout();
                    this.Close();
                }
            };

            panelSuperior.Controls.Add(lblTitulo);
            panelSuperior.Controls.Add(lblUsuarioActual);
            panelSuperior.Controls.Add(btnCerrarSesion);

            // Panel lateral con menú
            var panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            CrearMenu(panelMenu);

            // Panel de contenido principal
            panelContenido = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Mostrar pantalla de bienvenida por defecto
            MostrarBienvenida();

            // Agregar controles al formulario
            this.Controls.Add(panelContenido);
            this.Controls.Add(panelMenu);
            this.Controls.Add(panelSuperior);
        }

        private void CrearMenu(Panel panelMenu)
        {
            var lblMenu = new Label
            {
                Text = "MENÚ PRINCIPAL",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, 20),
                AutoSize = true
            };

            panelMenu.Controls.Add(lblMenu);

            int yPos = 60;

            // Crear botones del menú según el rol del usuario
            if (AutenticacionService.UsuarioActual.Rol == "Recepcionista" ||
                AutenticacionService.UsuarioActual.Rol == "Administrador")
            {
                CrearBotonMenu(panelMenu, "👥 Gestión de Pacientes", yPos, () => AbrirGestionPacientes());
                yPos += 50;

                CrearBotonMenu(panelMenu, "📅 Gestión de Citas", yPos, () => AbrirGestionCitas());
                yPos += 50;

                CrearBotonMenu(panelMenu, "💰 Gestión de Pagos", yPos, () => AbrirGestionPagos());
                yPos += 50;
            }

            if (AutenticacionService.UsuarioActual.Rol == "Medico" ||
                AutenticacionService.UsuarioActual.Rol == "Administrador")
            {
                CrearBotonMenu(panelMenu, "🩺 Diagnósticos", yPos, () => AbrirDiagnosticos());
                yPos += 50;

                CrearBotonMenu(panelMenu, "📋 Historial Médico", yPos, () => AbrirHistorialMedico());
                yPos += 50;
            }

            if (AutenticacionService.UsuarioActual.Rol == "Administrador")
            {
                CrearBotonMenu(panelMenu, "📊 Reportes", yPos, () => AbrirReportes());
                yPos += 50;

                CrearBotonMenu(panelMenu, "⚙️ Configuración", yPos, () => AbrirConfiguracion());
                yPos += 50;
            }
        }

        private void CrearBotonMenu(Panel panel, string texto, int yPos, Action accion)
        {
            var btn = new Button
            {
                Text = texto,
                Size = new Size(200, 40),
                Location = new Point(25, yPos),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(0, 122, 204),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 122, 204);

            btn.MouseEnter += (s, e) => { btn.ForeColor = Color.White; };
            btn.MouseLeave += (s, e) => { btn.ForeColor = Color.FromArgb(0, 122, 204); };

            btn.Click += (s, e) => accion?.Invoke();

            panel.Controls.Add(btn);
        }

        private void MostrarBienvenida()
        {
            panelContenido.Controls.Clear();

            var lblBienvenida = new Label
            {
                Text = $"¡Bienvenido al Sistema de Consultorio Médico!",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(50, 50),
                AutoSize = true
            };

            var lblInstrucciones = new Label
            {
                Text = $"Hola {AutenticacionService.UsuarioActual.NombreCompleto},\n\n" +
                       "Utilice el menú lateral para navegar por las diferentes funciones del sistema.\n" +
                       "Su rol actual es: " + AutenticacionService.UsuarioActual.Rol,
                Font = new Font("Arial", 12),
                ForeColor = Color.Gray,
                Location = new Point(50, 100),
                Size = new Size(600, 200)
            };

            panelContenido.Controls.Add(lblBienvenida);
            panelContenido.Controls.Add(lblInstrucciones);
        }

        private void AbrirGestionPacientes()
        {
            panelContenido.Controls.Clear();
            var formPacientes = new PacientesForm { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelContenido.Controls.Add(formPacientes);
            formPacientes.Show();
        }

        private void AbrirGestionCitas()
        {
            MessageBox.Show("Funcionalidad de Gestión de Citas en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirGestionPagos()
        {
            MessageBox.Show("Funcionalidad de Gestión de Pagos en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirDiagnosticos()
        {
            MessageBox.Show("Funcionalidad de Diagnósticos en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirHistorialMedico()
        {
            MessageBox.Show("Funcionalidad de Historial Médico en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirReportes()
        {
            MessageBox.Show("Funcionalidad de Reportes en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirConfiguracion()
        {
            MessageBox.Show("Funcionalidad de Configuración en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}