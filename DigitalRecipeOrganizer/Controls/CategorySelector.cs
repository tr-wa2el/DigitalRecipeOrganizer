using System.ComponentModel;

namespace DigitalRecipeOrganizer.Controls
{
    public enum RecipeCategory
    {
        Dessert,
        MainCourse,
        Appetizer,
        Beverage,
        Salad,
        Soup,
        Breakfast,
        Snack,
        Sauce,
        Other
    }

    public class CategoryChangedEventArgs : EventArgs
    {
        public RecipeCategory SelectedCategory { get; set; }

        public CategoryChangedEventArgs(RecipeCategory category)
        {
            SelectedCategory = category;
        }
    }

    public partial class CategorySelector : UserControl
    {
        public event EventHandler<CategoryChangedEventArgs>? CategoryChanged;

        private RecipeCategory _selectedCategory = RecipeCategory.MainCourse;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RecipeCategory SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                cmbCategory.SelectedItem = value;
            }
        }

        public CategorySelector()
        {
            InitializeComponent();
            InitializeCategoryComboBox();
        }

        private void InitializeCategoryComboBox()
        {
            cmbCategory.Items.Clear();
            foreach (RecipeCategory category in Enum.GetValues(typeof(RecipeCategory)))
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.SelectedIndex = 0;
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem != null)
            {
                _selectedCategory = (RecipeCategory)cmbCategory.SelectedItem;
                CategoryChanged?.Invoke(this, new CategoryChangedEventArgs(_selectedCategory));
            }
        }
    }
}
