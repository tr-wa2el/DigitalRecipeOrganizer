using DigitalRecipeOrganizer.Data;
using DigitalRecipeOrganizer.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DigitalRecipeOrganizer.Forms
{
    public partial class LoginForm : Form
    {
        private readonly RecipeDbContext _dbContext;

        public LoginForm()
        {
            InitializeComponent();
            _dbContext = new RecipeDbContext();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                Cursor = Cursors.WaitCursor;

                // Find user by username
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null || !user.IsActive)
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // Verify password
                if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // Login successful
                UserSession.Login(user);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger.LogError(ex, "Login");
            }
            finally
            {
                btnLogin.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using var registerForm = new RegisterForm();
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Registration successful! Please login with your credentials.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _dbContext?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
