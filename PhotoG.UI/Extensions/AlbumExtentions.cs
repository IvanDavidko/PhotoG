using System.Text.RegularExpressions;
using System.Web;
using PhotoG.UI.Models;

namespace PhotoG.UI.Extentions
{
    public static class AlbumExtentions
    {
        public static void CreateUrlFromTitle(this AlbumModel album)
        {
            var url = album.Title;
            url = Regex.Replace(url, @"^\W+|\W+$", "");
            url = Regex.Replace(url, "'\"", "");
            url = Regex.Replace(url, @"_", "-");
            url = Regex.Replace(url, @"\W+", "-");

            album.DirectUrl = url;
        }
    }
}