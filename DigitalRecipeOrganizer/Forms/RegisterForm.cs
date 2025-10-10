using DigitalRecipeOrganizer.Data;
using DigitalRecipeOrganizer.Models;
using DigitalRecipeOrganizer.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DigitalRecipeOrganizer.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly RecipeDbContext _dbContext;

        public RegisterForm()
        {
            InitializeComponent();
            _dbContext = new RecipeDbContext();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                btnRegister.Enabled = false;
                Cursor = Cursors.WaitCursor;

                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                // Check if username already exists
                if (await _dbContext.Users.AnyAsync(u => u.Username == username))
                {
                    MessageBox.Show("Username already exists. Please choose a different username.",
                        "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                // Check if email already exists
                if (await _dbContext.Users.AnyAsync(u => u.Email == email))
                {
                    MessageBox.Show("Email already exists. Please use a different email address.",
                        "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                // Create new user
                var newUser = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = PasswordHelper.HashPassword(password),
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();

                MessageBox.Show("Registration successful! You can now login with your credentials.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger.LogError(ex, "Registration");
            }
            finally
            {
                btnRegister.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private bool ValidateInput()
        {
            // Validate username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (txtUsername.Text.Trim().Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter an email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Validate password
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter a password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            // Validate confirm password
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
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
            txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _dbContext?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
