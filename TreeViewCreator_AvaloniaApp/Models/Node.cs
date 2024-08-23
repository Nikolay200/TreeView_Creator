using System.Collections.ObjectModel;

namespace TreeViewCreator_AvaloniaApp.Models
{
    public class Node
    {
        public ObservableCollection<Node>? SubNodes { get; }
        public string Title { get; }
        public int ImageIndex {  get; }
        public int SelectedImageIndex {  get; }
        public string FullPath { get; set; }
        public int Size {  get; set; }
        public Node Parent { get; }

        public Node(string title)
        {
            Title = title;
        }
        public Node(string title, ObservableCollection<Node> subNodes)
        {
            Title = title;
            SubNodes = subNodes;
        }
        public Node(string title, int imageIndex, ObservableCollection<Node> subNodes)
        {
            Title = title;
            ImageIndex = imageIndex;
            SubNodes = subNodes;
        }
        public Node(string title, int imageIndex)
        {
            Title = title;
            ImageIndex = imageIndex;
        }
        public Node(string title, int imageIndex, int selectedImageIndex)
        {
            Title = title;
            ImageIndex = imageIndex;
            SelectedImageIndex = selectedImageIndex;
        }
        public Node(string title, int imageIndex, int selectedImageIndex, ObservableCollection<Node> subNodes)
        {
            Title = title;
            ImageIndex = imageIndex;
            SelectedImageIndex = selectedImageIndex;
            SubNodes = subNodes;
        }
    }
}
