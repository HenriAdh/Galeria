namespace ProjetoDAM;

public partial class AddPhotosPage : ContentPage
{
    PhotosDatabase photosDatabase;
    string _currentPhoto = "";
    public AddPhotosPage()
	{
		InitializeComponent();
        photosDatabase = new PhotosDatabase();
        TirarFoto.BackgroundColor = Colors.Primary;
        ImportarFoto.BackgroundColor = Colors.Primary;
        SalvarFoto.BackgroundColor = Colors.Primary;
        Voltar.BackgroundColor = Colors.Secondary;
    }

    private async void TirarFoto_Clicked(object sender, EventArgs e)
    {
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await DisplayAlert("Erro", "Dispositivo sem acesso a camera", "OK");
            return;
        }

        FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

        if (photo == null)
        {
            return;
        }

        await LoadPhoto(photo);
    }

    private async void ImportarFoto_Clicked(object sender, EventArgs e)
    {
        FileResult? photo = await MediaPicker.Default.PickPhotoAsync();

        if (photo == null)
        {
            return;
        }

        await LoadPhoto(photo);
    }

    private async void SalvarFoto_Clicked(object sender, EventArgs e)
    {
        string comment = Description.Text;

        if (comment == "")
        {
            await DisplayAlert("Erro", "Descrição não informada", "OK");
            return;
        }

        if (_currentPhoto == "")
        {
            await DisplayAlert("Erro", "Foto não selecionada", "OK");
            return;
        }

        await SavePhoto(_currentPhoto, comment);

        Description.Text = "";
        LoadedPhoto.Source = "default_photo.png";
        _currentPhoto = "";
    }

    private async Task LoadPhoto(FileResult photo)
    {
        ChangeIsEnable(false);
        
        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

        using Stream sourceStream = await photo.OpenReadAsync();
        using FileStream localFileStream = File.OpenWrite(localFilePath);

        await sourceStream.CopyToAsync(localFileStream);

        LoadedPhoto.Source = localFilePath;
        _currentPhoto = localFilePath;

        ChangeIsEnable(true);
    }

    private async Task SavePhoto(string localFilePath, string comment)
    {
        ChangeIsEnable(false);

        Photos savePhoto = new Photos();
        savePhoto.Url = localFilePath;
        savePhoto.Comment = comment;
        savePhoto.Favorite = false;
        savePhoto.CreatedAt = DateTime.Now;

        await photosDatabase.SaveItemAsync(savePhoto);

        ChangeIsEnable(true);
    }

    private void ChangeIsEnable(bool enable)
    {
        TirarFoto.IsEnabled = enable;
        ImportarFoto.IsEnabled = enable;
    }

    private async void Voltar_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}