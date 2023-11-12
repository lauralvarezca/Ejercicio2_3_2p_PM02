using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Ejer2_3.Models;

namespace Ejer2_3.Views
{
    public partial class Lista : ContentPage
    {
        private ObservableCollection<Audios> listaAudios;
        private MediaElement mediaElement; 

        public Lista()
        {
            InitializeComponent();
            listaAudios = new ObservableCollection<Audios>();
            Lista_Audios.ItemsSource = listaAudios;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.BDAudios != null)
            {
                await ActualizarListaAudios();
            }
            else
            {
                Console.WriteLine("App.BDAudios es null");
            }
        }

        private async System.Threading.Tasks.Task ActualizarListaAudios()
        {
            var audios = await App.BDAudios.ListaAudios();
            listaAudios.Clear();

            foreach (var audio in audios)
            {
                listaAudios.Add(audio);
            }
        }

        private async void Lista_Audios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (App.BDAudios != null)
            {
                await ActualizarListaAudios();
                var audioSeleccionado = (Audios)e.Item;

                if (audioSeleccionado != null)
                {
                    string action = await DisplayActionSheet("Selecciona una opción", "Cancelar", null, "Reproducir", "Eliminar");

                    switch (action)
                    {
                        case "Reproducir":
                            ReproducirAudio(audioSeleccionado.url);
                            break;

                        case "Eliminar":
                            bool confirmacion = await DisplayAlert("Confirmar", "¿Estás seguro de que deseas eliminar este audio?", "Sí", "No");
                            if (confirmacion)
                            {
                                await EliminarAudio(audioSeleccionado);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("App.BDAudios es null");
            }
        }

        private void ReproducirAudio(string filePath)
        {
            if (mediaElement != null)
            {
                mediaElement.Stop();
                (Content as StackLayout)?.Children.Remove(mediaElement);
            }

            mediaElement = new MediaElement
            {
                Source = filePath,
                ShouldAutoPlay = true
            };

            (Content as StackLayout)?.Children.Add(mediaElement);
        }

        private async Task EliminarAudio(Audios audio)
        {
            if (App.BDAudios != null)
            {
                if (mediaElement != null)
                {
                    mediaElement.Stop();
                    (Content as StackLayout)?.Children.Remove(mediaElement);
                }

                bool eliminacionExitosa = await App.BDAudios.EliminarAudio(audio);

                if (eliminacionExitosa)
                {
                    await DisplayAlert("Éxito", "Audio eliminado correctamente", "Ok");
                    await ActualizarListaAudios();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar el audio", "Ok");
                }
            }
            else
            {
                Console.WriteLine("App.BDAudios es null");
            }
        }

        private async void B_Play_Invoked(object sender, EventArgs e)
        {
            if (Lista_Audios.SelectedItem != null)
            {
                var audioSeleccionado = (Audios)Lista_Audios.SelectedItem;
                ReproducirAudio(audioSeleccionado.url);
            }
            else
            {
                await DisplayAlert("Aviso", "Por favor, seleccione un audio para reproducir.", "Ok");
            }
        }

        private async void B_Delete_Invoked(object sender, EventArgs e)
        {
            if (Lista_Audios.SelectedItem != null)
            {
                var audioSeleccionado = (Audios)Lista_Audios.SelectedItem;
                bool confirmacion = await DisplayAlert("Confirmar", "¿Estás seguro de que deseas eliminar este audio?", "Sí", "No");
                if (confirmacion)
                {
                    await EliminarAudio(audioSeleccionado);
                }
            }
            else
            {
                await DisplayAlert("Aviso", "Por favor, seleccione un audio para eliminar.", "Ok");
            }
        }

        private void OnPausarAudioClicked(object sender, EventArgs e)
        {
            if (mediaElement != null)
            {
                mediaElement.Pause();
            }
        }

    }
}
