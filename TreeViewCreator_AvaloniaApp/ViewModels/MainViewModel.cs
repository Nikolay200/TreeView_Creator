using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

    private int progressValue;
    public int ProgressValue
    {
        get { return progressValue; }
        set
        {
            progressValue = value;
            OnPropertyChanged();
        }
    }
    private int maximum;
    public int Maximum
    {
        get { return maximum; }
        set
        {
            maximum = value;
            OnPropertyChanged();
        }
    }
    private int minimum;
    public int Minimum
    {
        get { return minimum; }
        set
        {
            minimum = value;
            OnPropertyChanged();
        }
    }

    private IStorageFolder folder;
    public ObservableCollection<Node> Nodes { get; set; } = new ObservableCollection<Node>();
    public HierarchicalTreeDataGridSource<Node> Source { get; set; }
    public ObservableCollection<Node> SelectedNodes { get; } 
    public MainViewModel()
    {
        //https://docs.avaloniaui.net/docs/reference/controls/treeview-1

        Source = new HierarchicalTreeDataGridSource<Node>(Nodes)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<Node>(
                    new TextColumn<Node, string>("Название", x => x.Title),
                    x => x.SubNodes),
                new TextColumn<Node, string>("Директория", x => x.FullPath),
                new TextColumn<Node, int>("Размер", x => x.Size),
            },
        };
    }

    [RelayCommand]
    private async Task SelectDirectory()
    {
        var folder = await DoOpenFilePickerAsync();
        if (folder is null) return;
        this.folder = folder;
        FolderDirectory = folder.Path.LocalPath;
    }

    [RelayCommand]
    private async Task LoadDirectory()
    {
        ProgressValue = 0;
        //Nodes.Clear();

        if (string.IsNullOrEmpty(FolderDirectory))
        {
            return;
        }
        else if(Directory.Exists(FolderDirectory))
        {
            LoadSelectedDirectory(FolderDirectory);
        }
    }

    private void LoadSelectedDirectory(string folderDirectory)
    {
        DirectoryInfo di = new DirectoryInfo(folderDirectory);
        //Setting ProgressBar Maximum Value
        //Maximum = Directory.GetFiles(folderDirectory, "*.*", SearchOption.AllDirectories).Length + 
        //    Directory.GetDirectories(folderDirectory, "**", SearchOption.AllDirectories).Length;

        var treeNode = new Node(di.Name, 0, new ObservableCollection<Node>());        
        treeNode.FullPath = di.FullName;            
        Nodes.Add(treeNode);

        LoadFiles(folderDirectory, treeNode);
        LoadSubDirectories(folderDirectory, treeNode);
    }
    private void LoadSubDirectories(string dir, Node node)
    {
        // Get all subdirectories
        string[] subdirectoryEntries = Directory.GetDirectories(dir);
        // Loop through them to see if they have any other subdirectories
        foreach (string subdirectory in subdirectoryEntries)
        {
            DirectoryInfo di = new DirectoryInfo(subdirectory);
            var treeNode = new Node(di.Name, 0, new ObservableCollection<Node>());
            treeNode.FullPath = di.FullName;
            node.SubNodes.Add(treeNode);
            LoadFiles(subdirectory, treeNode);
            LoadSubDirectories(subdirectory, treeNode);
            //UpdateProgress();
        }
    }
    private void LoadFiles(string dir, Node node)
    {
        string[] Files = Directory.GetFiles(dir, "*.*");
        // Loop through them to see files
        foreach (string file in Files)
        {
            FileInfo fi = new FileInfo(file);
            var treeNode = new Node(fi.Name, 1);
            treeNode.FullPath = fi.FullName;
            node.SubNodes.Add(treeNode);
            //UpdateProgress();
        }
    }

    private void UpdateProgress()
    {
        if (ProgressValue < Maximum)
        {
            ProgressValue++;
            int percent = (int)(((double)ProgressValue / (double)Maximum) * 100);
            //progressBar1.CreateGraphics().DrawString(percent.ToString() + "%", 
            //    new Font("Arial", (float)8.25, FontStyle.Regular), 
            //    Brushes.Black, 
            //    new PointF(progressBar1.Width / 2 - 10, progressBar1.Height / 2 - 7));            
        }
    }

    private async Task<IStorageFolder?> DoOpenFilePickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var folder = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "File Explorer",
            AllowMultiple = false
        });

        return folder[0];
    }

}
