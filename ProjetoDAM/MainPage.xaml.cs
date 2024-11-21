using System;

namespace ProjetoDAM
{
    public partial class MainPage : ContentPage
    {
        PhotosDatabase photosDatabase;

        public MainPage()
        {
            InitializeComponent();
            photosDatabase = new PhotosDatabase();
            NewPhoto.BackgroundColor = Colors.Primary;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadPhotos();
        }

        public async void LoadPhotos()
        {
            List<Photos> photos = await photosDatabase.GetPhotosAsync();
            ListPhotos.ItemsSource = new List<Photos>(photos);
        }

        private async void NewPhoto_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AddPhoto");
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            var teste = await DisplayActionSheet("Ação", "Voltar", "Excluir");
            
            if (teste.ToString() != "Excluir")
            {
                return;
            }

            var tappedItem = ((Border)sender).BindingContext;

            if (tappedItem == null)
            {
                return;
            }


            Photos? photo = tappedItem as Photos;
            if (photo == null)
            {
                return;
            }

            await photosDatabase.DeleteItemAsync(photo);
            LoadPhotos();
        }
    }

}
