using Firebase_modelo_singleton.Models;
using Microsoft.Maui.Controls;
using System;

namespace Firebase_modelo_singleton
{
    public partial class MainPage : ContentPage{
        public MainPage(){
            InitializeComponent();
        }

        private async void guardarButton_Clicked(object sender, EventArgs e)
        {
            string nombre = nombreEntry.Text;
            string edadText = edadEntry.Text;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(edadText))
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            if (!int.TryParse(edadText, out int edad))
            {
                await DisplayAlert("Error", "La edad debe ser un número entero.", "OK");
                return;
            }

            try{
                var firebaseInstance = Singleton.Instance;
                People persona = new People { Nombre = nombre, Edad = edad};

                await firebaseInstance.CreateData(persona);

                await DisplayAlert("Éxito", "Datos subidos correctamente.", "OK");

                nombreEntry.Text = string.Empty;
                edadEntry.Text = string.Empty;
            }catch (Exception ex){
                await DisplayAlert("Error", $"Error al subir datos: {ex.Message}", "OK");
            }
        }

        private async void listarButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Page_list());
        }
    }
}
