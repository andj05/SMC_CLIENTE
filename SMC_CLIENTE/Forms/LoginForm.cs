using SMC_CLIENTE.Services;

namespace SMC_CLIENTE
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Button btnIngresar;
        private Button btnSalir;
        private Panel panelPrincipal;
        private Panel panelForm;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblUsuario;
        private Label lblPassword;

        public LoginForm()
        {
            InitializeComponent();
            ConfigurarFormulario();
            CrearControles();
            ConfigurarEventos();
        }

        private void ConfigurarFormulario()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void CrearControles()
        {
            // Panel principal
            panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = System.Drawing.Color.White
            };

            // Título
            lblTitulo = new Label
            {
                Text = "CONSULTORIO MÉDICO",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(0, 122, 204),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };

            // Subtítulo
            lblSubtitulo = new Label
            {
                Text = "Sistema de Gestión",
                Font = new System.Drawing.Font("Arial", 10),
                ForeColor = System.Drawing.Color.Gray,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 25
            };

            // Panel de formulario
            panelForm = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 20, 0, 0)
            };

            // Usuario
            lblUsuario = new Label
            {
                Text = "Usuario:",
                Location = new System.Drawing.Point(0, 20),
                Size = new System.Drawing.Size(100, 20)
            };

            txtUsuario = new TextBox
            {
                Name = "txtUsuario",
                Location = new System.Drawing.Point(0, 45),
                Size = new System.Drawing.Size(300, 25),
                Font = new System.Drawing.Font("Arial", 10),
                Text = "admin" // Para pruebas
            };

            // Contraseña
            lblPassword = new Label
            {
                Text = "Contraseña:",
                Location = new System.Drawing.Point(0, 80),
                Size = new System.Drawing.Size(100, 20)
            };

            txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new System.Drawing.Point(0, 105),
                Size = new System.Drawing.Size(300, 25),
                PasswordChar = '*',
                Font = new System.Drawing.Font("Arial", 10)
            };

            // Botón ingresar
            btnIngresar = new Button
            {
                Text = "Ingresar",
                Location = new System.Drawing.Point(0, 150),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold)
            };

            // Botón salir
            btnSalir = new Button
            {
                Text = "Salir",
                Location = new System.Drawing.Point(130, 150),
                Size = new System.Drawing.Size(80, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Arial", 10)
            };

            // Agregar controles al formulario
            panelForm.Controls.Add(lblUsuario);
            panelForm.Controls.Add(txtUsuario);
            panelForm.Controls.Add(lblPassword);
            panelForm.Controls.Add(txtPassword);
            panelForm.Controls.Add(btnIngresar);
            panelForm.Controls.Add(btnSalir);

            panelPrincipal.Controls.Add(panelForm);
            panelPrincipal.Controls.Add(lblSubtitulo);
            panelPrincipal.Controls.Add(lblTitulo);

            this.Controls.Add(panelPrincipal);

            // Configurar Enter para login
            this.AcceptButton = btnIngresar;
        }

        private void ConfigurarEventos()
        {
            btnIngresar.Click += BtnIngresar_Click;
            btnSalir.Click += BtnSalir_Click;
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (AutenticacionService.Login(txtUsuario.Text.Trim(), txtPassword.Text))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}