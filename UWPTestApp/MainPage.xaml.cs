using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Get a list of files from the music library
            IReadOnlyList<StorageFile> files = await KnownFolders.MusicLibrary.GetFilesAsync();
            //Select random file from the music library
            StorageFile infoFile = await StorageFile.GetFileFromPathAsync(files[new Random().Next(0, files.Count - 1)].Path);

            //Create a MediaSource and add it to a MediaPlaybackList
            MediaSource source = MediaSource.CreateFromStorageFile(infoFile);
            MediaPlaybackList list = new MediaPlaybackList();
            list.Items.Add(new MediaPlaybackItem(source));

            //Create a MediaPlayer and add the MediaPlaybackList to it
            MediaPlayer player = new MediaPlayer();
            player.Source = list;
            //'mediaPlayer' was created in xaml: <MediaPlayerElement x:Name="mediaPlayer"/>
            mediaPlayer.SetMediaPlayer(player);

            //Get the music properties of the audio file
            MusicProperties musicProperties = await infoFile.Properties.GetMusicPropertiesAsync();
            //Edit them (so UWP will attempt to edit the file)
            musicProperties.Title = "woo";
            //Save the properties...
            //When using C:\etc as a MusicLibrary, this will work fine
            //When using a mapped location say X:\etc this save will thorw access denied
            await musicProperties.SavePropertiesAsync();
        }
    }
}
