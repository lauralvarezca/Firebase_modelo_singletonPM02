using Firebase_modelo_singleton.Models;
using Microsoft.Maui.Controls;
using System;

namespace Firebase_modelo_singleton
{
    public partial class Page_update : ContentPage
    {
        private People personaSeleccionada;

        public Page_update()
        {
            InitializeComponent();
        }

        public void SetPersonaSeleccionada(People persona)
        {
            personaSeleccionada = persona;

            nombreEntry.Text = persona.Nombre;
            edadEntry.Text = persona.Edad.ToString();
        }

        private async void actualizarButton_Clicked(object sender, EventArgs e)
        {
            if (personaSeleccionada != null)
            {
                string nombreAntiguo = personaSeleccionada.Nombre;
                int edadAntigua = personaSeleccionada.Edad;

                personaSeleccionada.Nombre = nombreEntry.Text;

                if (int.TryParse(edadEntry.Text, out int edad))
                {
                    personaSeleccionada.Edad = edad;
                }
                else
                {
                    await DisplayAlert("Error", "La edad debe ser un número entero.", "OK");
                    return;
                }

                try
                {
                    var firebaseInstance = Singleton.Instance;
                    //People temp= personaSeleccionada;
                    //temp.id="";

                    //await firebaseInstance.UpdateData(personaSeleccionada.id.ToString(),temp);
                    await firebaseInstance.UpdateData(personaSeleccionada.id.ToString(),personaSeleccionada);

                    await DisplayAlert("Éxito", "Datos actualizados correctamente.", "OK");
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    personaSeleccionada.Nombre = nombreAntiguo;
                    personaSeleccionada.Edad = edadAntigua;

                    await DisplayAlert("Error", $"Error al actualizar datos: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No se ha seleccionado ninguna persona para actualizar.", "OK");
            }
        }



        private async void listarButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Page_list());
        }
    }
}
