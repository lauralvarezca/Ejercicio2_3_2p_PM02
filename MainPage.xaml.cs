using Ejer2_3.Models;
using Ejer2_3.Views;
using Plugin.Maui.Audio;

namespace Ejer2_3
{
    public partial class MainPage : ContentPage
    {
        private readonly IAudioRecorder _audioRecorder;
        private bool isRecording = false;

        public string pathaudio, filename;

        public MainPage()
        {
            InitializeComponent();

            _audioRecorder = AudioManager.Current.CreateRecorder();
        }

        private async void B_Grabar_Clicked(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                var permiso = await Permissions.RequestAsync<Permissions.Microphone>();
                var permiso1 = await Permissions.RequestAsync<Permissions.StorageRead>();
                var permiso2 = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (permiso != PermissionStatus.Granted || permiso1 != PermissionStatus.Granted || permiso2 != PermissionStatus.Granted)
                {
                    return;
                }

                if (string.IsNullOrEmpty(txtdescripcion.Text))
                {
                    await DisplayAlert("Message", "Descripcion Vacia", "Ok");
                    return;
                }

                await _audioRecorder.StartAsync();
                isRecording = true;
                Console.WriteLine("Iniciando grabación...");
            }
            else
            {
                var recordedAudio = await _audioRecorder.StopAsync();

                if (recordedAudio != null)
                {
                    try
                    {
                        filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DateTime.Now.ToString("ddMMyyyymmss") + "_VoiceNote.wav");

                        using (var fileStorage = new FileStream(filename, FileMode.Create, FileAccess.Write))
                        {
                            recordedAudio.GetAudioStream().CopyTo(fileStorage);
                        }

                        pathaudio = filename;

                        Audios nuevoAudio = new Audios
                        {
                            descripcion = txtdescripcion.Text,
                            url = pathaudio,
                            fecha = DateTime.Now
                        };

                        int resultado = await App.BDAudios.GrabarAudio(nuevoAudio);

                        if (resultado > 0)
                        {
                            await DisplayAlert("", "Audio grabado correctamente", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Error", "Error al guardar en la base de datos", "Ok");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        await DisplayAlert("Error", "Ocurrió un error al procesar la grabación.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "La grabación de audio ha fallado.", "Ok");
                }

                isRecording = false;
                Console.WriteLine("Deteniendo grabación y guardando el audio...");
            }
        }

        private async void B_Ver_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Lista());
        }
    }
}