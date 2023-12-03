using Firebase.Database;
using Firebase_modelo_singleton.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Firebase_modelo_singleton
{
    public partial class Page_list : ContentPage
    {
        public ObservableCollection<People> PeopleList { get; set; }

        public Page_list()
        {
            InitializeComponent();

            PeopleList = new ObservableCollection<People>();

            peopleListView.ItemsSource = PeopleList;

            LoadData();
        }

        protected override async void OnAppearing() {
            base.OnAppearing();

            await LoadData();
        }

        public void SetPeopleList(ObservableCollection<People> people)
        {
            PeopleList.Clear();
            foreach (var person in people)
            {
                PeopleList.Add(person);
            }
        }

        private async void peopleListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedPerson = (People) e.SelectedItem;

            var action = await DisplayActionSheet($"Opciones para {selectedPerson.Nombre}", "Cancelar", null, "Editar", "Eliminar");

            switch (action)
            {
                case "Editar":
                    var pageUpdate = new Page_update();
                    pageUpdate.SetPersonaSeleccionada(selectedPerson);
                    await Navigation.PushAsync(pageUpdate);
                    break;
                case "Eliminar":
                    var firebaseInstance = Singleton.Instance;
                    try
                    {

                    Console.WriteLine("error: "+selectedPerson.id);
                    await firebaseInstance.DeleteData(selectedPerson.id.ToString());
                        await LoadData();
                        await DisplayAlert("Éxito", "Persona eliminada correctamente.", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Error al eliminar persona: {ex.Message}", "OK");
                    }
                    break;
            }

            peopleListView.SelectedItem = null;
        }

        private async Task LoadData()
        {
            try
            {
                var firebaseInstance = Singleton.Instance;

                var personas = await firebaseInstance.ReadData();

                SetPeopleList(new ObservableCollection<People>(personas));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar datos: {ex.Message}", "OK");
            }
        }
    }
}
