using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using TreeViewCreator_AvaloniaApp.Models;

namespace TreeViewCreator_AvaloniaApp.ViewModels;

//https://www.c-sharpcorner.com/article/display-sub-directories-and-files-in-treeview/
public partial class MainViewModel : ViewModelBase
{
    private string folderDirectory;
    public string FolderDirectory
    {
        get { return folderDirectory; }
        set
        {
            folderDirectory = value;
            OnPropertyChanged();
        }
    }

    private int progress;
    public int Progress
    {
        get { return progress; }
        set
        {
            progress = value;
            OnPropertyChanged();
        }
    }

    private IStorageFolder folder;
    public ObservableCollection<Node> Nodes { get; }


    public MainViewModel()
    {
    //https://docs.avaloniaui.net/docs/reference/controls/treeview-1
        Nodes = new ObservableCollection<Node>
            {
                new Node("Animals", new ObservableCollection<Node>
                {
                    new Node("Mammals", new ObservableCollection<Node>
                    {
                        new Node("Lion"), new Node("Cat"), new Node("Zebra")
                    })
                })
            };
    }

    [RelayCommand]
    private async Task SelectDirectory()
    {
        var folder = await DoOpenFilePickerAsync();
        if (folder is null) return;
        this.folder = folder;
        FolderDirectory = folder.Path.AbsolutePath;
    }

    [RelayCommand]
    private async Task LoadDirectory()
    {
        Progress = 0;
        Nodes.Clear();

        if(string.IsNullOrEmpty(FolderDirectory))
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
