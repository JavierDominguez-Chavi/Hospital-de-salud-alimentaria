using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace HospitalDeSaludAlimentaria
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Label_WrongHeight.Visibility = Visibility.Hidden;
            this.Label_WrongWeight.Visibility = Visibility.Hidden;
            InitializeComboBoxGender();
            InitializeComboBoxAge();

        }

        private void InitializeComboBoxGender()
        {
            this.ComboBox_Gender.Items.Add("Masculino");
            this.ComboBox_Gender.Items.Add("Femenino");
        }

        private void InitializeComboBoxAge()
        {
            for (int i = 0; i <= 120; i++)
            {
                this.ComboBox_Age.Items.Add(i);
            }
        }

        private void Button_CalculateCalories_Click(object sender, RoutedEventArgs e)
        {
            Boolean validationWeight = ValidateWeight();
            Boolean validationHeight = ValidateHeight();
            Boolean validationActivityOption = ValidateActivityOption();
            if (validationWeight && validationHeight && validationActivityOption)
            {
                float caloriesByDay = CalculateCalories();
                this.TextBox_CaloriesByDay.Text = caloriesByDay.ToString();
            }
        }

        private Boolean ValidateHeight()
        {
            Boolean validation = false;
            if (!String.IsNullOrEmpty(this.TextBox_Height.Text))
            {
                string heightText = this.TextBox_Height.Text.ToString();
                int heightNumber = Int32.Parse(heightText);
                if (heightNumber < 20 || heightNumber > 250)
                {
                    InvalidInputTextBoxHeight();
                }
                else
                {
                    this.TextBox_Height.BorderBrush = System.Windows.Media.Brushes.Gray;
                    this.Label_WrongHeight.Visibility = Visibility.Hidden;
                    validation = true;
                }
            } 
            else
            {
                InvalidInputTextBoxHeight();
            }
            return validation;
        }

        private void InvalidInputTextBoxHeight()
        {
            this.TextBox_Height.BorderBrush = System.Windows.Media.Brushes.Red;
            this.Label_WrongHeight.Visibility = Visibility.Visible;
            this.TextBox_CaloriesByDay.Clear();
        }

        private Boolean ValidateActivityOption()
        {
            Boolean validation = true;
            if (this.RadioButton_LittleActivity.IsChecked == false && this.RadioButton_LightExercise.IsChecked == false
            && this.RadioButton_ModerateExercise.IsChecked == false && this.RadioButton_RegularSport.IsChecked == false
            && this.RadioButton_EliteAthlete.IsChecked == false)
            {
                MessageBox.Show("Para consultar las calorías requeridas por día debes seleccionar el Nivel de actividad",
                                "Nivel de actividad no seleccinado", MessageBoxButton.OK, MessageBoxImage.Warning);
                validation = false;
            }
            return validation;
        }

        private Boolean ValidateWeight()
        {
            Boolean validation = false;
            if (!String.IsNullOrEmpty(this.TextBox_ActualWeight.Text))
            {
                string weigthText = this.TextBox_ActualWeight.Text.ToString();
                float weigthNumber = float.Parse(weigthText);
                if (weigthNumber < 0.25 || weigthNumber > 600)
                {
                    InvalidInputTextBoxWeight();
                }
                else
                {
                    this.TextBox_ActualWeight.BorderBrush = System.Windows.Media.Brushes.Gray;
                    this.Label_WrongWeight.Visibility = Visibility.Hidden;
                    validation = true;
                }
            }
            else
            {
                InvalidInputTextBoxWeight();
            }
            return validation;
        }

        private void InvalidInputTextBoxWeight()
        { 
            this.TextBox_ActualWeight.BorderBrush = System.Windows.Media.Brushes.Red;
            this.Label_WrongWeight.Visibility = Visibility.Visible;
            this.TextBox_CaloriesByDay.Clear();
        }

        private float CalculateCalories()
        {
            float totalCalories = 0;
            switch (this.ComboBox_Gender.SelectedValue.ToString()) 
            {
                case "Femenino":
                    totalCalories = (10*float.Parse(this.TextBox_ActualWeight.Text)) + (6.25f*float.Parse(this.TextBox_Height.Text))-(5*int.Parse(this.ComboBox_Age.SelectedValue.ToString())) - 161;
                    totalCalories = CalculateTotalCaloriesByPhysicalActivity(totalCalories);
                    break;
                case "Masculino":
                    totalCalories = (10 * float.Parse(this.TextBox_ActualWeight.Text)) + (6.25f * float.Parse(this.TextBox_Height.Text)) - (5 * int.Parse(this.ComboBox_Age.SelectedValue.ToString())) + 5;
                    totalCalories = CalculateTotalCaloriesByPhysicalActivity(totalCalories);
                    break;
            }
            return totalCalories;
        }

        private float CalculateTotalCaloriesByPhysicalActivity(float totalCalories)
        {   
            if (this.RadioButton_LittleActivity.IsChecked==true)
            {
                totalCalories = totalCalories*1.2f;
            }
            if(this.RadioButton_LightExercise.IsChecked==true)
            {
                totalCalories = totalCalories * 1.375f;
            }
            if (this.RadioButton_ModerateExercise.IsChecked == true)
            {
                totalCalories = totalCalories * 1.55f;
            }
            if (this.RadioButton_RegularSport.IsChecked == true)
            {
                totalCalories = totalCalories * 1.725f;
            }
            if (this.RadioButton_EliteAthlete.IsChecked == true)
            {
                totalCalories = totalCalories * 1.9f;
            }
            return totalCalories;
        }

        private void NumberValidationTextBoxHeight(object sender, TextCompositionEventArgs textCompositionEvent)
        {
            Regex regex = new Regex("[^0-9]+");
            textCompositionEvent.Handled = regex.IsMatch(textCompositionEvent.Text);
        }

        private void NumberValidationTextBoxWeight(object sender, TextCompositionEventArgs textCompositionEvent)
        {
            char identificadorDecimal = textCompositionEvent.Text[0];

            if ((Char.IsDigit(identificadorDecimal) || identificadorDecimal == '.'))
            {
                if (identificadorDecimal == '.' && this.TextBox_ActualWeight.Text.Contains('.'))
                    textCompositionEvent.Handled = true;
            }
            else
                textCompositionEvent.Handled = true;
        }
    }
}