using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TreeViewCreator_AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private string fileName;
    public string FileName
    {
        get { return fileName; }
        set
        {
            fileName = value;
            OnPropertyChanged();
        }
    }

    private IStorageFolder folder;

    [RelayCommand]
    private async Task LoadDirectory()
    {
        var folder = await DoOpenFilePickerAsync();
        if (folder is null) return;
        this.folder = folder;
        FileName = folder.Path.AbsolutePath;
    }
    private async Task<IStorageFolder?> DoOpenFilePickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var folder = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "File Explorer",
            AllowMultiple = false,
        });

        return folder[0];
    }

}
